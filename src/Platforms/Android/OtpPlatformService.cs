#if ANDROID
using Android.Content;
using Android.OS;
using Android.Views;
using Maui.Otp.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Handlers;

namespace Maui.Otp.Platforms.Android;

/// <summary>
/// Android implementation of IOtpPlatformService.
/// Uses MAUI's built-in HapticFeedback API.
/// SMS Retriever is scaffolded — wire up your SMS Retriever API here.
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

    public void SetEntryPasteEnabled(Entry entry, bool enabled)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var handler = entry.Handler as EntryHandler;
            var nativeEditText = handler?.PlatformView;

            if (nativeEditText == null) return;

            nativeEditText.CustomInsertionActionModeCallback = enabled
                ? null                        // restore default
                : new BlockPasteActionCallback();
        });
    }
}

/// <summary>
/// Filters out Paste from the native Android context menu.
/// </summary>
internal class BlockPasteActionCallback
    : Java.Lang.Object, ActionMode.ICallback
{
    public bool OnCreateActionMode(ActionMode? mode, IMenu? menu) => true;

    public bool OnPrepareActionMode(ActionMode? mode, IMenu? menu)
    {
        menu?.RemoveItem(global::Android.Resource.Id.Paste);
        menu?.RemoveItem(global::Android.Resource.Id.PasteAsPlainText);
        return true;
    }

    public bool OnActionItemClicked(ActionMode? mode, IMenuItem? item) => false;
    public void OnDestroyActionMode(ActionMode? mode) { }
}
#endif
