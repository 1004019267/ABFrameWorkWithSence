/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABDefine.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-15
 *Description:   工具类
 * 本项目所有的常量
 * 所有的委托定义
 * 枚举定义
 * 常量定义
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFrameWork
{
    public delegate void DelLoadComplete(string abName);

    /// <summary>
    /// 框架常量
    /// </summary>
    public class ABDefine
    {
        public static string ASSETBUNLDE_MANIFEST = "AssetBundleManifest";
    }
}