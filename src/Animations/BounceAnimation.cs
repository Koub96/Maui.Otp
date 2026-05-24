namespace Maui.Otp.Animations;

/// <summary>
/// Runs a staggered bounce animation across all OTP cells on success.
/// Each cell scales up then springs back, with a cascading delay.
/// </summary>
internal static class BounceAnimation
{
    // Animation tuning constants
    private const int CellDuration = 180;  // ms per cell up+down
    private const int StaggerDelay = 60;   // ms between each cell start
    private const double ScalePeak = 1.25; // how high each cell bounces
    private const double ScaleNormal = 1.0;  // resting scale

    /// <summary>
    /// Animates all cells with a staggered bounce effect.
    /// Safe to call from any context — marshals to UI thread internally.
    /// </summary>
    public static async Task RunAsync(IList<GraphicsView> cells)
    {
        if (cells == null || cells.Count == 0) return;

        // Launch all cell animations with staggered delays
        var tasks = cells.Select((cell, index) =>
            AnimateCellAsync(cell, index * StaggerDelay));

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Animates a single cell: wait stagger → scale up → scale down.
    /// </summary>
    private static async Task AnimateCellAsync(GraphicsView cell, int delayMs)
    {
        if (delayMs > 0)
            await Task.Delay((int)delayMs);

        // Scale up — spring out
        await cell.ScaleToAsync(ScalePeak, CellDuration / 2, Easing.CubicOut);

        // Scale down — spring back
        await cell.ScaleToAsync(ScaleNormal, CellDuration / 2, Easing.BounceOut);
    }
}
