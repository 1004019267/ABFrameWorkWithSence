/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     SingleAssetLoader.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-15
 *Description:   
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ABFrameWork
{
    public class SingleAssetLoader : System.IDisposable
    {
        /// <summary>
        /// 引用类资源加载
        /// </summary>
        AssetLoader assetLoader;

        DelLoadComplete loadCompleteHandle;

        string ABName;

        string ABDownLoadPath;

        public SingleAssetLoader(string abName, DelLoadComplete loadComplete)
        {
            ABName = abName;
            loadCompleteHandle = loadComplete;
            ABDownLoadPath = PathTools.GetWWWPath() + "/" + abName;
        }

        public IEnumerator LoadAB()
        {
            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(ABDownLoadPath))
            {              
                yield return req.SendWebRequest();

                if (req.downloadProgress >= 1)
                {

                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
                    if (ab != null)
                    {
                        assetLoader = new AssetLoader(ab);
                        loadCompleteHandle?.Invoke(ABName);
                    }
                    else
                    {
                        Debug.LogError($"{GetType() }/ LoadAB() loadError ,please check! the{ABDownLoadPath} should be null");
                    }
                }
            }
        }

        /// <summary>
        /// 加载AB包指定资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public Object LoadAsset(string assetName, bool isCache)
        {
            if (assetLoader != null)
            {
                return assetLoader.LoadAsset(assetName, isCache);
            }
            Debug.LogError($"{GetType() }/LoadAsset() assetLoader==null,please check!");
            return null;
        }

        /// <summary>
        /// 卸载AB包指定资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnLoadAsset(Object asset)
        {
            if (assetLoader != null)
            {
                assetLoader.UnLoadAsset(asset);
            }
            else
            {
                Debug.LogError($"{GetType() }/UnLoadAsset(Object asset) assetLoader==null,please check!");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

            if (assetLoader != null)
            {
                assetLoader.Dispose();
            }
            else
            {
                Debug.LogError($"{GetType() }/Dispose() assetLoader==null,please check!");
            }
        }


        public void DisposeAll()
        {
            if (assetLoader != null)
            {
                assetLoader.DisposeAll();
            }
            else
            {
                Debug.LogError($"{GetType() }/DisposeAll() assetLoader==null,please check!");
            }
        }

        /// <summary>
        /// 查询AB包中所有资源
        /// </summary>
        /// <returns></returns>
        public string[] RetrivalAllAssetName()
        {
            if (assetLoader != null)
            {
                return assetLoader.RetriveAllAssetName();
            }
            Debug.LogError($"{GetType() }/etrivalAllAssetName() assetLoader==null,please check!");
            return null;
        }
    }

}