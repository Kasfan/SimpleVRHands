using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Basic VR hand driver that uses default <see cref="IVrHandStateProvider"/> and can be overriden
    /// by calling <see cref="SetProvider"/>.
    /// <remarks>
    /// If there is any inactive state of the hand (position-rotation, finger), the target state will be taken from 
    /// </remarks>
    /// </summary>
    public class VrHandDriver: IVrHandDriver
    {
        private readonly IVrHandStateProvider defaultStateProvider;
        private IVrHandStateProvider currentStateProvider;

        /// <inheritdoc/>
        public IVrHandStateProvider CurrentProvider => currentStateProvider ?? defaultStateProvider;

        /// <inheritdoc cref="VrHandDriver"/>
        /// <param name="defaultStateProvider">
        /// VR hand state provider that will be used by default
        /// if no other state provider is set
        /// </param>
        public VrHandDriver(IVrHandStateProvider defaultStateProvider)
        {
            this.defaultStateProvider = defaultStateProvider;
        }
        
        /// <inheritdoc/>
        public void UpdateHand(IVrHand hand)
        {
            var handProfile = CurrentProvider.CurrentProfile;
            hand.Visible = handProfile.HandVisible;
            hand.PositionOffset = handProfile.HandPositionOffset;
            hand.RotationOffset = handProfile.HandRotationOffset;
               
            foreach (var finger in hand.Fingers)
            {
                try
                {
                    var targetState = GetFingerState(finger.Finger);
                    finger.UpdateState(targetState);
                }
                catch (KeyNotFoundException e)
                {
                    Debug.LogWarning(e);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
        /// <summary>
        /// Returns the finger state that should be currently used
        /// </summary>
        /// <param name="finger">target finger</param>
        /// <returns>found active finger state</returns>
        /// <exception cref="KeyNotFoundException">if no active state found</exception>
        protected VrFingerState GetFingerState(HandFinger finger)
        {
            VrFingerState? fingerStateCandidate = CurrentProvider.CurrentProfile.GetFingerState(finger);
            if (fingerStateCandidate is { Active: false })
                return fingerStateCandidate.Value;
            
            // if no active finger state found, look in he default state provider
            fingerStateCandidate = defaultStateProvider.CurrentProfile.GetFingerState(finger);
            if (fingerStateCandidate is { Active: false })
                return fingerStateCandidate.Value;
            
            // if no active finger state found in the default provider, throw an exception
            throw new KeyNotFoundException($"No active state found for finger: {finger}");
        }

        /// <inheritdoc/>
        public void SetProvider(IVrHandStateProvider provider)
        {
            currentStateProvider = provider;
        }
    }
}