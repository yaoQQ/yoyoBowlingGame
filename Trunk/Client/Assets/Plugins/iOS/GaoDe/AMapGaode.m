#import "AMapGaode.h"

  
#define AMapGaodeID @"e0cb439f085e7d5e951c6d0782c1e632"
#define DefaultLocationTimeout 10
#define DefaultReGeocodeTimeout 5
@interface AMapGaode()<AMapLocationManagerDelegate>

/**
 *  持续定位是否返回逆地理信息，默认NO。
 */
@property (nonatomic, assign) BOOL locatingWithReGeocode;

@end  
  
@implementation AMapGaode
- (void)startActive  
{

    //初始化AMapLocationManager对象，设置代理
    [AMapServices sharedServices].apiKey=AMapGaodeID;
    self.locationManager = [[AMapLocationManager alloc] init];
    [self.locationManager setDelegate:self];
	//设置不允许系统暂停定位
    [self.locationManager setPausesLocationUpdatesAutomatically:NO];
    //设置定位最小更新距离
	[self.locationManager setDistanceFilter:200];
    //设置允许在后台定位
    //[self.locationManager setAllowsBackgroundLocationUpdates:YES];
	
	
	
    // 带逆地理信息的一次定位（返回坐标和地址信息  高精度）  
    //[self.locationManager setDesiredAccuracy:kCLLocationAccuracyBest];  
    //设置定位超时时间
    [self.locationManager setLocationTimeout:DefaultLocationTimeout];
    
    //设置逆地理超时时间
    [self.locationManager setReGeocodeTimeout:DefaultReGeocodeTimeout]; 
	
	UnitySendMessage("[Login]", "OnPrintLog", "IOS:AMapGaode sdk_Init:startActive");
}

- (void)locateAction  
{
	[self.locationManager setLocatingWithReGeocode:YES];
	//开始进行连续定位
	[self.locationManager startUpdatingLocation];
	UnitySendMessage("[Login]", "OnPrintLog", "IOS:startUpdatingLocation()");
}

- (void)amapLocationManager:(AMapLocationManager *)manager didFailWithError:(NSError *)error
{
	UnitySendMessage("[Login]", "OnPrintLog", "IOS:定位错误");
    NSLog(@"%s, amapLocationManager = %@, error = %@", __func__, [manager class], error);
}

- (void)amapLocationManager:(AMapLocationManager *)manager didUpdateLocation:(CLLocation *)location reGeocode:(AMapLocationReGeocode *)reGeocode
{
	UnitySendMessage("[Login]", "OnPrintLog", "IOS:定位成功");
    NSLog(@"location:{lat:%f; lon:%f; accuracy:%f; reGeocode:%@}", location.coordinate.latitude, location.coordinate.longitude, location.horizontalAccuracy, reGeocode.formattedAddress);
    
	if (reGeocode)
	{
		NSString *lat;
		lat = [NSString stringWithFormat:@"1|%f|%f|%@|%@|%@|%@|%@", location.coordinate.longitude, location.coordinate.latitude, reGeocode.citycode, reGeocode.country, reGeocode.province, reGeocode.city, reGeocode.district];  
		  
		UnitySendMessage("[Login]", "OnLocation", [lat UTF8String]);
	}
}

- (void)reGeocodeAction
{
    //进行单次带逆地理定位请求
   // [self.locationManager requestLocationWithReGeocode:YES completionBlock:self.completionBlock];
}

- (void)locAction
{
    //进行单次定位请求
    //[self.locationManager requestLocationWithReGeocode:NO completionBlock:self.completionBlock];
}


	//打开外部高德地图App导航
-(void)openGaodeMapApp:(float)fromLng sed:(float)fromLat third:(NSString *)fromName four:(float)toLng five:(float)toLat six:(NSString *)toName {
         NSLog(@"IOS::test begain  !!!!!!!openGaodeMapApp ");
		NSString *app =[NSString stringWithFormat:@"%@",@"iosamap://"];
        BOOL canOpen =[self isCanInstall:app];
		//   lat = 23.13456
    //    lng = 113.3281
    
		 UnitySendMessage("[Login]", "OnPrintLog", [app UTF8String]);
		// m 驾车：0：速度最快，1：费用最少，2：距离最短，3：不走高速，4：躲避拥堵，5：不走高速且避免收费，6：不走高速且躲避拥堵，7：躲避收费和拥堵，8：不走高速躲避收费和拥堵 公交：0：最快捷，2：最少换乘，3：最少步行，5：不乘地铁 ，7：只坐地铁 ，8：时间短 是
		// t = 0：驾车 =1：公交 =2：步行
		NSString *url = [[NSString stringWithFormat:
		@"iosamap://path?sourceApplication=applicationName&sid=BGVIS1&slat=%f&slon=%f&sname=%@&did=BGVIS2&dlat=%f&dlon=%f&dname=%@&dev=0&m=0&t=0",
		fromLat, fromLng,fromName, toLat,toLng,toName] stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    NSLog(@"IOS::test begain  !!!!!!!openGaodeMapApp canOpen=%d   url=%@",canOpen,url);
	if (canOpen)
	{
        NSLog(@"IOS::test begain canOpen()");
		UnitySendMessage("[Login]", "OnPrintLog", "IOS:OnOpenGaodeMapApp()  success");
		UnitySendMessage("[Login]", "OnOpenGaodeMapApp", "1");
		[[UIApplication sharedApplication] openURL:[NSURL URLWithString:url]];
	}

	else
	{
         NSLog(@"IOS::没有安装高德地图客户端");
		UnitySendMessage("[Login]", "OnOpenGaodeMapApp", "0|没有安装高德地图客户端");
		UnitySendMessage("[Login]", "OnPrintLog", "IOS:没有安装高德地图客户端");

	}
}

	//判断是否安装目标应用
-(BOOL)isCanInstall:(NSString *)packageName
    {
		NSURL *scheme = [NSURL URLWithString:packageName]; 
		BOOL canOpen = [[UIApplication sharedApplication] canOpenURL:scheme];
		UnitySendMessage("[Login]", "OnPrintLog", "IOS:isCanInstall()");
		return canOpen;
	}
    

@end
