#if IOS
using Foundation;
using Maui.Otp.Services;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;

namespace Maui.Otp.Platforms.iOS;

/// <summary>
/// iOS implementation of IOtpPlatformService.
/// SMS AutoFill is handled via UITextContentType.OneTimeCode on the hidden Entry.
/// Haptic feedback uses UIKit's UIImpactFeedbackGenerator.
/// </summary>
internal class OtpPlatformService : IOtpPlatformService
{
    public async Task<string?> GetClipboardTextAsync()
    {
        try
        {
            if (!Clipboard.Default.HasText) return null;
            return await Clipboard.Default.GetTextAsync();
        }
        catch
        {
            // Clipboard access can fail silently on some devices
            return null;
        }
    }

    public async Task ClearClipboardAsync()
    {
        try
        {
            await Clipboard.Default.SetTextAsync(string.Empty);
        }
        catch
        {
            // Ignore — clearing clipboard is best-effort
        }
    }

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

    public void SetEntryPasteEnabled(Entry entry, bool enabled)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var handler = entry.Handler as EntryHandler;

            // Cast to our custom subclass
            if (handler?.PlatformView is OtpSecureTextField secureField)
            {
                secureField.PasteEnabled = enabled;
            }
        });
    }

}

/// <summary>
/// Subclasses MauiTextField (MAUI's native wrapper) so the handler
/// returns the correct type while still allowing us to intercept CanPerform.
/// </summary>
internal class OtpSecureTextField : MauiTextField
{
    public bool PasteEnabled { get; set; } = true;

    public override bool CanPerform(Selector action, NSObject? withSender)
    {
        // Block paste at the native level
        if (!PasteEnabled && action.Name == "paste:")
            return false;

        return base.CanPerform(action, withSender);
    }
}
#endif
