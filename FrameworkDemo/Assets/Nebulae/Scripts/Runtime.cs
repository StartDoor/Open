using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class Runtime
    {
        public static readonly Runtime Ins = new Runtime();

        internal LauncherSettingData VO;

        public string localResDir { get; private set; }

        public string[] SettingFileNetDirList { get; private set; }

        public EHotResMode HotResMode => VO.hotResMode; // 这样能使HotResMode变成只读

        void InitHotResRuntime()
        {
            SettingFileNetDirList = new string[VO.netRoots.Length];
            for (int i = 0; i < SettingFileNetDirList.Length; i++)
            {
                SettingFileNetDirList[i] = FileUtility.CombineDirs(false, VO.netRoots[i], NebulaeConst.PLATFORM_DIR_NAME);
            }

            switch (HotResMode)
            {
                case EHotResMode.NET_ASSET_BUNDLE:
                    localResDir = NebulaeConst.WWW_RES_PERSISTENT_DATA_PATH;
                    break;
                default:
                    localResDir = NebulaeConst.PUBLISH_RES_ROOT_DIR;
                    break;
            }

            if (Directory.Exists(localResDir) == false)
            {
                Directory.CreateDirectory(localResDir); 
            }
        }
    }
}
