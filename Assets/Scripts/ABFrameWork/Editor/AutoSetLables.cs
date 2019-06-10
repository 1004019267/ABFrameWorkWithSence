/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     AutoSetLables.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-13
 *Description:   AB�������
 * ������Ҫ�����Դ���ļ��и�Ŀ¼
 * ����ÿ��"����"�ļ���
 * ����������Ŀ¼������Ŀ¼�����ļ�
 * �����Ŀ¼��������ݹ���ʣ�ֱ����λ���ļ�
 * �ҵ��ļ�����ʹ��AssetImporter�� ��ǰ�������׺��
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
                //�������true��ʾ����ɾ���ǿ�Ŀ¼
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
        /// ����AB����
        /// </summary>
        [MenuItem("ABTools/SetABLable")]
        public static void SetABLable()
        {
            string strNeedSetLableRoot = string.Empty;
            //��Ŀ¼�����г���Ŀ¼
            DirectoryInfo[] dirSencesDIRArray;

            //�������AB���
            AssetDatabase.RemoveUnusedAssetBundleNames();

            strNeedSetLableRoot = PathTools.GetABResourcesPath();
            //��ȡ·���������ļ���
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
            Debug.Log("AB���ñ�����");
        }
        /// <summary>
        /// �ݹ��ж��Ƿ�ΪĿ¼���ļ����޸�AB���
        /// </summary>
        /// <param name="currentDIR">��ǰ�ļ���Ϣ (��Ŀ¼��Ϣ�����໥ת��)</param>
        /// <param name="scenesName"></param>
        static void JudgeDIRorFileByRecursive(FileSystemInfo fileSystemInfo, string scenesName)
        {

            if (!fileSystemInfo.Exists)
            {
                Debug.LogError("Ŀ¼���ļ�����" + fileSystemInfo + "������,����");
                return;
            }

            DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            foreach (var item in fileSystemInfos)
            {
                FileInfo fileInfo = item as FileInfo;

                if (fileInfo != null)
                {
                    //�޸�AB��ǩ
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
            //AB����
            string ABName = string.Empty;
            string assetFilePath = string.Empty;

            //����׺
            if (fileInfo.Extension == ".meta")
                return;

            ABName = GetABName(fileInfo, scenesName);

            //��ȡ��Asset֮���Ŀ¼
            int tmpIndex = fileInfo.FullName.IndexOf("Assets");
            assetFilePath = fileInfo.FullName.Substring(tmpIndex);
            //����Դ�ļ�����AB���Ƽ���׺
            AssetImporter tmpImpObj = AssetImporter.GetAtPath(assetFilePath);
            tmpImpObj.assetBundleName = ABName;
            if (fileInfo.Extension == ".unity")
            {
                //����AB����չ��
                tmpImpObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImpObj.assetBundleVariant = "ab";
            }
        }

        /// <summary>
        /// ���غϷ�AB������
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="scenesName"></param>
        /// <returns></returns>
        static string GetABName(FileInfo fileInfo, string scenesName)
        {
            string ABName = string.Empty;

            //Win·��
            string tmpWinPath = fileInfo.FullName;
            //Unity·��
            string tmpUnityPath = tmpWinPath.Replace("\\", "/");
            //��λ"��������"�����ַ�λ��
            int tmpScenceNamePos = tmpUnityPath.IndexOf(scenesName) + scenesName.Length;
            //AB����"��������"��������
            string strABFileNameArea = tmpUnityPath.Substring(tmpScenceNamePos + 1);

            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split('/');
                ABName = scenesName + "/" + tempStrArray[0];
            }
            else
            {
                //sences��������
                ABName = scenesName + "/" + scenesName;
            }
            return ABName;
        }
    }
}