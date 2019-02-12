#import <Foundation/Foundation.h>
#import <AlipaySDK/AlipaySDK.h>
@interface AliPayPlatform:NSObject

-(void)sendAuth:(NSString *)infoStr;
-(void)aliPay:(NSString *)orderInfo;

@end
