namespace U5BFA.Libraries
{
    /// <summary>
    /// Defines the preferred animation of a <see cref="TrayIconFlyout"/>.
    /// </summary>
    public enum TrayIconFlyoutAnimationMode
    {
        /// <summary>
        /// Preferred animation is direction movement from top to bottom.
        /// </summary>
        TopToBottom,

        /// <summary>
        /// Preferred animation is direction movement from bottom to top.
        /// </summary>
        BottomToTop,

        /// <summary>
        /// Preferred animation is direction movement from left to right.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Preferred animation is direction movement from right to left.
        /// </summary>
        RightToLeft,

        /// <summary>
        /// Preferred animation depends on the location and orientation of the flyout.
        /// If the flyout is located at a custom location, the flyout will appear without animation.
        /// </summary>
        Auto
    }
}
