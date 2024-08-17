 using System;
 using UnityEngine;
 using Object = UnityEngine.Object;
 #if UNITY_EDITOR
 using UnityEditor;
 #endif
 
 namespace MichaelWolfGames
 {
     ///-///////////////////////////////////////////////////////////
     ///
     [Serializable]
     public class SceneField
     {
#pragma warning disable CS0414
         [SerializeField] private Object sceneAsset = null;
#pragma warning restore CS0414
         [SerializeField] private string sceneName = "";
 
         public string scenePath => sceneName;
         
         ///-///////////////////////////////////////////////////////////
         ///
         /// makes it work with the existing Unity methods (LoadLevel/LoadScene)
         /// 
         public static implicit operator string(SceneField sceneField)
         {
             return sceneField.scenePath;
         }
     }
 
 #if UNITY_EDITOR
     ///-///////////////////////////////////////////////////////////
     ///
     [CustomPropertyDrawer(typeof(SceneField))]
     public class SceneFieldPropertyDrawer : PropertyDrawer
     {
         ///-///////////////////////////////////////////////////////////
         ///
         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
         {
             EditorGUI.BeginProperty(position, GUIContent.none, property);
             var sceneAsset = property.FindPropertyRelative("sceneAsset");
             var sceneName = property.FindPropertyRelative("sceneName");
             position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
             float totalPropertyHeight = position.height;
             position.height = totalPropertyHeight / 1.5f;
             if (sceneAsset != null)
             {
                 EditorGUI.BeginChangeCheck();
                 var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                 bool changed = EditorGUI.EndChangeCheck();
                 if (value != null)
                 {
                    changed |= (value.name != sceneName.stringValue);
                 }
                 changed &= (property.serializedObject.isEditingMultipleObjects == false);
                 if (changed)
                 {
                     sceneAsset.objectReferenceValue = value;
                     if (sceneAsset.objectReferenceValue != null)
                     {
                         var scenePath = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                         var assetsIndex = scenePath.IndexOf("Assets", StringComparison.Ordinal) + 7;
                         var extensionIndex = scenePath.LastIndexOf(".unity", StringComparison.Ordinal);
                         scenePath = scenePath.Substring(assetsIndex, extensionIndex - assetsIndex);
                         sceneName.stringValue = scenePath;
                     }
                     else
                     {
                         sceneName.stringValue = "";
                         sceneAsset.objectReferenceValue = null;
                     }
                 }
             }
             else
             {
                 sceneName.stringValue = "";
             }

             if (sceneAsset.objectReferenceValue == null)
             {
                 sceneName.stringValue = "";
             }

             // Draw the text of the sceneName immediately under the field.
             position.y += position.height;
             EditorGUI.LabelField(position, sceneName.stringValue, EditorStyles.miniLabel);
             EditorGUI.EndProperty();
         }

         ///-///////////////////////////////////////////////////////////
         ///
         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
         {
             return base.GetPropertyHeight(property, label) * 1.5f;
         }
     }
 #endif //UNITY_EDITOR
 }