using System.Collections;
using System.Collections.Generic;
using SimpleVRHand.Helpers;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Finger joint used by <see cref="VrFinger"/>
    /// </summary>
    public class VrFingerJoint: MonoBehaviour, IVrFingerJoint
    {
        [Tooltip("Optional. Next joint in the finger connected to the current one")]
        [SerializeField] 
        private VrFingerJoint followingJoint;
        
        [Tooltip("An axis in which the joint bends")] 
        [AxisSelector]
        [SerializeField]
        protected Vector3Int bendAxis = Vector3Int.right;

        [Tooltip("How much the finger can bend in degrees")]
        [SerializeField]
        protected float bendAngle = 65f;

        [Tooltip("Original rotation of the joint")] 
        [SerializeField]
        protected Quaternion originRotation;

        private float bend;
        
        /// <inheritdoc/>
        public IVrFingerJoint Next => followingJoint;
        
        /// <inheritdoc/>
        public virtual float Bend
        {
            get => bend;
            set
            {
                bend = Mathf.Clamp(value, 0f,1f);
                transform.localRotation = GetRotation();
            }
        }
        
        /// <summary>
        /// Returns target rotation of this transform
        /// </summary>
        /// <returns> rotation which current joint must have </returns>
        protected virtual Quaternion GetRotation()
        {
            var angle = Mathf.Lerp(0f, bendAngle, bend);
            return originRotation * Quaternion.AngleAxis(angle, bendAxis);
        }

        /// <summary>
        /// Recursively maps all the following joints by searching joint's child objects.
        /// <remarks>
        /// If multiple joint components found in direct children only first one will be mapped.
        /// </remarks>
        /// </summary>
        /// <param name="startJoint">Initial joint in the chain</param>
        public static void DetectJointsRecursive(VrFingerJoint startJoint)
        {
            foreach (Transform child in startJoint.transform)
            {
                var joint = child.GetComponent<VrFingerJoint>();
                if (joint)
                {
                    startJoint.followingJoint = joint;
                    DetectJointsRecursive(joint);
                    break; // if joint found we stop searching
                }
            }
        }

        IEnumerator<IVrFingerJoint> IEnumerable<IVrFingerJoint>.GetEnumerator()
        {
            return new JointEnumerator(this);
        }

        public IEnumerator GetEnumerator()
        {
            return new JointEnumerator(this);
        }
        
        /// <summary>
        /// Enumerator for the VrFingerJoint. It uses following joint to move next
        /// </summary>
        private class JointEnumerator: IEnumerator<VrFingerJoint>
        {
            private readonly VrFingerJoint startJoint;
            
            /// <inheritdoc cref="JointEnumerator"/>
            /// <param name="startJoint">joint from which iteration starts</param>
            public JointEnumerator(VrFingerJoint startJoint)
            {
                this.startJoint = startJoint;
                Current = null;
            }
            
            public bool MoveNext()
            {
                // if it's first element
                if (Current == null)
                {
                    Current = startJoint;
                    return Current != null;
                }

                if (Current.followingJoint == null)
                    return false;
                
                Current = Current.followingJoint;
                return true;
            }

            public void Reset()
            {
                Current = startJoint;
            }
            
            public VrFingerJoint Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // no need to dispose of anything
            }
        }
    }
}