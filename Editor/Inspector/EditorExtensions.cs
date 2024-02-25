using UnityEditor;

namespace SaveSystem.Editor.Inspector
{
    public static class EditorExtensions 
    {
        public static string GetFoldoutDataKey(this SerializedProperty property)
        {
            return $"{property.serializedObject.targetObject.GetInstanceID()}.{property.name}";
        }
    }
}
