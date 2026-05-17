namespace Maui.Otp.Services;

/// <summary>
/// Default no-op implementation of IOtpPlatformService.
/// Used on platforms where no native implementation is registered
/// (e.g. Windows, MacCatalyst, or unit test environments).
/// </summary>
internal class OtpPlatformService : IOtpPlatformService
{
    public void StartSmsListener(Action<string> onCodeReceived)
    {
        // No-op on unsupported platforms
    }

    public void StopSmsListener()
    {
        // No-op on unsupported platforms
    }

    public void TriggerHaptic(HapticType type)
    {
        // No-op on unsupported platforms
    }
}
