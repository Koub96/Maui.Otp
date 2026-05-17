
namespace Maui.Otp.Sample;

public partial class MainPage : ContentPage
{
    // The correct OTP for the interactive test
    private const string CorrectOtp = "123456";

    public MainPage()
    {
        InitializeComponent();
        //ApplyCustomStyle();
        WireUpValueLabels();
    }

    // ─────────────────────────────────────────────
    //  Custom style for Test 6 (applied in code)
    // ─────────────────────────────────────────────
    //private void ApplyCustomStyle()
    //{
    //    OtpCustom.CellStyle = new OtpCellStyle
    //    {
    //        Width = 52,
    //        Height = 52,
    //        CornerRadius = 26,           // ← full circle cells
    //        BorderColor = Color.FromArgb("#444"),
    //        FocusedBorderColor = Color.FromArgb("#BB86FC"),
    //        FilledBorderColor = Color.FromArgb("#BB86FC"),
    //        BorderWidth = 2f,
    //        BackgroundColor = Color.FromArgb("#1E1E2E"),
    //        FocusedBackgroundColor = Color.FromArgb("#2A2A3E"),
    //        TextColor = Colors.White,
    //        FontSize = 20,
    //        Spacing = 10
    //    };
    //}

    // ─────────────────────────────────────────────
    //  Wire up live value labels for Test 1 & 2
    // ─────────────────────────────────────────────
    private void WireUpValueLabels()
    {
        OtpDefault.ValueChanged += (s, val) =>
            LabelDefault.Text = $"Value: {val}";

        OtpPin.ValueChanged += (s, val) =>
            LabelPin.Text = $"Value: {new string('*', val.Length)} ({val.Length}/4)";
    }

    // ─────────────────────────────────────────────
    //  Test 1 — OtpCompleted
    // ─────────────────────────────────────────────
    private async void OnOtpCompleted(object? sender, string value)
    {
        LabelDefault.Text = $"✅ Completed: {value}";
        await DisplayAlert("OTP Complete!", $"You entered: {value}", "OK");
    }

    // ─────────────────────────────────────────────
    //  Test 7 — Interactive
    // ─────────────────────────────────────────────
    private void OnInteractiveValueChanged(object? sender, string value)
    {
        LabelInteractive.Text = value.Length > 0
            ? $"Entered {value.Length}/6 digits..."
            : "Waiting for input...";

        // Reset states on new input
        OtpInteractive.HasError = false;
        OtpInteractive.HasSuccess = false;
    }

    private void OnVerifyClicked(object? sender, EventArgs e)
    {
        if (OtpInteractive.Value.Length < 6)
        {
            LabelInteractive.Text = "⚠️ Please enter all 6 digits first.";
            return;
        }

        if (OtpInteractive.Value == CorrectOtp)
        {
            OtpInteractive.HasSuccess = true;
            LabelInteractive.Text = "✅ Correct OTP!";
        }
        else
        {
            OtpInteractive.HasError = true;
            LabelInteractive.Text = "❌ Wrong OTP! Try 123456";
        }
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        OtpInteractive.Clear();
        LabelInteractive.Text = "Waiting for input...";
    }
}
