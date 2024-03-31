using UnityEngine;
using UnityEngine.Serialization;

namespace SimpleVRHand
{
    /// <summary>
    /// Provides modifier profile for vr hand
    /// </summary>
    public abstract class VrHandStateModifier: MonoBehaviour, IVrHandStateProvider
    {
        [Tooltip("This profile will be applied when the user interacts with this object")]
        [SerializeField] 
        protected VrHandProfileSo selectProfile;

        /// <inheritdoc/>
        public virtual IVrHandProfile CurrentProfile => selectProfile;
    }
}