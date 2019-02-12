#import <Foundation/Foundation.h>
#import <AMapFoundationKit/AMapFoundationKit.h>
#import <AMapLocationKit/AMapLocationKit.h>
  
@interface AMapGaode : NSObject<AMapLocationManagerDelegate>


@property (nonatomic, strong) AMapLocationManager *locationManager;
-(void)startActive;
-(void)locateAction;
-(void)openGaodeMapApp:(float)fromLng sed:(float)fromLat third:(NSString *)fromName four:(float)toLng five:(float)toLat six:(NSString *)toName;

@end  
