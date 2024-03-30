using System.Collections;
using System.Collections.Generic;
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

        [Tooltip("Rotation of the joint, when it's fully bent")] [SerializeField]
        protected Vector3 fullBend = new Vector3(90, 0, 0);
        
        [Tooltip("Rotation of the joint, when it's not bent at all")]
        [SerializeField]
        protected Vector3 restBend = Vector3.zero;

        private float bend;
        
        /// <inheritdoc/>
        public IVrFingerJoint Next => followingJoint;
        
        /// <inheritdoc/>
        public float Bend
        {
            get => bend;
            set
            {
                bend = value;
                var targetRotation = Vector3.Lerp(restBend, fullBend, bend);
                transform.localRotation = Quaternion.Euler(targetRotation);
            }
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
                Current = this.startJoint;
            }
            
            public bool MoveNext()
            {
                if (Current == null || Current.followingJoint == null)
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