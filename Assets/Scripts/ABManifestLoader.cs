/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABManifestLoader.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-16
 *Description:   ��ȡAB��������ϵ(Windows.mainfest)
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
        /// ϵͳab�嵥�ļ�
        /// </summary>
        AssetBundleManifest manifest;
        /// <summary>
        /// ab�嵥�ļ�·��
        /// </summary>
        string strManifestPath;
        /// <summary>
        /// ��ȡAB�嵥�ļ���AB
        /// </summary>
        AssetBundle ABReadManifest;

        public bool isLoadFinish { get; private set; } = false;

        public ABManifestLoader()
        {
            strManifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatfromName();
        }

        /// <summary>
        /// ����Mainfest�嵥�ļ�
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
        /// ��ȡABManifest
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
        /// ��ȡABManifestϵͳ���������
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
        /// �ͷű�����Դ
        /// </summary>
        public void Dispose()
        {
            ABReadManifest?.Unload(true);
        }
    }
}
