using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleVRHand
{
    /// <summary>
    /// A hand state provider that blends multiple hand state profiles based on the input actions.
    /// Usually this class is used to visualize hands movement when the user presses controllers buttons.
    /// <remarks>
    /// This provider does not override hand's visibility, position and rotation.
    /// </remarks>
    /// </summary>
    [Serializable]
    public class VrHandActionBasedStateProvider: IVrHandStateProvider
    {
        [Tooltip("Based hand profile.")]
        [SerializeField]
        protected VrHandProfileSo freeMovementProfile;
        
        [Tooltip("Set here an action and corresponding hand profile. Note that at runtime you can only change profiles, but not delete or add input actions.")]
        [SerializeField] 
        protected SerializableDictionary<InputActionReference, VrHandProfileSo> actionsProfileMap;

        /// <inheritdoc />
        public virtual IVrHandProfile CurrentProfile => currentProfile;

        protected VrHandProfile currentProfile = new();

        protected  IEnumerable<HandFinger> FingerNamesEnum = Enum.GetValues(typeof(HandFinger)).Cast<HandFinger>();

        /// <summary>
        /// Initializes the <see cref="VrHandActionBasedStateProvider"/>.
        /// It's needed to subscribe to the input actions.
        /// </summary>
        public void Initialize()
        {
            foreach (var inputAction in actionsProfileMap.Keys)
            {
                inputAction.ToInputAction().started += context => UpdateProfile();
                inputAction.ToInputAction().performed += context => UpdateProfile();
                inputAction.ToInputAction().canceled += context => UpdateProfile();
            }
        }

        /// <summary>
        /// Blends multiple profiles based on the input actions.
        /// </summary>
        protected virtual void UpdateProfile()
        {
            // reset to the base profile
            foreach (var finger in FingerNamesEnum)
            {
                var state = freeMovementProfile.GetFingerState(finger);
                if (state is { Muted: false })
                    currentProfile.FingerStates[finger] = state.Value;
            }

            // update the profile to merge actions profiles
            foreach (var (action, profile) in actionsProfileMap)
            {
                if (!action.ToInputAction().triggered)
                    continue;
                
                foreach (var finger in FingerNamesEnum)
                {
                    var state = profile.GetFingerState(finger);
                    if (state is { Muted: false })
                        currentProfile.FingerStates[finger] = state.Value;
                }
            }
        }

        /// <summary>
        /// Dummy hand profile where only finger states get updated.
        /// </summary>
        protected class VrHandProfile: IVrHandProfile
        {
            /// <inheritdoc />
            public bool OverrideVisibility => false;
            /// <inheritdoc />
            public bool HandVisible => true;
            /// <inheritdoc />
            public bool OverridePosition => false;
            /// <inheritdoc />
            public Vector3 HandPositionOffset => Vector3.zero;
            /// <inheritdoc />
            public bool OverrideRotation => false;
            /// <inheritdoc />
            public Quaternion HandRotationOffset => Quaternion.identity;
            
            /// <summary>
            /// All the fingers are pre set. <see cref="VrHandActionBasedStateProvider"/> only updates finger states.
            /// </summary>
            public Dictionary<HandFinger, VrFingerState> FingerStates { get; set; } = new()
            {
                { HandFinger.Index ,  new VrFingerState { Bends = Array.Empty<float>() }},
                { HandFinger.Middle , new VrFingerState { Bends = Array.Empty<float>() }},
                { HandFinger.Pinky ,  new VrFingerState { Bends = Array.Empty<float>() }},
                { HandFinger.Ring ,   new VrFingerState { Bends = Array.Empty<float>() }},
                { HandFinger.Thumb ,  new VrFingerState { Bends = Array.Empty<float>() }},
            };
            
            /// <inheritdoc />
            public VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive=false)
            {
                return FingerStates[fingerName];
            }
        }
    }
}