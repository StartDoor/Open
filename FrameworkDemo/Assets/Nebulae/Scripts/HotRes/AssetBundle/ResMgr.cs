using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebulae
{
    public class ResMgr
    {
        public enum EResMarType
        {
            ASSET_BUNDLE,
            RESOURCES,
            ASSET_DATA_BASe,
        }

        public static ResMgr Ins { get; } = new ResMgr();

        private ResMgr()
        {

        }

        AResMgr _mgr;

        public void DoGC()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}
