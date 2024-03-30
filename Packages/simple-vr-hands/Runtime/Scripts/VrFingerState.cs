using System;
using System.Linq;
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
        [Tooltip("If muted, the state values will not affect related finger")]
        [field: SerializeField]
        public bool Muted { get; set; }
        
        /// <summary>
        /// Tilt of the finger
        /// </summary>
        public float Tilt 
        {
            get => tilt;
            set => tilt = value;
        }

        [Tooltip("Tilt of the finger (lef to right)")] 
        [Range(-1f, 1f)] 
        [SerializeField]
        public float tilt;

        /// <summary>
        /// A set of bends for the finger joints. First element - root joint, last element - tip of the finger
        /// </summary>
        public float[] Bends
        {
            get => bends;
            set => bends = value;
        }

        [Tooltip("A set of bends for the finger joints. First element - root joint, last element - tip of the finger")]
        [Range(0,1)]
        [SerializeField]
        public float[] bends;

        /// <summary>
        /// Default state of a finger, useful to reset the finger rotations
        /// </summary>
        public static readonly VrFingerState DefaultState = new()
        {
            Muted = false,
            Tilt = 0,
            Bends = Enumerable.Repeat(0f, 10).ToArray()
        };
    }
}