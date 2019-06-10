/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     MultABMgr.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-16
 *Description:   ���һ�������й��ڶ��AB�ۺϹ���
 * ��ȡAB��֮���������ϵ�����ù�ϵ
 * ����AB��֮����Զ�����(�ݹ���ػ���)
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
        /// ������ "����AB������ʵ��"
        /// </summary>
        SingleAssetLoader currentSingleABLoader;
        /// <summary>
        /// Ab�����漯��(��ֹ�ظ�����)
        /// </summary>
        Dictionary<string, SingleAssetLoader> singleABLoaderCache = new Dictionary<string, SingleAssetLoader>();
        /// <summary>
        /// ��ǰ����(����ʹ��)
        /// </summary>
        string currentScenceName;

        /// <summary>
        /// Ab����
        /// </summary>
        string currentABName;

        /// <summary>
        /// AB�����Ӧ������ϵ����
        /// </summary>
        Dictionary<string, ABRelation> abRelation = new Dictionary<string, ABRelation>();
        /// <summary>
        /// ����AB��������ɻص�
        /// </summary>
        DelLoadComplete LoadALLABPackageCompleteHandel;

        public MultABMgr(string senceName, string abName, DelLoadComplete loadAllABPackCompleteHandle)
        {
            currentScenceName = senceName;
            currentABName = abName;

            LoadALLABPackageCompleteHandel = loadAllABPackCompleteHandle;
        }

        /// <summary>
        /// ���ָ��AB������
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
            //AB����ϵ����
            if (!abRelation.ContainsKey(abName))
            {
                abRel = new ABRelation(abName);
                abRelation.Add(abName, abRel);
            }
            abRel = abRelation[abName];
            //�õ�ָ��AB������������ϵ(��ѯManifest�嵥�ļ�)
            string[] strDependeceArray = ABManifestLoader.Instance.RetrivalDependce(abName);

            foreach (var item in strDependeceArray)
            {
                //���������
                abRel.AddDependence(item);
                //����������(�ݹ����)
                yield return LoadReference(item, abName);
            }

            //����AB��
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
        /// ��������AB��
        /// </summary>
        /// <param name="abName">ab������</param>
        /// <param name="refName">������AB������</param>
        /// <returns></returns>
        IEnumerator LoadReference(string abName, string refName)
        {
            ABRelation tmpABRelation;
            if (abRelation.ContainsKey(abName))
            {
                tmpABRelation = abRelation[abName];
                //���AB�����ù�ϵ(������)
                tmpABRelation.AddReference(refName);
            }
            else
            {
                tmpABRelation = new ABRelation(abName);
                tmpABRelation.AddReference(refName);
                abRelation.Add(abName, tmpABRelation);

                //��ʼ����������(�ݹ�)
                yield return LoadAB(abName);
            }

            yield return null;
        }

        /// <summary>
        /// ����AB������Դ
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
                //��һ�ͷ����м��ع���AB������Դ
                foreach (var item in singleABLoaderCache.Values)
                {
                    item.DisposeAll();
                }
            }
            finally
            {
                singleABLoaderCache.Clear();
                singleABLoaderCache = null;

                //�ͷ���������ռ����Դ
                abRelation.Clear();
                abRelation = null;
                currentABName = null;
                currentScenceName = null;
                LoadALLABPackageCompleteHandel = null;

                //ж��û���õ�����Դ
                Resources.UnloadUnusedAssets();
                //ǿ�������ռ�
                System.GC.Collect();

            }

        }
    }
}