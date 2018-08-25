using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogErrorFormat("Duplicate singleton init attempted. Existing instance is a component of: {0}. This is not supported.", instance.name);
            return;
        }

        instance = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public void OnDestroy()
    {
        instance = null;
    }
}
