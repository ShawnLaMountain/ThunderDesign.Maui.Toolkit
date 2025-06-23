using System.Runtime.CompilerServices;


namespace ThunderDesign.Maui.Toolkit.Contols;

public class ExtendedSlider : Slider, ISliderController, ISlider
{
    public ExtendedSlider() : base()
    {
        ValueChanged += OnValueChanged;
    }

    public new event EventHandler DragCompleted;

    public static readonly BindableProperty DragCompletedCommandParameterProperty =
                           BindableProperty.Create(propertyName: nameof(DragCompletedCommandParameter), returnType: typeof(object), declaringType: typeof(ExtendedSlider), defaultValue: null);

    public static readonly BindableProperty TickCountProperty =
                           BindableProperty.Create(propertyName: nameof(TickCount), returnType: typeof(int), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(ExtendedSlider), validateValue: OnTickCountValidateValue, coerceValue: OnTickCountCoerceValue, propertyChanged: OnTickCountPropertyChanged);

    public static readonly BindableProperty TickValueProperty =
                           BindableProperty.Create(propertyName: nameof(TickValue), returnType: typeof(int), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(ExtendedSlider), coerceValue: OnTickValueCoerceValue, propertyChanged: OnTickValuePropertyChanged);

    public static readonly BindableProperty IsSnapToTickEnabledProperty =
                           BindableProperty.Create(propertyName: nameof(IsSnapToTickEnabled), returnType: typeof(bool), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(ExtendedSlider), propertyChanged: OnIsSnapToTickEnabledPropertyChanged);

    public object DragCompletedCommandParameter
    {
        get { return GetValue(DragCompletedCommandParameterProperty); }
        set { SetValue(DragCompletedCommandParameterProperty, value); }
    }

    public int TickCount
    {
        get { return (int)GetValue(TickCountProperty); }
        set { SetValue(TickCountProperty, value); }
    }
    public int TickValue
    {
        get { return (int)GetValue(TickValueProperty); }
        set { SetValue(TickValueProperty, value); }
    }
    public bool IsSnapToTickEnabled
    {
        get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
        set { SetValue(IsSnapToTickEnabledProperty, value); }
    }

    void ISlider.DragCompleted()
    {
        (this as ISliderController).SendDragCompleted();
    }

    void ISliderController.SendDragCompleted()
    {
        if (IsEnabled)
        {
            DragCompletedCommand?.Execute(DragCompletedCommandParameter);
            DragCompleted?.Invoke(this, new EventArgs());
        }
    }

    protected static bool OnTickCountValidateValue(BindableObject bindable, object value)
    {
        return (int)value >= 0;
    }

    protected static object OnTickCountCoerceValue(BindableObject bindable, object value)
    {
        var extendedSlider = (ExtendedSlider)bindable;

        extendedSlider.TickValue = Math.Clamp(extendedSlider.TickValue, 0, (int)value);
        return value;
    }

    protected static void OnTickCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<int>.Default.Equals((int)oldValue, (int)newValue))
            return;

        var control = bindable as ExtendedSlider;
        control?.RecalculateAll();
    }

    protected static object OnTickValueCoerceValue(BindableObject bindable, object value)
    {
        var extendedSlider = (ExtendedSlider)bindable;
        return Math.Clamp((int)value, 0, extendedSlider.TickCount);
    }

    protected static void OnTickValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<int>.Default.Equals((int)oldValue, (int)newValue))
            return;

        var control = bindable as ExtendedSlider;
        control?.RecalculateValue();
    }

    protected static void OnIsSnapToTickEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<bool>.Default.Equals((bool)oldValue, (bool)newValue))
            return;

        var control = bindable as ExtendedSlider;
        if ((bool)newValue)
            control?.RecalculateValue();
        control?.RecalculateAll();
    }

    protected virtual void OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        RecalculateAll();
    }

    protected virtual void CalculateTickFrequency()
    {
        var tickFrequency = Math.Max(0, (Maximum - Minimum) / TickCount);
        _tickFrequency = double.IsInfinity(tickFrequency) ? 0 : tickFrequency;
    }

    protected virtual void RecalculateTickValue()
    {
        int newTickValue = 0;
        try
        {
            if (_tickFrequency == 0)
                CalculateTickFrequency();
            if (_tickFrequency == 0)
                return;

            newTickValue = (int)Math.Round((Value - Minimum) / _tickFrequency);
        }
        finally
        {
            if (!EqualityComparer<int>.Default.Equals(TickValue, newTickValue))
                TickValue = newTickValue;
        }
    }

    protected virtual void RecalculateValue()
    {
        if (!IsSnapToTickEnabled)
            return;

        double newValue = Minimum;
        try
        {
            if (_tickFrequency == 0)
                CalculateTickFrequency();
            if (_tickFrequency == 0)
                return;

            newValue = TickValue * _tickFrequency + Minimum;
        }
        finally
        {
            if (!EqualityComparer<double>.Default.Equals(Value, newValue))
                Value = newValue;
        }
    }

    protected virtual void RecalculateAll()
    {
        if (!IsSnapToTickEnabled)
            return;

        CalculateTickFrequency();

        RecalculateTickValue();

        RecalculateValue();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }

    private double _tickFrequency;
}
