/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     AutoSetLables.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-13
 *Description:   AB打包工具
 * 定义需要打包资源的文件夹根目录
 * 遍历每个"场景"文件夹
 * 遍历本场景目录下所有目录或者文件
 * 如果是目录，则继续递归访问，直到定位到文件
 * 找到文件，则使用AssetImporter类 标记包名、后缀名
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace ABFrameWork
{

    public class DeleteAB
    {
        [MenuItem("ABTools/DeleteAllAB")]
        public static void DeleteAllAB()
        {
            string strABNeedPathDir = string.Empty;

            strABNeedPathDir = PathTools.GetABOutPath();
            if (!string.IsNullOrEmpty(strABNeedPathDir))
            {
                //这里参数true表示可以删除非空目录
                Directory.Delete(strABNeedPathDir,true);

                File.Delete(strABNeedPathDir+".meta");
                AssetDatabase.Refresh();
            }
        }
    }


    public class BuildAB
    {
        [MenuItem("ABTools/BuildAllAB")]
        public static void BuildAllAB()
        {
            string strABOutPathDir = string.Empty;

            strABOutPathDir = PathTools.GetABOutPath();
            if (!Directory.Exists(strABOutPathDir))
            {
                Directory.CreateDirectory(strABOutPathDir);
            }

            BuildPipeline.BuildAssetBundles(strABOutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();
        }
    }



    public class AutoSetLables
    {
        /// <summary>
        /// 设置AB包名
        /// </summary>
        [MenuItem("ABTools/SetABLable")]
        public static void SetABLable()
        {
            string strNeedSetLableRoot = string.Empty;
            //根目录下所有场景目录
            DirectoryInfo[] dirSencesDIRArray;

            //清空无用AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();

            strNeedSetLableRoot = PathTools.GetABResourcesPath();
            //获取路径下所有文件夹
            DirectoryInfo directoryInfo = new DirectoryInfo(strNeedSetLableRoot);
            dirSencesDIRArray = directoryInfo.GetDirectories();

            foreach (var item in dirSencesDIRArray)
            {
                string tmpSencesDIR = strNeedSetLableRoot + "/" + item.Name;
                int tmpIndex = tmpSencesDIR.LastIndexOf("/");
                string tmpScenesName = tmpSencesDIR.Substring(tmpIndex + 1);

                JudgeDIRorFileByRecursive(item, tmpScenesName);
            }
            AssetDatabase.Refresh();
            Debug.Log("AB设置标记完成");
        }
        /// <summary>
        /// 递归判断是否为目录与文件，修改AB标记
        /// </summary>
        /// <param name="currentDIR">当前文件信息 (和目录信息可以相互转换)</param>
        /// <param name="scenesName"></param>
        static void JudgeDIRorFileByRecursive(FileSystemInfo fileSystemInfo, string scenesName)
        {

            if (!fileSystemInfo.Exists)
            {
                Debug.LogError("目录或文件名称" + fileSystemInfo + "不存在,请检查");
                return;
            }

            DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            foreach (var item in fileSystemInfos)
            {
                FileInfo fileInfo = item as FileInfo;

                if (fileInfo != null)
                {
                    //修改AB标签
                    SetFileABLable(fileInfo, scenesName);
                }
                else
                {
                    JudgeDIRorFileByRecursive(item, scenesName);
                }
            }
        }

        static void SetFileABLable(FileInfo fileInfo, string scenesName)
        {
            //AB包名
            string ABName = string.Empty;
            string assetFilePath = string.Empty;

            //检查后缀
            if (fileInfo.Extension == ".meta")
                return;

            ABName = GetABName(fileInfo, scenesName);

            //截取到Asset之后的目录
            int tmpIndex = fileInfo.FullName.IndexOf("Assets");
            assetFilePath = fileInfo.FullName.Substring(tmpIndex);
            //给资源文件设置AB名称及后缀
            AssetImporter tmpImpObj = AssetImporter.GetAtPath(assetFilePath);
            tmpImpObj.assetBundleName = ABName;
            if (fileInfo.Extension == ".unity")
            {
                //定义AB包扩展名
                tmpImpObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImpObj.assetBundleVariant = "ab";
            }
        }

        /// <summary>
        /// 返回合法AB包名称
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="scenesName"></param>
        /// <returns></returns>
        static string GetABName(FileInfo fileInfo, string scenesName)
        {
            string ABName = string.Empty;

            //Win路径
            string tmpWinPath = fileInfo.FullName;
            //Unity路径
            string tmpUnityPath = tmpWinPath.Replace("\\", "/");
            //定位"场景名称"后面字符位置
            int tmpScenceNamePos = tmpUnityPath.IndexOf(scenesName) + scenesName.Length;
            //AB包中"类型名称"所在区域
            string strABFileNameArea = tmpUnityPath.Substring(tmpScenceNamePos + 1);

            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split('/');
                ABName = scenesName + "/" + tempStrArray[0];
            }
            else
            {
                //sences特殊名字
                ABName = scenesName + "/" + scenesName;
            }
            return ABName;
        }
    }
}