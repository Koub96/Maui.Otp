using Maui.Otp.Services;

namespace Maui.Otp.Extensions;

public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Registers Maui.Otp and all its platform handlers and services.
    /// Call this in your MauiProgram.cs: builder.UseMauiOtp();
    /// </summary>
    public static MauiAppBuilder UseMauiOtp(this MauiAppBuilder builder)
    {
#if ANDROID
        builder.Services.AddSingleton<IOtpPlatformService,
            Maui.Otp.Platforms.Android.OtpPlatformService>();
#elif IOS
        builder.Services.AddSingleton<IOtpPlatformService,
            Maui.Otp.Platforms.iOS.OtpPlatformService>();
#else
        // Fallback no-op for Windows, Mac, etc.
        builder.Services.AddSingleton<IOtpPlatformService,
            OtpPlatformService>();
#endif
        return builder;
    }
}
