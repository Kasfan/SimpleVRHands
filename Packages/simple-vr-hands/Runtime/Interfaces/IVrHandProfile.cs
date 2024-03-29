using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// A VR hand profile contains all the information of a state of a hand.
    /// When the profile is applied to a hand 
    /// </summary>
    public interface IVrHandProfile
    {
        /// <summary>
        /// Whether the hand should be visible or not
        /// </summary>
        bool HandVisible { get; }
        
        /// <summary>
        /// Target hand global position
        /// </summary>
        Vector3 HandPosition { get; }

        /// <summary>
        /// Target hand global rotation
        /// </summary>
        Quaternion HandRotation { get; }

        /// <summary>
        /// Returns target finger state
        /// </summary>
        /// <param name="fingerName"></param>
        /// <returns></returns>
        VrFingerState GetFingerState(HandFinger fingerName);
    }
}