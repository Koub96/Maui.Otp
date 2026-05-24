namespace Maui.Otp.Models;

/// <summary>
/// Defines the visual appearance of each OTP cell.
/// Supports all states: Default, Focused, Filled, Error, Success, Disabled.
/// </summary>
public class OtpCellStyle
{
    // --- Size & Shape ---

    /// <summary>Cell width in device-independent units.</summary>
    public double Width { get; set; } = 48;

    /// <summary>Cell height in device-independent units.</summary>
    public double Height { get; set; } = 48;

    /// <summary>Corner radius. Set to 50 for a circle shape.</summary>
    public float CornerRadius { get; set; } = 10;

    /// <summary>Spacing between cells.</summary>
    public double Spacing { get; set; } = 8;

    // --- Border ---

    /// <summary>Default border color (unfocused, empty).</summary>
    public Color BorderColor { get; set; } = Color.FromArgb("#CCCCCC");

    /// <summary>Border color when the cell is focused.</summary>
    public Color FocusedBorderColor { get; set; } = Color.FromArgb("#2979FF");

    /// <summary>Border color when the cell has a value.</summary>
    public Color FilledBorderColor { get; set; } = Color.FromArgb("#2979FF");

    /// <summary>Border color on error state.</summary>
    public Color ErrorBorderColor { get; set; } = Color.FromArgb("#F44336");

    /// <summary>Border color on success state.</summary>
    public Color SuccessBorderColor { get; set; } = Color.FromArgb("#4CAF50");

    /// <summary>Border thickness in pixels.</summary>
    public float BorderWidth { get; set; } = 1.5f;

    // --- Background ---

    /// <summary>Default cell background color.</summary>
    public Color BackgroundColor { get; set; } = Colors.White;

    /// <summary>Background color when focused.</summary>
    public Color FocusedBackgroundColor { get; set; } = Color.FromArgb("#F0F5FF");

    /// <summary>Background color on error state.</summary>
    public Color ErrorBackgroundColor { get; set; } = Color.FromArgb("#FFF0F0");

    /// <summary>Background color on success state.</summary>
    public Color SuccessBackgroundColor { get; set; } = Color.FromArgb("#F0FFF4");

    // --- Text ---

    /// <summary>Font size of the digit/character inside the cell.</summary>
    public float FontSize { get; set; } = 22;

    /// <summary>Text color of the digit.</summary>
    public Color TextColor { get; set; } = Color.FromArgb("#212121");

    /// <summary>Text color on error state.</summary>
    public Color ErrorTextColor { get; set; } = Color.FromArgb("#F44336");

    /// <summary>Text color on success state.</summary>
    public Color SuccessTextColor { get; set; } = Color.FromArgb("#4CAF50");

    // --- Mask (PIN mode) ---

    /// <summary>
    /// Character used to mask input in PIN mode.
    /// Default is a bullet "●".
    /// </summary>
    public string MaskCharacter { get; set; } = "●";

    /// <summary>Size of the mask character.</summary>
    public float MaskSize { get; set; } = 14;
}
