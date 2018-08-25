using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolService : Singleton<PoolService>
{
    private Dictionary<MonoPoolable, MonoPool<MonoPoolable>> pools = new Dictionary<MonoPoolable, MonoPool<MonoPoolable>>(25);

    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoPoolable
    {
        if(prefab == null)
        {
            throw new MissingReferenceException();
        }

        MonoPool<MonoPoolable> pool = GetPool(prefab);

        T result = (T) pool.Spawn(position, rotation);
        result.transform.SetParent(transform);
        result.Prefab = prefab;

        return result;
    }

    public void Despawn(MonoPoolable instance)
    {
        MonoPool<MonoPoolable> pool = GetPool(instance.Prefab);
        pool.Despawn(instance);
    }

    private MonoPool<MonoPoolable> GetPool(MonoPoolable prefab)
    {
        if(pools.ContainsKey(prefab))
        {
            return pools[prefab];
        }
        else
        {
            MonoPool<MonoPoolable> newPool = new MonoPool<MonoPoolable>(prefab);
            pools.Add(prefab, newPool);

            return newPool;
        }
    }
}
