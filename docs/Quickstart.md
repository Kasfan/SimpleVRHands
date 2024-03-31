# Quickstart guide

Before diving in the framework it's highly recommended to try demo scene first: [`Samples/Scenes/InteractablesDemo.unity`](../Packages/simple-vr-hands/Samples/Scenes/InteractablesDemo.unity)

## VR Hand

VRHand - a visual hand that replaces the VR controller visuals. 
Each hand has at lest on VRFinger connected to it. 
Each VRFinger has at least one VRFingerJoint - `Root` joint which is an origin of the VRFinger.
Multiple joints are connected to one by one to a 'chain' like structure.

The hand structure resembles a tree where each node has a state. 
We can set hand in any position by adjusting the states of the node.

VRHandState - a combination of the states of the hand (visibility, fingers rotation, etc..)

States are stored in `VrHandProfileSo` assets files, 
it makes it possible to reuse common hand state profiles.

Drag and drop [`Samples/Prefabs/Setup/Complete XR Origin Set Up Variant.prefab`](../Packages/simple-vr-hands/Samples/Prefabs/Setup/Complete XR Origin Set Up Variant.prefab)
to the scene. VRHands are placed as children of `XRController`.

![xr-origin-setup-hands](images/xr-origin-setup-hands.png)

The VrHand component in the prefab is fully set up for free moving hand.
If hand does not interact with anything and no controller buttons are pressed
hand state goes to open position. If the user presses controller button which action is set to `Actions Profile Map`,
the corresponded hand profile will be applied. If user presses multiple buttons at the same time,
the profiles will be mixed. 
For example: user presses only grip-button - pinky, ring, and middle fingers are bend inwards, 
when user presses trigger-button - only index finger will be bend, if user presses both buttons at the same time,
all 4 fingers (pinky, ring, middle, index) are bend according to the profile where their states are defined.

![VRHand](images/vr-hand-component.png)

## Hand state modifiers

When a hand interacts with an object, its state can be modified by adding `VrHandStateModifier` to the `XRInteractableObject`.

Currently, there are two implementations of `VrHandStateModifier`.

`VrHandSelectStateModifier` - applies `Select Profile` to the hand when the object is selected(grabbed)

![VrHandSelectStateModifier](images/vr-hand-select-state-modifier.png)

`VrHandSelectActivateStateModifier` - works the same way as `VrHandSelectStateModifier`, but also applies
`ActivateProfile` when the object is activated. 

For example, in demo scene we have a drill object, when user grabs the drill all fingers except index wrap around the handle of the drill 
and index finger stays unaffected. When user activates the drill, the index finger bends toward the drill trigger button.

![VrHandSelectActivateStateModifier](images/vr-hand-select-activate-state-modifier.png)

## Profile configuration

To create a new hand state profile, create `VrHandProfileSo` assets in the project view. 
In `Project` view click right button `Create->SimpleVRHands->CreateProfile`.

In inspector you can set up the profile.

![VrHandProfileSo](images/vr-hand-profile-so.png)

> **🟩 Note**  
> The project is in prototype state, therefore not all the features are implemented.
> For now offsets and visibility do not affect the hand.

To simplify the process of tweaking profile parameters, 
it's possible to preview the changes in Edit mode using [VrProfilePreviewWindow](ProfilePreviewWindow.md).



