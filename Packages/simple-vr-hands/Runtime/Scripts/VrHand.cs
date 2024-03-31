using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SimpleVRHand
{
    public class VrHand: MonoBehaviour, IVrHand
    {
        [Tooltip("Fingers attached to the hand. Must be children of the hand.")]
        [SerializeField] 
        protected VrFinger[] fingers;

        [Tooltip("HandStateProvider that is used when hand does not interact with anything.")]
        [field:SerializeField]
        protected VrHandActionBasedStateProvider defaultHandStateProvider;

        [Tooltip("Controllers that are used to detect when hand interacts with objects.")]
        [SerializeField] 
        private XRBaseControllerInteractor[] handControllerInteractors;

        /// <summary>
        /// Active hand driver
        /// </summary>
        internal VrHandDriver HandDriver;

        /// <inheritdoc />
        public bool Visible { get; set; }
        
        /// <inheritdoc />
        public Vector3 PositionOffset { get; set; }
        
        /// <inheritdoc />
        public Quaternion RotationOffset { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IVrFinger> Fingers => fingers;
        
        /// <inheritdoc />
        public IVrFinger GetFinger(HandFinger fingerName)
        {
            foreach (var finger in fingers)
            {
                if (finger.Finger == fingerName)
                    return finger;
            }

            return null;
        }

        protected virtual void Awake()
        {
            defaultHandStateProvider.Initialize();
            HandDriver = new VrHandDriver(defaultHandStateProvider);
        }

        protected void Update()
        {
            HandDriver.UpdateHand(this);
        }

        protected virtual void OnEnable()
        {
            foreach (var interactor in handControllerInteractors)
            {
                interactor.selectEntered.AddListener(InteractableSelected);           
                interactor.selectExited.AddListener(InteractableReleased);
            }
        }

        /// <summary>
        /// Called when an interactor selects and object.
        /// <remarks>
        /// We assume that only one interactor can select at a time
        /// and only one object cant be selected at a time
        /// </remarks>
        /// </summary>
        private void InteractableSelected(SelectEnterEventArgs args)
        {
            var interactable = args.interactableObject.transform;
            var stateModifier = interactable.GetComponent<VrHandStateModifier>();
            
            if(stateModifier!=null)
                HandDriver.SetProvider(stateModifier);
                
        }        
        /// <summary>
        /// Called when an interactor releases and object
        /// <remarks>
        /// We assume that only one interactor can select at a time
        /// and only one object cant be selected at a time
        /// </remarks>
        /// </summary>
        private void InteractableReleased(SelectExitEventArgs arg0)
        {
            HandDriver.SetProvider(null);
        }

        protected virtual void OnDisable()
        {
            foreach (var interactor in handControllerInteractors)
            {
                interactor.selectEntered.RemoveListener(InteractableSelected);              
                interactor.selectExited.RemoveListener(InteractableReleased);     
            }
        }


        protected virtual void OnValidate()
        {

            foreach (var finger in fingers)
            {
                if (!finger)
                {
                    Debug.LogError($"Finger can't be null");
                    continue;
                }
                
                if(!finger.transform.IsChildOf(this.transform))
                    Debug.LogError($"{finger.name} must be a child of current transform");
            }
        }
    }
}