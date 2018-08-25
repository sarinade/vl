using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : Singleton<GameLoop>
{
    #region Inspector

    [SerializeField]
    private EnemySpawnParams spawnParams = null;

    #endregion

    private int kills = 0;
    private float nextSpawnTimestamp = 0.0f;

    void Start()
    {
        StartCoroutine(LoopRoutine());
    }

    private IEnumerator LoopRoutine()
    {
        while(true)
        {
            if(Time.time >= nextSpawnTimestamp)
            {
                nextSpawnTimestamp = Time.time + spawnParams.SpawnInterval;
                Vector3 nextPosition = GetRandomSpawnPosition();
                bool spawnTitan = spawnParams.GetSpawnTitanNext(kills);
                
                if(spawnTitan)
                {
                    Enemy titan = spawnParams.TitanPrefab;
                    PoolService.Instance.Spawn(titan, nextPosition, Quaternion.identity);

                    yield break;
                }
                else
                {
                    Enemy prefab = spawnParams.GetWeighedRandomEnemy();
                    PoolService.Instance.Spawn(prefab, nextPosition, Quaternion.identity);
                }
            }

            yield return null;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 pointPonCircle = Random.insideUnitCircle.normalized;
        Vector3 result = transform.position + new Vector3(pointPonCircle.x, 0.0f, pointPonCircle.y) * spawnParams.SpawnRadius;

        return result;
    }
}
