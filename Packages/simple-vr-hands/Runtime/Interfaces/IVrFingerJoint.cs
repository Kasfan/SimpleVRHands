using System.Collections;
using System.Collections.Generic;

namespace SimpleVRHand
{
    /// <summary>
    /// Represents a joint of a finger.
    /// Joints can have only one following joint connected to them.
    /// <remarks>
    /// It's possible to iterate through all the following joints since <see cref="IVrFingerJoint"/> implements <see cref="IEnumerable"/>.
    /// </remarks>
    /// </summary>
    public interface IVrFingerJoint : IEnumerable<IVrFingerJoint>
    {
        /// <summary>
        /// Following joint connected to the current one. If NULL - current joint is the last one in the chain.
        /// </summary>
        IVrFingerJoint Next { get; }
        
        /// <summary>
        /// A bend of the joint in range 0 to 1
        /// <br/>   0 rest position
        /// <br/>   1 max bend
        /// </summary>
        float Bend { get; set; }
    }
}