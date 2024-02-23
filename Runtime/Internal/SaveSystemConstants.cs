namespace SaveSystem.Internal
{
    internal class SaveSystemConstants
    {
        public const string GLOBAL_DATA_KEY = "GlobalData";
        public const string PROFILE_KEY = "CurrentProfileKey";
        public const string DEFAULT_PROFILE_NAME = "DefaultProfile";

#if UNITY_EDITOR
        /// <summary>
        /// Editor only!
        /// </summary>
        public const string MENU_ITEM_NAME = "Save System"; 
#endif
    }
}

