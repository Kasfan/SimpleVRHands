using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SimpleVRHand
{
    /// <summary>
    /// Provides a hand profile when an interactor selects the object.
    /// </summary>
    [RequireComponent(typeof(IXRSelectInteractable))]
    public class VrHandSelectStateModifier: VrHandStateModifier
    {
    }
}