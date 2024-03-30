using SimpleVRHand.Helpers;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Finger attached to <see cref="VrHand"/>
    /// </summary>
    public class VrFinger: VrFingerJoint, IVrFinger
    {
        /// <inheritdoc />
        [Tooltip("Name of the finger. In a hand all fingers names must be unique.")]
        [field:SerializeField]
        public HandFinger Finger { get; protected set; }
        
        [Tooltip("An axis in which the finger tilts")] 
        [AxisSelector]
        [SerializeField]
        protected Vector3Int tiltAxis = Vector3Int.up;

        [Tooltip("Rotation of a finger when it fully tilted left.")] 
        [SerializeField]
        protected float leftTilt = -5;
        
        [Tooltip("Rotation of a finger when it fully tilted right.")]
        [SerializeField]
        protected float rightTilt = 5;

        /// <inheritdoc />
        public IVrFingerJoint Root => this;
        
        private float tilt;
        
        /// <inheritdoc />
        public float Tilt {             
            get => tilt;
            set
            {
                tilt = Mathf.Clamp(value,-1f,1f);
                transform.localRotation = GetRotation();
            }
        }
                
        /// <inheritdoc />
        protected override Quaternion GetRotation()
        {
            var tAngle = Mathf.LerpAngle(
                0,
                tilt < 0 ? leftTilt : rightTilt,
                Mathf.Abs(tilt));

            var bAngle = Mathf.Lerp(0f, bendAngle, Bend);

            return originRotation *
                   Quaternion.AngleAxis(tAngle, tiltAxis) *
                   Quaternion.AngleAxis(bAngle, bendAxis);
        }

        /// <inheritdoc />
        public  void UpdateState(VrFingerState state)
        {
            tilt = Mathf.Clamp(state.Tilt,-1f,1f); // to avoid double rotation calculation

            // update all joints
            var c = 0;
            foreach (IVrFingerJoint finger in Root)
            {
                // stop, if no more bends left
                if(c >= state.Bends.Length) 
                    break;
                
                finger.Bend = state.Bends[c];
                c++;
            }
        }
    }
}