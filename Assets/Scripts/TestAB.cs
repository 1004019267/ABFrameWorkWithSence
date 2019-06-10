/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     TestAB.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-15
 *Description:   ²âÊÔÀà
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABFrameWork;
public class TestAB : MonoBehaviour
{
    //string abName = "sences_1/prefabs.ab";
    //string assetName = "Cube.prefab";
    //SingleAssetLoader loader;

    string scenesName = "scence_1";
    string abName = "sences_1/prefabs.ab";
    string assetName = "Cube.prefab";
    void Start()
    {
        //loader = new SingleAssetLoader(abName, (abName) =>
        //{
        //    var a = loader.LoadAsset(assetName, false);
        //    Instantiate(a);
        //});
        //StartCoroutine(loader.LoadAB());

        StartCoroutine(ABMgr.Instance.LoadAB(scenesName, abName, (abName) =>
        {
            Object tmpObj = ABMgr.Instance.LoadAsset(scenesName, abName, assetName, false);
            if (tmpObj != null)
            {
                Instantiate(tmpObj);
            }
        }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ABMgr.Instance.DisposeAllAssets(scenesName);
        }
    }

}
