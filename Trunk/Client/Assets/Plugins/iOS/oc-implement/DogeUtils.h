
#import <Foundation/Foundation.h>

@interface DogeUtils : NSObject

/**
 *  通过 JSON object 初始化字符串
 *
 *  @param object JSON 对象，NSDictionary 或者 NSArray，子项只支持简单对象
 *
 *  @return 序列化后的 json string，转换失败返回 nil
 */
+ (NSString *)jsonEncode:(id)object;

+ (id)jsonDecode:(NSString *)str;

@end
