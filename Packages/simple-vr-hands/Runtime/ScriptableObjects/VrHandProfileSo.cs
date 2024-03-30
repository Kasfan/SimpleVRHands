using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Hand state profile asset that can be assigned to a monoBehaviour
    /// </summary>
    [CreateAssetMenu(fileName = "VRHandProfile", menuName = "SimpleVRHands/CreateProfile", order = 0)]
    public class VrHandProfileSo : ScriptableObject, IVrHandProfile
    {
        [Tooltip("A map of the fingers and their states for current profile.")]
        [field: SerializeField] 
        private SerializableDictionary<HandFinger, VrFingerState> fingerStates;

        /// <inheritdoc />
        [Tooltip("If visibility state should be applied")]
        [field: SerializeField]
        public bool OverrideVisibility { get; set; }

        /// <inheritdoc />
        [Tooltip("Whether or not the hand model should be visible.")]
        [field: SerializeField]
        public bool HandVisible { get; protected set; } = true;

        /// <inheritdoc />
        [Tooltip("If position offset should be applied")]
        [field: SerializeField]
        public bool OverridePosition { get; set; }

        /// <inheritdoc />
        [Tooltip("Position offset of the hand model in local space.")]
        [field:SerializeField]
        public Vector3 HandPositionOffset { get; protected set; }
        
        /// <inheritdoc />
        [Tooltip("If rotation offset should be applied")]
        [field:SerializeField]
        public bool OverrideRotation { get; set; }

        /// <inheritdoc />
        [Tooltip("Rotation offset of the hand model in local space.")]
        [field:SerializeField]
        public Quaternion HandRotationOffset { get; protected set; }
        
        /// <inheritdoc />
        public VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive=false)
        {
            if (!fingerStates.ContainsKey(fingerName) 
                || (onlyActive && fingerStates[fingerName].Muted))
                return null;

            return fingerStates[fingerName];
        }
    }
}