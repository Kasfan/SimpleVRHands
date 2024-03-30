using System;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Target finger state
    /// </summary>
    [Serializable]
    public struct VrFingerState
    {
        /// <summary>
        /// Whether the state should be applied to the finger or not
        /// </summary>
        [Tooltip("If disabled, the state values will not affect related finger")]
        [field:SerializeField]
        public bool Active { get; set; }
        
        /// <summary>
        /// Tilt of the finger
        /// </summary>
        [field:SerializeField]
        public float Tilt { get; set; }
        
        /// <summary>
        /// A set of bends for the finger joints. First element - root joint, last element - tip of the finger
        /// </summary>
        [field:SerializeField]
        public float[] Bends { get; set; }
    }
}