using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class NebulaeConst
    {
        public const string AB_DIR_NAME = "ab";

        public const string AB_EXTENSION = ".ab";

        public const string MANIFEST_FILE_NAME = "manifest";

        public const string ROOT_AB_FILE_NAME = "root_assets";

        public const string PLATFORM_NAME_PC = "pc";

        public const string PLATFORM_NAME_OSX = "osx";

        public const string PLATFORM_NAME_IOS = "ios";

        public const string PLATFORM_NAME_ANDROID = "android";

        public const string LAUNCHER_SETTING_NAME = "launcher_setting";

        static public string HOT_RESOURCES_ROOT_DIR = "Assets/@Resources";

        static public string NEBULAE_LIBRARY_DIR = "LibrarayNebulae";

        static public string PUBLISH_RES_ROOT_DIR = FileUtility.CombineDirs(false, NEBULAE_LIBRARY_DIR, "Release", "res", PLATFORM_DIR_NAME);

        static string _platformDirName = null;

        public static string PLATFORM_DIR_NAME
        {
            get
            {
                if (_platformDirName == null)
                {
#if UNITY_STANDALONE_WIN
                    _platformDirName = PLATFORM_NAME_PC;
#elif UNITY_STANDALONE_OSX
                    _platformDirName = PLATFORM_NAME_PC;
#elif UNITY_IPHONE
                    _platformDirName = PLATFORM_NAME_IOS;
#elif UNITY_ANDROID
                    _platformDirName = PLATFORM_NAME_ANDROID;
#endif
                }

                return _platformDirName;
            }
        }

        static string _persistentDataPath = null;

        public static string PersistentDataPath
        {
            get
            {
                if (_persistentDataPath == null)
                {
                    _persistentDataPath = Application.persistentDataPath;
#if UNITY_EDITOR
                    _persistentDataPath = FileUtility.CombineDirs(false, Directory.GetParent(Application.dataPath).FullName, NEBULAE_LIBRARY_DIR, "RuntimeCaches");
#elif   UNITY_STANDALONE
                    _persistentDataPath = FileUtility.CombineDirs(false, Application.dataPath, "Caches");
#endif
                }
                return _persistentDataPath;
            }
        }

        public static string WWW_RES_PERSISTENT_DATA_PATH = FileUtility.CombineDirs(false, PersistentDataPath, "nebulae", "res");

        public static string STREAMING_ASSET_RES_DATA_PATH = FileUtility.CombinePaths(Application.streamingAssetsPath, "res", PLATFORM_DIR_NAME);
    }
}
