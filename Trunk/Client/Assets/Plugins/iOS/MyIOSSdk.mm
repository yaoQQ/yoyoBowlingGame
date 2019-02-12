#import "MyIOSSdk.h"
#import "WXSdk.h" 
#import "AMapGaode.h"
#import "DogeUtils.h"
#import "IOSAlbumCameraController.h"
#import "AliPayPlatform.h"
#include "UnityViewControllerBaseiOS.h"

@implementation MyIOSSdk

WXSdk *wxSdk = nil;
AMapGaode *iapAmap = nil;
IOSAlbumCameraController *photo = nil;
AliPayPlatform *aliPayPlatform = nil;

#if defined (__cplusplus)
extern "C" {
#endif
/**
 Unity调用iOS示例
 需提供方法名,返回值根据业务协定,可为void,参数可定,建议直接传递json字符串
 */
char* MakeStringCopy (const char* str) {
    if (str == NULL)
        return NULL;

    char* res = (char*)malloc(strlen(str) + 1);
    strcpy(res, str);
    return res;
}


 void sdk_Init()  
{
	UnitySendMessage("[Login]", "OnPrintLog", "sdk_Init");
    
	//微信初始化
	wxSdk = [[WXSdk alloc] init];
	[wxSdk registerWX];
	
	//定位初始化
    iapAmap = [[AMapGaode alloc] init];
    [iapAmap startActive];
    
    //定位初始化
    photo = [[IOSAlbumCameraController alloc] init];
    photo.cutWidth =256;
    photo.cutHeight =256;
	
	aliPayPlatform = [[AliPayPlatform alloc] init];
    
}  

void activeInitializeUI()
{
	UIWindow* window = [[UIApplication sharedApplication].delegate window];
	if (window != nil && window.rootViewController != nil)
	{
		[window makeKeyAndVisible];
	}
}

bool is_wx_app_installed()
{
	return [wxSdk isWXAppInstalled];  
} 

 void wx_send_auth()
{
	UnitySendMessage("[Login]", "OnPrintLog", "wx_send_auth");
	
    //微信登录
	[wxSdk sendAuth];  
} 

//微信支付
 void wx_pay(char* appid,char* partnerid,char *prepayid,char *noncster,char *time,char *sign,char *messageName)

{    

        PayReq *request = [[PayReq alloc]init];

        request.openID =[NSString stringWithUTF8String:appid];

        //商家id

        request.partnerId = [NSString stringWithUTF8String:partnerid];

        //订单id

        request.prepayId = [NSString stringWithUTF8String:prepayid];

        //扩展字段(官方文档:暂时填写固定值)

        request.package = @"Sign=WXPay";

        //随机字符串

        request.nonceStr = [NSString stringWithUTF8String:noncster];

        //时间戳

        //request.timeStamp = (UInt32)[[NSDate date] timeIntervalSince1970];

        NSMutableString *stampmp =[[NSString stringWithUTF8String:time] mutableCopy];

		request.timeStamp = stampmp.intValue;

		request.sign =[NSString stringWithUTF8String:sign];

        //带起微信支付

        if ([WXApi sendReq:request]) {

        }else{

            //未安装微信客户端

            [[[UIAlertView alloc]initWithTitle:@"测试demo" message:@"您还未安装微信客户端,请前往Appstore下载或者选择其他支付方式!" delegate:nil cancelButtonTitle:@"知道了" otherButtonTitles:nil, nil]show];    
        }    

    }

 void start_Location()  
{
	UnitySendMessage("[Login]", "OnPrintLog", "start_Location");
	
    //获取定位
	[iapAmap locateAction];  
} 

    
    // 打开相册--可编辑
    void _iosOpenPhotoAlbums_allowsEditing(const char* param)
    {
       // NSLog([NSString stringWithFormat:@"json=%@",[NSString stringWithUTF8String:param]]);
        NSString *json = [NSString stringWithUTF8String:param];
        UnitySendMessage( "[Login]", "OnPrintLog", [json UTF8String]);
        NSDictionary *dict = [DogeUtils jsonDecode:json];
        bool action = [[dict objectForKey:@"allowEditong"] boolValue];
        photo.cutWidth = [[dict objectForKey:@"width"] intValue];
        photo.cutWidth = [[dict objectForKey:@"height"] intValue];
        UnitySendMessage( "[Login]", "OnPrintLog", [[NSString stringWithFormat:@"cutWidth=%d cutHeight=%d",photo.cutWidth,photo.cutWidth] UTF8String]);
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeSavedPhotosAlbum])
        {
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: photo.view];
            
            [photo showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:action];
        }
        else
        {
            [photo iosOpenPhotoLibrary];
        }
        
    }
    
    // 打开相机--可编辑
    void _iosOpenCamera_allowsEditing(const char* param)
    {
        NSString *json = [NSString stringWithUTF8String:param];
        UnitySendMessage( "[Login]", "OnPrintLog", [json UTF8String]);
        NSDictionary *dict = [DogeUtils jsonDecode:json];
        bool action = [[dict objectForKey:@"allowEditong"] boolValue];
        photo.cutWidth = [[dict objectForKey:@"width"] intValue];
        photo.cutHeight = [[dict objectForKey:@"height"] intValue];
        UnitySendMessage( "[Login]", "OnPrintLog", [[NSString stringWithFormat:@"cutWidth=%d cutHeight=%d",photo.cutWidth,photo.cutWidth] UTF8String]);
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera])
        {
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: photo.view];
            
            [photo showPicker:UIImagePickerControllerSourceTypeCamera allowsEditing:action];
        }
        else
        {
            UnitySendMessage("[Login]", "OnPrintLog", "不支持照相机");
        }
    }

