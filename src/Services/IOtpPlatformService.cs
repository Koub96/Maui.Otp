namespace Maui.Otp.Services;

/// <summary>
/// Platform-specific service contract for OtpView.
/// </summary>
public interface IOtpPlatformService
{
    /// <summary>
    /// Triggers platform-native haptic feedback.
    /// </summary>
    /// <param name="type">The type of haptic pattern to use.</param>
    void TriggerHaptic(HapticType type);
}
