using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace MichaelWolfGames
{
    [Serializable]
    public class MinMaxRange
    {
        public float min;
        public float max;

        public MinMaxRange(float _min, float _max)
        {
            min = _min;
            max = _max;
        }
    }
    
    // // This is not an editor script. The property attribute class should be placed in a regular script file.
    // public class RangeAttribute : PropertyAttribute
    // {
    //     public float min;
    //     public float max;
    //
    //     public RangeAttribute(float min, float max)
    //     {
    //         this.min = min;
    //         this.max = max;
    //     }
    // }
    
#if UNITY_EDITOR
    
    // [CustomPropertyDrawer(typeof(RangeAttribute))]
    // public class RangeDrawer : PropertyDrawer
    // {
    //     // Draw the property inside the given rect
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         // First get the attribute since it contains the range for the slider
    //         RangeAttribute range = attribute as RangeAttribute;
    //
    //         // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
    //         if (property.propertyType == SerializedPropertyType.Float)
    //             EditorGUI.Slider(position, property, range.min, range.max, label);
    //         else if (property.propertyType == SerializedPropertyType.Integer)
    //             EditorGUI.IntSlider(position, property, Convert.ToInt32(range.min), Convert.ToInt32(range.max), label);
    //         else
    //             EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
    //     }
    // }
    
    [CustomPropertyDrawer(typeof(MinMaxRange))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            EditorGUI.BeginProperty(position, label, property);
            Rect minPos = position;
            minPos.width *= 0.25f;
            Rect barPos = position;
            barPos.x += minPos.width;
            barPos.width *= 0.5f;
            Rect maxPos = position;
            maxPos.width = minPos.width;
            maxPos.x = barPos.x + barPos.width;
            EditorGUI.PropertyField(minPos, property.FindPropertyRelative("min"),  GUIContent.none);
            EditorGUI.PropertyField(maxPos, property.FindPropertyRelative("max"),  GUIContent.none);
            
            float minVal = property.FindPropertyRelative("min").floatValue;
            float maxVal = property.FindPropertyRelative("max").floatValue;
            EditorGUI.MinMaxSlider(
                barPos,
                //label,
                GUIContent.none,
                ref minVal, ref maxVal,
                -100, 100);

            property.FindPropertyRelative("min").floatValue = minVal;
            property.FindPropertyRelative("max").floatValue = maxVal;
            
            EditorGUI.EndProperty();
        }

    }
#endif
}
