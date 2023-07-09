using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class ResourceResMgr : AResMgr
    {
        public ResourceResMgr() { }

        string GetNameWithoutExt(string name)
        {
            var ext = Path.GetExtension(name);
            if (!string.IsNullOrEmpty(ext))
            {
                name = name.Replace(ext, "");
            }
            return name;
        }

        string AssetBundlePath2ResourcePath(string abName, string assetName)
        {
            assetName = GetNameWithoutExt(assetName);

            abName = ABNameWithoutExtension(abName);
            if (abName.ToLower() != NebulaeConst.ROOT_AB_FILE_NAME)
            {
                assetName = FileUtility.CombinePaths(abName, assetName);
            }

            return assetName;  
        }

        public override string[] GetAllAssetsNames(string abName)
        {
            const string RESOURE_ROOT = "/";
            abName = ABNameWithExtension(abName);
            string dir;
            if (abName.ToLower() != NebulaeConst.ROOT_AB_FILE_NAME)
            {
                dir = FileUtility.CombinePaths(RESOURE_ROOT, abName);
            }
            else
            {
                dir = RESOURE_ROOT;
            }

            var assets = Resources.LoadAll(dir);
            string[] assetNames = new string[assets.Length];
            for (int i = 0; i < assets.Length; i++)
            {
                assetNames[i] = assets[i].name;
            }
            return assetNames;
        }

        public override string[] GetDepends(string abName)
        {
            return new string[0];
        }

        public override UnityEngine.Object Load(string abName, string assetName)
        {
            return Load<UnityEngine.Object>(abName, assetName);
        }

        public override T Load<T>(string abName, string assetName)
        {
            string path = AssetBundlePath2ResourcePath(abName, assetName);
            var asset = Resources.Load<T>(path);
            return asset;
        }

        public override UnityEngine.Object[] LoadAll(string abName)
        {
            abName = ABNameWithoutExtension(abName);
            return Resources.LoadAll(abName);
        }

        public override void LoadAllAsync(string abName, Action<UnityEngine.Object[]> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }

        public override void LoadAsync(string abName, string assetName, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }

        public override void LoadAsync<T>(string abName, string assetName, Action<T> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }

        public override void Unload(string abName, bool isUnlocadAllLoaded = false, bool isUnLoadDepends = true)
        {
            Resources.UnloadUnusedAssets();
        }

        public override void UnloadAll(bool isUnloadAllLoaded = false)
        {
            Resources.UnloadUnusedAssets();
        }
    }
}