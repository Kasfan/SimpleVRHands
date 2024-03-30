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

        [Tooltip("Rotation of a finger when it fully tilted left.")]
        [SerializeField]
        protected Vector3 maxLeftTilt;
        
        [Tooltip("Rotation of a finger when it fully tilted right.")]
        [SerializeField]
        protected Vector3 maxRightTilt;
        
        [Tooltip("Rotation of a finger when it's in rest position.")]
        [SerializeField]
        protected Vector3 restTilt;
        
        /// <inheritdoc />
        [field:SerializeField]
        public IVrFingerJoint Root { get; set; }
        
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
                tilt = value;
                transform.localRotation = GetRotationFromTilt(tilt, restTilt, maxLeftTilt, maxRightTilt);
            }
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

        /// <summary>
        /// Transforms tilt value to the finger rotation based on preset constrains
        /// </summary>
        /// <param name="tilt">value of the tilt</param>
        /// <param name="rest">rest position: 0</param>
        /// <param name="left">max left tilt rotation: -1</param>
        /// <param name="right">max right tilt rotation: 1</param>
        /// <returns>finger rotation for the tilt</returns>
        public static Quaternion GetRotationFromTilt(float tilt, Vector3 rest, Vector3 left, Vector3 right)
        {
            Vector3 targetRotation = Vector3.Lerp(
                rest,
                tilt<0? left : right,
                Mathf.Abs(tilt));

            return Quaternion.Euler(targetRotation);
        }
    }
}