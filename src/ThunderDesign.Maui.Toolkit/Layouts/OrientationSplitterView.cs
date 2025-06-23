namespace ThunderDesign.Maui.Toolkit.Layouts;

public class OrientationSplitterView : ContentView
{
    public OrientationSplitterView()
    {
        _FirstContentView.BackgroundColor = Colors.Transparent;
        _SecondContentView.BackgroundColor = Colors.Transparent;

        _MainStackLayout.Spacing = 0;
        _MainStackLayout.Children.Add(_FirstContentView);
        _MainStackLayout.Children.Add(_SecondContentView);

        base.Content = _MainStackLayout;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            // Equivalent to Loaded
            ScreenOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }
        else
        {
            // Equivalent to Unloaded
            DeviceDisplay.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
        }
    }

    public new static readonly BindableProperty ContentProperty =
                               BindableProperty.Create(propertyName: nameof(Content), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.OneWay, declaringType: typeof(OrientationSplitterView));
    public static readonly BindableProperty ScreenOrientationProperty =
                           BindableProperty.Create(propertyName: nameof(ScreenOrientation), returnType: typeof(DisplayOrientation), defaultValue: DisplayOrientation.Unknown, defaultBindingMode: BindingMode.OneWay, declaringType: typeof(OrientationSplitterView));
    public static readonly BindableProperty FirstViewProperty =
                           BindableProperty.Create(propertyName: nameof(FirstView), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(OrientationSplitterView), propertyChanged: OnFirstViewPropertyChanged);
    public static readonly BindableProperty SecondViewProperty =
                           BindableProperty.Create(propertyName: nameof(SecondView), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(OrientationSplitterView), propertyChanged: OnSecondViewPropertyChanged);
    public static readonly BindableProperty SplitPercentageProperty =
                           BindableProperty.Create(propertyName: nameof(SplitPercentage), returnType: typeof(double), defaultValue: 0.5d, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(OrientationSplitterView), coerceValue: OnSplitPercentageCoerceValue, propertyChanged: OnSplitPercentagePropertyChanged);

    public new View Content
    {
        get { return base.Content; }
    }

    public DisplayOrientation ScreenOrientation
    {
        get { return (DisplayOrientation)GetValue(ScreenOrientationProperty); }
        private set { SetValue(ScreenOrientationProperty, value); }
    }

    public View FirstView
    {
        get { return (View)GetValue(FirstViewProperty); }
        set { SetValue(FirstViewProperty, value); }
    }

    public View SecondView
    {
        get { return (View)GetValue(SecondViewProperty); }
        set { SetValue(SecondViewProperty, value); }
    }

    public double SplitPercentage
    {
        get { return (double)GetValue(SplitPercentageProperty); }
        set { SetValue(SplitPercentageProperty, value); }
    }

    protected static void OnFirstViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<View>.Default.Equals((View)oldValue, (View)newValue))
            return;

        var control = bindable as OrientationSplitterView;
        control._FirstContentView.Content = (View)newValue;
    }

    protected static void OnSecondViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<View>.Default.Equals((View)oldValue, (View)newValue))
            return;

        var control = bindable as OrientationSplitterView;
        control._SecondContentView.Content = (View)newValue;
    }

    protected static object OnSplitPercentageCoerceValue(BindableObject bindable, object value)
    {
        return Math.Clamp((double)value, 0d, 1d);
    }

    protected static void OnSplitPercentagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<double>.Default.Equals((double)oldValue, (double)newValue))
            return;

        var control = bindable as OrientationSplitterView;
        control.InvalidateLayout();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width != -1 && height != -1)
        {
            RecalculateSize(width, height);
        }
    }

    private void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        ScreenOrientation = e.DisplayInfo.Orientation;
        InvalidateLayout();
    }

    private void RecalculateSize(double width, double height)
    {
        switch (ScreenOrientation)
        {
            case DisplayOrientation.Unknown:
            case DisplayOrientation.Portrait:
                _MainStackLayout.Orientation = StackOrientation.Vertical;
                _FirstContentView.WidthRequest = width;
                _SecondContentView.WidthRequest = width;
                _FirstContentView.HeightRequest = height * SplitPercentage;
                _SecondContentView.HeightRequest = height * (1 - SplitPercentage);
                break;
            case DisplayOrientation.Landscape:
                _MainStackLayout.Orientation = StackOrientation.Horizontal;
                _FirstContentView.WidthRequest = width * SplitPercentage;
                _SecondContentView.WidthRequest = width * (1 - SplitPercentage);
                _FirstContentView.HeightRequest = height;
                _SecondContentView.HeightRequest = height;
                break;
        }    
    }

    protected readonly StackLayout _MainStackLayout = new StackLayout();
    protected readonly ContentView _FirstContentView = new ContentView();
    protected readonly ContentView _SecondContentView = new ContentView();
}