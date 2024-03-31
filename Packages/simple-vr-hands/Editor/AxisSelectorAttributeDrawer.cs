using SimpleVRHand.Helpers;
using UnityEditor;
using UnityEngine;

namespace SimpleVRHand.EditorScripts
{
	[CustomPropertyDrawer(typeof(AxisSelectorAttribute))]
	public class AxisSelectorDrawer : PropertyDrawer
	{
		private readonly string[] axisNames = new string[] { " X", " Y", " Z"};
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// if unsupported type, draw default property view
			if (property.propertyType != SerializedPropertyType.Vector3Int)
			{
				EditorGUILayout.PropertyField(property);
				return;
			}

			int axis = 0;
			axis = property.vector3IntValue.y > 0? 1 : axis;
			axis = property.vector3IntValue.z > 0? 2 : axis;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField($"{label}:");
			axis = GUILayout.SelectionGrid(axis, axisNames, 3, EditorStyles.radioButton);

			switch (axis)
			{
				case 0:
					property.vector3IntValue = Vector3Int.right;
					break;
				case 1:
					property.vector3IntValue = Vector3Int.up;
					break;
				case 2:
					property.vector3IntValue = Vector3Int.forward;
					break;
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}