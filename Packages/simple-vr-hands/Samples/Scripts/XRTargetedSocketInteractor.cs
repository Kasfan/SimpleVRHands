using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SimpleVRHand.Samples
{
    /// <summary>
    /// XR Socket Interactor that only accesses selected objects.
    /// </summary>
    public class XRTargetedSocketInteractor: XRSocketInteractor
    {
        /// <summary>
        /// A set of interactables that can be accepted by the socket.
        /// </summary>
        [Tooltip("Only interactable objects set in here can be accepted by the Socket")]
        [SerializeField] 
        private XRBaseInteractable[] targetInteractables;
        
        /// <inheritdoc />
        /// <remarks>
        /// Returns true if provided interactable is in the <see cref="targetInteractables"/> set.
        /// </remarks>
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return base.CanSelect(interactable) && targetInteractables.Contains(interactable);
        }
        
        /// <inheritdoc />
        /// <remarks>
        /// Returns true if provided interactable is in the <see cref="targetInteractables"/> set.
        /// </remarks>
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return base.CanHover(interactable) && targetInteractables.Contains(interactable);
        }
    }
}