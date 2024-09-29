using UnityEngine;
using UnityEditor;
using SaveSystem.Editor.Utils;
using SaveSystem.Internal.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SaveSystem.Editor.Inspector
{
    [CustomEditor(typeof(SaveContainerScriptable))]
    public class SaveDataScriptableEditor : UnityEditor.Editor
    {
        private const string CONTAINER_FIELD_NAME = "_saveData";
        private const string DATA_LIST_NAME = "_dataList";
        private const float SCROLL_MIN_HEIGHT = 400;

        private Vector2 _scrollPos;
        private string _searchFilter;
        private int _popupSelection;
        private bool _showPopup;

        private void OnEnable()
        {
            _scrollPos = Vector2.zero;
        }

        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Load from JSON"))
            {
                JsonToScriptableParser.ImportDataFromJson(target as SaveContainerScriptable);
            }

            if (GUILayout.Button("Save to JSON"))
            {
                JsonToScriptableParser.ExportDataToJson(target as SaveContainerScriptable);
            }

            var cotnainerProperty = serializedObject.FindProperty(CONTAINER_FIELD_NAME);
            if (cotnainerProperty != null)
            {
                var dataListProperty = cotnainerProperty.FindPropertyRelative(DATA_LIST_NAME);
                if (dataListProperty != null)
                {
                    DrawList(dataListProperty);
                    DrawAddButton(cotnainerProperty);

                    serializedObject.ApplyModifiedProperties();
                }

                if (_showPopup)
                {
                    ShowTypePopup(cotnainerProperty);
                }

                return;
            }

            base.OnInspectorGUI();
        }

        private void DrawList(SerializedProperty listProperty)
        {
            _searchFilter = GUILayout.TextField(_searchFilter);
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, EditorStyles.helpBox, GUILayout.MinHeight(SCROLL_MIN_HEIGHT));
            {
                if (string.IsNullOrWhiteSpace(_searchFilter))
                    DrawElements(listProperty);
                else
                    DrawElementsFiltered(listProperty);
            }
            GUILayout.EndScrollView();
        }

        private void DrawElementsFiltered(SerializedProperty listProperty)
        {
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                var name = GetElementName(element);

                if (!name.Contains(_searchFilter, System.StringComparison.InvariantCultureIgnoreCase)) continue;
                DrawElement(element);
            }
        }
        private void DrawElements(SerializedProperty listProperty)
        {
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                DrawElement(element);
            }
        }


        private void DrawElement(SerializedProperty element)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(GetElementName(element), EditorStyles.boldLabel);

                GUILayout.Space(2);

                foreach (var property in GetChildProperties(element))
                {
                    EditorGUILayout.PropertyField(property);
                }

                GUILayout.Space(2);
            }
            EditorGUILayout.EndVertical();
        }
        private string GetElementName(SerializedProperty property)
        {
            return property.managedReferenceFieldTypename.Split('.')[^1];
        }

        private IEnumerable<SerializedProperty> GetChildProperties(SerializedProperty element)
        {
            var enumerator = element.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not SerializedProperty childProperty)
                {
                    continue;
                }

                yield return childProperty.Copy();
            }
        }

        private void DrawAddButton(SerializedProperty containerProperty)
        {
            if (GUILayout.Button("Add"))
            {
                _popupSelection = -1;
                _showPopup = true;
                ShowTypePopup(containerProperty);
            }
        }

        private void ShowTypePopup(SerializedProperty containerProperty)
        {
            var container = containerProperty.boxedValue as SaveContainer;
            if (container == null)
            {
                Debug.LogError("Can`t get container");
                return;
            }

            var dictionary = container.IndexesDictionary;

            var availableTypes = TypeCache
                .GetTypesDerivedFrom(typeof(SaveData))
                .Where(p =>
                        (p.IsPublic || p.IsNestedPublic || p.IsNestedPrivate)
                        && !p.IsAbstract
                        && !p.IsGenericType
                        && Attribute.IsDefined(p, typeof(SerializableAttribute))
                        && dictionary.ContainsKey(p))
                .ToArray();

            var namesArray = availableTypes.Select(type => type.Name).ToArray();

            _popupSelection = EditorGUILayout.Popup(_popupSelection, namesArray);
            if(_popupSelection >= 0)
            {
                _showPopup = false;
                var type = availableTypes[_popupSelection];
                container.DataList.Add(Activator.CreateInstance(type) as SaveData);
            }
        }
    }
}
