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

    /// <summary>
    /// Reads the current clipboard text.
    /// Returns null if clipboard is empty or unavailable.
    /// </summary>
    Task<string?> GetClipboardTextAsync();

    /// <summary>
    /// Clears the clipboard after a successful paste.
    /// Prevents sensitive OTP codes from lingering.
    /// </summary>
    Task ClearClipboardAsync();

    /// <summary>
    /// Enables or disables the native paste action on the hidden entry.
    /// </summary>
    void SetEntryPasteEnabled(Entry entry, bool enabled);
}
