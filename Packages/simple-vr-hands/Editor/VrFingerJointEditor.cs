using UnityEditor;
using UnityEngine;

namespace SimpleVRHand.EditorScripts
{
    /// <summary>
    /// Custom inspector view for <see cref="VrFingerJoint"/>
    /// </summary>
    [CustomEditor(typeof(VrFingerJoint))]
    public class VrFingerJointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Try map following joints"))
                VrFingerJoint.DetectJointsRecursive(serializedObject.targetObject as VrFingerJoint);
        }
    }
}