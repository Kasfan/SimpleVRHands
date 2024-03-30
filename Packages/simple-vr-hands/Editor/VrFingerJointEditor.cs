using UnityEditor;
using UnityEngine;

namespace SimpleVRHand.EditorScripts
{
    /// <summary>
    /// Custom inspector view for <see cref="VrFingerJoint"/>
    /// </summary>
    [CustomEditor(typeof(VrFingerJoint), true)]
    public class VrFingerJointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set origin rotation"))
            {
                // to prevent accidental clicks that can ruin the hands setup
                // show dialog to confirm the action
                if(!EditorUtility.DisplayDialog( "Set origin rotation",
                    "This action will overwrite \"OriginRotation\" property of the hand.",
                   "Ok", "Cancel"))
                    return;
                
                var originRotationProperty = serializedObject.FindProperty("originRotation");
                var obj = (VrFingerJoint)(serializedObject.targetObject);
                originRotationProperty.quaternionValue = obj.transform.localRotation;

                serializedObject.ApplyModifiedProperties();
            }
            
            if (GUILayout.Button("Find nested joints"))
                VrFingerJoint.DetectJointsRecursive(serializedObject.targetObject as VrFingerJoint);
            
            EditorGUILayout.EndHorizontal();
        }
    }
}