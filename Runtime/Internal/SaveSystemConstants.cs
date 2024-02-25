namespace SaveSystem.Internal
{
    internal class SaveSystemConstants
    {
        public const string GLOBAL_DATA_KEY = "GlobalData";
        public const string PROFILE_KEY = "CurrentProfile";
        public const string FILE_EXTENSION = ".json";

#if UNITY_EDITOR
        /// <summary>
        /// Editor only!
        /// </summary>
        public const string MENU_ITEM_NAME = "Tools/Save System/"; 
#endif
    }
}

