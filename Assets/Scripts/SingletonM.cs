using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// 继承于MonoBehaviour的单利类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonM<T> : MonoBehaviour where T : MonoBehaviour
{

    static T _instance = default(T);

    public static T Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance =(T)FindObjectOfType(typeof(T));              
                if (_instance==null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
            Destroy(this.gameObject);
    }
}