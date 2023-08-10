using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Represents a single Wave
 * */
[System.Serializable]
public class Wave
{
    [SerializeField] private EnemyTypeToSpawn[] enemyTypeToSpawnArray;

    private List<Enemy.EnemyType> enemyList = new List<Enemy.EnemyType>();

    public void AddEnemiesToList()
    {
        foreach (EnemyTypeToSpawn enemyTypeToSpawn in enemyTypeToSpawnArray)
        {
            for (int i = 0; i < enemyTypeToSpawn.enemyCount; i++)
            {
                enemyList.Add(enemyTypeToSpawn.enemyType);
            }
        }
    }

    public void SpawnAnEnemy(Vector3 position)
    {
        if (IsWaveOver())
        {
            return;
        }
        Enemy.Create(position, enemyList[0]);
        enemyList.RemoveAt(0);
    }

    public bool IsWaveOver()
    {
        if (enemyList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Enemy.EnemyType> GetEnemyList()
    {
        return enemyList;
    }
}

/*
 * Represents single Enemy Type To Spawn
 * */
[System.Serializable]
public class EnemyTypeToSpawn
{
    public Enemy.EnemyType enemyType;
    public int enemyCount;
}