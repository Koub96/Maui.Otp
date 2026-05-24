using Maui.Otp.Animations;
using Maui.Otp.Models;
using Maui.Otp.Services;

namespace Maui.Otp;

/// <summary>
/// OtpView — A feature-rich OTP/PIN input control for .NET MAUI.
/// Supports SMS auto-read, haptic feedback, shake/bounce animations,
/// lockout, biometric fallback, clipboard paste, RTL and accessibility.
/// </summary>
public class OtpView : ContentView
{
    #region Private Fields

    private readonly HorizontalStackLayout _cellContainer;
    private readonly Entry _hiddenEntry;        // invisible entry that captures keyboard input
    private readonly List<GraphicsView> _cells; // visual cell drawables
    private string _currentValue = string.Empty;

    #endregion

    #region Bindable Properties
    /// <summary>If the OTP should auto focus to the first input field.</summary>
    public static readonly BindableProperty AutoFocusOnFirstFieldProperty =
        BindableProperty.Create(
            nameof(Length),
            typeof(bool),
            typeof(OtpView),
            defaultValue: false,
            propertyChanged: (b, o, n) => ((OtpView)b).OnLengthChanged());
    public bool AutoFocusOnFirstField
    {
        get => (bool)GetValue(AutoFocusOnFirstFieldProperty);
        set => SetValue(AutoFocusOnFirstFieldProperty, value);
    }


    /// <summary>Number of OTP digits/cells. Default is 6.</summary>
    public static readonly BindableProperty LengthProperty =
        BindableProperty.Create(
            nameof(Length),
            typeof(int),
            typeof(OtpView),
            defaultValue: 6,
            propertyChanged: (b, o, n) => ((OtpView)b).OnLengthChanged());

    public int Length
    {
        get => (int)GetValue(LengthProperty);
        set => SetValue(LengthProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// The current OTP value entered by the user.
    /// Bindable — two-way binding supported.
    /// </summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(string),
            typeof(OtpView),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (b, o, n) => ((OtpView)b).OnValueChanged((string)n));

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// When true, input is masked (PIN mode).
    /// Uses OtpCellStyle.MaskCharacter for display.
    /// </summary>
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(OtpView),
            defaultValue: false,
            propertyChanged: (b, o, n) => ((OtpView)b).RedrawAllCells());

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// Sets the OTP view into Error state.
    /// Triggers shake animation + haptic feedback automatically.
    /// </summary>
    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.Create(
            nameof(HasError),
            typeof(bool),
            typeof(OtpView),
            defaultValue: false,
            propertyChanged: (b, o, n) => ((OtpView)b).OnHasErrorChanged((bool)n));

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// Sets the OTP view into Success state.
    /// Triggers bounce animation + haptic feedback automatically.
    /// </summary>
    public static readonly BindableProperty HasSuccessProperty =
        BindableProperty.Create(
            nameof(HasSuccess),
            typeof(bool),
            typeof(OtpView),
            defaultValue: false,
            propertyChanged: (b, o, n) => ((OtpView)b).OnHasSuccessChanged((bool)n));

