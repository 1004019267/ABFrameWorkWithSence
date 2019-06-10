using System;
using System.Collections;
using System.Collections.Generic;


public class Singleton<T> where T : class, new()
{

    static T _instance = default(T);
    static readonly object syslock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    _instance = Activator.CreateInstance<T>();
                }
            }
            return _instance;
        }
    }
}
