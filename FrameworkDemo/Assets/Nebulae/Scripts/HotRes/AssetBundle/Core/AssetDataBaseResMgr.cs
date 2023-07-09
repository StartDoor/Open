using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    class AssetDataBaseResMgr : AResMgr
    {
        string _assetRoot;

        public AssetDataBaseResMgr(string assetRoot)
        {
#if !UNITY_EDITOR
            throw new Exception("AssetDataBaseResMgr仅在Editor模式下可用")
#endif
            _assetRoot = assetRoot;
        }

        string GetDirOfAB(string abName)
        {
            abName = ABNameWithExtension(abName);
            string dir;
            if (abName.ToLower() != NebulaeConst.ROOT_AB_FILE_NAME)
            {
                dir = FileUtility.CombinePaths(_assetRoot, abName);
            }
            else
            {
                dir = FileUtility.CombinePaths(_assetRoot);
            }
            return dir;
        }

        string AssetBundlePath2ResourcePath(string abName, string assetName)
        {
            try
            {
                string dir = GetDirOfAB(abName);

                var files = Directory.GetFiles(dir);

                bool isAssetNameContainExt = assetName.Contains(".");

                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".meta")
                    {
                        continue;
                    }

                    if (isAssetNameContainExt && Path.GetFileName(file) == assetName)
                    {
                        return file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file) == assetName)
                    {
                        return file;
                    }
                }
            }
            catch
            {
                throw new Exception(string.Format("在[{0}]下无法找到资源文件[{1}/{2}]", _assetRoot, ABNameWithoutExtension(abName), assetName));
            }
            return null;
        }

        public override string[] GetDepends(string abName)
        {
            return new string[0];
        }

        public override void Unload(string abName, bool isUnlocadAllLoaded = false, bool isUnLoadDepends = true)
        {
            Resources.UnloadUnusedAssets();
        }

        public override void UnloadAll(bool isUnloadAllLoaded = false)
        {
            Resources.UnloadUnusedAssets();
        }

        public override string[] GetAllAssetsNames(string abName)
        {
#if UNITY_EDITOR
            var assetNames = new List<string>();

            var dirPath = GetDirOfAB(abName);

            var files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                var filePath = FileUtility.StandardizeBackslashSeparator(file);
                if (Path.GetExtension(filePath) == ".meta")
                {
                    continue;
                }

                if (File.Exists(filePath + ".meta"))
                {
                    assetNames.Add(Path.GetFileName(filePath));
                }
            }

            return assetNames.Count > 0 ? assetNames.ToArray() : null;
#else
            return null;
#endif
        }

        public override UnityEngine.Object Load(string abName, string assetName)
        {
            return Load<UnityEngine.Object>(abName, assetName);
        }

        public override UnityEngine.Object[] LoadAll(string abName)
        {
#if UNITY_EDITOR
            var assets = new List<UnityEngine.Object>();

            var dirPath = GetDirOfAB(abName);

            var files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                var filePath = FileUtility.StandardizeBackslashSeparator(file);
                if (Path.GetExtension(filePath) == ".meta")
                {
                    continue;
                }

                if (File.Exists(filePath + ".meta"))
                {
                    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath);
                    assets.Add(asset);
                }
            }

            return assets.Count > 0 ? assets.ToArray() : null;
#else
            return null;
#endif
        }

        public override T Load<T>(string abName, string assetName)
        {
#if UNITY_EDITOR
            string path = AssetBundlePath2ResourcePath(abName, assetName);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                Debug.LogErrorFormat("资源不存在: {0}", ResMgr.Ins.LinkAssetPath(abName, assetName));// 不是很理解为什么不直接用CombinePaths
            }
            return asset;
#else
            return default(T); // 返回T的默认值
#endif
        }

        public override void LoadAsync(string abName, string assetName, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }

        public override void LoadAsync<T>(string abName, string assetName, Action<T> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }

        public override void LoadAllAsync(string abName, Action<UnityEngine.Object[]> onLoaded, Action<float> onProgress = null)
        {
            throw new NotImplementedException();
        }
    }
}