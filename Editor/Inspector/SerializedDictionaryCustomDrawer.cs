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

        private static float _rectVerticalOffset = EditorGUIUtility.standardVerticalSpacing;
        private static float _fieldVerticalOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private static float _indentOffset = EditorGUIUtility.singleLineHeight / 2;
        private static float _minimalHeight = 2 * EditorGUIUtility.singleLineHeight;

        private static Color _headerColor = new Color(0.2075472f, 0.2075472f, 0.2075472f, 1);
        private static Color _backgroundColor = new Color(0.15f, 0.15f, 0.15f, 1);
        private static Color _fieldColor = new Color(0.22f, 0.22f, 0.22f, 1);

        private bool _isExpanded = false;
        private float _propertyHeight;
        private string _searchString = string.Empty;

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
            _propertyHeight = _minimalHeight;

            var headerRect = position;
            headerRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginProperty(headerRect, label, property);
            {
                EditorGUI.DrawRect(headerRect, _headerColor);
                _isExpanded = EditorGUI.Foldout(headerRect, _isExpanded, label, true);
            }
            EditorGUI.EndProperty();

            if (_isExpanded)
            {
                var keysProperty = property.FindPropertyRelative(KEYS_LIST_NAME);
                var valuesProperty = property.FindPropertyRelative(VALUES_LIST_NAME);
                _propertyHeight = (keysProperty.arraySize + 2) * EditorGUIUtility.singleLineHeight + _rectVerticalOffset * 2;

                var contentRect = position;
                contentRect.y += _fieldVerticalOffset;
                contentRect.x += _indentOffset;
                contentRect.width -= _indentOffset;
                contentRect.height = _propertyHeight + _rectVerticalOffset - EditorGUIUtility.singleLineHeight;

                EditorGUI.DrawRect(contentRect, _backgroundColor);

                if (keysProperty.arraySize == 0)
                {
                    var labelRect = contentRect;
                    labelRect.height = EditorGUIUtility.singleLineHeight;
                    labelRect.y += EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.LabelField(labelRect, "No saved data");
                    
                    return;
                }

                var searchPosition = contentRect;
                searchPosition.y += EditorGUIUtility.standardVerticalSpacing;
                searchPosition.height = EditorGUIUtility.singleLineHeight;
                _searchString = EditorGUI.TextField(searchPosition, "", _searchString);
                if(_searchString.Length == 0 || _searchString.Equals(string.Empty))
                {
                    var style = new GUIStyle(GUI.skin.label);
                    style.fontStyle = FontStyle.Italic;
                    style.normal.textColor = Color.gray;
                    EditorGUI.LabelField(searchPosition, "Search...", style);
                }

                var keyRect = contentRect;
                keyRect.x += _indentOffset;
                keyRect.y += EditorGUIUtility.standardVerticalSpacing;
                keyRect.height = EditorGUIUtility.singleLineHeight;
                keyRect.width *= KEYS_SECTION_PERCENT;
                keyRect.width -= _indentOffset;

                var valueRect = contentRect;
                valueRect.height = EditorGUIUtility.singleLineHeight;
                valueRect.width *= VALUES_SECTION_PERCENT;
                valueRect.width -= _indentOffset;
                valueRect.x += keyRect.width + _indentOffset;
                valueRect.y += EditorGUIUtility.standardVerticalSpacing;

                var fieldRect = contentRect;
                fieldRect.height = EditorGUIUtility.singleLineHeight;
                fieldRect.width -= _indentOffset * 2;
                fieldRect.x += _indentOffset * 2;
                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                string keyLabel;
                SerializedProperty valueProperty;
                var valueLabel = new GUIContent();

                for (int i = 0; i < keysProperty.arraySize; i++)
                {
                    keyLabel = keysProperty.GetArrayElementAtIndex(i).stringValue;
                    valueProperty = valuesProperty.GetArrayElementAtIndex(i);

                    if (!_searchString.Equals(string.Empty))
                    {
                        if (!IsSuitableForSearch(keyLabel) && !IsSuitableForSearch(valueProperty.stringValue))
                        {
                            continue;
                        }
                    }

                    keyRect.y += _fieldVerticalOffset;
                    valueRect.y += _fieldVerticalOffset;
                    fieldRect.y += _fieldVerticalOffset;

                    EditorGUI.DrawRect(fieldRect, _fieldColor);

                    EditorGUI.LabelField(keyRect, keyLabel);                                    
                    EditorGUI.BeginProperty(valueRect, valueLabel, property);
                    {
                        EditorGUI.PropertyField(valueRect, valueProperty, valueLabel);
                    }
                    EditorGUI.EndProperty();
                }
            }
        }

        private bool IsSuitableForSearch(string value)
        {
            return value.Contains(_searchString, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
