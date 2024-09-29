using System;

namespace SaveSystem.Utils
{
    public static class SaveSystemSeriallizationUtility
    {
        public static string GetSeriallizationKey(this Type type) => type.AssemblyQualifiedName;
    }
}
