#if ANDROID
using Android.Content;
using Android.OS;
using Maui.Otp.Services;
using Microsoft.Maui.ApplicationModel;

namespace Maui.Otp.Platforms.Android;

/// <summary>
/// Android implementation of IOtpPlatformService.
/// Uses MAUI's built-in HapticFeedback API.
/// SMS Retriever is scaffolded — wire up your SMS Retriever API here.
/// </summary>
internal class OtpPlatformService : IOtpPlatformService
{
    private Action<string>? _onCodeReceived;

    // -------------------------------------------------------
    // SMS Listener
    // -------------------------------------------------------

    public void StartSmsListener(Action<string> onCodeReceived)
    {
        _onCodeReceived = onCodeReceived;

        // TODO Phase 2: Start Android SMS Retriever API here
        // Google Play Services SMS Retriever:
        // SmsRetrieverClient client = SmsRetriever.GetClient(context);
        // client.StartSmsRetriever();
        // Then register a BroadcastReceiver to capture the SMS
        // and call _onCodeReceived(extractedCode);
    }

    public void StopSmsListener()
    {
        _onCodeReceived = null;

        // TODO Phase 2: Unregister BroadcastReceiver here
    }

    // -------------------------------------------------------
    // Haptic Feedback
    // -------------------------------------------------------

    public void TriggerHaptic(HapticType type)
    {
        try
        {
            // Use MAUI's cross-platform HapticFeedback API
            switch (type)
            {
                case HapticType.Input:
                    HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                    break;

                case HapticType.Error:
                    // Android: double click pattern simulates error
                    HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
                    break;

                case HapticType.Success:
                    HapticFeedback.Default.Perform(HapticFeedbackType.Click);
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
