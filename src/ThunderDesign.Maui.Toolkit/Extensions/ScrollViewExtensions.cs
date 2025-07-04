namespace ThunderDesign.Maui.Toolkit.Extensions;

/// <summary>
/// Extension methods for ScrollView to work with FloatingActionButton.
/// </summary>
public static class ScrollViewExtensions
{
    private static readonly Dictionary<ScrollView, FabScrollState> _trackedScrollViews = 
        new Dictionary<ScrollView, FabScrollState>();
        
    private class FabScrollState
    {
        public Controls.FloatingActionButton Fab { get; set; } = null!;
        public double LastY { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAnimating { get; set; }
        public double TotalScrollDistance { get; set; } // Track cumulative scroll distance
        public DateTime LastScrollTime { get; set; } // Track time of last scroll event
    }

    /// <summary>
    /// Attaches a FloatingActionButton to respond to scroll events.
    /// </summary>
    /// <param name="scrollView">The ScrollView to attach to</param>
    /// <param name="fab">The FloatingActionButton to control</param>
    /// <param name="hideOnScroll">Whether to hide the button when scrolling down</param>
    /// <param name="threshold">The cumulative scroll distance threshold to trigger hiding</param>
    /// <returns>The ScrollView for method chaining</returns>
    public static ScrollView AttachFab(
        this ScrollView scrollView,
        Controls.FloatingActionButton fab,
        bool hideOnScroll = true,
        double threshold = 20)
    {
        if (scrollView == null || fab == null)
            return scrollView;

        // Clean up any existing handler
        DetachFab(scrollView);
        
        // Initialize with button's actual visibility state
        bool isCurrentlyVisible = fab.Opacity > 0 && fab.Scale > 0 && fab.IsVisible;
        
        // Store initial state
        _trackedScrollViews[scrollView] = new FabScrollState 
        { 
            Fab = fab, 
            LastY = 0, 
            IsVisible = isCurrentlyVisible,
            IsAnimating = false,
            TotalScrollDistance = 0,
            LastScrollTime = DateTime.Now
        };

        // Only attach the event if hide-on-scroll is enabled
        if (hideOnScroll)
        {
            scrollView.Scrolled += OnScrollViewScrolled;
            
            // Also handle when reaching the top/bottom of the scroll area
            scrollView.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(ScrollView.ScrollY))
                {
                    if (scrollView.ScrollY <= 0 && _trackedScrollViews.TryGetValue(scrollView, out var state) && !state.IsVisible)
                    {
                        // Force show when at the top
                        ShowFab(scrollView, state);
                    }
                }
            };
        }

        return scrollView;
    }

    private static async void OnScrollViewScrolled(object? sender, ScrolledEventArgs e)
    {
        if (sender is not ScrollView scrollView || !_trackedScrollViews.TryGetValue(scrollView, out var state))
            return;

        if (state.IsAnimating)
            return;
            
        // Calculate the scroll direction and distance
        double currentY = e.ScrollY;
        double deltaY = currentY - state.LastY;
        
        // Update last position
        state.LastY = currentY;
        
        // Skip very tiny movements (less than 1px) that could be noise
        if (Math.Abs(deltaY) < 1)
            return;
            
        var now = DateTime.Now;
        var timeSinceLastScroll = (now - state.LastScrollTime).TotalMilliseconds;
        state.LastScrollTime = now;
        
        // Reset accumulated scroll if there was a significant pause or direction change
        if (timeSinceLastScroll > 300 || (state.TotalScrollDistance != 0 && Math.Sign(deltaY) != Math.Sign(state.TotalScrollDistance)))
        {
            state.TotalScrollDistance = 0;
        }
        
        // Add to the cumulative scroll distance
        state.TotalScrollDistance += deltaY;
        
        // Threshold for slow scrolling (15px accumulated in same direction)
        double actionThreshold = 15;
        
        // Determine if we should show/hide based on accumulated scroll
        if (Math.Abs(state.TotalScrollDistance) >= actionThreshold)
        {
            bool scrollingDown = state.TotalScrollDistance > 0;
            
            if (scrollingDown && state.IsVisible)
            {
                // Scrolling down and FAB is visible - hide it
                await HideFab(scrollView, state);
                state.TotalScrollDistance = 0; // Reset after action
            }
            else if (!scrollingDown && !state.IsVisible)
            {
                // Scrolling up and FAB is hidden - show it
                await ShowFab(scrollView, state);
                state.TotalScrollDistance = 0; // Reset after action
            }
        }
        
        // Special case: always show at the top
        if (currentY <= 0 && !state.IsVisible)
        {
            await ShowFab(scrollView, state);
            state.TotalScrollDistance = 0; // Reset after action
        }
    }
    
    private static async Task ShowFab(ScrollView scrollView, FabScrollState state)
    {
        if (!state.IsVisible && !state.IsAnimating && _trackedScrollViews.ContainsKey(scrollView))
        {
            try
            {
                state.IsAnimating = true;
                await state.Fab.ShowAsync();
                state.IsVisible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing FAB: {ex.Message}");
            }
            finally
            {
                if (_trackedScrollViews.ContainsKey(scrollView))
                {
                    state.IsAnimating = false;
                }
            }
        }
    }
    
    private static async Task HideFab(ScrollView scrollView, FabScrollState state)
    {
        if (state.IsVisible && !state.IsAnimating && _trackedScrollViews.ContainsKey(scrollView))
        {
            try
            {
                state.IsAnimating = true;
                await state.Fab.HideAsync();
                state.IsVisible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error hiding FAB: {ex.Message}");
            }
            finally
            {
                if (_trackedScrollViews.ContainsKey(scrollView))
                {
                    state.IsAnimating = false;
                }
            }
        }
    }

    /// <summary>
    /// Detaches a FloatingActionButton from the ScrollView.
    /// </summary>
    /// <param name="scrollView">The ScrollView to detach from</param>
    public static void DetachFab(this ScrollView scrollView)
    {
        if (scrollView == null)
            return;

        if (_trackedScrollViews.ContainsKey(scrollView))
        {
            scrollView.Scrolled -= OnScrollViewScrolled;
            _trackedScrollViews.Remove(scrollView);
        }
    }
}