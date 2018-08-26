using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : Singleton<GameLoop>
{
    private const int spawnAttemptsPerFrame = 3;

    #region Inspector

    [SerializeField]
    private EnemySpawnParams spawnParams = null;

    [SerializeField]
    private SceneParams sceneParams = null;

    #endregion

    private int kills = 0;
    private int spawnedEnemies = 0;

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
                while(spawnedEnemies >= spawnParams.SpawnCap)
                {
                    yield return null;
                }

                nextSpawnTimestamp = Time.time + spawnParams.SpawnInterval;

                bool spawnTitan = spawnParams.GetSpawnTitanNext(kills);
                Enemy enemy;

                if(spawnTitan)
                {
                    enemy = spawnParams.TitanPrefab;
                }
                else
                {
                    enemy = spawnParams.GetWeighedRandomEnemy();
                    spawnedEnemies++;
                }

                Vector3 nextPosition = GetRandomSpawnPosition();
                bool positionValidated = false;

                while (!positionValidated)
                {
                    for (int i = 0; i < spawnAttemptsPerFrame; i++)
                    {
                        if (!MathHelpers.CustomBoxCheck(nextPosition, Vector3.zero, 0.0f, Quaternion.identity, enemy.transform.localScale))
                        {
                            positionValidated = true;
                            break;
                        }

                        nextPosition = GetRandomSpawnPosition();
                    }

                    yield return null;
                }

                PoolService.Instance.Spawn(enemy, nextPosition, Quaternion.identity);

                if (spawnTitan)
                {
                    kills = 0;
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

    public void OnEnemyKilled()
    {
        spawnedEnemies--;
        kills++;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(sceneParams.MenuSceneIndex);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(sceneParams.GameSceneIndex);
    }
}
