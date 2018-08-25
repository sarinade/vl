using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnableEnemy
{
    public Enemy Prefab;
    public int weight;
}

[CreateAssetMenu(fileName = "EnemySpawnParams", menuName = "VL/Gameplay/EnemySpawnParams", order = 1)]
public class EnemySpawnParams : ScriptableObject
{
    public float SpawnInterval = 3.0f;
    public float SpawnRadius = 25.0f;

    [Space]

    public int SpawnCap = 3;
    public int KillsToSpawnTitan = 25;

    [Space]

    public Enemy TitanPrefab = null;
    public SpawnableEnemy[] SpawnableEnemies = null;

    public bool GetSpawnTitanNext(int totalKills)
    {
        return totalKills >= KillsToSpawnTitan;
    }

    public Enemy GetWeighedRandomEnemy()
    {
        int totalWeight = 0;

        for(int i = 0; i < SpawnableEnemies.Length; i++)
        {
            totalWeight += SpawnableEnemies[i].weight;
        }

        int weightCutoff = Random.Range(0, totalWeight);
        Enemy result = null;

        for(int i = 0; i < SpawnableEnemies.Length; i++)
        {
            if(weightCutoff < SpawnableEnemies[i].weight)
            {
                result = SpawnableEnemies[i].Prefab;
                break;
            }

            weightCutoff -= SpawnableEnemies[i].weight;
        }

        return result;
    }
}
