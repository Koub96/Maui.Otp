# Maui.Otp

A feature-rich, fully themeable OTP / PIN input control for .NET MAUI.  
Supports auto-submit, smart clipboard paste, haptic feedback,  
shake/bounce animations, lockout state and SMS auto-fill out of the box.

---

## ✨ Features

- 🔢 Configurable cell length
- 🎨 Full visual theming via `OtpCellStyle`
- 🔒 PIN mode with custom mask character
- ✅ Success & ❌ Error states with animations
- 📋 Smart clipboard paste with auto-clear
- 📱 iOS SMS `OneTimeCode` keyboard hint (native suggestion)
- 📳 Haptic feedback on input, error and success
- 🔁 Two-way `Value` binding — MVVM ready
- ⚡ `AutoSubmit` toggle for OTP vs PIN flows
- 🚫 Lockout support via `IsOtpEnabled`
- 💉 `FillOtp()` unified external fill API (SMS, biometric, programmatic)

---

## 📦 Installation

```bash
dotnet add package Maui.Otp
```

Or search for `Maui.Otp` in the NuGet Package Manager.

**Supported platforms:**

| Platform | Support |
|---|---|
| iOS 15+ | ✅ |
| Android 8.0+ | ✅ |
| Windows (WinUI 3) | ✅ |
| macOS (Mac Catalyst) | ✅ |

---

## ⚙️ Setup

### 1. Register in `MauiProgram.cs`

```csharp
using Maui.Otp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiOtp(); // ← Add this

        return builder.Build();
    }
}
```

### 2. Add the XAML namespace

```xml
xmlns:otp="clr-namespace:Maui.Otp;assembly=Maui.Otp"
```

---

## 🚀 Basic Usage

```xml
<otp:OtpView
    Length="6"
    OtpCompleted="OnOtpCompleted" />
```

```csharp
private async void OnOtpCompleted(object sender, string code)
{
    var isValid = await MyAuthService.VerifyAsync(code);

    otpView.HasError   = !isValid;
    otpView.HasSuccess = isValid;
}
```

---

## 📋 Properties

### Input

| Property | Type | Default | Description |
|---|---|---|---|
| `Length` | `int` | `6` | Number of OTP cells |
| `Value` | `string` | `""` | Current OTP value. Two-way bindable |
| `IsPassword` | `bool` | `false` | Masks input using `CellStyle.MaskCharacter` |
| `IsOtpEnabled` | `bool` | `true` | Enables or disables the entire control |

### Behaviour

| Property | Type | Default | Description |
|---|---|---|---|
| `AutoSubmit` | `bool` | `true` | Fires `OtpCompleted` automatically on last digit. Set `false` for PIN mode |
| `AllowPaste` | `bool` | `true` | Allows clipboard paste. Disabling also forces `SmartPaste` off |
| `SmartPaste` | `bool` | `true` | Auto-fills from clipboard on focus if a valid code is found |
| `ClearClipboardAfterPaste` | `bool` | `true` | Clears clipboard after a successful paste |

### State

| Property | Type | Default | Description |
|---|---|---|---|
| `HasError` | `bool` | `false` | Error state — triggers shake animation + error haptic |
| `HasSuccess` | `bool` | `false` | Success state — triggers bounce animation + success haptic |

### Feedback

| Property | Type | Default | Description |
|---|---|---|---|
| `HapticFeedbackEnabled` | `bool` | `true` | Haptic on input, error and success |
| `AnimationsEnabled` | `bool` | `true` | Shake on error, bounce on success |

### Styling

| Property | Type | Default | Description |
|---|---|---|---|
| `CellStyle` | `OtpCellStyle` | Built-in theme | Full visual configuration for all cells |

---

## 📡 Events

| Event | Args | Fires when |
|---|---|---|
| `ValueChanged` | `string` | Any digit is added or removed |
| `OtpCompleted` | `string` | All cells are filled (respects `AutoSubmit`) |

---

## 🔧 Public Methods

| Method | Description |
|---|---|
| `FillOtp(string code)` | Programmatically fills all cells. Use for SMS auto-read, biometric auth, or any external fill. Respects `AutoSubmit` |
| `Submit()` | Manually fires `OtpCompleted`. Use when `AutoSubmit = false` |
| `Clear()` | Clears all cells, resets `HasError` and `HasSuccess` |
| `FocusInput()` | Focuses the input and shows the keyboard |

---

## 🎨 Theming

Assign a custom `OtpCellStyle` to fully control the visual appearance:

```xml
<otp:OtpView Length="6">
    <otp:OtpView.CellStyle>
        <otp:OtpCellStyle
            Width="48"
            Height="56"
            Spacing="10"
            CornerRadius="12"
            BackgroundColor="#F5F5F5"
            BorderColor="#CCCCCC"
            FocusedBorderColor="#6200EE"
            FilledBorderColor="#6200EE"
            ErrorBorderColor="#B00020"
            SuccessBorderColor="#00C853"
            TextColor="#000000"
            FontSize="22"
            MaskCharacter="●" />
    </otp:OtpView.CellStyle>
</otp:OtpView>
```

### `OtpCellStyle` Properties

