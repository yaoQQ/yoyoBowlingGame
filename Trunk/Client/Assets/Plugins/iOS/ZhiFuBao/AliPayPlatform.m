#import "AliPayPlatform.h"  
#import <AlipaySDK/AlipaySDK.h>

#define AliPaySDKID @"zhifu002cf6e637ffbe33"

//IMPL_APP_CONTROLLER_SUBCLASS (AliPayPlatform)

@implementation AliPayPlatform

- (void)sendAuth:(NSString *)authInfoStr
{
	UnitySendMessage("[Login]", "OnPrintLog", "alipaySendAuth");
	
	//应用注册scheme,在AlixPayDemo-Info.plist定义URL types
    NSString *appScheme = AliPaySDKID;
	
	[[AlipaySDK defaultService] auth_V2WithInfo:authInfoStr fromScheme:appScheme callback:^(NSDictionary *resultDic) {
		UnitySendMessage("[Login]", "OnPrintLog", "auth_V2WithInfo reslut");
		
		NSString *resultStatus = resultDic[@"resultStatus"];
        if ([resultStatus isEqualToString:@"9000"]) {
			NSString *result = resultDic[@"result"];
			UnitySendMessage("[Login]", "OnAlipayAuthSucceed", [result UTF8String]);
        } else {
			UnitySendMessage("[Login]", "OnAlipayAuthFail", [resultStatus UTF8String]);
        }
	}];
}

-(void)aliPay:(NSString *)orderInfo
{
    if (orderInfo != nil) {
        //应用注册scheme,在AliSDKDemo-Info.plist定义URL types
        NSString *appScheme = AliPaySDKID;
 /**
 *  支付接口
 *
 *  @param orderStr       订单信息
 *  @param schemeStr      调用支付的app注册在info.plist中的scheme
 *  @param completionBlock 支付结果回调Block，用于wap支付结果回调（非跳转钱包支付）
 */
        [[AlipaySDK defaultService] payOrder:orderInfo fromScheme:appScheme callback:^(NSDictionary *resultDic) {
            NSLog(@"reslut = %@",resultDic);
                NSString *memo = resultDic[@"memo"];
				NSString *result =resultDic[@"result"];
				NSString *resultStatus =resultDic[@"resultStatus"];
                NSLog(@"IOS Apliy:===memo:%@", memo);
				 NSLog(@"IOS Apliy:===memo:%@", result);
				 	 NSLog(@"IOS resultStatus:===memo:%@", result);
			UnitySendMessage("[Login]", "OnAliPay", [resultStatus UTF8String]);
            
        }];
    }
}

/*
9000	订单支付成功
8000	正在处理中，支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态
4000	订单支付失败
5000	重复请求
6001	用户中途取消
6002	网络连接出错
6004	支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态
其它	其它支付错误

*/

@end
