#import "WXSdk.h"  
#import "WXApi.h"
#import "WechatAuthSDK.h"

#define WeiXinID @"wx001cf6e637ffbe12"

@implementation WXSdk

+ (void) onResp:(BaseResp *)resp{
  /*
     enum  WXErrCode {
     WXSuccess           = 0,    成功
     WXErrCodeCommon     = -1,  普通错误类型
     WXErrCodeUserCancel = -2,    用户点击取消并返回
     WXErrCodeSentFail   = -3,   发送失败
     WXErrCodeAuthDeny   = -4,    授权失败
     WXErrCodeUnsupport  = -5,   微信不支持
     };
     */
   
	UnitySendMessage("[Login]", "OnPrintLog", "微信onResp");
	if([resp isKindOfClass:[SendAuthResp class]]) // 登录授权  
    {
		UnitySendMessage("[Login]", "OnPrintLog", "微信登录返回");
		if (resp.errCode==0) {
			UnitySendMessage("[Login]", "OnPrintLog", "微信登录成功");
			// 返回成功，获取Code
			SendAuthResp *sendResp =(SendAuthResp *) resp;
			NSString *code = sendResp.code;
			NSLog(@"code=%@",sendResp.code);
			UnitySendMessage("[Login]","OnWxAuthSucceed",[code UTF8String]);
		}else{
			UnitySendMessage("[Login]", "OnPrintLog", "微信登录失败");
			//NSLog(@"微信登录失败！ %d",resp.errCode);
			UnitySendMessage("[Login]","OnWxAuthFail","");
		}
    }  
    else if([resp isKindOfClass:[SendMessageToWXResp class]])
    {
        // 分享
        if(resp.errCode==0)
        {
            UnitySendMessage("[Login]","OnWxShareSucceed","分享成功");
        }
        else if(resp.errCode==-2) {
            UnitySendMessage("[Login]","OnWxShareCancel","分享取消");
        }
        else if(resp.errCode == -4){
            UnitySendMessage("[Login]","OnWxShareCancel","分享失败");
        }
    }
	else if([resp isKindOfClass:[PayResp class]]){
        PayResp *response = (PayResp*)resp;
        switch(response.errCode){
            case WXSuccess:
                //服务器端查询支付通知或查询API返回的结果再提示成功
                NSLog(@"\n\n\n\n\n支付成功\n\n\n\n\n\n");
                UnitySendMessage("[Login]", "WxPayIosCallBack","0");
                //发送通知给带有微信支付功能的视图控制器，告诉他支付成功了，请求后台订单状态，如果后台返回的订单也是成功的状态，那么可以进行下一步操作
                [[NSNotificationCenter defaultCenter] postNotificationName:@"WeiXinPaysucceed" object:nil userInfo:nil];
                break;
            default:
                /*
                 resp.errCode = 2 用户取消支付
                 resp.errCode = -1 错误
                 */
                NSLog(@"支付失败，retcode=%d ---- %@",resp.errCode,resp.errStr);
                break;
                
        }
        
    }

}



- (bool)isWXAppInstalled
{
    NSLog(@"isWXAppInstalled");
	bool result = [WXApi isWXAppInstalled];
    return result;
}  

- (bool)isWXAppSupportApi
{  
   return [WXApi isWXAppSupportApi];  
}  

- (void)registerWX
{
	//向微信注册  
	[WXApi registerApp:WeiXinID]; 
}

