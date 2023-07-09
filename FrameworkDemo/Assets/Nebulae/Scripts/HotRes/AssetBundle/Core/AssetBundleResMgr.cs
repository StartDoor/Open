using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class AssetBundleResMgr : AResMgr
    {
        AssetBundleManifest _manifest;

        Dictionary<string, AssetBundle> _loadedABDic;

        public string HotResAssetBundleRoot { get; private set; }

        public string BuiltinAssetBundleRoot { get; private set; }

        public AssetBundleResMgr(string manifestFileName)
        {
            UnloadAll();
            _loadedABDic = new Dictionary<string, AssetBundle>();

            HotResAssetBundleRoot = FileUtility.CombinePaths(Runtime.Ins.localResDir, NebulaeConst.AB_DIR_NAME);
            BuiltinAssetBundleRoot = FileUtility.CombinePaths(NebulaeConst.STREAMING_ASSET_RES_DATA_PATH, NebulaeConst.AB_DIR_NAME);

            var manifestPath = FileUtility.CombinePaths(HotResAssetBundleRoot, manifestFileName);
            if (File.Exists(manifestPath) == false)
            {
                manifestPath = FileUtility.CombinePaths(BuiltinAssetBundleRoot, manifestFileName);
            }

            AssetBundle ab = AssetBundle.LoadFromFile(manifestPath);
            if (ab == null)
            {
                throw new Exception($"[{manifestFileName}] 不存在: {manifestPath}");
            }

            _manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (_manifest == null)
            {
                throw new Exception(string.Format("错误的 Manifest File : {0}", manifestFileName));
            }

            ab.Unload(false);
        }

        internal void Inherit(AssetBundleResMgr source)
        {
            _loadedABDic = source._loadedABDic;
        }

        void MakeABNameNotEmpty(ref string abName)
        {
            if (string.IsNullOrEmpty(abName))
            {
                abName = NebulaeConst.ROOT_AB_FILE_NAME;
            }
        }

        public override string[] GetDepends(string abName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithoutExtension(abName);
            string[] dependList = _manifest.GetDirectDependencies(abName);
            return dependList;
        }

        public override string[] GetAllAssetsNames(string abName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithoutExtension(abName);
            AssetBundle ab = LoadAssetBundle(abName);
            string[] assetNames = ab.GetAllAssetNames();
            for (int i = 0; i < assetNames.Length; i++)
            {
                assetNames[i] = Path.GetFileName(assetNames[i]);
            }
            return assetNames;
        }


        public override UnityEngine.Object Load(string abName, string assetName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithoutExtension(abName);
            AssetBundle ab = LoadAssetBundle(abName);
            if (ab == null)
            {
                Debug.LogErrorFormat("AB资源不存在  abName: {0}", abName);
                return null;
            }
            var asset = ab.LoadAsset(assetName);
            if (asset == null)
            {
                Debug.LogErrorFormat("获取的资源不存在： AssetBundle: {0}  Asset: {1}", abName, assetName);
            }
            return asset;
        }

        public override T Load<T>(string abName, string assetName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithExtension(abName);
            AssetBundle ab = LoadAssetBundle(abName);
            T asset = ab.LoadAsset<T>(assetName);
            if (asset = null)
            {
                Debug.LogErrorFormat("获取的资源不存在： AssetBundle: {0}  Asset: {1}", abName, assetName);
            }
            return asset;
        }

        public override UnityEngine.Object[] LoadAll(string abName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithExtension(abName);
            AssetBundle ab = LoadAssetBundle(abName);
            return ab.LoadAllAssets();
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


        IEnumerator LoadAsync<T>(AssetBundle ab, string assetName, Action<T> onLoaded, Action<float> onProgress) where T : UnityEngine.Object
        {
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(assetName);

            do
            {
                if (onProgress != null)
                {
                    onProgress.Invoke(abr.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            while (false == abr.isDone);

            //加载完成
            onLoaded.Invoke((T)abr.asset);
        }


        IEnumerator LoadAllAsync(AssetBundle ab, Action<UnityEngine.Object[]> onLoaded, Action<float> onProgress)
        {
            AssetBundleRequest abr = ab.LoadAllAssetsAsync();

            do
            {
                if (onProgress != null)
                {
                    onProgress.Invoke(abr.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            while (false == abr.isDone);

            //加载完成
            onLoaded.Invoke(abr.allAssets);
        }

        public override void Unload(string abName, bool isUnlocadAllLoaded = false, bool isUnLoadDepends = true)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithExtension(abName);
            if (_loadedABDic.ContainsKey(abName))
            {
                AssetBundle ab = _loadedABDic[abName];
                _loadedABDic.Remove(abName);
                ab.Unload(isUnlocadAllLoaded);

                if (isUnLoadDepends)
                {
                    string[] dependList = _manifest.GetAllDependencies(abName);
                    foreach (string depend in dependList) 
                    {
                        if (CheckDependencies(depend) == false)
                        {
                            Unload(depend, isUnlocadAllLoaded, isUnLoadDepends);
                        }
                    }
                }
            }
        }

        bool CheckDependencies(string ab)
        {
            foreach (var loadedEntry in _loadedABDic)
            {
                var entryDepends = _manifest.GetAllDependencies(loadedEntry.Key);
                foreach (var entryDepend in entryDepends)
                {
                    if (entryDepend == ab)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void UnloadAll(bool isUnloadAllLoaded = false)
        {
            if (_loadedABDic != null)
            {
                foreach (AssetBundle cached in _loadedABDic.Values)
                {
                    cached.Unload(isUnloadAllLoaded);
                }
                _loadedABDic.Clear();
            }
            ResMgr.Ins.DoGC();
        }

        AssetBundle LoadAsssetBundleFormFile(string abName) 
        {
            var abPath = FileUtility.CombinePaths(HotResAssetBundleRoot, abName);
            if (File.Exists(abPath))
            {
                abPath = FileUtility.CombinePaths(BuiltinAssetBundleRoot, abName);
            }

            AssetBundle ab = AssetBundle.LoadFromFile(abPath);
            if (ab == null)
            {
                Debug.LogErrorFormat($"AssetBunle 文件 [{abName}] 不存在: {abPath}");
            }

            return ab;
        }

        private AssetBundle LoadAssetBundle(string abName)
        {
            MakeABNameNotEmpty(ref abName);
            abName = ABNameWithExtension(abName);

            AssetBundle ab = null;
            if (_loadedABDic.ContainsKey(abName))
            {
                ab = _loadedABDic[abName];
            }
            else
            {
                ab = LoadAsssetBundleFormFile(abName);

                if (ab == null)
                {
                    return null;
                }
                _loadedABDic[abName] = ab;
            }

            string[] dependList = _manifest.GetAllDependencies(abName);
            foreach (var depend in dependList)
            {
                if (_loadedABDic.ContainsKey(depend) == false)
                {
                    _loadedABDic[depend] = LoadAssetBundle(depend);
                }
            }

            return ab;
        }
    }

}

