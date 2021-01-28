//
//  Converters.h
//  HHCloudKit
//
//  Created by Jonathan on 1/22/20.
//  Copyright © 2020 Unity Technologies. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface GCControllerConverters : NSObject
+(NSArray*) BridgedArray: (void*_Nonnull*_Nullable) arr withCount:(long) count;
+(NSArray*) StringArray: (const char**)arr withCount:(long) count;
+(void) NSArrayToRetainedCArray: (NSArray*) array withBuffer:(void* _Nonnull[_Nonnull]) buffer;
+(const void*_Nullable*_Nullable) NSArrayToUnretainedCArray: (NSArray*) array;
+(const void*_Nullable*_Nullable) NSArrayToRetainedCArray: (NSArray*) array;
+(const void*_Nullable*_Nullable) NSSetToRetainedCArray:(NSSet<id>*) set;
+(const char**) NSArrayOfStringsToCArrayOfStringPointers: (NSArray<NSString*>*) array;
+(void**) NSDictionaryToValuesArray: (NSDictionary*) dictionary;
+(void**) NSDictionaryToKeysArray: (NSDictionary*) dictionary;
+(const long*) ToTypeIdArray:(NSArray<id>*) array;
+(long) GetTypeId:(id) item;
@end

NS_ASSUME_NONNULL_END
