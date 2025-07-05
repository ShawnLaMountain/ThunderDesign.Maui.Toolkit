using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ThunderDesign.Maui.Toolkit.Controls;

/// <summary>
/// Defines the size variants for FloatingActionButton according to Material Design specifications.
/// </summary>
public enum FloatingActionButtonSize
{
    /// <summary>
    /// Normal size (56dp).
    /// </summary>
    Normal,
    
    /// <summary>
    /// Mini size (40dp).
    /// </summary>
    Mini,
    
    /// <summary>
    /// Custom size defined by user.
    /// </summary>
    Custom
}

/// <summary>
/// Defines the elevation level according to Material 3 design specifications.
/// </summary>
public enum MaterialElevation
{
    Level0 = 0,  // 0dp - No elevation
    Level1 = 1,  // 1dp - Subtle elevation for cards, etc.
    Level2 = 2,  // 3dp - Default for FABs
    Level3 = 3,  // 6dp - Higher elevation for dialogs
    Level4 = 4,  // 8dp - Highest elevation for modals
    Level5 = 5   // 12dp - Maximum elevation for temporary surfaces
}

/// <summary>
/// Defines the position variants for FloatingActionButton.
/// </summary>
public enum FabPosition
{
    BottomRight,
    BottomLeft,
    TopRight,
    TopLeft,
    Center
}

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
        
        // Material 3 standard size
        double size = 56;
        HeightRequest = size;
        WidthRequest = size;
        
        // For a perfectly circular button, radius should be half the size
        CornerRadius = (int)(size / 2); 
        
        // Set up shadows according to Material 3 elevation system
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
        
        // Setup visual state manager
        SetupVisualStateManager();
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
    /// Identifies the FabSize bindable property.
    /// </summary>
    public static readonly BindableProperty FabSizeProperty =
        BindableProperty.Create(nameof(FabSize), typeof(FloatingActionButtonSize), typeof(FloatingActionButton),
            FloatingActionButtonSize.Normal, propertyChanged: OnFabSizePropertyChanged);
    
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

    /// <summary>
    /// Identifies the Position bindable property.
    /// </summary>
    public static readonly BindableProperty PositionProperty =
        BindableProperty.Create(nameof(Position), typeof(string), typeof(FloatingActionButton), 
            "BottomRight", propertyChanged: OnPositionPropertyChanged);

    /// <summary>
    /// Identifies the ShowOnlyWhenScrolledUp bindable property.
    /// </summary>
    public static readonly BindableProperty ShowOnlyWhenScrolledUpProperty =
        BindableProperty.Create(nameof(ShowOnlyWhenScrolledUp), typeof(bool), typeof(FloatingActionButton), false);

    /// <summary>
    /// Identifies the GradientStart bindable property.
    /// </summary>
    public static readonly BindableProperty GradientStartProperty =
        BindableProperty.Create(nameof(GradientStart), typeof(Color), typeof(FloatingActionButton), Colors.Transparent);

    /// <summary>
    /// Identifies the GradientEnd bindable property.
    /// </summary>
    public static readonly BindableProperty GradientEndProperty =
        BindableProperty.Create(nameof(GradientEnd), typeof(Color), typeof(FloatingActionButton), Colors.Transparent);

    /// <summary>
    /// Identifies the EnableStateLayers bindable property.
    /// </summary>
    public static readonly BindableProperty EnableStateLayersProperty =
        BindableProperty.Create(nameof(EnableStateLayers), typeof(bool), typeof(FloatingActionButton),
            true);

    /// <summary>
    /// Identifies the StateLayerOpacity bindable property.
    /// </summary>
    public static readonly BindableProperty StateLayerOpacityProperty =
        BindableProperty.Create(nameof(StateLayerOpacity), typeof(float), typeof(FloatingActionButton),
            0.12f);

    /// <summary>
    /// Identifies the UseMaterial3Colors bindable property.
    /// </summary>
    public static readonly BindableProperty UseMaterial3ColorsProperty =
        BindableProperty.Create(nameof(UseMaterial3Colors), typeof(bool), typeof(FloatingActionButton),
            true, propertyChanged: OnUseMaterial3ColorsPropertyChanged);

    /// <summary>
    /// Identifies the Elevation bindable property.
    /// </summary>
    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(nameof(Elevation), typeof(MaterialElevation), typeof(FloatingActionButton),
            MaterialElevation.Level2, propertyChanged: OnElevationPropertyChanged);
    
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
    /// Gets or sets the size variant of the FAB.
    /// </summary>
    public FloatingActionButtonSize FabSize
    {
        get => (FloatingActionButtonSize)GetValue(FabSizeProperty);
        set => SetValue(FabSizeProperty, value);
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

    /// <summary>
    /// Gets or sets the position of the FAB.
    /// </summary>
    public string Position
    {
        get => (string)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the FAB should only be visible when scrolled up.
    /// </summary>
    public bool ShowOnlyWhenScrolledUp
    {
        get => (bool)GetValue(ShowOnlyWhenScrolledUpProperty);
        set => SetValue(ShowOnlyWhenScrolledUpProperty, value);
    }

    /// <summary>
    /// Gets or sets the start color for the gradient background.
    /// </summary>
    public Color GradientStart
    {
        get => (Color)GetValue(GradientStartProperty);
        set => SetValue(GradientStartProperty, value);
    }

    /// <summary>
    /// Gets or sets the end color for the gradient background.
    /// </summary>
    public Color GradientEnd
    {
        get => (Color)GetValue(GradientEndProperty);
        set => SetValue(GradientEndProperty, value);
    }

    /// <summary>
    /// Gets or sets whether Material state layers are enabled for hover/pressed states.
    /// </summary>
    public bool EnableStateLayers
    {
        get => (bool)GetValue(EnableStateLayersProperty);
        set => SetValue(EnableStateLayersProperty, value);
    }

    /// <summary>
    /// Gets or sets the opacity for state layers (hover/pressed states).
    /// </summary>
    public float StateLayerOpacity
    {
        get => (float)GetValue(StateLayerOpacityProperty);
        set => SetValue(StateLayerOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the FAB should use Material 3 color tokens.
    /// </summary>
    public bool UseMaterial3Colors
    {
        get => (bool)GetValue(UseMaterial3ColorsProperty);
        set => SetValue(UseMaterial3ColorsProperty, value);
    }

    /// <summary>
    /// Gets or sets the Material 3 elevation level of the FAB.
    /// </summary>
    public MaterialElevation Elevation
    {
        get => (MaterialElevation)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
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
        
        // Update FabSize property to stay in sync with IsMini
        if (control != null)
        {
            control.FabSize = ((bool)newValue) ? FloatingActionButtonSize.Mini : FloatingActionButtonSize.Normal;
        }
    }

    protected static void OnFabSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (EqualityComparer<FloatingActionButtonSize>.Default.Equals((FloatingActionButtonSize)oldValue, (FloatingActionButtonSize)newValue))
            return;

        var control = bindable as FloatingActionButton;
        
        // Update IsMini property to stay in sync with FabSize
        if (control != null)
        {
            if ((FloatingActionButtonSize)newValue == FloatingActionButtonSize.Mini)
                control.IsMini = true;
            else if ((FloatingActionButtonSize)newValue == FloatingActionButtonSize.Normal)
                control.IsMini = false;
                
            control.UpdateSize();
        }
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

    protected static void OnPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FloatingActionButton;
        control?.UpdatePosition();
    }

    private static void OnUseMaterial3ColorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FloatingActionButton;
        control?.UpdateMaterial3Colors();
    }

    private static void OnElevationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FloatingActionButton;
        control?.UpdateElevation();
    }
    
    #endregion
    
    #region Methods
    
    private ScrollView? _attachedScrollView;

    /// <summary>
    /// Updates the size of the FAB based on the FabSize and IsMini properties.
    /// </summary>
    protected virtual void UpdateSize()
    {
        double size;
        
        switch (FabSize)
        {
            case FloatingActionButtonSize.Mini: // Small FAB in Material 3
                size = 40; // Material 3 small FAB is 40dp
                break;
                
            case FloatingActionButtonSize.Normal: // Regular FAB
                size = 56; // Material 3 standard FAB is 56dp
                break;
                
            default:
                size = IsMini ? 40 : 56; // Fallback for backward compatibility
                break;
        }
        
        HeightRequest = size;
        
        if (IsExtended)
        {
            WidthRequest = -1; // Auto width for extended FAB
            // For extended FABs, use standard corner radius according to Material Design
            CornerRadius = (int)(size / 2);
        }
        else
        {
            WidthRequest = size; // Equal width and height for circular FAB
            // For a circle, corner radius should be half the width/height
            CornerRadius = (int)(size / 2);
        }
        
        UpdatePadding();
    }

    /// <summary>
    /// Updates the padding of the FAB based on its type.
    /// </summary>
    protected virtual void UpdatePadding()
    {
        if (IsExtended)
        {
            // Material 3 extended FAB has 16dp horizontal padding and 16dp minimum spacing between icon and label
            Padding = new Thickness(16, 0);
            ContentLayout = new Microsoft.Maui.Controls.Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 16);
        }
        else
        {
            // Regular/mini FAB padding for icon
            Padding = IsMini ? new Thickness(8) : new Thickness(16);
        }
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
    /// Updates the position of the FAB.
    /// </summary>
    protected virtual void UpdatePosition()
    {
        switch (Position)
        {
            case "BottomRight":
                VerticalOptions = LayoutOptions.End;
                HorizontalOptions = LayoutOptions.End;
                break;
            case "BottomLeft":
                VerticalOptions = LayoutOptions.End;
                HorizontalOptions = LayoutOptions.Start;
                break;
            case "TopRight":
                VerticalOptions = LayoutOptions.Start;
                HorizontalOptions = LayoutOptions.End;
                break;
            case "TopLeft":
                VerticalOptions = LayoutOptions.Start;
                HorizontalOptions = LayoutOptions.Start;
                break;
            default:
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Center;
                break;
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
    /// Updates the Material 3 colors for the FAB.
    /// </summary>
    protected virtual void UpdateMaterial3Colors()
    {
        if (!UseMaterial3Colors)
            return;
        
        // Try to find Material 3 color tokens from resources
        if (Application.Current?.Resources != null)
        {
            // Try to find primary and on-primary colors from Material 3 color system
            if (Application.Current.Resources.TryGetValue("PrimaryContainer", out var primaryColor))
            {
                ButtonColor = (Color)primaryColor;
            }
            
            if (Application.Current.Resources.TryGetValue("OnPrimaryContainer", out var onPrimaryColor))
            {
                IconTintColor = (Color)onPrimaryColor;
            }
        }
    }

    /// <summary>
    /// Updates the elevation of the FAB based on the Elevation property.
    /// </summary>
    protected virtual void UpdateElevation()
    {
        if (!HasShadow)
            return;
        
        // Material 3 elevation mapping to shadow properties
        switch (Elevation)
        {
            case MaterialElevation.Level0:
                ShadowRadius = 0;
                ShadowOpacity = 0;
                ShadowOffset = new Point(0, 0);
                break;
            case MaterialElevation.Level1:
                ShadowRadius = 2;
                ShadowOpacity = 0.2f;
                ShadowOffset = new Point(0, 1);
                break;
            case MaterialElevation.Level2: // Default for FAB
                ShadowRadius = 6;
                ShadowOpacity = 0.3f;
                ShadowOffset = new Point(0, 3);
                break;
            case MaterialElevation.Level3:
                ShadowRadius = 8;
                ShadowOpacity = 0.35f;
                ShadowOffset = new Point(0, 4);
                break;
            case MaterialElevation.Level4:
                ShadowRadius = 12;
                ShadowOpacity = 0.4f;
                ShadowOffset = new Point(0, 6);
                break;
            case MaterialElevation.Level5:
                ShadowRadius = 16;
                ShadowOpacity = 0.45f;
                ShadowOffset = new Point(0, 8);
                break;
        }
        
        UpdateShadow();
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
        else if (propertyName == nameof(IsPressed) && EnableStateLayers)
        {
            // Apply state layer effect when pressed
            UpdateStateLayer();
        }
    }

    private void UpdateStateLayer()
    {
        if (!EnableStateLayers)
            return;
        
        if (IsPressed)
        {
            // Apply pressed state - darken the color slightly using state layer
            var darkened = new Color(
                ButtonColor.Red * (1 - StateLayerOpacity),
                ButtonColor.Green * (1 - StateLayerOpacity),
                ButtonColor.Blue * (1 - StateLayerOpacity),
                ButtonColor.Alpha
            );
            BackgroundColor = darkened;
        }
        else
        {
            // Restore normal state
            BackgroundColor = ButtonColor;
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
    
    /// <summary>
    /// Sets up the visual state manager for the FAB.
    /// </summary>
    private void SetupVisualStateManager()
    {
        VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList
        {
            new VisualStateGroup
            {
                Name = "CommonStates",
                States =
                {
                    new VisualState
                    {
                        Name = "Normal"
                    },
                    new VisualState
                    {
                        Name = "PointerOver",
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 0.9 }
                        }
                    },
                    new VisualState
                    {
                        Name = "Pressed",
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 0.8 }
                        }
                    },
                    new VisualState
                    {
                        Name = "Disabled",
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 0.5 }
                        }
                    }
                }
            }
        });
    }
    
    #endregion
}