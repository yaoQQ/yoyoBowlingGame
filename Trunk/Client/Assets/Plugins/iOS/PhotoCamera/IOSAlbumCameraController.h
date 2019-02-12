//
//  IOSAlbumCameraController.h
//  Unity-iPhone
//
//  Created by AnYuanLzh
//
//
#import <Foundation/Foundation.h> 
@interface IOSAlbumCameraController : UIViewController<UIImagePickerControllerDelegate,UINavigationControllerDelegate>
@property(nonatomic) int cutWidth;
@property(nonatomic) int cutHeight;
- (void)showActionSheet;
- (void)showActionButton;
-(void)showPicker:(UIImagePickerControllerSourceType)type  allowsEditing:(BOOL)flag;
-(void)iosOpenPhotoLibrary;
@end
