using System.Collections.Generic;
using UnityEngine;

namespace SimpleVRHand
{
    /// <summary>
    /// Configurable <see cref="IVrHandProfile"/>
    /// </summary>
    public class VrHandProfile: IVrHandProfile
    {
        /// <inheritdoc/>
        public bool OverrideVisibility { get; set; }
        /// <inheritdoc/>
        public bool HandVisible { get; set;  }
        /// <inheritdoc/>
        public bool OverridePosition { get; set; }
        /// <inheritdoc/>
        public Vector3 HandPositionOffset { get; set;  }
        /// <inheritdoc/>
        public bool OverrideRotation { get; set;  }
        /// <inheritdoc/>
        public Quaternion HandRotationOffset { get; set;  }
        
        /// <summary>
        /// Dictionary that contains finger state by finger name key
        /// </summary>
        public Dictionary<HandFinger, VrFingerState> FingerStates { get; set; } = new();
        
        /// <inheritdoc/>
        public VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive = false)
        {
            if (!FingerStates.ContainsKey(fingerName) 
                || (onlyActive && FingerStates[fingerName].Muted))
                return null;

            return FingerStates[fingerName];
        }
    }
}