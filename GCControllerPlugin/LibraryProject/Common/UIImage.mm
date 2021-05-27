//
//  UIImage.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "UIImage.h"
#import "GCControllerConverters.h"
#if TARGET_OS_IOS || TARGET_OS_TV
#import <UIKit/UIKit.h>
#else
#import <AppKit/AppKit.h>
#endif


extern "C" {

//ClassMethods
void* UIImage_systemImageNamed(
	const char* name, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            UIImage* val = [UIImage systemImageNamed:[NSString stringWithUTF8String:name]];
            // Tint it white because black is useless in Unity
            val = [val imageWithTintColor:[UIColor colorWithRed:1 green:1 blue:1 alpha:1]];
    #else
            NSImage* val = [NSImage imageWithSystemSymbolName:[NSString stringWithUTF8String:name] accessibilityDescription:@""];
    #endif
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}


void* UIImage_systemImageNamed_compatibleWithTraitCollection(
	const char* name, 
	void* traitCollection, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImage systemImageNamed:[NSString stringWithUTF8String:name] compatibleWithTraitCollection:(__bridge UITraitCollection*) traitCollection];
            
            // Tint it white because black is useless in Unity
            val = [val imageWithTintColor:[UIColor colorWithRed:1 green:1 blue:1 alpha:1]];
            
            return (__bridge_retained void*) val;
    #else
            NSLog(@"UIImage_systemImageNamed_campatibleWithTraitCollection is not implemented");
            return nil;
    #endif
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}

void* UIImage_systemImageNamed_withConfiguration(
	const char* name, 
	void* configuration, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImage systemImageNamed:[NSString stringWithUTF8String:name] withConfiguration:(__bridge UIImageConfiguration*) configuration];
            
            // Tint it white because black is useless in Unity
            val = [val imageWithTintColor:[UIColor colorWithRed:1 green:1 blue:1 alpha:1]];
    #else
            NSImage* val = [NSImage imageWithSystemSymbolName:[NSString stringWithUTF8String:name] accessibilityDescription:nil];
            NSImageSymbolConfiguration* config = (__bridge NSImageSymbolConfiguration*)configuration;
            val = [val imageWithSymbolConfiguration:config];
    #endif
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}

void* UIImage_PNGRepresentation_GetData(
    const void* image,
    long* const bufferLen,
	void** exception
    )
{
	@try {
#if TARGET_OS_IOS || TARGET_OS_TV
        id uiImage = (__bridge UIImage*) image;
	    NSData* val = UIImagePNGRepresentation(uiImage);
#else
        NSImage* nsImage = (__bridge NSImage*)image;
        
        // Tint it white because black is useless in Unity
        NSColor* glyph_color = [NSColor colorWithRed:1 green:1 blue:1 alpha:1];
        [nsImage lockFocus];
        [glyph_color set];
        NSRect rect = {NSZeroPoint, [nsImage size]};
        
        // If we're on BigSur, some genius at apple broke the tinting operation,
        // and the Compositing source copy can't be used to tint
        if(@available(macOS 10.16, *))
        {
            NSRectFillUsingOperation(rect, NSCompositingOperationSourceAtop );
        }
        else
        {
            NSRectFillUsingOperation(rect, NSCompositingOperationCopy);
        }
        [nsImage unlockFocus];
        
        // rasterize this
        CGImageRef cgRef = [nsImage CGImageForProposedRect:nil context:nil hints:nil];
        NSBitmapImageRep* bitmapRep = [[NSBitmapImageRep alloc] initWithCGImage:cgRef];
        [bitmapRep setSize:[nsImage size]];
        NSData* val = [bitmapRep representationUsingType:NSBitmapImageFileTypePNG properties:@{NSImageCompressionFactor:@1.0}];
#endif
        
        *bufferLen = [val length];
        return (__bridge_retained void*) val;
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
}

void* UIImage_JPEGRepresentation_GetData(
	const void* image,
    float compressionQuality,
    long* const bufferLen,
	void** exception
    )
{
	@try {
#if TARGET_OS_IOS || TARGET_OS_TV
	    NSData* val = UIImageJPEGRepresentation((__bridge UIImage*) image, compressionQuality);
#else
        NSImage* nsImage = (__bridge NSImage*)image;
        CGImageRef cgRef = [nsImage CGImageForProposedRect:nil context:nil hints:nil];
        NSBitmapImageRep* bitmapRep = [[NSBitmapImageRep alloc] initWithCGImage:cgRef];
        [bitmapRep setSize:[nsImage size]];
        NSData* val = [bitmapRep representationUsingType:NSBitmapImageFileTypeJPEG properties:@{NSImageCompressionFactor:@1.0}];
#endif
        *bufferLen = [val length];
        return (__bridge_retained void*) val;
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
}

void UIImage_CopyBufferAndReleaseData(
    const void* dataPtr,
    Byte* managedBuffer
    )
{
    NSData* val = (__bridge NSData*)dataPtr;
    [val getBytes:managedBuffer length:[val length]];
    CFRelease(dataPtr);
}

bool UIImage_SFSymbolsAreAvailable()
{
#if TARGET_OS_IOS || TARGET_OS_TV
    return [UIImage respondsToSelector:@selector(systemImageNamed:)];
#else
    return [NSImage respondsToSelector:@selector(imageWithSystemSymbolName:accessibilityDescription:)];
#endif
}


//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void UIImage_Dispose(void* ptr)
{
#if TARGET_OS_IOS || TARGET_OS_TV
    UIImage* val = (__bridge UIImage*) ptr;
#else
    NSImage* val = (__bridge NSImage*) ptr;
#endif
    if(val != nil)
    {
        CFRelease(ptr);
    }
}

}
