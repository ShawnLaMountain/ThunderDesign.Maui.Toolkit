using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ThunderDesign.Maui.Toolkit.Controls;

/// <summary>
/// A Material Design Floating Action Button (FAB) implementation for .NET MAUI.
/// </summary>
public class FloatingActionButton : Button
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the FloatingActionButton class.
    /// </summary>
    public FloatingActionButton() : base()
    {
        // Set default values
        Padding = 0;
        CornerRadius = 28; // Default size for standard FAB
        
        // Set default sizing
        HeightRequest = 56;
        WidthRequest = 56;
        
        // Set up shadows
        Shadow = new Shadow
        {
            Brush = new SolidColorBrush(ShadowColor),
            Offset = new Point(0, 3),
            Radius = 6,
            Opacity = 0.3f
        };
        
        // Setup default visual appearance
        UpdateVisualState();
        
        // Connect to parent change events
        Loaded += OnFloatingActionButtonLoaded!;
        Unloaded += OnFloatingActionButtonUnloaded!;
    }
    
    #endregion
    
    #region Bindable Properties
    
    // Shadow Properties
    
    /// <summary>
    /// Identifies the ShadowColor bindable property.
    /// </summary>
    public static readonly BindableProperty ShadowColorProperty =
        BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(FloatingActionButton), 
            Colors.Black, propertyChanged: OnShadowPropertyChanged);
    
    /// <summary>
    /// Identifies the ShadowOffset bindable property.
    /// </summary>
    public static readonly BindableProperty ShadowOffsetProperty =
        BindableProperty.Create(nameof(ShadowOffset), typeof(Point), typeof(FloatingActionButton), 
            new Point(0, 3), propertyChanged: OnShadowPropertyChanged);
    
    /// <summary>
    /// Identifies the ShadowOpacity bindable property.
    /// </summary>
    public static readonly BindableProperty ShadowOpacityProperty =
        BindableProperty.Create(nameof(ShadowOpacity), typeof(float), typeof(FloatingActionButton), 
            0.3f, propertyChanged: OnShadowPropertyChanged);
    
    /// <summary>
    /// Identifies the ShadowRadius bindable property.
    /// </summary>
    public static readonly BindableProperty ShadowRadiusProperty =
        BindableProperty.Create(nameof(ShadowRadius), typeof(float), typeof(FloatingActionButton), 
            6f, propertyChanged: OnShadowPropertyChanged);
    
    /// <summary>
    /// Identifies the HasShadow bindable property.
    /// </summary>
    public static readonly BindableProperty HasShadowProperty =
        BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(FloatingActionButton), 
            true, propertyChanged: OnShadowPropertyChanged);
    
    // Animation Properties
    
    /// <summary>
    /// Identifies the AnimationEnabled bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationEnabledProperty =
        BindableProperty.Create(nameof(AnimationEnabled), typeof(bool), typeof(FloatingActionButton), true);
    
    /// <summary>
    /// Identifies the AnimationShowDelay bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationShowDelayProperty =
        BindableProperty.Create(nameof(AnimationShowDelay), typeof(int), typeof(FloatingActionButton), 300);
    
    /// <summary>
    /// Identifies the AnimationShowDuration bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationShowDurationProperty =
        BindableProperty.Create(nameof(AnimationShowDuration), typeof(uint), typeof(FloatingActionButton), 250u);
    
    /// <summary>
    /// Identifies the AnimationHideDuration bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationHideDurationProperty =
        BindableProperty.Create(nameof(AnimationHideDuration), typeof(uint), typeof(FloatingActionButton), 250u);
    
    /// <summary>
    /// Identifies the AnimationShowEasing bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationShowEasingProperty =
        BindableProperty.Create(nameof(AnimationShowEasing), typeof(Easing), typeof(FloatingActionButton), Easing.SpringOut);
    
    /// <summary>
    /// Identifies the AnimationHideEasing bindable property.
    /// </summary>
    public static readonly BindableProperty AnimationHideEasingProperty =
        BindableProperty.Create(nameof(AnimationHideEasing), typeof(Easing), typeof(FloatingActionButton), Easing.SpringIn);
    
    // Appearance Properties
    
    /// <summary>
    /// Identifies the IsMini bindable property.
    /// </summary>
    public static readonly BindableProperty IsMiniProperty =
        BindableProperty.Create(nameof(IsMini), typeof(bool), typeof(FloatingActionButton), 
            false, propertyChanged: OnIsMiniPropertyChanged);
    
    /// <summary>
    /// Identifies the ButtonColor bindable property.
    /// </summary>
    public static readonly BindableProperty ButtonColorProperty =
        BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(FloatingActionButton), 
            Colors.Blue, propertyChanged: OnButtonColorPropertyChanged);
    
    /// <summary>
    /// Identifies the Icon bindable property.
    /// </summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(FloatingActionButton), 
            null, propertyChanged: OnIconPropertyChanged);
    
    /// <summary>
    /// Identifies the IconTintColor bindable property.
    /// </summary>
    public static readonly BindableProperty IconTintColorProperty =
        BindableProperty.Create(nameof(IconTintColor), typeof(Color), typeof(FloatingActionButton), 
            Colors.White, propertyChanged: OnIconTintColorPropertyChanged);
    
    /// <summary>
    /// Identifies the IsExtended bindable property.
    /// </summary>
    public static readonly BindableProperty IsExtendedProperty =
        BindableProperty.Create(nameof(IsExtended), typeof(bool), typeof(FloatingActionButton), 
            false, propertyChanged: OnIsExtendedPropertyChanged);
    
    /// <summary>
    /// Identifies the AutoHideOnScroll bindable property.
    /// </summary>
    public static readonly BindableProperty AutoHideOnScrollProperty =
        BindableProperty.Create(nameof(AutoHideOnScroll), typeof(bool), typeof(FloatingActionButton), 
            false, propertyChanged: OnAutoHideOnScrollPropertyChanged);

    /// <summary>
    /// Identifies the ScrollView bindable property.
    /// </summary>
    public static readonly BindableProperty ScrollViewProperty =
        BindableProperty.Create(nameof(ScrollView), typeof(ScrollView), typeof(FloatingActionButton),
            null, propertyChanged: OnScrollViewPropertyChanged);
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// Gets or sets the shadow color.
    /// </summary>
    public Color ShadowColor
    {
        get => (Color)GetValue(ShadowColorProperty);
        set => SetValue(ShadowColorProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the shadow offset.
    /// </summary>
    public Point ShadowOffset
    {
        get => (Point)GetValue(ShadowOffsetProperty);
        set => SetValue(ShadowOffsetProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the shadow opacity.
    /// </summary>
    public float ShadowOpacity
    {
        get => (float)GetValue(ShadowOpacityProperty);
        set => SetValue(ShadowOpacityProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the shadow radius.
    /// </summary>
    public float ShadowRadius
    {
        get => (float)GetValue(ShadowRadiusProperty);
        set => SetValue(ShadowRadiusProperty, value);
    }
    
    /// <summary>
    /// Gets or sets whether the button has shadow effect.
    /// </summary>
    public bool HasShadow
    {
        get => (bool)GetValue(HasShadowProperty);
        set => SetValue(HasShadowProperty, value);
    }
    
    /// <summary>
    /// Gets or sets whether animations are enabled.
    /// </summary>
    public bool AnimationEnabled
    {
        get => (bool)GetValue(AnimationEnabledProperty);
        set => SetValue(AnimationEnabledProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the delay before showing the button, in milliseconds.
    /// </summary>
    public int AnimationShowDelay
    {
        get => (int)GetValue(AnimationShowDelayProperty);
        set => SetValue(AnimationShowDelayProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the duration of the show animation, in milliseconds.
    /// </summary>
    public uint AnimationShowDuration
    {
        get => (uint)GetValue(AnimationShowDurationProperty);
        set => SetValue(AnimationShowDurationProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the duration of the hide animation, in milliseconds.
    /// </summary>
    public uint AnimationHideDuration
    {
        get => (uint)GetValue(AnimationHideDurationProperty);
        set => SetValue(AnimationHideDurationProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the easing function for the show animation.
    /// </summary>
    public Easing AnimationShowEasing
    {
        get => (Easing)GetValue(AnimationShowEasingProperty);
        set => SetValue(AnimationShowEasingProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the easing function for the hide animation.
    /// </summary>
    public Easing AnimationHideEasing
    {
        get => (Easing)GetValue(AnimationHideEasingProperty);
        set => SetValue(AnimationHideEasingProperty, value);
    }
    
    /// <summary>
    /// Gets or sets whether the button is mini size (40dp instead of 56dp).
    /// </summary>
    public bool IsMini
    {
        get => (bool)GetValue(IsMiniProperty);
        set => SetValue(IsMiniProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the background color of the button.
    /// </summary>
    public Color ButtonColor
    {
        get => (Color)GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the icon image source for the FAB.
    /// </summary>
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the tint color for the icon and text.
    /// </summary>
    public Color IconTintColor
    {
        get => (Color)GetValue(IconTintColorProperty);
        set => SetValue(IconTintColorProperty, value);
    }
    
    /// <summary>
    /// Gets or sets whether the FAB is extended (shows text alongside the icon).
    /// </summary>
    public bool IsExtended
    {
        get => (bool)GetValue(IsExtendedProperty);
        set => SetValue(IsExtendedProperty, value);
    }
    
    /// <summary>
    /// Gets or sets whether the FAB should automatically hide on scroll when inside a ScrollView.
    /// </summary>
    public bool AutoHideOnScroll
    {
        get => (bool)GetValue(AutoHideOnScrollProperty);
        set => SetValue(AutoHideOnScrollProperty, value);
    }

    /// <summary>
    /// Gets or sets the ScrollView that this FAB should attach to for hide-on-scroll behavior.
    /// When set, this takes precedence over auto-detection.
    /// </summary>
    public ScrollView ScrollView
    {
        get => (ScrollView)GetValue(ScrollViewProperty);
        set => SetValue(ScrollViewProperty, value);
    }
    
    #endregion
    
    #region Property Change Handlers
    
    protected static void OnShadowPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FloatingActionButton;
        control?.UpdateShadow();
    }
    
    protected static void OnIsMiniPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<bool>.Default.Equals((bool)oldValue, (bool)newValue))
            return;

        var control = bindable as FloatingActionButton;
        control?.UpdateSize();
    }
    
    protected static void OnButtonColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<Color>.Default.Equals((Color)oldValue, (Color)newValue))
            return;

        var control = bindable as FloatingActionButton;
        control?.UpdateVisualState();
    }
    
    protected static void OnIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<ImageSource>.Default.Equals((ImageSource)oldValue, (ImageSource)newValue))
            return;

        var control = bindable as FloatingActionButton;
        control?.UpdateIcon();
    }
    
    protected static void OnIconTintColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<Color>.Default.Equals((Color)oldValue, (Color)newValue))
            return;

        var control = bindable as FloatingActionButton;
        control?.UpdateIconTint();
    }
    
    protected static void OnIsExtendedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<bool>.Default.Equals((bool)oldValue, (bool)newValue))
            return;

        var control = bindable as FloatingActionButton;
        control?.UpdateLayout();
    }
    
    protected static void OnAutoHideOnScrollPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FloatingActionButton fab)
        {
            fab.UpdateScrollViewAttachment();
        }
    }

    private static void OnScrollViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FloatingActionButton fab)
        {
            fab.UpdateScrollViewAttachment();
        }
    }
    
    #endregion
    
    #region Methods
    
    private ScrollView? _attachedScrollView;

    /// <summary>
    /// Updates the size of the FAB based on the IsMini property.
    /// </summary>
    protected virtual void UpdateSize()
    {
        if (IsMini)
        {
            HeightRequest = 40;
            WidthRequest = IsExtended ? -1 : 40; // Auto width when extended
            CornerRadius = 20;
        }
        else
        {
            HeightRequest = 56;
            WidthRequest = IsExtended ? -1 : 56; // Auto width when extended
            CornerRadius = 28;
        }
        
        // Update padding
        UpdatePadding();
    }
    
    /// <summary>
    /// Updates the shadow of the FAB based on shadow properties.
    /// </summary>
    protected virtual void UpdateShadow()
    {
        if (HasShadow)
        {
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(ShadowColor),
                Offset = ShadowOffset,
                Radius = ShadowRadius,
                Opacity = ShadowOpacity
            };
        }
        else
        {
            Shadow = null!; // Use null-forgiving operator to suppress warning
        }
    }
    
    /// <summary>
    /// Updates the visual state of the FAB.
    /// </summary>
    protected virtual void UpdateVisualState()
    {
        BackgroundColor = ButtonColor;
    }
    
    /// <summary>
    /// Updates the icon of the FAB.
    /// </summary>
    protected virtual void UpdateIcon()
    {
        ImageSource = Icon;
    }
    
    /// <summary>
    /// Updates the tint color for icon and text.
    /// </summary>
    protected virtual void UpdateIconTint()
    {
        TextColor = IconTintColor;
    }
    
    /// <summary>
    /// Updates the layout of the FAB based on the IsExtended property.
    /// </summary>
    protected virtual void UpdateLayout()
    {
        if (IsExtended)
        {
            // Extended FAB has flexible width
            WidthRequest = -1;
            HorizontalOptions = LayoutOptions.Start;
        }
        else
        {
            // Regular FAB has fixed width based on size
            UpdateSize();
            HorizontalOptions = LayoutOptions.Center;
        }
        
        UpdatePadding();
    }
    
    /// <summary>
    /// Updates the padding of the FAB based on its type.
    /// </summary>
    protected virtual void UpdatePadding()
    {
        // Set appropriate padding based on FAB type
        if (IsExtended)
        {
            // Extended FAB needs horizontal padding
            Padding = new Thickness(16, 0);
        }
        else
        {
            // Regular/mini FAB
            Padding = IsMini ? new Thickness(8) : new Thickness(16);
        }
    }
    
    /// <summary>
    /// Updates the attachment of the FAB to a ScrollView for auto-hide functionality.
    /// </summary>
    private void UpdateScrollViewAttachment()
    {
        // First detach from any existing scroll view
        if (_attachedScrollView != null)
        {
            Extensions.ScrollViewExtensions.DetachFab(_attachedScrollView);
            _attachedScrollView = null;
        }
        
        // Priority 1: Use explicitly set ScrollView if available
        if (ScrollView != null)
        {
            _attachedScrollView = ScrollView;
            Extensions.ScrollViewExtensions.AttachFab(_attachedScrollView, this, true);
            return;
        }
        
        // Priority 2: Auto-detect if enabled
        if (AutoHideOnScroll && Parent != null)
        {
            // Find the nearest ScrollView ancestor
            var scrollView = FindParentOfType<ScrollView>();
            
            if (scrollView != null)
            {
                _attachedScrollView = scrollView;
                Extensions.ScrollViewExtensions.AttachFab(scrollView, this, true);
            }
        }
    }
    
    // Improved version of FindParentOfType that can look inside visual trees
    private T? FindParentOfType<T>() where T : Element
    {
        Element? current = Parent;
        
        while (current != null)
        {
            if (current is T result)
            {
                return result;
            }
            
            // Special case: look inside content-holding controls
            if (current is ContentView cv && cv.Content is Element contentElement)
            {
                var found = FindInElement<T>(contentElement);
                if (found != null)
                    return found;
            }
            
            current = current.Parent;
        }
        
        return null;
    }

    private T? FindInElement<T>(Element element) where T : Element
    {
        if (element is T result)
            return result;
            
        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (child is Element childElement)
                {
                    var found = FindInElement<T>(childElement);
                    if (found != null)
                        return found;
                }
            }
        }
        else if (element is ContentView cv && cv.Content is Element content)
        {
            return FindInElement<T>(content);
        }
        
        return null;
    }
    
    /// <summary>
    /// Shows the FAB with animation.
    /// </summary>
    /// <returns>Task that completes when the animation finishes.</returns>
    public virtual async Task ShowAsync()
    {
        if (Scale == 0 || Opacity == 0)
        {
            Scale = 0;
            Opacity = 0;
            IsVisible = true;
            
            if (AnimationEnabled)
            {
                // Wait for show delay
                if (AnimationShowDelay > 0)
                {
                    await Task.Delay(AnimationShowDelay);
                }
                
                // Run show animation
                await Task.WhenAll(
                    this.ScaleTo(1, AnimationShowDuration, AnimationShowEasing),
                    this.FadeTo(1, AnimationShowDuration, AnimationShowEasing)
                );
            }
            else
            {
                Scale = 1;
                Opacity = 1;
            }
        }
    }
    
    /// <summary>
    /// Hides the FAB with animation.
    /// </summary>
    /// <returns>Task that completes when the animation finishes.</returns>
    public virtual async Task HideAsync()
    {
        if (Scale == 1 || Opacity == 1)
        {
            if (AnimationEnabled)
            {
                // Run hide animation
                await Task.WhenAll(
                    this.ScaleTo(0, AnimationHideDuration, AnimationHideEasing),
                    this.FadeTo(0, AnimationHideDuration, AnimationHideEasing)
                );
            }
            else
            {
                Scale = 0;
                Opacity = 0;
            }
            
            IsVisible = false;
        }
    }
    
    /// <summary>
    /// Called when the FAB is loaded.
    /// </summary>
    protected virtual async void OnFloatingActionButtonLoaded(object? sender, EventArgs e)
    {
        // When first loaded, start hidden
        Scale = 0;
        Opacity = 0;
        IsVisible = true;
        
        // Show with animation
        await ShowAsync();
    }
    
    /// <summary>
    /// Called when the FAB is unloaded.
    /// </summary>
    protected virtual async void OnFloatingActionButtonUnloaded(object? sender, EventArgs e)
    {
        // Hide when unloaded
        await HideAsync();
    }
    
    /// <summary>
    /// Initializes the FAB for animation.
    /// </summary>
    public virtual void InitializeAnimation()
    {
        Scale = 0;
        Opacity = 0;
    }
    
    /// <summary>
    /// Called when a property is changed.
    /// </summary>
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        
        // Handle changes to base Button properties
        if (propertyName == nameof(base.ImageSource) && base.ImageSource != Icon)
        {
            Icon = base.ImageSource;
        }
        else if (propertyName == nameof(base.BackgroundColor) && base.BackgroundColor != ButtonColor)
        {
            ButtonColor = base.BackgroundColor;
        }
        else if (propertyName == nameof(base.TextColor) && base.TextColor != IconTintColor)
        {
            IconTintColor = base.TextColor;
        }
    }
    
    /// <summary>
    /// Called when the parent of the FAB is set.
    /// </summary>
    protected override void OnParentSet()
    {
        base.OnParentSet();
        
        // When the parent changes, check if we need to attach to a ScrollView
        UpdateScrollViewAttachment();
    }

    /// <summary>
    /// Called when the handler of the FAB is changing.
    /// </summary>
    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);
        
        if (args.NewHandler == null)
        {
            // Clean up when the control is removed
            if (_attachedScrollView != null)
            {
                Extensions.ScrollViewExtensions.DetachFab(_attachedScrollView);
                _attachedScrollView = null;
            }
        }
    }
    
    #endregion
}