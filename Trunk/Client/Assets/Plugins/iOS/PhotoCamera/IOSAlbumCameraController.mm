//
//  OpenPhotoController.m
//
//  Created by AnYuanLzh
//

#import "IOSAlbumCameraController.h"
#import "DogeUtils.h"

@implementation IOSAlbumCameraController

- (void)showActionSheet
{
    NSLog(@"########### --- showActionSheet !!");
    
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction *albumAction = [UIAlertAction actionWithTitle:@"相册" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click album action!");
        [self showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:YES];
    }];
    
    UIAlertAction *cameraAction = [UIAlertAction actionWithTitle:@"相机" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click camera action!");
        [self showPicker:UIImagePickerControllerSourceTypeCamera allowsEditing:YES];
    }];
    
    UIAlertAction *album_cameraAction = [UIAlertAction actionWithTitle:@"相册&相机" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click album&camera action!");
        //[self showPicker:UIImagePickerControllerSourceTypeCamera];
        [self showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:YES];
    }];
    
    UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click cancel action!");
    }];
    
    
    [alertController addAction:albumAction];
    [alertController addAction:cameraAction];
    [alertController addAction:album_cameraAction];
    [alertController addAction:cancelAction];
    
    UIViewController *vc = UnityGetGLViewController();
    [vc presentViewController:alertController animated:YES completion:^{
        NSLog(@"showActionSheet -- completion");
    }];
}

- (void)showPicker:(UIImagePickerControllerSourceType)type  allowsEditing:(BOOL)flag
{
    NSLog(@" --- showPicker!!");
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.delegate = self;
    picker.sourceType = type;
    picker.allowsEditing = flag;
    NSLog(@" --- imagePickerController didFinishPickingMediaWithInfo!! picker.allowsEditin=%d",picker.allowsEditing);
    
    [self presentViewController:picker animated:YES completion:nil];
}

// 打开相册后选择照片时的响应方法
- (void)imagePickerController:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
   // [self showActionButton:picker didFinishPickingMediaWithInfo:info];
    NSLog(@" --- imagePickerController didFinishPickingMediaWithInfo!!");
      NSLog(@" --- imagePickerController!! info=%@",info);
    // Grab the image and write it to disk
    UIImage *image;
    UIImage *image2;
    
    NSLog(@" --- imagePickerController!! allowsEditing  true self.cutWidth=%d  self.cutHeight=%d",self.cutWidth,self.cutHeight);
    if ([picker allowsEditing]){
        //获取用户编辑之后的图像
        image = [info objectForKey:UIImagePickerControllerEditedImage];
        if(image==NULL){
             image = [info objectForKey:UIImagePickerControllerOriginalImage];
        }
        UIGraphicsBeginImageContext(CGSizeMake(self.cutWidth,self.cutHeight));
        [image drawInRect:CGRectMake(0, 0, self.cutWidth, self.cutHeight)];
        image2 = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
        UnitySendMessage( "[Login]", "OnPrintLog", [[NSString stringWithFormat:@"cutWidth=%d cutHeight=%d",self.cutWidth,self.cutHeight] UTF8String] );
	    [self sendImgToUnity:picker Img:image2];
         NSLog(@" --- imagePickerController!! allowsEditing  true @@@@@@@@@@@@@@@@@@");
    } else {
        [self showActionButton:picker didFinishPickingMediaWithInfo:info];
    }
  
}

//选择照片弹出确定框
- (void)showActionButton:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
    NSLog(@"########### --- showActionButton !!");
    UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"确定选择照片?" message:nil preferredStyle:UIAlertControllerStyleAlert];
    //确定
    UIAlertAction *okAlert = [UIAlertAction actionWithTitle:@"确定" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action){
        // 照片的元数据参数
        UIImage* img = [info objectForKey:UIImagePickerControllerOriginalImage];
        [self sendImgToUnity:picker Img:img];
        //具体操作内容
    }];
    //取消
    UIAlertAction *cancelAlert = [UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleDestructive handler:^(UIAlertAction *action){
        //具体操作内容
        
    }];
    
    [alert addAction:okAlert];
    [alert addAction:cancelAlert];
    [picker presentViewController:alert animated:YES completion:^{
        NSLog(@"showActionSheet -- completion");
    }];
}
-(void)sendImgToUnity:(UIImagePickerController*)picker Img:(UIImage*)img
{
    NSData *imgData;
    if(img==nil){
        UnitySendMessage( "[Login]", "OnPrintLog", "错误：照片数据为空");
        [picker dismissViewControllerAnimated:YES completion:nil];
        return;
    }
    imgData= UIImageJPEGRepresentation(img, .6);
    
    NSString *_encodeImageStr = [imgData base64Encoding];
    UnitySendMessage( "[Login]", "PickImageCallBack_Base64", _encodeImageStr.UTF8String);
    
    // 关闭相册
    [picker dismissViewControllerAnimated:YES completion:nil];
    
}

