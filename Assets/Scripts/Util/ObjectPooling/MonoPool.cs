using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> where T : MonoPoolable
{
    private T prefab = null;

    private List<T> pool = new List<T>(25);
    private List<T> deployed = new List<T>(25);

    public T Prefab
    {
        get
        {
            return prefab;
        }
    }

    public MonoPool(T prefab)
    {
        this.prefab = prefab;
    }

    public T Spawn(Vector3 position, Quaternion rotation)
    {
        if(pool.Count > 0)
        {
            T instance = pool[pool.Count - 1];

            instance.transform.position = position;
            instance.transform.rotation = rotation;

            pool.RemoveAt(pool.Count - 1);
            deployed.Add(instance);

            instance.OnSpawned();
            instance.Reinitialize();
            instance.gameObject.SetActive(true);

            return instance;
        }
        else
        {
            T instance = GameObject.Instantiate(prefab, position, rotation);

            deployed.Add(instance);

            instance.OnSpawned();
            instance.Reinitialize();
            return instance;
        }
    }

    public void Despawn(T instance)
    {
        instance.gameObject.SetActive(false);
        instance.OnDespawned();

        if (!deployed.Contains(instance))
        {
            Debug.LogError("Despawned object was not spawned from this pool. This is not supported.");
        }
        else
        {
            deployed.Remove(instance);
        }

        pool.Add(instance);
    }
}
