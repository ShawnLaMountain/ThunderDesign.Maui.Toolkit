namespace ThunderDesign.Maui.Toolkit.Extensions;

/// <summary>
/// Extension methods for ScrollView to work with FloatingActionButton.
/// </summary>
public static class ScrollViewExtensions
{
    /// <summary>
    /// Attaches a FloatingActionButton to respond to scroll events.
    /// </summary>
    /// <param name="scrollView">The ScrollView to attach to</param>
    /// <param name="fab">The FloatingActionButton to control</param>
    /// <param name="hideOnScroll">Whether to hide the button when scrolling down</param>
    /// <param name="scrollThreshold">The scroll distance threshold to trigger hiding</param>
    /// <returns>The ScrollView for method chaining</returns>
    public static ScrollView AttachFab(
        this ScrollView scrollView,
        Controls.FloatingActionButton fab,
        bool hideOnScroll = true,
        double scrollThreshold = 30)
    {
        if (scrollView == null || fab == null)
            return scrollView;

        double lastScrollY = 0;
        bool isVisible = true;

        // Subscribe to scroll events
        scrollView.Scrolled += async (sender, e) => 
        {
            if (!hideOnScroll) return;
            
            var delta = e.ScrollY - lastScrollY;
            lastScrollY = e.ScrollY;
            
            if (Math.Abs(delta) < scrollThreshold) return;
            
            if (delta > 0 && isVisible)
            {
                // Scrolling down - hide FAB
                isVisible = false;
                await fab.HideAsync();
            }
            else if (delta < 0 && !isVisible)
            {
                // Scrolling up - show FAB
                isVisible = true;
                await fab.ShowAsync();
            }
        };
        
        return scrollView;
    }
}