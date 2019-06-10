/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     PathTools.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-13
 *Description:   路径工具类
 * 包含路径常量
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFrameWork
{
    public class PathTools 
    {

        public const string AB_ResourcesPath = "AB_Res";

        /// <summary>
        /// 获取AB标记路径
        /// </summary>
        /// <returns></returns>
        public static string GetABResourcesPath()
        {
            return Application.dataPath + "/" + AB_ResourcesPath;
        }

        /// <summary>
        /// 获取AB输出路径
        /// 1.平台(PC/移动)路径
        /// </summary>
        public static string GetABOutPath()
        {        
            return GetPlatfromPath()+"/"+ GetPlatfromName();
        }

        /// <summary>
        /// 获取平台路径
        /// </summary>
        /// <returns></returns>
        public static string GetPlatfromPath()
        {
            string strReturnPlatformPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    strReturnPlatformPath = Application.persistentDataPath;
                    break;
            }
            return strReturnPlatformPath;
        }

        /// <summary>
        /// 获取平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetPlatfromName()
        {
            string strReturnPlatformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformName = "Windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strReturnPlatformName = "IPhone";
                    break;
                case RuntimePlatform.Android:
                    strReturnPlatformName = "Android";
                    break;
            }
            return strReturnPlatformName;
        }
        public static string GetWWWPath()
        {
            string strWWWPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strWWWPath = "file://"+GetABOutPath();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strWWWPath = GetABOutPath()+"/Raw/";
                    break;
                case RuntimePlatform.Android:
                    strWWWPath = "jar:file://" + GetABOutPath();
                    break;
            }
            return strWWWPath;
        }
    }
   
}