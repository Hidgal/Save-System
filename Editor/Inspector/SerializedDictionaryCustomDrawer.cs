using System.Collections;
using System.Collections.Generic;
using SaveSystem.Utils;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor.Inspector
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<string, string>))]
    public class StringStringDictionaryDrawer : SerializedDictionaryCustomDrawer { }

    [CustomPropertyDrawer(typeof(SerializedDictionary<string, float>))]
    public class StringFloatDictionaryDrawer : SerializedDictionaryCustomDrawer { }

    [CustomPropertyDrawer(typeof(SerializedDictionary<string, int>))]
    public class StringIntDictionaryDrawer : SerializedDictionaryCustomDrawer { }

    [CustomPropertyDrawer(typeof(SerializedDictionary<string, bool>))]
    public class StringBoolDictionaryDrawer : SerializedDictionaryCustomDrawer { }

    public abstract class SerializedDictionaryCustomDrawer : PropertyDrawer
    {
        private const float KEYS_SECTION_PERCENT = 0.35f;
        private const float VALUES_SECTION_PERCENT = 1 - KEYS_SECTION_PERCENT;

        private const string KEYS_LIST_NAME = "_keys";
        private const string VALUES_LIST_NAME = "_values";

        private static float _rectVerticalOffset = EditorGUIUtility.singleLineHeight / 2;
        private static float _minimalHeight = 2 * EditorGUIUtility.singleLineHeight;
        private static Color _backgroundColor = new Color(0.2075472f, 0.2075472f, 0.2075472f, 1);

        private bool _isExpanded = false;
        private float _propertyHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_isExpanded)
            {
                return _propertyHeight;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var headerPos = position;
            headerPos.height = EditorGUIUtility.singleLineHeight;

            var backRect = position;
            
            if(_isExpanded)
                backRect.height += _rectVerticalOffset;

            EditorGUI.DrawRect(backRect, _backgroundColor);

            EditorGUI.BeginProperty(headerPos, label, property);
            {
                _isExpanded = EditorGUI.Foldout(headerPos, _isExpanded, label, true);
            }
            EditorGUI.EndProperty();

            if (_isExpanded)
            {
                var keysProperty = property.FindPropertyRelative(KEYS_LIST_NAME);
                var valuesProperty = property.FindPropertyRelative(VALUES_LIST_NAME);

                _propertyHeight = Mathf.Max(keysProperty.arraySize * EditorGUIUtility.singleLineHeight, _minimalHeight);

                if (keysProperty.arraySize == 0)
                {
                    var labelRect = position;
                    labelRect.height = EditorGUIUtility.singleLineHeight;
                    labelRect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(labelRect, "No saved data");
                }

                var keyRect = position;
                keyRect.height = EditorGUIUtility.singleLineHeight;
                keyRect.width = position.width * KEYS_SECTION_PERCENT;

                var valueRect = position;
                valueRect.height = EditorGUIUtility.singleLineHeight;
                valueRect.width = position.width * VALUES_SECTION_PERCENT;
                valueRect.x += keyRect.width;

                string keyLabel;

                for (int i = 0; i < keysProperty.arraySize; i++)
                {
                    keyRect.y += EditorGUIUtility.singleLineHeight;
                    valueRect.y += EditorGUIUtility.singleLineHeight;

                    keyLabel = keysProperty.GetArrayElementAtIndex(i).stringValue;
                    EditorGUI.LabelField(keyRect, keyLabel);

                    var valueProperty = valuesProperty.GetArrayElementAtIndex(i);
                    var valueLabel = new GUIContent();

                    EditorGUI.BeginProperty(valueRect, valueLabel, property);
                    {
                        EditorGUI.PropertyField(valueRect, valueProperty, valueLabel);
                    }
                    EditorGUI.EndProperty();
                }
            }
        }
    }
}
