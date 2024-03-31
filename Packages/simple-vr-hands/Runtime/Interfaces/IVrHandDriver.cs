using System;

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
        /// <param name="speed">
        /// Speed of transitioning between current hand profile and a new one.
        /// If set to float.PositiveInfinity, the transition will be immediate.
        /// Must be greater that 0f.
        /// </param>
        /// <exception cref="ArgumentException">thrown if transition time is less that zero</exception>
        void UpdateHand(IVrHand hand,float speed = float.PositiveInfinity);

        /// <summary>
        /// Assigns hand state provider to the driver
        /// </summary>
        /// <param name="provider">hand state provider to set (can be null)</param>
        void SetProvider(IVrHandStateProvider provider);
    }
}