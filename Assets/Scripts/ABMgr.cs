/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABMgr.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-19
 *Description:   ���������������
 * ��ȡ�嵥�ļ� ����ű�
 * �Գ���Ϊ��λ ����������Ŀ������AB��
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFrameWork
{

    public class ABMgr : SingletonM<ABMgr>
    {
        //��������
        Dictionary<string, MultABMgr> allScenes = new Dictionary<string, MultABMgr>();
        //AB�嵥�ļ�
        AssetBundleManifest manifest;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(ABManifestLoader.Instance.LoadMainfestFile());
        }
        public IEnumerator LoadAB(string sencesName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            //�������
            if (string.IsNullOrEmpty(sencesName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError(GetType() + "/LoadAB()/sencesName or abName==null,please check!");
            }
            //�ȴ�Manifest�嵥�ļ��������
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
            //�ѵ�ǰ�������뵽������
            if (!allScenes.ContainsKey(sencesName))
            {
                multABMgr = new MultABMgr(sencesName, abName, loadAllCompleteHandle);
                allScenes.Add(sencesName, multABMgr);
            }

            //������һ��(�������Ա)
            multABMgr = allScenes[sencesName];
            if (multABMgr == null)
            {
                Debug.LogError(GetType() + "/LoadAB()/multABMgr==null,please check!");
            }
            //���ö��������ļ���ָ��AB��
            yield return multABMgr.LoadAB(abName);
        }
        /// <summary>
        /// ����AB����Դ
        /// </summary>
        /// <param name="scenesName">������</param>
        /// <param name="abName">����</param>
        /// <param name="assetName">��Դ��</param>
        /// <param name="isCache">�Ƿ񻺴�</param>
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
        /// ����AB����Դ
        /// </summary>
        /// <param name="scenesName">������</param>
        /// <param name="abName">����</param>
        /// <param name="assetName">��Դ��</param>
        /// <param name="isCache">�Ƿ񻺴�</param>
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