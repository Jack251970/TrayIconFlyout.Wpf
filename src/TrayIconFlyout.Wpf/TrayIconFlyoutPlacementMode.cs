namespace U5BFA.Libraries
{
    /// <summary>
    /// Defines the preferred placement of a <see cref="TrayIconFlyout"/> relative to the screen.
    /// </summary>
    public enum TrayIconFlyoutPlacementMode
    {
        /// <summary>
        /// Preferred location is at the top left corner of the screen.
        /// </summary>
        TopEdgeAlignedLeft,

        /// <summary>
        /// Preferred location is at the top right corner of the screen.
        /// </summary>
        TopEdgeAlignedRight,

        /// <summary>
        /// Preferred location is at the bottom left corner of the screen.
        /// </summary>
        BottomEdgeAlignedLeft,

        /// <summary>
        /// Preferred location is at the bottom right corner of the screen.
        /// </summary>
        BottomEdgeAlignedRight,
    }
}
