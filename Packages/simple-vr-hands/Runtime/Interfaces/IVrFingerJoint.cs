using System.Collections;
using System.Collections.Generic;

namespace SimpleVRHand
{
    /// <summary>
    /// Represents a joint of a finger.
    /// Joints can have only one following joint connected to them.
    /// <remarks>
    /// It's possible to iterate through all the following joints since <see cref="IVrFingerJoint"/> implements <see cref="IEnumerator"/>.
    /// </remarks>
    /// </summary>
    public interface IVrFingerJoint : IEnumerator<IVrFingerJoint>
    {
        /// <summary>
        /// Following joint connected to the current one. If NULL - current joint is the last one in the chain.
        /// </summary>
        IVrFingerJoint Next { get; set; }
        
        /// <summary>
        /// A bend of the joint in degrees
        /// </summary>
        float Bend { get; set; }
    }
}