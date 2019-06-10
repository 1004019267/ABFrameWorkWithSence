/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     MultABMgr.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-16
 *Description:   针对一个场景中关于多个AB综合管理
 * 获取AB包之间的依赖关系和引用关系
 * 管理AB包之间的自动连接(递归加载机制)
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFrameWork
{
    public class MultABMgr
    {
        /// <summary>
        /// 引用类 "单个AB包加载实现"
        /// </summary>
        SingleAssetLoader currentSingleABLoader;
        /// <summary>
        /// Ab包缓存集合(防止重复加载)
        /// </summary>
        Dictionary<string, SingleAssetLoader> singleABLoaderCache = new Dictionary<string, SingleAssetLoader>();
        /// <summary>
        /// 当前场景(调用使用)
        /// </summary>
        string currentScenceName;

        /// <summary>
        /// Ab包名
        /// </summary>
        string currentABName;

        /// <summary>
        /// AB包与对应依赖关系集合
        /// </summary>
        Dictionary<string, ABRelation> abRelation = new Dictionary<string, ABRelation>();
        /// <summary>
        /// 所有AB包加载完成回调
        /// </summary>
        DelLoadComplete LoadALLABPackageCompleteHandel;

        public MultABMgr(string senceName, string abName, DelLoadComplete loadAllABPackCompleteHandle)
        {
            currentScenceName = senceName;
            currentABName = abName;

            LoadALLABPackageCompleteHandel = loadAllABPackCompleteHandle;
        }

        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        /// <param name="name"></param>
        void CompleteLoadAB(string abName)
        {
            if (abName.Equals(currentABName))
            {
                LoadALLABPackageCompleteHandel?.Invoke(abName);
            }
        }

        public IEnumerator LoadAB(string abName)
        {
            ABRelation abRel;
            //AB包关系建立
            if (!abRelation.ContainsKey(abName))
            {
                abRel = new ABRelation(abName);
                abRelation.Add(abName, abRel);
            }
            abRel = abRelation[abName];
            //得到指定AB包所有依赖关系(查询Manifest清单文件)
            string[] strDependeceArray = ABManifestLoader.Instance.RetrivalDependce(abName);

            foreach (var item in strDependeceArray)
            {
                //添加依赖项
                abRel.AddDependence(item);
                //加载引用项(递归调用)
                yield return LoadReference(item, abName);
            }

            //加载AB包
            if (singleABLoaderCache.ContainsKey(abName))
            {
                yield return singleABLoaderCache[abName].LoadAB();
            }
            else
            {
                currentSingleABLoader = new SingleAssetLoader(abName, CompleteLoadAB);
                singleABLoaderCache.Add(abName,currentSingleABLoader);
                yield return currentSingleABLoader.LoadAB();
            }
            yield return null;
        }
        /// <summary>
        /// 加载引用AB包
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="refName">被引用AB包名称</param>
        /// <returns></returns>
        IEnumerator LoadReference(string abName, string refName)
        {
            ABRelation tmpABRelation;
            if (abRelation.ContainsKey(abName))
            {
                tmpABRelation = abRelation[abName];
                //添加AB包引用关系(被依赖)
                tmpABRelation.AddReference(refName);
            }
            else
            {
                tmpABRelation = new ABRelation(abName);
                tmpABRelation.AddReference(refName);
                abRelation.Add(abName, tmpABRelation);

                //开始加载依赖包(递归)
                yield return LoadAB(abName);
            }

            yield return null;
        }

        /// <summary>
        /// 加载AB包中资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public Object LoadAsset(string abName, string assetName, bool isCache)
        {
            foreach (var item in singleABLoaderCache.Keys)
            {
                if (abName == item)
                {
                    return singleABLoaderCache[item].LoadAsset(assetName, isCache);
                }
            }
            Debug.LogError(GetType() + $"/LoadAsset()/do not find AB please check! abName{abName} assetName{assetName}");
            return null;
        }

        public void DisposeAllAsset()
        {
            try
            {
                //逐一释放所有加载过的AB包中资源
                foreach (var item in singleABLoaderCache.Values)
                {
                    item.DisposeAll();
                }
            }
            finally
            {
                singleABLoaderCache.Clear();
                singleABLoaderCache = null;

                //释放其他对象占用资源
                abRelation.Clear();
                abRelation = null;
                currentABName = null;
                currentScenceName = null;
                LoadALLABPackageCompleteHandel = null;

                //卸载没有用到的资源
                Resources.UnloadUnusedAssets();
                //强制垃圾收集
                System.GC.Collect();

            }

        }
    }
}