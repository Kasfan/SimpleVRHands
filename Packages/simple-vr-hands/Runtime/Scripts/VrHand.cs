using System.Collections.Generic;
using UnityEngine;

namespace SimpleVRHand
{
    public class VrHand: MonoBehaviour, IVrHand
    {
        [Tooltip("Fingers attached to the hand. Must be children of the hand")]
        [SerializeField] 
        protected VrFinger[] fingers;

        /// <inheritdoc />
        public bool Visible { get; set; }
        
        /// <inheritdoc />
        public Vector3 PositionOffset { get; set; }
        
        /// <inheritdoc />
        public Quaternion RotationOffset { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IVrFinger> Fingers => fingers;
        
        /// <inheritdoc />
        public IVrFinger GetFinger(HandFinger fingerName)
        {
            foreach (var finger in fingers)
            {
                if (finger.Finger == fingerName)
                    return finger;
            }

            return null;
        }

        protected virtual void OnValidate()
        {

            foreach (var finger in fingers)
            {
                if (!finger)
                {
                    Debug.LogError($"Finger can't be null");
                    continue;
                }
                
                if(!finger.transform.IsChildOf(this.transform))
                    Debug.LogError($"{finger.name} must be a child of current transform");
            }
        }
    }
}