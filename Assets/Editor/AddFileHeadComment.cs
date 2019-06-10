using UnityEngine;
using UnityEditor;
using System.IO;
//UnityEditor.AssetModificationProcessor监听Project视图中资源的  创建  删除  移动 保存
public class AddFileHeadComment :UnityEditor.AssetModificationProcessor
{
    /// <summary>
    /// asset被创建完，文件生成在磁盘上但没有生成.meta文件和import之前调用
    /// </summary>
    /// <param name="newFileMeta">是由创建文件的path加上.meta组成的</param>
	public static void OnWillCreateAsset(string newFileMeta)
    {
        string newPath = newFileMeta.Replace(".meta", "");
        //获取的是文件后面的.xx的后缀
        string fileExt = Path.GetExtension(newPath);
        if (fileExt!=".cs")
            return;
        //Application.datapath会根据使用平台不同而不同   多了一个Assets替换掉
        string realPath = Application.dataPath.Replace("Assets", "") + newPath;
        string scriptContent = File.ReadAllText(realPath);

        //这里实现自定义的一些规则 就是替换注释信息
        scriptContent = scriptContent.Replace("#SCRIPTFULLNAME#", Path.GetFileName(newPath));
        scriptContent = scriptContent.Replace("#COMPANY#", PlayerSettings.companyName);
        scriptContent = scriptContent.Replace("#AUTHOR#", "why");
        scriptContent = scriptContent.Replace("#VERSION#", "1.0");
        scriptContent = scriptContent.Replace("#UNITYVERSION#", Application.unityVersion);
        scriptContent = scriptContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy-MM-dd"));

        File.WriteAllText(realPath, scriptContent);
    }
}
