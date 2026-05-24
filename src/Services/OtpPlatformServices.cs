namespace Maui.Otp.Services;

/// <summary>
/// Default no-op implementation of IOtpPlatformService.
/// Used on platforms where no native implementation is registered
/// (e.g. Windows, MacCatalyst, or unit test environments).
/// </summary>
internal class OtpPlatformService : IOtpPlatformService
{
    public void TriggerHaptic(HapticType type)
    {
        // No-op on unsupported platforms
    }

    public async Task<string?> GetClipboardTextAsync()
    {
        // No-op on unsupported platforms
        return null;
    }

    public async Task ClearClipboardAsync()
    {
        // No-op on unsupported platforms
    }

    public void SetEntryPasteEnabled(Entry entry, bool enabled)
    {
        // No-op on unsupported platforms
    }
}
