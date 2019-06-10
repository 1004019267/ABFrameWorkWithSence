/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABManifestLoader.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-16
 *Description:   读取AB的依赖关系(Windows.mainfest)
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ABFrameWork
{
    public class ABManifestLoader : Singleton<ABManifestLoader>,System.IDisposable
    {
       
        /// <summary>
        /// 系统ab清单文件
        /// </summary>
        AssetBundleManifest manifest;
        /// <summary>
        /// ab清单文件路径
        /// </summary>
        string strManifestPath;
        /// <summary>
        /// 读取AB清单文件的AB
        /// </summary>
        AssetBundle ABReadManifest;

        public bool isLoadFinish { get; private set; } = false;

        public ABManifestLoader()
        {
            strManifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatfromName();
        }

        /// <summary>
        /// 加载Mainfest清单文件
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadMainfestFile()
        {
            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(strManifestPath))
            {
                yield return req.SendWebRequest();

                if (req.downloadProgress >= 1)
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
                    if (ab != null)
                    {
                        ABReadManifest = ab;
                        manifest = ab.LoadAsset(ABDefine.ASSETBUNLDE_MANIFEST) as AssetBundleManifest;
                        isLoadFinish = true;
                    }
                    else
                    {
                        Debug.LogError($"{GetType()}/LoadMainfestFile() downLoad Error, please check{strManifestPath}");
                    }
                }
            }
        }

        /// <summary>
        /// 获取ABManifest
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetABManifest()
        {
            if (isLoadFinish)
            {
                if (manifest != null)
                {
                    return manifest;
                }
                else
                {
                    Debug.LogError($"{GetType()}/GetABManifest() manifest==null, please check");
                }
            }
            else
            {
                Debug.LogError($"{GetType()}/GetABManifest() isLoadFinish==false, please check");
            }
            return null;
        }

        /// <summary>
        /// 获取ABManifest系统类的依赖项
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public string[] RetrivalDependce(string abName)
        {
            if (!string.IsNullOrEmpty(abName))
            {
                return manifest?.GetAllDependencies(abName);
            }
            return null;
        }

        /// <summary>
        /// 释放本类资源
        /// </summary>
        public void Dispose()
        {
            ABReadManifest?.Unload(true);
        }
    }
}