- (void)sendAuth
{
	UnitySendMessage("[Login]", "OnPrintLog", "sendAuth");
	//构造SendAuthReq结构体
	SendAuthReq* req =[[SendAuthReq alloc] init ];
	req.scope = @"snsapi_userinfo";
	req.state = @"wechat_sdk_auth";
	//第三方向微信终端发送一个SendAuthReq消息结构
	[WXApi sendReq:req];
}
-(void)shareToWechat:(NSDictionary *)dict {
    NSString *action = [dict objectForKey:@"action"];//unity 逻辑处理的要操作方法名
    //    int shareType = [[dict objectForKey:@"shareType"] intValue];
    int mode = [[dict objectForKey:@"mode"] intValue];
    NSString *url = [dict objectForKey:@"url"];
    NSString *title = [dict objectForKey:@"title"];
    NSString *desc = [dict objectForKey:@"description"];
    NSString *bmpUrl = [dict objectForKey:@"bmpUrl"];
    NSString *test =[NSString stringWithFormat:@"IOS:shareToWechat() action=%@,mode=%d,url=%@,title=%@,desc=%@,bmpUrl=%@",action,mode,url,title,desc,bmpUrl];
    UnitySendMessage( "[Login]", "OnPrintLog", [test UTF8String]);
    
    
    if ([action isEqualToString:@"shareMessage"]) {
         NSLog(@"shareMessage  in!!!!!!");
        [self shareMessage:desc sed:mode];
        
    }else if ([action isEqualToString:@"shareImage"]){
             UnitySendMessage( "[Login]", "OnPrintLog", "shareImage!!!");
        [self shareImage:url sed:mode];
    }
    
    else if ([action isEqualToString:@"shareMusic"]){
        [self shareMusic:title sed:desc third:url four:mode];
    }
    else if ([action isEqualToString:@"shareVideo"]){
        [self shareVideo:title sed:desc third:url four:mode];
    }
    else if ([action isEqualToString:@"shareWebpage"]){
        [self shareWebpage:title sed:desc third:url four:mode];
    }
}
-(void)shareMessage:(NSString *)desc sed:(int)scene
{
    NSLog(@"shareMessage()");
	SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
	req.text = desc;
	req.bText = YES;
	req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
	[WXApi sendReq:req];  
}
-(void)shareImage:(NSString *)imgPath sed:(int)scene
{
    UnitySendMessage( "[Login]", "OnPrintLog", "shareImage()");
	WXMediaMessage *message = [WXMediaMessage message];
    UIImage *image =[UIImage imageWithContentsOfFile:imgPath];
    UIImage *thumbImage = [self compressImage:image toByte:32768];//32 字节图片
	[message setThumbImage:thumbImage];
	WXImageObject *imageObject = [WXImageObject object];
    imageObject.imageData = [NSData dataWithContentsOfFile:imgPath];
	message.mediaObject = imageObject;
	SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;  
    req.message = message;  
    req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
    [WXApi sendReq:req];  
}
-(UIImage *)compressImage:(UIImage *)image toByte:(NSUInteger)maxLength {
    // Compress by quality
    CGFloat compression = 1;
    NSData *data = UIImageJPEGRepresentation(image, compression);
    if (data.length < maxLength) return image;
    
    CGFloat max = 1;
    CGFloat min = 0;
    for (int i = 0; i < 6; ++i) {
        compression = (max + min) / 2;
        data = UIImageJPEGRepresentation(image, compression);
        if (data.length < maxLength * 0.9) {
            min = compression;
        } else if (data.length > maxLength) {
            max = compression;
        } else {
            break;
        }
    }
    UIImage *resultImage = [UIImage imageWithData:data];
    if (data.length < maxLength) return resultImage;
    
    // Compress by size
    NSUInteger lastDataLength = 0;
    while (data.length > maxLength && data.length != lastDataLength) {
        lastDataLength = data.length;
        CGFloat ratio = (CGFloat)maxLength / data.length;
        CGSize size = CGSizeMake((NSUInteger)(resultImage.size.width * sqrtf(ratio)),
                                 (NSUInteger)(resultImage.size.height * sqrtf(ratio))); // Use NSUInteger to prevent white blank
        UIGraphicsBeginImageContext(size);
        [resultImage drawInRect:CGRectMake(0, 0, size.width, size.height)];
        resultImage = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
        data = UIImageJPEGRepresentation(resultImage, compression);
    }
    
    return resultImage;
}

-(void)shareWebpage:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene
{  
      //  NSString *titleStr=[NSString stringWithUTF8String:title];  
     //   NSString *descStr=[NSString stringWithUTF8String:desc];
     //   NSString *urlStr=[NSString stringWithUTF8String:url];  
        NSLog(@"shareWebpage titleStr:%@",title);  
                NSLog(@"shareWebpage descStr:%@",desc);
                NSLog(@"shareWebpage urlStr:%@",url);

        // 分享  
        WXMediaMessage *message = [WXMediaMessage message];  
        message.title = title;
        message.description = desc;  
        [message setThumbImage:[UIImage imageNamed:@"AppIcon72x72"]];  
          
        WXWebpageObject *ext = [WXWebpageObject object];  
        ext.webpageUrl = url;
          
        message.mediaObject = ext;  
          
        SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;  
        req.message = message;
    
        req.scene = scene;//[NSNumber numberWithInt:scene];  //0 = 好友列表 1 = 朋友圈 2 = 收藏
        [WXApi sendReq:req];  
}  
-(void)shareMusic:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene
{  
    //获取沙盒路径
    NSString *path = [NSHomeDirectory() stringByAppendingPathComponent:url];
    
    WXMediaMessage *message = [WXMediaMessage message];
    message.title=title;
    message.description =desc;
    
    
    [message setThumbImage:[UIImage imageWithContentsOfFile:path]];

    WXMusicObject *ext =[WXMusicObject object];
    ext.musicUrl = url;//音乐url
    ext.musicLowBandUrl =ext.musicUrl;
    ext.musicDataUrl =@"音乐数据url";
    ext.musicLowBandDataUrl =ext.musicDataUrl;
    message.mediaObject =ext;
    
    
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
    [WXApi sendReq:req];
    [WXApi sendReq:req];
}  
-(void)shareVideo:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene
{  
    //获取沙盒路径
    NSString *path = [NSHomeDirectory() stringByAppendingPathComponent:url];
    
    WXMediaMessage *message = [WXMediaMessage message];
    message.title=title;
    message.description =desc;
    
    [message setThumbImage:[UIImage imageWithContentsOfFile:path]];
    

    WXVideoObject *videoObject = [WXVideoObject object];
    videoObject.videoUrl =url;
    videoObject.videoLowBandUrl =videoObject.videoUrl;
    
    
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
    
    [WXApi sendReq:req];
}  
/*void shareMessage(const char* desc,int scene){
	NSString *descStr=[NSString stringWithUTF8String:desc];
	SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
	req.text = descStr;
	req.bText = YES;
	req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
	[WXApi sendReq:req];  
}*/

