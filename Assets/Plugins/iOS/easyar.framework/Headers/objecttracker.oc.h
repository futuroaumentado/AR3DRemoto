﻿//=============================================================================================================================
//
// EasyAR Sense 3.1.0-final-7bf6504c6
// Copyright (c) 2015-2020 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

#import "easyar/types.oc.h"
#import "easyar/target.oc.h"

/// <summary>
/// Result of `ObjectTracker`_ .
/// </summary>
@interface easyar_ObjectTrackerResult : easyar_TargetTrackerResult

+ (instancetype)new NS_UNAVAILABLE;
- (instancetype)init NS_UNAVAILABLE;

/// <summary>
/// Returns the list of `TargetInstance`_ contained in the result.
/// </summary>
- (NSArray<easyar_TargetInstance *> *)targetInstances;
/// <summary>
/// Sets the list of `TargetInstance`_ contained in the result.
/// </summary>
- (void)setTargetInstances:(NSArray<easyar_TargetInstance *> *)instances;

@end

/// <summary>
/// ObjectTracker implements 3D object target detection and tracking. ObjectTracker requires a Pro license.
/// ObjectTracker occupies (1 + SimultaneousNum) buffers of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components. Refer to `Overview &lt;Overview.html&gt;`__ .
/// After creation, you can call start/stop to enable/disable the track process. start and stop are very lightweight calls.
/// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
/// ObjectTracker inputs `FeedbackFrame`_ from feedbackFrameSink. `FeedbackFrameSource`_ shall be connected to feedbackFrameSink for use. Refer to `Overview &lt;Overview.html&gt;`__ .
/// Before a `Target`_ can be tracked by ObjectTracker, you have to load it using loadTarget/unloadTarget. You can get load/unload results from callbacks passed into the interfaces.
/// </summary>
@interface easyar_ObjectTracker : easyar_RefBase

+ (instancetype)new NS_UNAVAILABLE;
- (instancetype)init NS_UNAVAILABLE;

/// <summary>
/// Returns true only with a Pro license.
/// </summary>
+ (bool)isAvailable;
/// <summary>
/// `FeedbackFrame`_ input port. The InputFrame member of FeedbackFrame must have raw image, timestamp, and camera parameters.
/// </summary>
- (easyar_FeedbackFrameSink *)feedbackFrameSink;
/// <summary>
/// Camera buffers occupied in this component.
/// </summary>
- (int)bufferRequirement;
/// <summary>
/// `OutputFrame`_ output port.
/// </summary>
- (easyar_OutputFrameSource *)outputFrameSource;
/// <summary>
/// Creates an instance.
/// </summary>
+ (easyar_ObjectTracker *)create;
/// <summary>
/// Starts the track algorithm.
/// </summary>
- (bool)start;
/// <summary>
/// Stops the track algorithm. Call start to start the track again.
/// </summary>
- (void)stop;
/// <summary>
/// Close. The component shall not be used after calling close.
/// </summary>
- (void)close;
/// <summary>
/// Load a `Target`_ into the tracker. A Target can only be tracked by tracker after a successful load.
/// This method is an asynchronous method. A load operation may take some time to finish and detection of a new/lost target may take more time during the load. The track time after detection will not be affected. If you want to know the load result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
/// </summary>
- (void)loadTarget:(easyar_Target *)target callbackScheduler:(easyar_CallbackScheduler *)callbackScheduler callback:(void (^)(easyar_Target * target, bool status))callback;
/// <summary>
/// Unload a `Target`_ from the tracker.
/// This method is an asynchronous method. An unload operation may take some time to finish and detection of a new/lost target may take more time during the unload. If you want to know the unload result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
/// </summary>
- (void)unloadTarget:(easyar_Target *)target callbackScheduler:(easyar_CallbackScheduler *)callbackScheduler callback:(void (^)(easyar_Target * target, bool status))callback;
/// <summary>
/// Returns current loaded targets in the tracker. If an asynchronous load/unload is in progress, the returned value will not reflect the result until all load/unload finish.
/// </summary>
- (NSArray<easyar_Target *> *)targets;
/// <summary>
/// Sets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
/// </summary>
- (bool)setSimultaneousNum:(int)num;
/// <summary>
/// Gets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
/// </summary>
- (int)simultaneousNum;

@end
