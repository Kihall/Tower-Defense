using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Represents a single Wave Spawner
 * */
[System.Serializable]
public class WaveSpawner
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float timerMax;
    [SerializeField] private WaveTimerUI waveTimer;
    [SerializeField] private List<Wave> waveList;

    private float timer;
    private bool isWaveOver;

    public void Start()
    {
        PrepareEnemyList();

        timer = timerMax;
    }

    public void Update()
    {
        EnemySpawningTimer();
        WhenWaveIsOver();
    }

    private void PrepareEnemyList()
    {
        foreach (Wave wave in waveList)
        {
            wave.AddEnemiesToList();
        }
    }

    private void EnemySpawningTimer()
    {
        if (timer > 0 && !isWaveOver)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                waveList[0].SpawnAnEnemy(spawnPosition.position);

                timer = timerMax;
            }
        }
    }

    private void WhenWaveIsOver()
    {
        if (waveList.Count == 0) return;

        if (waveList[0].IsWaveOver() && !isWaveOver)
        {
            waveList.RemoveAt(0);
            isWaveOver = true;
        }
    }

    public void ActivateWaveTimer(bool activate)
    {
        waveTimer.gameObject.SetActive(activate);

        if (waveList[0].GetEnemyList().Count <= 0)
        {
            waveTimer.gameObject.SetActive(false);
        }
    }

    public void StartTheWave()
    {
        isWaveOver = false;
    }

    public bool GetIsWaveOver()
    {
        return isWaveOver;
    }

    public bool AreWavesOver()
    {
        if (waveList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWaveTotalNumber()
    {
        return waveList.Count;
    }
}
