using System.Collections.Generic;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Represents a VR hand.
    /// </summary>
    public interface IVrHand
    {
        /// <summary>
        /// If hand model is visible or hidden
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Global position of a hand in the scene
        /// </summary>
        Vector3 Position { get; set; }
        
        /// <summary>
        /// Global rotation of a hand in the space
        /// </summary>
        Quaternion Rotation { get; set; }
        
        /// <summary>
        /// Fingers attached to the hand
        /// </summary>
        IReadOnlyCollection<IVrFinger> Fingers { get; }

        /// <summary>
        /// Returns a finger object for the provided finger name.
        /// <remarks>
        /// All the classes implementing this interface must ensure that every attached finger has a unique name.
        /// </remarks>
        /// </summary>
        /// <param name="fingerName">Name of the finger to get</param>
        /// <returns>requested finger of the hand, or NULL if not found.</returns>
        IVrFinger GetFinger(HandFinger fingerName);
    }
}