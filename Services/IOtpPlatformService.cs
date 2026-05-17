namespace Maui.Otp.Services;

/// <summary>
/// Platform-specific service contract for OtpView.
/// Handles SMS auto-read and haptic feedback per platform.
/// </summary>
public interface IOtpPlatformService
{
    /// <summary>
    /// Starts listening for an incoming SMS OTP code.
    /// Android: uses SMS Retriever API (no permission needed).
    /// iOS: relies on QuickType/AutoFill — no active listener needed.
    /// </summary>
    /// <param name="onCodeReceived">Callback fired when a code is extracted.</param>
    void StartSmsListener(Action<string> onCodeReceived);

    /// <summary>
    /// Stops the SMS listener (call on control detach or dispose).
    /// </summary>
    void StopSmsListener();

    /// <summary>
    /// Triggers platform-native haptic feedback.
    /// </summary>
    /// <param name="type">The type of haptic pattern to use.</param>
    void TriggerHaptic(HapticType type);
}
