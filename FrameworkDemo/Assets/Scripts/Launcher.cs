using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebulae
{
    public class Launcher
    {
        public LauncherSettingData launcherData;

        public Launcher(LauncherSettingData data, bool isAutoOffline = false)
        {
            this.launcherData = data;
        }

        public void Star()
        {
            InitRuntime();
        }

        void InitRuntime()
        {
            Runtime.Ins.Init(launcherData);
        }
    }
}