    public bool HasSuccess
    {
        get => (bool)GetValue(HasSuccessProperty);
        set => SetValue(HasSuccessProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// When true, the control is disabled (e.g. during lockout).
    /// Cells are grayed out and input is blocked.
    /// </summary>
    public static readonly BindableProperty IsOtpEnabledProperty =
        BindableProperty.Create(
            nameof(IsOtpEnabled),
            typeof(bool),
            typeof(OtpView),
            defaultValue: true,
            propertyChanged: (b, o, n) => ((OtpView)b).OnIsEnabledChanged((bool)n));

    public bool IsOtpEnabled
    {
        get => (bool)GetValue(IsOtpEnabledProperty);
        set => SetValue(IsOtpEnabledProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// Enables or disables haptic feedback on input, error and success.
    /// Default is true.
    /// </summary>
    public static readonly BindableProperty HapticFeedbackEnabledProperty =
        BindableProperty.Create(
            nameof(HapticFeedbackEnabled),
            typeof(bool),
            typeof(OtpView),
            defaultValue: true);

    public bool HapticFeedbackEnabled
    {
        get => (bool)GetValue(HapticFeedbackEnabledProperty);
        set => SetValue(HapticFeedbackEnabledProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// Enables or disables animations on error and success.
    /// Default is true.
    /// </summary>
    public static readonly BindableProperty AnimationsEnabledProperty =
        BindableProperty.Create(
            nameof(AnimationsEnabled),
            typeof(bool),
            typeof(OtpView),
            defaultValue: true);

    public bool AnimationsEnabled
    {
        get => (bool)GetValue(AnimationsEnabledProperty);
        set => SetValue(AnimationsEnabledProperty, value);
    }


    // -------------------------------------------------------

    /// <summary>
    /// Enables or disables clipboard paste support.
    /// When true, pasting a string of the correct length auto-fills all cells.
    /// Default is true.
    /// </summary>
    public static readonly BindableProperty AllowPasteProperty =
        BindableProperty.Create(
            nameof(AllowPaste),
            typeof(bool),
            typeof(OtpView),
            defaultValue: true);

    public bool AllowPaste
    {
        get => (bool)GetValue(AllowPasteProperty);
        set => SetValue(AllowPasteProperty, value);
    }

    // -------------------------------------------------------

    /// <summary>
    /// The visual style configuration for all cells.
    /// Assign a custom OtpCellStyle to fully theme the control.
    /// </summary>
    public static readonly BindableProperty CellStyleProperty =
        BindableProperty.Create(
            nameof(CellStyle),
            typeof(OtpCellStyle),
            typeof(OtpView),
            defaultValueCreator: _ => new OtpCellStyle(),
            propertyChanged: (b, o, n) => ((OtpView)b).RedrawAllCells());

    public OtpCellStyle CellStyle
    {
        get => (OtpCellStyle)GetValue(CellStyleProperty);
        set => SetValue(CellStyleProperty, value);
    }

    #endregion

    #region Events

    /// <summary>Fired every time a digit is entered or removed.</summary>
    public event EventHandler<string>? ValueChanged;

    /// <summary>
    /// Fired when all cells are filled and the OTP is complete.
    /// The string argument is the full OTP value.
    /// </summary>
    public event EventHandler<string>? OtpCompleted;

    /// <summary>
    /// Fired when SMS auto-read successfully retrieves a code.
    /// Useful for logging or showing a "Code filled from SMS" toast.
    /// </summary>
    public event EventHandler<string>? SmsCodeReceived;

    /// <summary>
    /// Fired when the biometric fallback button is tapped.
    /// The consumer app handles the actual biometric auth.
    /// </summary>
    public event EventHandler? BiometricRequested;

    #endregion

    #region Private Properties

    private readonly IOtpPlatformService? _platformService;

    #endregion

    #region Constructor

    public OtpView()
    {
        // Resolve platform service from DI
        _platformService = IPlatformApplication.Current?
            .Services
            .GetService<IOtpPlatformService>();

        _cells = new List<GraphicsView>();

        // Hidden entry — captures all keyboard input invisibly
        _hiddenEntry = new Entry
        {
            Opacity = 0,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            InputTransparent = false,
            Keyboard = Keyboard.Numeric,
            MaxLength = Length,
            IsPassword = false, // we handle masking ourselves
        };

        _hiddenEntry.TextChanged += OnHiddenEntryTextChanged;
        _hiddenEntry.Focused += (s, e) => RedrawAllCells();
        _hiddenEntry.Unfocused += (s, e) => RedrawAllCells();

        _hiddenEntry.HandlerChanged += OnHiddenEntryHandlerChanged;

        // Cell container — horizontal row of GraphicsView cells
        _cellContainer = new HorizontalStackLayout
        {
            Spacing = CellStyle.Spacing,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };


        // Root layout: cells stacked over the hidden entry
        var rootGrid = new Grid();
        rootGrid.Add(_hiddenEntry);
        rootGrid.Add(_cellContainer);

        Content = rootGrid;

        // Build initial cells
        BuildCells();
    }
    #endregion

    #region Cell Building

    /// <summary>
    /// Builds or rebuilds all cells based on the current Length.
    /// Called on init and when Length changes.
    /// Also, sets the tap gesture recognizer on each cell to focus the hidden entry in order for the keyboard to appear.
    /// </summary>
    private void BuildCells()
    {
        _cellContainer.Children.Clear();
        _cells.Clear();

        for (int i = 0; i < Length; i++)
        {
            var cell = CreateCell(i);

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => this._hiddenEntry.Focus();
            cell.GestureRecognizers.Add(tap);

            _cells.Add(cell);

            _cellContainer.Children.Add(cell);
        }

        // Update hidden entry max length
        _hiddenEntry.MaxLength = Length;

        RedrawAllCells();
    }

    /// <summary>Creates a single GraphicsView cell with its drawable.</summary>
    private GraphicsView CreateCell(int index)
    {
        var drawable = new OtpCellDrawable(this, index);

        var cell = new GraphicsView
        {
            Drawable = drawable,
            WidthRequest = CellStyle.Width,
            HeightRequest = CellStyle.Height,
            AutomationId = $"OtpCell_{index}",
        };

        return cell;
    }

    #endregion

    #region Input Handling
    private void OnHiddenEntryHandlerChanged(object? sender, EventArgs e)
    {
#if IOS
    if (_hiddenEntry.Handler?.PlatformView is UIKit.UITextField textField)
    {
        // Tells iOS to suggest OTP codes from SMS
        textField.TextContentType = UIKit.UITextContentType.OneTimeCode;
    }
#endif
    }

    /// <summary>
    /// Focuses the hidden entry to bring up the keyboard.
    /// </summary>
    public void FocusInput()
    {
        if (IsOtpEnabled)
            _hiddenEntry.Focus();
    }

    /// <summary>
    /// Handles text changes from the hidden entry.
    /// Filters to digits only, updates value and redraws cells.
    /// </summary>
    private void OnHiddenEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!IsOtpEnabled) return;

        // Filter: digits only
        var filtered = new string(
            e.NewTextValue
             .Where(char.IsDigit)
             .Take(Length)
             .ToArray()
        );

        // Prevent recursive loop
        if (_hiddenEntry.Text != filtered)
        {
            _hiddenEntry.Text = filtered;
            return;
        }

        _currentValue = filtered;

        // Haptic feedback on every digit input (add/remove)
        if (HapticFeedbackEnabled)
            _platformService?.TriggerHaptic(HapticType.Input);

        // Update bindable Value property (triggers two-way binding)
        Value = _currentValue;

        // Notify listeners
        ValueChanged?.Invoke(this, _currentValue);

        // Redraw all cells to reflect new value
        RedrawAllCells();

        // Fire OtpCompleted when all cells are filled
        if (_currentValue.Length == Length)
            OtpCompleted?.Invoke(this, _currentValue);
    }

    /// <summary>
    /// Called when the Value bindable property changes externally.
    /// Syncs the hidden entry and redraws cells.
    /// </summary>
    private void OnValueChanged(string newValue)
    {
        if (newValue == _currentValue) return;

        _currentValue = newValue ?? string.Empty;

        // Sync hidden entry without triggering recursive event
        _hiddenEntry.TextChanged -= OnHiddenEntryTextChanged;
        _hiddenEntry.Text = _currentValue;
        _hiddenEntry.TextChanged += OnHiddenEntryTextChanged;

        RedrawAllCells();
    }

    #endregion

    #region State Changes

    private void OnLengthChanged()
    {
        _currentValue = string.Empty;
        Value = string.Empty;
        BuildCells();
    }

    private void OnHasErrorChanged(bool hasError)
    {
        if (hasError)
        {
            RedrawAllCells();

            //Haptic feedback on error state — fires only when HasError becomes true
            if (HapticFeedbackEnabled)
                _platformService?.TriggerHaptic(HapticType.Error);

            if(AnimationsEnabled)
                _ = ShakeAnimation.RunAsync(_cellContainer);
        }
        else
        {
            RedrawAllCells();
        }
    }

    private void OnHasSuccessChanged(bool hasSuccess)
    {
        if (hasSuccess)
        {
            RedrawAllCells();

            // Haptic feedback on success state — fires only when HasSuccess becomes true
            if (HapticFeedbackEnabled)
                _platformService?.TriggerHaptic(HapticType.Success);

            if(AnimationsEnabled)
                _ = BounceAnimation.RunAsync(_cells);
        }
        else
        {
            RedrawAllCells();
        }
    }

    private void OnIsEnabledChanged(bool isEnabled)
    {
        _hiddenEntry.IsEnabled = isEnabled;
        RedrawAllCells();
    }

    #endregion

    #region Drawing

    /// <summary>
    /// Forces all cells to redraw with the latest state.
    /// </summary>
    internal void RedrawAllCells()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            _cells[i].WidthRequest = CellStyle.Width;
            _cells[i].HeightRequest = CellStyle.Height;
            _cells[i].Invalidate(); // triggers GraphicsView redraw
        }

        // Sync spacing
        _cellContainer.Spacing = CellStyle.Spacing;
    }

    /// <summary>
    /// Returns the character to display in a cell at the given index.
    /// Respects IsPassword masking.
    /// </summary>
    internal string GetCellDisplayChar(int index)
    {
        if (index >= _currentValue.Length) return string.Empty;
        return IsPassword ? CellStyle.MaskCharacter : _currentValue[index].ToString();
    }

    /// <summary>
    /// Returns true if the cell at the given index is the currently focused cell.
    /// The focused cell is always the next empty cell.
    /// </summary>
    internal bool IsCellFocused(int index)
        => _hiddenEntry.IsFocused && index == Math.Min(_currentValue.Length, Length - 1);

    /// <summary>
    /// Returns true if the cell at the given index should auto-focus.
    /// </summary>
    internal bool ShouldAutoFocus(int index)
       => index == 0 && AutoFocusOnFirstField;

    #endregion

    #region Public API

    /// <summary>Clears all cells and resets the value.</summary>
    public void Clear()
    {
        _currentValue = string.Empty;
        Value = string.Empty;
        _hiddenEntry.Text = string.Empty;
        HasError = false;
        HasSuccess = false;
        RedrawAllCells();
    }

    /// <summary>
    /// Programmatically fills the OTP from an SMS-retrieved code.
    /// Fires SmsCodeReceived event.
    /// </summary>
    public void FillFromSms(string code)
    {
        if (string.IsNullOrEmpty(code)) return;

        var digits = new string(code.Where(char.IsDigit).Take(Length).ToArray());
        if (digits.Length == Length)
        {
            Value = digits;
            SmsCodeReceived?.Invoke(this, digits);
        }
    }

    /// <summary>Triggers the BiometricRequested event.</summary>
    public void RequestBiometric()
        => BiometricRequested?.Invoke(this, EventArgs.Empty);

    #endregion
}
