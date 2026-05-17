namespace Maui.Otp.Extensions;

public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Registers Maui.Otp and all its platform handlers and services.
    /// Call this in your MauiProgram.cs: builder.UseMauiOtp();
    /// </summary>
    public static MauiAppBuilder UseMauiOtp(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<OtpView, OtpViewHandler>();
        });

        return builder;
    }
}
