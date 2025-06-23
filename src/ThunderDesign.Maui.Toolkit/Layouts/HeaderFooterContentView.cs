namespace ThunderDesign.Maui.Toolkit.Layouts;

public class HeaderFooterContentView : ContentView
{
    public HeaderFooterContentView()
    {
        _mainGrid.ColumnDefinitions = new ColumnDefinitionCollection
        {
            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
        };
        _mainGrid.RowDefinitions = new RowDefinitionCollection
        {
            new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
            new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
            new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
        };
        _mainGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
        _mainGrid.VerticalOptions = LayoutOptions.FillAndExpand;
        base.Content = _mainGrid;
    }

    public new static readonly BindableProperty ContentProperty =
                               BindableProperty.Create(propertyName: nameof(Content), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.OneWay, declaringType: typeof(HeaderFooterContentView));
    public static readonly BindableProperty HeaderProperty =
                           BindableProperty.Create(propertyName: nameof(Header), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(HeaderFooterContentView), propertyChanged: OnHeaderPropertyChanged);
    public static readonly BindableProperty BodyProperty =
                           BindableProperty.Create(propertyName: nameof(Body), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(HeaderFooterContentView), propertyChanged: OnBodyPropertyChanged);
    public static readonly BindableProperty FooterProperty =
                           BindableProperty.Create(propertyName: nameof(Footer), returnType: typeof(View), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(HeaderFooterContentView), propertyChanged: OnFooterPropertyChanged);

    public new View Content
    {
        get { return base.Content; }
    }

    public View Header
    {
        get { return (View)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public View Body
    {
        get { return (View)GetValue(BodyProperty); }
        set { SetValue(BodyProperty, value); }
    }

    public View Footer
    {
        get { return (View)GetValue(FooterProperty); }
        set { SetValue(FooterProperty, value); }
    }

    protected static void OnHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<View>.Default.Equals((View)oldValue, (View)newValue))
            return;

        var control = bindable as HeaderFooterContentView;
        if (control?._mainGrid != null && oldValue is View oldView)
            control._mainGrid.Children.Remove(oldView);
        if (control?._mainGrid != null && newValue is View newView)
        {
            Grid.SetRow(newView, 0);
            Grid.SetColumn(newView, 0);
            control._mainGrid.Children.Add(newView);
        }
    }

    protected static void OnBodyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<View>.Default.Equals((View)oldValue, (View)newValue))
            return;

        var control = bindable as HeaderFooterContentView;
        if (control?._mainGrid != null && oldValue is View oldView)
            control._mainGrid.Children.Remove(oldView);
        if (control?._mainGrid != null && newValue is View newView)
        {
            Grid.SetRow(newView, 1);
            Grid.SetColumn(newView, 0);
            control._mainGrid.Children.Add(newView);
        }
    }

    protected static void OnFooterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<View>.Default.Equals((View)oldValue, (View)newValue))
            return;

        var control = bindable as HeaderFooterContentView;
        if (control?._mainGrid != null && oldValue is View oldView)
            control._mainGrid.Children.Remove(oldView);
        if (control?._mainGrid != null && newValue is View newView)
        {
            Grid.SetRow(newView, 2);
            Grid.SetColumn(newView, 0);
            control._mainGrid.Children.Add(newView);
        }
    }

    protected readonly Grid _mainGrid = new Grid();
}