/*void shareImage(const char* imgpath,int scene){
	//获取沙盒路径  
    NSString *path = [NSHomeDirectory() stringByAppendingPathComponent:imgpath];  
	
	WXMediaMessage *message = [WXMediaMessage message];
	[message setThumbImage:[UIImage imageWithContentsOfFile:path]];
	WXImageObject *imageObject = [WXImageObject object];
	NSString *resourcePath = [[NSBundle mainBundle] path];
	imageObject.imageData = [NSData dataWithContentsOfURL:resourcePath];
	message.mediaObject = imageObject;
	
	SendMessageToWXReq* req = [[[SendMessageToWXReq alloc] init]autorelease];  
    req.bText = NO;  
    req.message = message;  
    req.scene = scene;  //0 = 好友列表 1 = 朋友圈 2 = 收藏
    [WXApi sendReq:req];  
}*/

	/*
void shareWebpage(const char* title,const char* desc,const char* url,int scene)  
{  
        NSString *titleStr=[NSString stringWithUTF8String:title];  
        NSString *descStr=[NSString stringWithUTF8String:desc];
        NSString *urlStr=[NSString stringWithUTF8String:url];  
        NSLog(@"shareWebpage titleStr:%@",titleStr);  
                NSLog(@"shareWebpage descStr:%@",descStr);  
                NSLog(@"shareWebpage urlStr:%@",urlStr);  

        // 分享  
        WXMediaMessage *message = [WXMediaMessage message];  
        message.title = titleStr;  
        message.description = descStr;  
        [message setThumbImage:[UIImage imageNamed:@"AppIcon72x72"]];  
          
        WXWebpageObject *ext = [WXWebpageObject object];  
        ext.webpageUrl = urlStr;
          
        message.mediaObject = ext;  
          
        SendMessageToWXReq* req = [[[SendMessageToWXReq alloc] init]autorelease];  
        req.bText = NO;  
        req.message = message;
    
        req.scene = scene;//[NSNumber numberWithInt:scene];  //0 = 好友列表 1 = 朋友圈 2 = 收藏
        [WXApi sendReq:req];  
}  */
-(void)wechatPay:(NSDictionary *)dict{
    if([WXApi isWXAppInstalled] == NO){
        //        LuaBridge::pushLuaFunctionById(callback);
        //        LuaStack *stack = LuaBridge::getStack();
        //
        //        LuaValueDict item;
        //        item["errCode"] = LuaValue::intValue(1234);
        //        stack->pushLuaValueDict (item);
        //        stack->executeFunction (1);
        //        LuaBridge::releaseLuaFunctionById (callback);
        //        飘字提示
        
        return;
    }
    [WXApi registerApp:[dict objectForKey:@"appid"] enableMTA:NO];//应用id
    
    //创建支付签名对象
    PayReq *request = [[PayReq alloc] init];
    request.partnerId = [dict objectForKey:@"partnerid"];//商户号
    request.prepayId= [dict objectForKey:@"prepayid"];//预支付交互会话id
    request.package = [dict objectForKey:@"package"];// 扩展字段
    request.nonceStr= [dict objectForKey:@"noncestr"];//随机字符串
    request.timeStamp= [[dict objectForKey:@"timestamp"] intValue];//时间挫
   // request.openID = [dict objectForKey:@"appid"];
    
    request.sign= [dict objectForKey:@"sign"];
    [WXApi sendReq:request];
    
}

@end

