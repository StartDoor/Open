using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Nebulae {

    [Serializable]
    [HideLabel]
    public class LauncherSettingData
    {
        [Title("基础")]
        [SuffixLabel("关闭日志打印可以提高效率")]
        [LabelText("是否允许打印日志")]
        public bool isLogEnable = true;


        [Title("资源读取配置")]
        [LabelText("资源读取模式"), ValueDropdown("HotResMode")]
        public EHotResMode hotResMode = EHotResMode.ASSET_DATA_BASE;

        [InfoBox("Zero会按照队列依次尝试资源的下载,直到其中一个成功为止", InfoMessageType = InfoMessageType.Info)]
        [LabelText("网络资源的根目录"), ShowIf("hotResMode", EHotResMode.NET_ASSET_BUNDLE)]
        [ListDrawerSettings(Expanded = true, NumberOfItemsPerPage = 3)]
        public string[] netRoots = new string[1] { "http://YourHotResRootUrl" };
    }
}
