
#import "DogeUtils.h"

@implementation DogeUtils

+ (id)jsonDecode:(NSString *)str
{
    NSData *data = [str dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error = nil;
    id jsonObject = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingAllowFragments error:&error];
    
    if (error) {
        NSException *e = [NSException exceptionWithName:@"Doge JSONParser Error" reason:error.localizedDescription userInfo:nil];
        [e raise];
    }
    
    return jsonObject;
}

+ (NSString *)jsonEncode:(id)object
{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:NSJSONWritingPrettyPrinted error:&error];
    
    if (error) {
        NSException *e = [NSException exceptionWithName:@"Doge JSONParser Error" reason:error.localizedDescription userInfo:nil];
        [e raise];
    }
    
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}


@end
