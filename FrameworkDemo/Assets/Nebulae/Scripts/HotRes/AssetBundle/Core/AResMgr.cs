using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Nebulae
{
    public abstract class AResMgr
    {
        protected string ABNameWithExtension(string abName)
        {
            if (abName.EndsWith(NebulaeConst.AB_EXTENSION) == false)
            {
                abName += NebulaeConst.AB_EXTENSION;
            }
            abName = FileUtility.StandardizeBackslashSeparator(abName);
            return abName;
        }

        protected string ABNameWithoutExtension(string abName)
        {
            if (abName.EndsWith(NebulaeConst.AB_EXTENSION))
            {
                abName = abName.Replace(NebulaeConst.AB_EXTENSION, "");
            }
            abName = FileUtility.StandardizeBackslashSeparator(abName);
            return abName;
        }

        public abstract string[] GetDepends(string abName);

        public abstract void Unload(string abName, bool isUnlocadAllLoaded = false, bool isUnLoadDepends = true);

        public abstract void UnloadAll(bool isUnloadAllLoaded = false);

        public abstract string[] GetAllAssetsNames(string abName);

        public abstract UnityEngine.Object Load(string abName, string assetName);

        public abstract UnityEngine.Object[] LoadAll(string abName);

        public abstract T Load<T>(string abName, string assetName) where T : UnityEngine.Object;

        public abstract void LoadAsync(string abName, string assetName, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null);

        public abstract void LoadAsync<T>(string abName, string assetName, Action<T> onLoaded, Action<float> onProgress = null) where T : UnityEngine.Object;

        public abstract void LoadAllAsync(string abName, Action<UnityEngine.Object[]> onLoaded, Action<float> onProgress = null);
    }
}
