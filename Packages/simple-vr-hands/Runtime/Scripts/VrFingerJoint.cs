using System;
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

        [Tooltip("Rotation angle of the joint, when it's fully bent")]
        [SerializeField]
        protected float fullBendAngle = 65f;
        
        [Tooltip("Rotation angle of the joint, when it's not bent at all")]
        [SerializeField]
        protected float restBendAngle = 0f;

        private float bend;
        
        /// <inheritdoc/>
        public IVrFingerJoint Next => followingJoint;
        
        /// <inheritdoc/>
        public float Bend
        {
            get => bend;
            set
            {
                bend = Mathf.Clamp(0f,1f, value);
                var bendRotation = Mathf.LerpAngle(restBendAngle, fullBendAngle, bend);
                transform.localRotation = UpdateOneRotationAxis(transform.localRotation, bendRotation, bendAxis);
            }
        }

        /// <summary>
        /// Updates rotation euler angles in only one axis
        /// </summary>
        /// <param name="originalRotation">original rotation</param>
        /// <param name="newAngle">new euler angle in degrees for the axis</param>
        /// <param name="axis">axis to apply rotation in</param>
        /// <returns>updated rotation</returns>
        public static Quaternion UpdateOneRotationAxis(Quaternion originalRotation, float newAngle, Vector3Int axis)
        {
            var newRotation = originalRotation.eulerAngles;
            if (axis == Vector3Int.right)
            {
                newRotation.x = newAngle;
                return Quaternion.Euler(newRotation);
            }
            
            if (axis == Vector3Int.up)
            {
                newRotation.y = newAngle;
                return Quaternion.Euler(newRotation);
            }
            
            if (axis == Vector3Int.forward)
            {
                newRotation.z = newAngle;
                return Quaternion.Euler(newRotation);
            }
            
            Debug.LogError($"Target [axis] value can be only: {Vector3Int.right}, {Vector3Int.up} or {Vector3Int.forward}");
            return originalRotation;
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

        /// <summary>
        /// When the component is added to a gameObject, try to guess basic parameters of the joint
        /// </summary>
        protected void Reset()
        {
            if (bendAxis == Vector3Int.right)
                restBendAngle = transform.localRotation.eulerAngles.x;
            
            if (bendAxis == Vector3Int.up)
                restBendAngle = transform.localRotation.eulerAngles.y;
            
            if (bendAxis == Vector3Int.forward)
                restBendAngle = transform.localRotation.eulerAngles.z;

            // usually a finger joint bends up to 90 degrees forward
            // to be safe we use 80, because some hand will look crooked at full bend
            // the user, can change in anyway
            fullBendAngle = restBendAngle + 80; 
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