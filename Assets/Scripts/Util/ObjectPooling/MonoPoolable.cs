using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPoolable : MonoBehaviour
{
    private MonoPoolable prefab = null;
    private bool isSpawned = true;

    public MonoPoolable Prefab
    {
        get
        {
            return prefab;
        }
        set
        {
            prefab = value;
        }
    }

    public void OnSpawned()
    {
        isSpawned = true;
    }

    public void OnDespawned()
    {
        isSpawned = false;
    }

    public virtual void Reinitialize() { }

    public void Despawn(float delay)
    {
        if (!isSpawned)
            return;

        StartCoroutine(DespawnRoutine(delay));
    }

    private IEnumerator DespawnRoutine(float delay)
    {
        float elapsed = 0.0f;

        while(elapsed < delay)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }

        StopAllCoroutines();
        PoolService.Instance.Despawn(this);
    }
}
