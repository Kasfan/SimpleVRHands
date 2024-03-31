using NUnit.Framework;
using UnityEngine;

namespace SimpleVRHand.Tests
{
    /// <summary>
    /// Unit tests for <see cref="VrFingerJoint"/>
    /// </summary>
    [TestFixture(Category = "XR")]
    public class VrFingerJointTests
    {
        private VrFingerJoint[] testJoints;

        [TearDown]
        public void CleanUp()
        {
            DestroyJoints(testJoints);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(100)]
        public void DetectJointsRecursive_WithNJoints_MapsJointsInOrder(int nJoints)
        {
            testJoints = BuildJointsChain(3, false);
            VrFingerJoint.DetectJointsRecursive(testJoints[0]);
            
            var c = 0;
            foreach (var joint in testJoints)
            {
                Assert.AreEqual(testJoints[c],joint);
                c++;
            }
        }

        /// <summary>
        /// Destroys all the joint objets in the provided array
        /// </summary>
        /// <param name="joints"></param>
        private void DestroyJoints(VrFingerJoint[] joints)
        {
            // destroying from the las one, because they can be both connected or not
            var i = testJoints.Length - 1;
            do
            {
                Object.Destroy(testJoints[i]);
                i--;
            } while (i>=0);
        }

        /// <summary>
        /// Builds a chain of joints connected together one by one
        /// </summary>
        /// <param name="numJoints">number of joints to create</param>
        /// <param name="chain">if true, chains all the joints together, otherwise keeps them separate</param>
        /// <returns>all joints in array starting with root joint</returns>
        private VrFingerJoint[] BuildJointsChain(int numJoints, bool chain=false)
        {
            var joints = new VrFingerJoint[numJoints];
            
            for (int i = 0; i < numJoints; i++)
            {
                var jointObject = new GameObject($"Joint{i}");
                joints[i] = jointObject.AddComponent<VrFingerJoint>();

                // set previous joint's transform a parent to the current one, if needed
                if(chain)
                    joints[i].transform.parent = i == 0 ? null : joints[i - 1].transform;
            }
            
            if(chain)
                VrFingerJoint.DetectJointsRecursive(joints[0]);
            
            return joints;
        }
    }
}