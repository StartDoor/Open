using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class ResMgr
    {
        public enum EResMarType
        {
            ASSET_BUNDLE,
            RESOURCES,
            ASSET_DATA_BASE,
        }

        public static ResMgr Ins { get; } = new ResMgr();

        private ResMgr()
        {

        }

        AResMgr _mgr;

        public void Init(EResMarType type, string assetsInfo = null)
        {
            switch (type)
            {
                case EResMarType.ASSET_BUNDLE:
                    Debug.Log($"初始化资源管理器... 资源来源：[AssetBundle]  Manifest名称：{assetsInfo}");
                    var newMgr = new AssetBundleResMgr(assetsInfo);
                    if (_mgr != null && _mgr is AssetBundleResMgr)
                    {
                        newMgr.Inherit(_mgr as AssetBundleResMgr);
                    }
                    _mgr = newMgr;
                    break;
                case EResMarType.RESOURCES:
                    Debug.Log("初始化资源管理器... 资源来源：[Resources]");
                    _mgr = new ResourceResMgr();
                    break;
                case EResMarType.ASSET_DATA_BASE: 
                    Debug.Log($" 初始化资源管理器... 资源来源：[AssetDataBase] 资源根目录：{assetsInfo}");
                    break;

            }
        }

        public void DoGC()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public string[] GetDepends(string abName)
        {
            return _mgr.GetDepends(abName); 
        }

        public void Unload(string abName, bool isUnloadAllLoaded = false, bool isUnloadDepends = true)
        {
            _mgr.Unload(abName, isUnloadAllLoaded, isUnloadDepends);
        }

        public void UnloadAll(bool isUnloadAllLoaded = false)
        {
            _mgr.UnloadAll(isUnloadAllLoaded);
        }

        public UnityEngine.Object Load(string abName, string assetName)
        {
            return _mgr.Load(abName, assetName);
        }

        public UnityEngine.Object[] LoadAll(string abName)
        {
            return _mgr.LoadAll(abName);
        }

        public UnityEngine.Object Load(string assetPath)
        {
            string abName;
            string assetName;
            SeparateAssetPath(assetPath, out abName, out assetName);
            return Load(abName, assetName);
        }

        public T Load<T>(string abName, string assetName) where T : UnityEngine.Object
        {
            T obj = _mgr.Load<T>(abName, assetName);

            return obj;
        }

        public T Load<T>(string assetPath) where T : UnityEngine.Object
        {
            string abName;
            string assetName;
            SeparateAssetPath(assetPath, out abName, out assetName);
            return Load<T>(abName, assetName);
        }

        public void LoadAsync(string abName, string assetName, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null)
        {
            _mgr.LoadAsync(abName, assetName, onLoaded, onProgress);
        }

        public void LoadAsync<T>(string abName, string assetName, Action<T> onLoaded, Action<float> onProgress = null) where T : UnityEngine.Object
        {
            _mgr.LoadAsync<T>(abName, assetName, onLoaded, onProgress);
        }

        public void LoadAsync(string assetPath, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null)
        {
            string abName;
            string assetName;
            SeparateAssetPath(assetPath, out abName, out assetName);
            _mgr.LoadAsync(abName, assetName, onLoaded, onProgress);
        }

        public void LoadAsync<T>(string assetPath, Action<T> onLoaded, Action<float> onProgress = null) where T : UnityEngine.Object
        {
            string abName;
            string assetName;
            SeparateAssetPath(assetPath, out abName, out assetName);
            _mgr.LoadAsync<T>(abName, assetName, onLoaded, onProgress);
        }

        public void LoadAllAsync(string abName, Action<UnityEngine.Object[]> onLoaded, Action<float> onProgress = null)
        {
            _mgr.LoadAllAsync(abName, onLoaded, onProgress);
        }

        public string LinkAssetPath(string abName, string assetName)
        {
            if (abName == null)
            {
                abName = "";
            }

            if (assetName == null)
            {
                assetName = "";
            }

            return FileUtility.CombinePaths(abName, assetName);
        }

        public void SeparateAssetPath(string assetPath, out string abName, out string assetName)
        {
            if (assetPath == null)
            {
                assetPath = "";
            }

            abName = FileUtility.StandardizeBackslashSeparator(Path.GetDirectoryName(assetPath));
            assetName = Path.GetFileName(assetPath);
        }

    }
}
