namespace SimpleVRHand
{
    /// <summary>
    /// Provides a <see cref="IVrHandProfile"/> that should be applied to a hand.
    /// </summary>
    public interface IVrHandStateProvider
    {
        /// <summary>
        /// The profile that should be applied to a hand.
        /// </summary>
        IVrHandProfile CurrentProfile { get; }
    }
}