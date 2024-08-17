using UnityEditor;
using UnityEngine;

namespace MichaelWolfGames
{
	///-///////////////////////////////////////////////////////////
	///
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	class LayerAttributeDrawer : PropertyDrawer
	{
		///-///////////////////////////////////////////////////////////
		///
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Draw the int as a layer field.
			property.intValue = EditorGUI.LayerField(position, label,  property.intValue);
		}
	}
}