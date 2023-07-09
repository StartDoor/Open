using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Nebulae
{
    public class CenterUI : MonoBehaviour
    {
        public Dropdown dropdown;
        public Image imageA;
        public RawImage rawimageB;

        // Start is called before the first frame update
        void Start()
        {
            var vo = LauncherSetting.LoadLauncherSettingDataFromResources();
            var launcher = new Launcher(vo);
            launcher.Star();
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            OnDropdownValueChanged(0);
        }

        public void OnDropdownValueChanged(int index)
        {
            if (index == 0)
            {
                ResMgr.Ins.Init(ResMgr.EResMgrType.ASSET_BUNDLE, NebulaeConst.MANIFEST_FILE_NAME + NebulaeConst.AB_EXTENSION);

            }
            else if (index == 1)
            {
                ResMgr.Ins.Init(ResMgr.EResMgrType.RESOURCES);

            }
            else
            {
                ResMgr.Ins.Init(ResMgr.EResMgrType.ASSET_DATA_BASE, NebulaeConst.HOT_RESOURCES_ROOT_DIR);
            }
        }

        public void OnButtonClick()
        {
            Debug.LogError("OnButtonClick");
            //imageA.sprite = ResMgr.Ins.Load<Sprite>("examples/Data/altas/activity_PointsRedemption_view_atlas.png");
            rawimageB.texture = ResMgr.Ins.Load<Texture>("examples/Data/texture/hghl_002");
        }
    }
}

