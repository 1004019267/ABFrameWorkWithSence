/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     AssetLoader.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-15
 *Description:   
 * ���������ָ��AB��Դ
 * ���ؾ���"���湦��"����Դ ��ѡ�ò���
 * ж�ء��ͷ�AB��Դ
 * �鿴��ǰAB����Դ
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
        /// ��������
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
        /// ���ص�ǰ����ָ������
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
            //�Ƿ񻺴漯���Ѿ�����
            if (ht.Contains(assetName))
            {
                return ht[assetName] as T;
            }

            //��ʽ����
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
        /// ж��ָ������Դ
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
        /// �ͷŵ�ǰAB�ڴ澵����Դ
        /// </summary>
        public void Dispose()
        {
            currentAB.Unload(false);
        }
        /// <summary>
        /// �ͷŵ�ǰAB�ڴ澵����Դ,���ͷ��ڴ���Դ
        /// </summary>
        public void DisposeAll()
        {
            currentAB.Unload(true);
        }

        /// <summary>
        /// ��ѯ��ǰAB��������������Դ����
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            return currentAB.GetAllAssetNames();
        }
    }
}