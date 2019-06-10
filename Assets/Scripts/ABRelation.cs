/**
 *Copyright(C) 2019 by DefaultCompany
 *All rights reserved.
 *FileName:     ABRelation.cs
 *Author:       why
 *Version:      1.0
 *UnityVersion:2018.3.9f1
 *Date:         2019-05-16
 *Description:  AB关系类
 * 储存制定AB包的所有依赖关系包
 * 储存制定AB包所有的引用关系包
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFrameWork
{
    public class ABRelation
    {
        string ABName;
        List<string> allDependenceAB=new List<string>();
        List<string> allReferenceAB=new List<string>();

        public ABRelation(string ABName)
        {
            this.ABName = ABName;

        }
        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="Name"></param>
        public void AddDependence(string abName)
        {
            if (!allDependenceAB.Contains(abName))
            {
                allDependenceAB.Add(abName);
            }
            
        }
        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="abName"></param>
        /// true 此AB包没有依赖项
        /// false 此AB包还有其他的依赖项
        /// <returns></returns>
        public bool RemoveDependence(string abName)
        {
            if (allDependenceAB.Contains(abName))
            {
                allDependenceAB.Remove(abName);
            }

            if (allDependenceAB.Count>0)
                return false;
            else
                return true;
        }

        public List<string>GetAllDependence()
        {
            return allDependenceAB;
        }


        /// <summary>
        /// 增加引用关系
        /// </summary>
        /// <param name="Name"></param>
        public void AddReference(string abName)
        {
            if (!allReferenceAB.Contains(abName))
            {
                allReferenceAB.Add(abName);
            }

        }
        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName"></param>
        /// true 此AB包没有依赖项
        /// false 此AB包还有其他的依赖项
        /// <returns></returns>
        public bool RemoveReference(string abName)
        {
            if (allReferenceAB.Contains(abName))
            {
                allReferenceAB.Remove(abName);
            }

            if (allReferenceAB.Count > 0)
                return false;
            else
                return true;
        }

        public List<string> GetAllReference()
        {
            return allReferenceAB;
        }
    }
}