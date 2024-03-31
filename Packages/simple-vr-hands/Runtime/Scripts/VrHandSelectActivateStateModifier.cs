using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SimpleVRHand
{
    /// <summary>
    /// Provides a hand profile when an interactor selects the object, that also can be activated.
    /// </summary>
    [RequireComponent(typeof(IXRSelectInteractable))]
    [RequireComponent(typeof(IXRActivateInteractable))]
    public class VrHandSelectActivateStateModifier: VrHandSelectStateModifier
    {
        [Tooltip("This profile will be applied when the user activates this object")]
        [SerializeField] 
        private VrHandProfileSo activateProfile;
        
        /// <inheritdoc/>
        public override IVrHandProfile CurrentProfile => currentProfile;

        private IXRActivateInteractable interactable;
        private IVrHandProfile currentProfile;
        private VrHandProfile mixedProfile;

        private void Awake()
        {
            mixedProfile = new VrHandProfile()
            {
                OverrideVisibility = selectProfile.OverrideVisibility,
                OverridePosition = selectProfile.OverridePosition,
                OverrideRotation = selectProfile.OverrideRotation,
                HandPositionOffset = selectProfile.HandPositionOffset,
                HandRotationOffset = selectProfile.HandRotationOffset,
                HandVisible = selectProfile.HandVisible,
                FingerStates = new ()
            };
            var fingers = Enum.GetValues(typeof(HandFinger)).Cast<HandFinger>();

            foreach (var finger in fingers)
            {
                var selectState = selectProfile.GetFingerState(finger);
                if (selectState is { Muted: false })
                    mixedProfile.FingerStates[finger] = selectState.Value;
                
                var activateState = activateProfile.GetFingerState(finger);
                if (activateState is { Muted: false })
                    mixedProfile.FingerStates[finger] = activateState.Value;
            }
            currentProfile = selectProfile;

            interactable = GetComponent<IXRActivateInteractable>();
            interactable.activated.AddListener(OnActivated);
            interactable.deactivated.AddListener(OnDeactivated);
        }

        private void OnDeactivated(DeactivateEventArgs arg0)
        {
            currentProfile = selectProfile;
        }

        private void OnActivated(ActivateEventArgs arg)
        {
            currentProfile = mixedProfile;
        }
    }
}