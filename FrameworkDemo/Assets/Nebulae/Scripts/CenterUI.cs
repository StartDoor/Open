using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nebulae
{
    public class CenterUI : MonoBehaviour
    {
        public Button button;
        public Text textTip;
        public Image imageA;
        public RawImage rawimageB;
        public Text textC;

        // Start is called before the first frame update
        void Start()
        {
            var vo = LauncherSetting.LoadLauncherSettingDataFromResources();
            var launcher = new Launcher(vo);
            launcher.Star();
        }

        public void OnValueChange(string Type)
        {

        }

        public void OnButtonClick()
        {

        }
    }
}

