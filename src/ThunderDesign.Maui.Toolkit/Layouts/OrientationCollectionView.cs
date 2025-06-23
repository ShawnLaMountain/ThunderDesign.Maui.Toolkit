using System;
using System.Collections.Generic;

namespace ThunderDesign.Maui.Toolkit.Layouts;

public class OrientationCollectionView : CollectionView
{
    public OrientationCollectionView()
    {
        ScreenOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            // Equivalent to Loaded
            ScreenOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
            RefreshLayout();
        }
        else
        {
            // Equivalent to Unloaded
            DeviceDisplay.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
        }
    }

    //Hide property
    private new static BindableProperty ItemsLayoutProperty => throw new NotImplementedException();

    public static readonly BindableProperty InvertOrientationLayoutProperty =
                           BindableProperty.Create(propertyName: nameof(InvertOrientationLayout), returnType: typeof(bool), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, declaringType: typeof(OrientationCollectionView), propertyChanged: OnInvertOrientationLayoutPropertyChanged);
    public static readonly BindableProperty ItemSpacingProperty =
                           BindableProperty.Create(propertyName: nameof(ItemSpacing), returnType: typeof(double), defaultValue: default(double), declaringType: typeof(GridItemsLayout), validateValue: (bindable, value) => (double)value >= 0, propertyChanged: OnItemSpacingPropertyChanged);
    public static readonly BindableProperty ScreenOrientationProperty =
                           BindableProperty.Create(propertyName: nameof(ScreenOrientation), returnType: typeof(DisplayOrientation), defaultValue: DisplayOrientation.Unknown, defaultBindingMode: BindingMode.OneWay, declaringType: typeof(OrientationCollectionView));
    public static readonly BindableProperty SpanProperty =
                           BindableProperty.Create(propertyName: nameof(Span), returnType: typeof(int), defaultValue: 1, declaringType: typeof(OrientationCollectionView), validateValue: (bindable, value) => (int)value >= 1, propertyChanged: OnSpanPropertyChanged);

    private new IItemsLayout ItemsLayout
    {
        get => base.ItemsLayout;
        set => base.ItemsLayout = value;
    }

    public bool InvertOrientationLayout
    {
        get => (bool)GetValue(InvertOrientationLayoutProperty);
        private set => SetValue(InvertOrientationLayoutProperty, value);
    }

    public double ItemSpacing
    {
        get => (double)GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    public DisplayOrientation ScreenOrientation
    {
        get => (DisplayOrientation)GetValue(ScreenOrientationProperty);
        private set => SetValue(ScreenOrientationProperty, value);
    }

    public int Span
    {
        get => (int)GetValue(SpanProperty);
        set => SetValue(SpanProperty, value);
    }

    private static void OnItemSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<double>.Default.Equals((double)oldValue, (double)newValue))
            return;

        var control = bindable as OrientationCollectionView;
        if (control == null)
            return;

        var itemsLayout = control.ItemsLayout as GridItemsLayout;
        if (itemsLayout != null)
        {
            itemsLayout.HorizontalItemSpacing = (double)newValue;
            itemsLayout.VerticalItemSpacing = (double)newValue;
        }
    }

    private static void OnInvertOrientationLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<bool>.Default.Equals((bool)oldValue, (bool)newValue))
            return;

        var control = bindable as OrientationCollectionView;
        if (control == null)
            return;

        control.RefreshLayout();
    }

    private static void OnSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<int>.Default.Equals((int)oldValue, (int)newValue))
            return;

        var control = bindable as OrientationCollectionView;
        if (control == null)
            return;

        var itemsLayout = control.ItemsLayout as GridItemsLayout;
        if (itemsLayout != null)
            itemsLayout.Span = (int)newValue;
    }

    private void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        ScreenOrientation = e.DisplayInfo.Orientation;
        RefreshLayout();
    }

    private void RefreshLayout()
    {
        ItemsLayoutOrientation GetItemsLayoutOrientation()
        {
            switch (ScreenOrientation)
            {
                case DisplayOrientation.Landscape:
                    return InvertOrientationLayout ? ItemsLayoutOrientation.Vertical : ItemsLayoutOrientation.Horizontal;
                case DisplayOrientation.Unknown:
                case DisplayOrientation.Portrait:
                default:
                    return InvertOrientationLayout ? ItemsLayoutOrientation.Horizontal : ItemsLayoutOrientation.Vertical;
            }
        };

        InternalItemsLayout = new GridItemsLayout(Span, GetItemsLayoutOrientation()) { HorizontalItemSpacing = ItemSpacing, VerticalItemSpacing = ItemSpacing };
    }
}
