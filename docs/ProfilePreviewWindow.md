# VR Hand profile preview window

It's possible to preview VrHandProfiles in edit mode using VrProfilePreviewWindow. 
The windows allows to select a `VrHand` in current scene and apply `VrHandProfileSo` to it.

Menu location - `Window/SimpleVRHands/Hand profile preview`.

 ![VrProfilePreviewWindow](images/vr-profile-preview-window.png)


Working with the previewer is straight forward.
- select available hand from the dropdown
- set `VrHandProfileSo` asset to `HandProfile` field
- activate the previewer

While the previewer is active it constantly applies assigned profile to selected hand, 
which means that you can modify the profile and immediately see changes.

`Reset hand pose` button - resets the hand to default position, it's only available when the previewer is inactive.

`Show in hierarchy` button will highlight selected hand in hierarchy window.