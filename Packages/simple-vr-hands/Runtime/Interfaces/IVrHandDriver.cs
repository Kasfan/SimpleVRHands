namespace SimpleVRHand
{
    /// <summary>
    /// Represents a driver of a VR hand. It's responsible for updating position, fingers states, etc.
    /// </summary>
    public interface IVrHandDriver
    {
        /// <summary>
        /// Active hand state provider
        /// </summary>
        IVrHandStateProvider CurrentProvider { get; }

        /// <summary>
        /// Updates state of the provided hand
        /// </summary>
        /// <param name="hand">hand to update</param>
        void UpdateHand(IVrHand hand);

        /// <summary>
        /// Assigns hand state provider to the driver
        /// </summary>
        /// <param name="provider">hand state provider to set (can be null)</param>
        void SetProvider(IVrHandStateProvider provider);
    }
}