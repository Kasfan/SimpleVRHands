namespace SimpleVRHand
{
    /// <summary>
    /// Names of the fingers
    /// </summary>
    public enum HandFinger
    {
        Thumb = 0,
        Index = 1,
        Middle = 2,
        Ring = 3,
        Pinkie = 4
    }
    
    /// <summary>
    /// Represents a finger of a <see cref="IVrHand"/>.
    /// <remarks>
    /// A finger can move in two axis - tilt(left/right)
    /// and bend(each joint can bend from open hand position to closed hand position)
    /// <br/>
    /// - To tilt a finger use <see cref="Tilt"/>.
    /// <br/>
    /// - To bend finger iterate thought <see cref="Root"/> and assign <see cref="IVrFingerJoint.Bend"/> to each joint.
    /// </remarks>
    /// </summary>
    public interface IVrFinger
    {
        /// <summary>
        /// Name of the current finger
        /// </summary>
        HandFinger Finger { get; }
        
        /// <summary>
        /// Tilt of a finger in degrees. (left and right rotation)
        /// </summary>
        float Tilt { get; set; }
        
        /// <summary>
        /// Root joint of a finger.
        /// </summary>
        IVrFingerJoint Root { get; }
    }
}