| Property | Type | Description |
|---|---|---|
| `Width` | `double` | Cell width in dp |
| `Height` | `double` | Cell height in dp |
| `Spacing` | `double` | Gap between cells |
| `CornerRadius` | `float` | Cell corner radius |
| `BackgroundColor` | `Color` | Default cell background |
| `BorderColor` | `Color` | Default border color |
| `FocusedBorderColor` | `Color` | Border when the cell is focused |
| `FilledBorderColor` | `Color` | Border when the cell has a value |
| `ErrorBorderColor` | `Color` | Border in error state |
| `SuccessBorderColor` | `Color` | Border in success state |
| `TextColor` | `Color` | Digit text color |
| `FontSize` | `double` | Digit font size |
| `MaskCharacter` | `string` | Character shown when `IsPassword = true` |

---

## 🧩 Recipes

### OTP Verification

```xml
<otp:OtpView
    x:Name="OtpInput"
    Length="6"
    AutoSubmit="True"
    OtpCompleted="OnOtpCompleted" />
```

```csharp
private async void OnOtpCompleted(object sender, string code)
{
    // Disable during validation to prevent re-entry
    OtpInput.IsOtpEnabled = false;

    var isValid = await MyAuthService.VerifyOtpAsync(code);

    OtpInput.HasError   = !isValid;
    OtpInput.HasSuccess = isValid;

    // Re-enable on failure so user can retry
    if (!isValid)
    {
        OtpInput.IsOtpEnabled = true;
        OtpInput.Clear();
    }
}
```

---

### PIN Entry (Manual Submit)

```xml
<otp:OtpView
    x:Name="PinInput"
    Length="4"
    IsPassword="True"
    AutoSubmit="False"
    ValueChanged="OnPinValueChanged" />

<Button
    x:Name="ConfirmButton"
    Text="Confirm"
    IsEnabled="False"
    Clicked="OnConfirmClicked" />
```

```csharp
private void OnPinValueChanged(object sender, string value)
{
    // Enable confirm only when all digits are entered
    ConfirmButton.IsEnabled = value.Length == 4;
}

private void OnConfirmClicked(object sender, EventArgs e)
{
    PinInput.Submit();
}
```

---

### SMS Auto-Fill

```csharp
// After your SMS reader retrieves the code
var smsCode = await SmsReaderService.GetCodeAsync();
otpView.FillOtp(smsCode);
```

> **iOS:** `TextContentType.OneTimeCode` is applied automatically.  
> The system keyboard will suggest the SMS code natively — no extra code needed.  
> **Android:** Integrate your preferred SMS Retriever API and call `FillOtp()` with the result.

---

### Biometric Fallback

The control does not perform biometric authentication itself.  
Your app handles auth — then calls `FillOtp()` with the result.

```csharp
// User taps "Use Face ID" in your UI
private async void OnBiometricTapped(object sender, EventArgs e)
{
    var result = await BiometricAuthService.AuthenticateAsync();

    if (result.IsSuccess)
        otpView.FillOtp(result.Code);  // fill with the token/code
    else
        otpView.HasError = true;
}
```

---

### Lockout After Failed Attempts

```csharp
private int _attempts = 0;
private const int MaxAttempts = 3;

private async void OnOtpCompleted(object sender, string code)
{
    var isValid = await MyAuthService.VerifyOtpAsync(code);

    if (isValid)
    {
        otpView.HasSuccess = true;
        return;
    }

    _attempts++;
    otpView.HasError = true;
    otpView.Clear();

    if (_attempts >= MaxAttempts)
    {
        otpView.IsOtpEnabled = false;
        await ShowLockoutMessageAsync();
    }
}
```

---

### MVVM Binding

```xml
<otp:OtpView
    Length="6"
    Value="{Binding OtpCode, Mode=TwoWay}"
    HasError="{Binding HasError}"
    HasSuccess="{Binding HasSuccess}"
    OtpCompleted="OnOtpCompleted" />
```

```csharp
// ViewModel — using CommunityToolkit.Mvvm
[ObservableProperty] private string _otpCode = string.Empty;
[ObservableProperty] private bool _hasError;
[ObservableProperty] private bool _hasSuccess;
```

---

## 🌍 Platform Notes

| Platform | Notes |
|---|---|
| **iOS** | `TextContentType.OneTimeCode` applied automatically. Native SMS suggestion appears above the keyboard |
| **Android** | Numeric keyboard shown by default. Integrate SMS Retriever API in your app and call `FillOtp()` |
| **Windows** | Fully supported. Haptic feedback is silently ignored (not supported by platform) |
| **macOS** | Fully supported. Haptic feedback is silently ignored |

---

## ♿ Accessibility

- Each cell exposes `AutomationId` as `OtpCell_0`, `OtpCell_1` ... `OtpCell_N`
- The hidden entry is the actual focusable element — compatible with screen readers
- Tap anywhere on any cell to focus input and show the keyboard

---

## 📝 Changelog

### v1.0.0
- Initial release
- Configurable OTP / PIN input with variable length
- Full cell theming via `OtpCellStyle`
- Smart clipboard paste with auto-clear security
- Haptic feedback on input, error and success
- Shake animation on error
- Bounce animation on success
- `AutoSubmit` toggle for OTP vs PIN mode
- `FillOtp()` unified external fill API
- `Submit()` for manual submission
- `Clear()` full state reset
- iOS SMS `OneTimeCode` keyboard hint
- Two-way `Value` binding
- `IsOtpEnabled` lockout support
- `AllowPaste` / `SmartPaste` granular paste control

---

## 📄 License

MIT © 2025 Konstantinos Koumpanakis

---

## 🤝 Contributing

Issues and PRs are welcome.  
Please open an issue first to discuss what you would like to change.
