/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     AssetLoader.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-15
 *Description:   
 * 管理与加载指定AB资源
 * 加载具有"缓存功能"的资源 带选用参数
 * 卸载、释放AB资源
 * 查看当前AB的资源
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFrameWork
{
    public class AssetLoader : System.IDisposable
    {
        AssetBundle currentAB;
        /// <summary>
        /// 缓存容器
        /// </summary>
        Hashtable ht=new Hashtable();

        public AssetLoader(AssetBundle abObj)
        {
            if (abObj != null)
            {
                currentAB = abObj;
            }
            else
            {
                Debug.LogError($"{GetType()}/AssetLoader/abObj= null,please check!");
            }

        }

        /// <summary>
        /// 加载当前包中指定数据
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public Object LoadAsset(string assetName, bool isCache = false)
        {
            return LoadRes<Object>(assetName, isCache);
        }

        T LoadRes<T>(string assetName, bool isCache) where T : Object
        {
            //是否缓存集合已经存在
            if (ht.Contains(assetName))
            {
                return ht[assetName] as T;
            }

            //正式加载
            T tmpRes = currentAB.LoadAsset<T>(assetName);
            if (tmpRes != null && isCache)
            {
                ht.Add(assetName, tmpRes);
            }
            else if (tmpRes == null)
            {
                Debug.LogError($"{GetType()}/LoadRes<T>/tmpRes == null,please check!");
            }
            return tmpRes;
        }

        /// <summary>
        /// 卸载指定的资源
        /// </summary>
        public bool UnLoadAsset(Object asset)
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }
            Debug.LogError($"{GetType()}/UnLoadAsset/asset == null,please check!");
            return false;
        }


        /// <summary>
        /// 释放当前AB内存镜像资源
        /// </summary>
        public void Dispose()
        {
            currentAB.Unload(false);
        }
        /// <summary>
        /// 释放当前AB内存镜像资源,且释放内存资源
        /// </summary>
        public void DisposeAll()
        {
            currentAB.Unload(true);
        }

        /// <summary>
        /// 查询当前AB包包含的所有资源名称
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            return currentAB.GetAllAssetNames();
        }
    }
}