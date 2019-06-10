/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABMgr.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-19
 *Description:   多包管理按场景加载
 * 读取清单文件 缓存脚本
 * 以场景为单位 管理整个项目中所有AB包
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFrameWork
{

    public class ABMgr : SingletonM<ABMgr>
    {
        //场景集合
        Dictionary<string, MultABMgr> allScenes = new Dictionary<string, MultABMgr>();
        //AB清单文件
        AssetBundleManifest manifest;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(ABManifestLoader.Instance.LoadMainfestFile());
        }
        public IEnumerator LoadAB(string sencesName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            //参数检查
            if (string.IsNullOrEmpty(sencesName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError(GetType() + "/LoadAB()/sencesName or abName==null,please check!");
            }
            //等待Manifest清单文件加载完成
            while (!ABManifestLoader.Instance.isLoadFinish)
            {
                yield return null;
            }
            manifest = ABManifestLoader.Instance.GetABManifest();
            if (manifest == null)
            {
                Debug.LogError(GetType() + "/LoadAB()/manifest==null,please check!");
                yield return null;
            }

            MultABMgr multABMgr;
            //把当前场景加入到集合中
            if (!allScenes.ContainsKey(sencesName))
            {
                multABMgr = new MultABMgr(sencesName, abName, loadAllCompleteHandle);
                allScenes.Add(sencesName, multABMgr);
            }

            //调用下一层(多包管理员)
            multABMgr = allScenes[sencesName];
            if (multABMgr == null)
            {
                Debug.LogError(GetType() + "/LoadAB()/multABMgr==null,please check!");
            }
            //调用多包管理类的加载指定AB包
            yield return multABMgr.LoadAB(abName);
        }
        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="scenesName">场景名</param>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public Object LoadAsset(string scenesName, string abName, string assetName, bool isCache)
        {
            if (allScenes.ContainsKey(scenesName))
            {
                MultABMgr multABMgr = allScenes[scenesName];
                return multABMgr.LoadAsset(abName, assetName, isCache);
            }
            Debug.LogError(GetType() + $"/LoadAsset()do not find scenes,can't loadAsset, scenesName={scenesName}");
            return null;
        }

        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="scenesName">场景名</param>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public void DisposeAllAssets(string scenesName)
        {
            if (allScenes.ContainsKey(scenesName))
            {
                MultABMgr multABMgr = allScenes[scenesName];
                multABMgr.DisposeAllAsset();
            }
            else
            {
                Debug.LogError(GetType() + $"/DisposeAllAssets()do not find scenes,can't DisposeAllAssets, scenesName={scenesName}");
            }
        }
    }

}