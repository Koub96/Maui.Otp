namespace Maui.Otp.Services;

/// <summary>
/// Defines the type of haptic feedback to trigger.
/// </summary>
public enum HapticType
{
    /// <summary>Light tap — triggered on each digit input.</summary>
    Input,

    /// <summary>Error pattern — triggered when HasError = true.</summary>
    Error,

    /// <summary>Success pattern — triggered when HasSuccess = true.</summary>
    Success
}
