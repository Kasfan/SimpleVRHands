using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleVRHand.EditorScripts
{
    /// <summary>
    /// Allows to preview <see cref="VrHandProfileSo"/> on a hand in the scene.
    /// </summary>
    public class VrProfilePreviewWindow : EditorWindow
    {
        public bool active = false;
        
        [SerializeField] 
        private VrHand[] sceneHands;
        [SerializeField] 
        private string[] handNames;
        [SerializeField] 
        private int selectedHand = 0;

        [SerializeField] 
        private VrHandProfileSo handProfile;

        private readonly VrHandProfileDefault defaultProfile = new();
        
        [MenuItem ("Window/SimpleVRHands/Hand profile preview")]
        public static void OpenWindow ()
        {
            if(!HasOpenInstances<VrProfilePreviewWindow>())
                GetWindow<VrProfilePreviewWindow>();
                
            FocusWindowIfItsOpen<VrProfilePreviewWindow>();
        }
        
        private void OnFocus()
        {
            var hands = FindObjectsOfType<VrHand>().ToList();
            var options = hands.Select(hand => hand.name).ToList();
            
            hands.Insert(0, null);
            options.Insert(0, "None");
            
            sceneHands = hands.ToArray();
            handNames = options.ToArray();

            if (selectedHand >= handNames.Length)
                selectedHand = 0;
        }

        void OnGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("In play mode..");
                return;
            }

            active = EditorGUILayout.ToggleLeft("Active", active);

            // if preview is not running and a hand is selected
            // show a button to reset hand poses to default
            if (!active && selectedHand > 0 &&
                GUILayout.Button("Reset hand pose"))
            {
                foreach (var finger in sceneHands[selectedHand].Fingers)
                {
                    finger.UpdateState(defaultProfile.GetFingerState(finger.Finger).Value);
                }
                EditorUtility.SetDirty(sceneHands[selectedHand]);
            }

            EditorGUILayout.BeginHorizontal();
            // draw hand selector
            selectedHand = EditorGUILayout.Popup("Active hand", selectedHand, handNames);
            // show button to select current hand gameObject in hierarchy
            if (selectedHand > 0 && GUILayout.Button("Show in hierarchy"))
            {
                EditorGUIUtility.PingObject(sceneHands[selectedHand]);
            }

            EditorGUILayout.EndHorizontal();
            
            handProfile = EditorGUILayout.ObjectField(
                    "HandProfile", handProfile,
                    typeof(VrHandProfileSo), false) as VrHandProfileSo;
            

        }

        /// <summary>
        /// Update hand each frame
        /// </summary>
        private void Update()
        {            
            if(!active)
                return;
            
            var hand = sceneHands[selectedHand];
            if (hand == null)
                return;
            
            
            IVrHandProfile applyProfile = handProfile;
            if (handProfile == null)
                applyProfile = defaultProfile;

            foreach (var finger in hand.Fingers)
            {
                var state = applyProfile.GetFingerState(finger.Finger);
                if (state is { Muted: false })
                    finger.UpdateState(state.Value);
                else
                    finger.UpdateState(defaultProfile.GetFingerState(finger.Finger).Value);
            }
            
            EditorUtility.SetDirty(hand);
        }

        /// <summary>
        /// Dummy profile to reset hand state
        /// </summary>
        private class VrHandProfileDefault: IVrHandProfile
        {
            public bool OverrideVisibility => false;
            public bool HandVisible => true;
            public bool OverridePosition => false;
            public Vector3 HandPositionOffset => Vector3.zero;
            public bool OverrideRotation => false;
            public Quaternion HandRotationOffset => Quaternion.identity;
            
            public VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive = false)
            {
                return VrFingerState.DefaultState;
            }
        }
    }
}