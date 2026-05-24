using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.Otp.Platforms.iOS;

/// <summary>
/// Replaces the default UITextField with our OtpSecureTextField.
/// </summary>
internal class OtpEntryHandler : EntryHandler
{
    protected override MauiTextField CreatePlatformView()
    {
        return new OtpSecureTextField();
    }
}
