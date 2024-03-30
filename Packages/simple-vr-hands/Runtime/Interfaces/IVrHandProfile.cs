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
        /// If visibility state should be applied
        /// </summary>
        bool OverrideVisibility { get; }
        
        /// <summary>
        /// Whether the hand should be visible or not
        /// </summary>
        bool HandVisible { get; }
        
        /// <summary>
        /// If position offset should be applied
        /// </summary>
        bool OverridePosition { get; }
        
        /// <summary>
        /// Position offset of the hand model in local space
        /// </summary>
        Vector3 HandPositionOffset { get; }

        /// <summary>
        /// If rotation offset should be applied
        /// </summary>
        bool OverrideRotation { get; }
        
        /// <summary>
        /// Rotation offset of the hand model in local space
        /// </summary>
        Quaternion HandRotationOffset { get; }

        /// <summary>
        /// Returns target finger state
        /// </summary>
        /// <param name="fingerName">name of the finger</param>
        /// <param name="onlyActive">if only active profiles should be returned</param>
        /// <returns>fount finger state or null</returns>
        VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive=false);
    }
}