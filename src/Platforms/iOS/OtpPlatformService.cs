#if IOS
using UIKit;
using Maui.Otp.Services;

namespace Maui.Otp.Platforms.iOS;

/// <summary>
/// iOS implementation of IOtpPlatformService.
/// SMS AutoFill is handled via UITextContentType.OneTimeCode on the hidden Entry.
/// Haptic feedback uses UIKit's UIImpactFeedbackGenerator.
/// </summary>
internal class OtpPlatformService : IOtpPlatformService
{
    // -------------------------------------------------------
    // SMS Listener
    // -------------------------------------------------------

    public void StartSmsListener(Action<string> onCodeReceived)
    {
        // iOS does NOT require an active SMS listener.
        // AutoFill is triggered automatically by the OS when:
        // 1. The hidden Entry has Keyboard = Keyboard.Numeric
        // 2. The UITextField has textContentType = .oneTimeCode
        // This is handled directly in OtpView's hidden Entry setup.
        // No action needed here.
    }

    public void StopSmsListener()
    {
        // Nothing to stop on iOS
    }

    // -------------------------------------------------------
    // Haptic Feedback
    // -------------------------------------------------------

    public void TriggerHaptic(HapticType type)
    {
        try
        {
            switch (type)
            {
                case HapticType.Input:
                    var inputFeedback = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
                    inputFeedback.Prepare();
                    inputFeedback.ImpactOccurred();
                    break;

                case HapticType.Error:
                    var errorFeedback = new UINotificationFeedbackGenerator();
                    errorFeedback.Prepare();
                    errorFeedback.NotificationOccurred(
                        UINotificationFeedbackType.Error);
                    break;

                case HapticType.Success:
                    var successFeedback = new UINotificationFeedbackGenerator();
                    successFeedback.Prepare();
                    successFeedback.NotificationOccurred(
                        UINotificationFeedbackType.Success);
                    break;
            }
        }
        catch
        {
            // Silently ignore — haptic is non-critical
        }
    }
}
#endif
