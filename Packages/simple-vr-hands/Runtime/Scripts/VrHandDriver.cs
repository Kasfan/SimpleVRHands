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
        public void UpdateHand(IVrHand hand, float speed = float.PositiveInfinity)
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

                    if (!float.IsPositiveInfinity(speed))
                    {            
                        if (speed <= 0f)
                            throw new ArgumentException($"Transition must > 0, but {speed} provided");
                        
                        targetState = GetFingerTransitionState(finger, targetState, speed);
                    }

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
        /// Interpolates between current and target finger states
        /// </summary>
        /// <param name="finger">target finger</param>
        /// <param name="targetState">target finger state</param>
        /// <param name="t">0f - 1f interpolation factor</param>
        /// <returns>transition state of the finger</returns>
        protected VrFingerState GetFingerTransitionState(IVrFinger finger, VrFingerState targetState, float t)
        {
            var transitionState = new VrFingerState
            {
                Muted = targetState.Muted,
                Tilt = Mathf.Lerp(finger.Tilt, targetState.Tilt, t),
                Bends = new float[targetState.Bends.Length]
            };
            targetState.Bends.CopyTo(transitionState.Bends,0);
            
            int c = 0;
            foreach (var joint in finger.Root)
            {
                if(transitionState.Bends.Length <= c)
                    break;
                
                transitionState.Bends[c] = Mathf.Lerp(joint.Bend, transitionState.Bends[c], t);
                c++;
            }

            return transitionState;
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
            if (fingerStateCandidate is { Muted: false })
                return fingerStateCandidate.Value;
            
            // if no active finger state found, look in he default state provider
            fingerStateCandidate = defaultStateProvider.CurrentProfile.GetFingerState(finger);
            if (fingerStateCandidate is { Muted: false })
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