#import <Foundation/Foundation.h>
#import "WXApi.h"
  
@interface WXSdk : NSObject

+ (void) onResp:(BaseResp *)resp;

- (bool)isWXAppInstalled;
- (bool)isWXAppSupportApi;
- (void)registerWX;
- (void)sendAuth;
- (void)sendAuth;

-(void)shareToWechat:(NSDictionary *)dict;
-(void)shareMessage:(NSString *)desc sed:(int)scene;
-(void)shareImage:(NSString *)imgpath sed:(int)scene;
-(void)shareWebpage:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene;
-(void)shareMusic:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene;
-(void)shareVideo:(NSString *)title sed:(NSString *)desc third:(NSString *)url four:(int)scene;
-(void)wechatPay:(NSDictionary *)dict;


@end
