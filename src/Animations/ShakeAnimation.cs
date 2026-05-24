namespace Maui.Otp.Animations;

/// <summary>
/// Runs a horizontal shake animation on the entire cell container on error.
/// Mimics the native iOS "wrong password" shake.
/// </summary>
internal static class ShakeAnimation
{
    private const int StepDuration = 50;   // ms per shake step
    private const double ShakeOffset = 10; // px horizontal travel

    /// <summary>
    /// Shakes the entire container left/right rapidly.
    /// </summary>
    public static async Task RunAsync(View container)
    {
        if (container == null) return;

        // 4-step shake sequence
        await container.TranslateToAsync(-ShakeOffset, 0, StepDuration, Easing.Linear);
        await container.TranslateToAsync(ShakeOffset, 0, StepDuration, Easing.Linear);
        await container.TranslateToAsync(-ShakeOffset, 0, StepDuration, Easing.Linear);
        await container.TranslateToAsync(ShakeOffset, 0, StepDuration, Easing.Linear);
        await container.TranslateToAsync(-ShakeOffset / 2, 0, StepDuration, Easing.Linear);
        await container.TranslateToAsync(ShakeOffset / 2, 0, StepDuration, Easing.Linear);

        // Snap back to center
        await container.TranslateToAsync(0, 0, StepDuration, Easing.Linear);
    }
}