//打开高德app
void openGaodeMapApp(const char *pid) { 
	UnitySendMessage("[Login]", "OnPrintLog", "openGaodeMapApp()");
	NSString *json = [NSString stringWithUTF8String:pid];
	NSDictionary *dict = [DogeUtils jsonDecode:json];
	float fromLng = [[dict objectForKey:@"fromLng"] floatValue];
   float fromLat = [[dict objectForKey:@"fromLat"] floatValue];
   NSString *fromName = [dict objectForKey:@"fromName"];
   	float toLng = [[dict objectForKey:@"toLng"] floatValue];
   float toLat = [[dict objectForKey:@"toLat"] floatValue];
   NSString *toName = [dict objectForKey:@"toName"];

   [iapAmap openGaodeMapApp:fromLng sed:fromLat third:fromName four:toLng five:toLat six:toName];
}

//支付宝授权
void alipay_send_auth(char* authInfo) {
	UnitySendMessage("[Login]", "OnPrintLog", "支付宝授权");
	[aliPayPlatform sendAuth:[NSString stringWithUTF8String:authInfo]];  
}

//支付宝支付
void alipay_pay(const char *pid){
	[aliPayPlatform aliPay:[NSString stringWithUTF8String:pid]];
}

void _showStatusBar(const char *pid,const char *isWhiteStr){
    bool isShow = [[NSString stringWithUTF8String:pid] boolValue];
     bool isWhite = [[NSString stringWithUTF8String:isWhiteStr] boolValue];
    NSString *test =[NSString stringWithFormat:@"_showStatusBar isShow:%b isWhite:%b",isShow,isWhite];
    UnitySendMessage( "[Login]", "OnPrintLog", [test UTF8String]);
    
    UIViewController *vc = UnityGetGLViewController();
    UnityViewControllerBase *te = (UnityViewControllerBase*)vc;
    [te setStatusBarStyleValue:isWhite];
    [te setPrefersStatusBarHidden:!isShow];
  
    //[vc prefersStatusBarHidden];
   // [[UIApplication sharedApplication] setStatusBarHidden:isShow];
    [vc setNeedsStatusBarAppearanceUpdate];

}
void _showStatusBarColor(const char *isWhiteStr){
     bool isWhite = [[NSString stringWithUTF8String:isWhiteStr] boolValue];
    NSString *test =[NSString stringWithFormat:@"_showStatusBarColor  isWhite:%b",isWhite];
    UnitySendMessage( "[Login]", "OnPrintLog", [test UTF8String]);
    
    UIViewController *vc = UnityGetGLViewController();
    UnityViewControllerBase *te = (UnityViewControllerBase*)vc;
    [te setStatusBarStyleValue:isWhite];

    //[vc prefersStatusBarHidden];
   // [[UIApplication sharedApplication] setStatusBarHidden:isShow];
    [vc setNeedsStatusBarAppearanceUpdate];

}

void call_native(const char *pid) {
	NSString *json = [NSString stringWithUTF8String:pid];
	NSDictionary *dict = [DogeUtils jsonDecode:json];
    NSString *action = [dict objectForKey:@"IOSMethod"];
    NSString *code = [NSString stringWithFormat:@"call_native json:%@,methodName=%@",json,action];
    NSLog(@"shareMessage json=%@,methodName=%@",json,action);
	UnitySendMessage("[Login]", "OnPrintLog", MakeStringCopy([code UTF8String]));
	  


    
    UnitySendMessage("[Login]", "OnPrintLog", MakeStringCopy([action UTF8String]));
    //NSDictionary *data = [dict objectForKey:@"data"];
    if ([action isEqualToString:@"shareToWechat"]) {
        [wxSdk shareToWechat:dict];
		
    }/*else if ([action isEqualToString:@"wechatShare"]){
        [self shareToWechat:data];
    }else if ([action isEqualToString:@"shareImage"]){

    }*/
//    默认返回空字符串
	// NSString *json = [NSString stringWithUTF8String:pid];
     //   [p callNative:json];
}

//数据直接返回到unity
const char* get_native_value(const char *pid) {
		UnitySendMessage("[Login]", "OnPrintLog", "get_native_value");
		 NSString *json = [NSString stringWithUTF8String:pid];
        return MakeStringCopy([json UTF8String]);
}
#if defined (__cplusplus)
}
#endif
@end
