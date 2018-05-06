using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유니티용 싱글톤 템플릿
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    public static T instance{
        get{
            if (!_instance)
            {
                var gameObject = new GameObject();
                var component = gameObject.AddComponent<T>();
                _instance = component;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(!instance){
            _instance = GetComponent<T>();
        }
    }
}
