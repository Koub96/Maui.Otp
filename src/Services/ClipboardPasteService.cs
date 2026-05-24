namespace Maui.Otp.Services;

/// <summary>
/// Handles smart clipboard paste logic for OtpView.
/// Validates clipboard content before auto-filling.
/// </summary>
internal static class ClipboardPasteService
{
    /// <summary>
    /// Tries to extract a valid OTP code from the clipboard.
    /// Returns null if clipboard content is not a valid OTP.
    /// </summary>
    public static async Task<string?> TryGetOtpFromClipboardAsync(
        IOtpPlatformService platformService,
        int expectedLength)
    {
        var text = await platformService.GetClipboardTextAsync();

        if (string.IsNullOrWhiteSpace(text)) return null;

        // Strip whitespace — handle "123 456" or " 123456 "
        var cleaned = text.Trim().Replace(" ", "").Replace("-", "");

        // Must be digits only
        if (!cleaned.All(char.IsDigit)) return null;

        // Must match expected OTP length exactly
        if (cleaned.Length != expectedLength) return null;

        return cleaned;
    }
}
