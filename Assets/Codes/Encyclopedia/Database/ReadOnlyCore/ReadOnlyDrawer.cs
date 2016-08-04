/*using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer {
	public override void OnGUI(Rect positions, SerializedProperty prop, GUIContent label)
	{
		string value;
		switch (prop.propertyType) {
		case SerializedPropertyType.Integer:
			value = prop.intValue.ToString ();
			break;
		case SerializedPropertyType.Boolean:
			value = prop.intValue.ToString ();
			break;
		case SerializedPropertyType.Float:
			value = prop.intValue.ToString ();
			break;
		case SerializedPropertyType.String:
			value = prop.intValue.ToString ();
			break;
		default:
			value = "(Не поддерживает)";
			break;
		}
		EditorGUI.LabelField (positions, label.text, value);
	}
}
*/