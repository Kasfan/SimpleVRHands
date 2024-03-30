using SimpleVRHand.Helpers;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Finger attached to <see cref="VrHand"/>
    /// </summary>
    public class VrFinger: MonoBehaviour, IVrFinger
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
        protected float maxLeftTilt;
        
        [Tooltip("Rotation of a finger when it fully tilted right.")]
        [SerializeField]
        protected float maxRightTilt;
        
        [Tooltip("Rotation of a finger when it's in rest position.")]
        [SerializeField]
        protected float restTilt;

        [Tooltip("Root joint of the finger")] 
        [SerializeField]
        protected VrFingerJoint rootJoint;

        /// <inheritdoc />
        public IVrFingerJoint Root => rootJoint;
        
        private float tilt;
        
        /// <summary>
        /// <inheritdoc />
        /// <remarks>
        /// If finger transform is the same as root joint transform,
        /// updating Tilt will reset the bend of the joint.
        /// In most cases it's better to use <see cref="UpdateState"/>.
        /// </remarks>
        /// </summary>
        public float Tilt {             
            get => tilt;
            set
            {
                tilt = Mathf.Clamp(0f,1f, value);
                var tiltRotation = Mathf.Lerp(
                    restTilt,
                    tilt<0? maxLeftTilt : maxRightTilt,
                    Mathf.Abs(tilt));
                transform.localRotation = VrFingerJoint.UpdateOneRotationAxis(transform.localRotation, tiltRotation, tiltAxis);
            }
        }
        
        /// <summary>
        /// When the component is added to a gameObject, try to guess basic parameters of the finger
        /// </summary>
        protected void Reset()
        {
            if (tiltAxis == Vector3Int.right)
                restTilt = transform.localRotation.eulerAngles.x;
            
            if (tiltAxis == Vector3Int.up)
                restTilt = transform.localRotation.eulerAngles.y;
            
            if (tiltAxis == Vector3Int.forward)
                restTilt = transform.localRotation.eulerAngles.z;

            // usually a finger tilts bends small range
            // we use 5 degrees by default
            maxLeftTilt = restTilt + 5; 
            maxRightTilt = restTilt - 5; 
        }
        
        /// <inheritdoc />
        public void UpdateState(VrFingerState state)
        {
            // update tilt of the finger
            Tilt = state.Tilt;
            
            // update all joints
            var c = 0;
            foreach (IVrFingerJoint finger in Root)
            {
                // stop, if no more bends left
                if(c > state.Bends.Length) 
                    break;
                
                finger.Bend = state.Bends[c];
                c++;
            }
        }
    }
}