// 打开相册后点击“取消”的响应
- (void)imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
    NSLog(@" --- imagePickerControllerDidCancel !!");
    [self dismissViewControllerAnimated:YES completion:nil];
}


+(void) saveImageToPhotosAlbum:(NSString*) readAdr
{
    NSLog(@"readAdr: ");
   // NSLog(readAdr);
    UIImage* image = [UIImage imageWithContentsOfFile:readAdr];
    UIImageWriteToSavedPhotosAlbum(image,
                                   self,
                                   @selector(image:didFinishSavingWithError:contextInfo:),
                                   NULL);
}

+(void) image:(UIImage*)image didFinishSavingWithError:(NSError*)error contextInfo:(void*)contextInfo
{
    NSString* result;
    if(error)
    {
        result = @"图片保存到相册失败!";
    }
    else
    {
        result = @"图片保存到相册成功!";
    }
    UnitySendMessage( "[Login]", "SaveImageToPhotosAlbumCallBack", result.UTF8String);
}


// 打开相册
void iosOpenPhotoLibrary()
{
    if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary])
    {
        IOSAlbumCameraController * app = [[IOSAlbumCameraController alloc] init];
        UIViewController *vc = UnityGetGLViewController();
        [vc.view addSubview: app.view];
        
        [app showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:NO];
    }
    else
    {
        UnitySendMessage( "[Login]", "PickImageCallBack_Base64", (@"").UTF8String);
    }
}

@end

//------------- called by unity -----begin-----------------
#if defined (__cplusplus)
extern "C" {
#endif
    
    // 弹出一个菜单项：相册、相机
    void _showActionSheet()
    {
        NSLog(@" -unity call-- _showActionSheet !!");
        IOSAlbumCameraController * app = [[IOSAlbumCameraController alloc] init];
        UIViewController *vc = UnityGetGLViewController();
        [vc.view addSubview: app.view];
        
        [app showActionSheet];
    }
    

    
    // 打开相册
    void _iosOpenPhotoAlbums()
    {
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeSavedPhotosAlbum])
        {
            IOSAlbumCameraController * app = [[IOSAlbumCameraController alloc] init];
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: app.view];
            
            [app showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:NO];
        }
        else
        {
            iosOpenPhotoLibrary();
        }
    }
    
    // 打开相机
    void _iosOpenCamera()
    {
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera])
        {
            IOSAlbumCameraController * app = [[IOSAlbumCameraController alloc] init];
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: app.view];
            
            [app showPicker:UIImagePickerControllerSourceTypeCamera allowsEditing:NO];
        }
        else
        {
            UnitySendMessage( "[Login]", "PickImageCallBack_Base64", (@"").UTF8String);
        }
    }
    
    
    // 打开照相--可编辑
    void _iosOpenPhotoLibrary_allowsEditing()
    {
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary])
        {
            IOSAlbumCameraController * app = [[IOSAlbumCameraController alloc] init];
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: app.view];
            
            [app showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:YES];
        }
        else
        {
            UnitySendMessage( "[Login]", "PickImageCallBack_Base64", (@"").UTF8String);
        }
        
    }
    
  
    
    // 保存照片到相册
    //    void _iosSaveImageToPhotosAlbum(char* base64)
    //    {
    //        NSString* temp = [NSString stringWithUTF8String:base64];
    //        [IOSAlbumCameraController saveImageToPhotosAlbum:temp];
    //    }
    void _iosSaveImageToPhotosAlbum(char* readAddr)
    {
        NSString* temp = [NSString stringWithUTF8String:readAddr];
        [IOSAlbumCameraController saveImageToPhotosAlbum:temp];
    }
    
    
  
    
#if defined (__cplusplus)
}
#endif
//------------- called by unity -----end-----------------
