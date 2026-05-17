using Maui.Otp.Models;
using Microsoft.Maui.Graphics;

namespace Maui.Otp;

/// <summary>
/// Draws a single OTP cell using MAUI Graphics.
/// Handles all visual states: Default, Focused, Filled, Error, Success, Disabled.
/// </summary>
internal class OtpCellDrawable : IDrawable
{
    private readonly OtpView _otpView;
    private readonly int _index;

    public OtpCellDrawable(OtpView otpView, int index)
    {
        _otpView = otpView;
        _index = index;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var style = _otpView.CellStyle;
        var isFocused = _otpView.IsCellFocused(_index);
        var hasError = _otpView.HasError;
        var hasSuccess = _otpView.HasSuccess;
        var isEnabled = _otpView.IsOtpEnabled;
        var displayChar = _otpView.GetCellDisplayChar(_index);
        var isFilled = !string.IsNullOrEmpty(displayChar);

        // --- Resolve colors based on state (priority: error > success > focused > filled > default) ---
        Color bgColor;
        Color borderColor;
        Color textColor;

        if (!isEnabled)
        {
            bgColor = Colors.LightGray;
            borderColor = Colors.Gray;
            textColor = Colors.Gray;
        }
        else if (hasError)
        {
            bgColor = style.ErrorBackgroundColor;
            borderColor = style.ErrorBorderColor;
            textColor = style.ErrorTextColor;
        }
        else if (hasSuccess)
        {
            bgColor = style.SuccessBackgroundColor;
            borderColor = style.SuccessBorderColor;
            textColor = style.SuccessTextColor;
        }
        else if (isFocused)
        {
            bgColor = style.FocusedBackgroundColor;
            borderColor = style.FocusedBorderColor;
            textColor = style.TextColor;
        }
        else if (isFilled)
        {
            bgColor = style.BackgroundColor;
            borderColor = style.FilledBorderColor;
            textColor = style.TextColor;
        }
        else
        {
            bgColor = style.BackgroundColor;
            borderColor = style.BorderColor;
            textColor = style.TextColor;
        }

        float x = dirtyRect.X;
        float y = dirtyRect.Y;
        float w = dirtyRect.Width;
        float h = dirtyRect.Height;
        float r = style.CornerRadius;

        // --- Draw background ---
        canvas.FillColor = bgColor;
        canvas.FillRoundedRectangle(x, y, w, h, r);

        // --- Draw border ---
        canvas.StrokeColor = borderColor;
        canvas.StrokeSize = style.BorderWidth;
        canvas.DrawRoundedRectangle(
            x + style.BorderWidth / 2,
            y + style.BorderWidth / 2,
            w - style.BorderWidth,
            h - style.BorderWidth,
            r
        );

        // --- Draw character or cursor ---
        if (isFilled)
        {
            canvas.FontColor = textColor;
            canvas.FontSize = style.FontSize;
            canvas.DrawString(
                displayChar,
                x, y, w, h,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
            );
        }
        else if (isFocused)
        {
            // Draw a blinking cursor line in the center of the empty focused cell
            canvas.StrokeColor = style.FocusedBorderColor;
            canvas.StrokeSize = 2;
            float cx = x + w / 2;
            float cy1 = y + h * 0.25f;
            float cy2 = y + h * 0.75f;
            canvas.DrawLine(cx, cy1, cx, cy2);
        }
    }
}
