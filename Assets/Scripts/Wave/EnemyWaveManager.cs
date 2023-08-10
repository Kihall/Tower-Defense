using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveChanged;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
        WavesOver
    }

    [SerializeField] private WaveSpawner[] waveSpawnerArray;
    [SerializeField] private Transform endingPointTransform;
    [SerializeField] private float timeBetweenWavesMax = 30;

    private State state;
    private int waveNumber;
    private int waveNumberTotal;
    private float timeBetweenWaves;
    private GridPosition endGridPosition;

    private List<WaveSpawner> finishedWaveList = new List<WaveSpawner>();
    private GameObject[] enemiesAlive;

    private bool areTimersActivated;
    private bool areWavesOver;

    private float checkTimerMax = 5f;
    private float checkTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (WaveSpawner waveSpawner in waveSpawnerArray)
        {
            waveSpawner.Start();
        }

        waveNumberTotal = waveSpawnerArray[0].GetWaveTotalNumber();

        endGridPosition = LevelGrid.Instance.GetGridPosition(endingPointTransform.position);

        timeBetweenWaves = timeBetweenWavesMax;

        state = State.WaitingToSpawnNextWave;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                timeBetweenWaves -= Time.deltaTime;
                if (!areTimersActivated)
                {
                    foreach (WaveSpawner waveSpawner in waveSpawnerArray)
                    {
                        waveSpawner.ActivateWaveTimer(true);
                    }

                    areTimersActivated = true;
                }

                if (timeBetweenWaves <= 0)
                {
                    foreach (WaveSpawner waveSpawner in waveSpawnerArray)
                    {
                        waveSpawner.StartTheWave();
                        waveSpawner.ActivateWaveTimer(false);
                    }
                    areTimersActivated = false;
                    waveNumber++;

                    OnWaveChanged?.Invoke(this, EventArgs.Empty);
                    state = State.SpawningWave;
                }
                break;
            case State.SpawningWave:
                foreach (WaveSpawner waveSpawner in waveSpawnerArray)
                {
                    waveSpawner.Update();
                    if (waveSpawner.GetIsWaveOver())
                    {
                        if (finishedWaveList.Contains(waveSpawner)) continue;

                        finishedWaveList.Add(waveSpawner);
                    }
                }

                if (finishedWaveList.Count == waveSpawnerArray.Length)
                {
                    finishedWaveList.Clear();
                    state = State.WavesOver;
                }
                break;
            case State.WavesOver:
                if (!areWavesOver)
                {
                    foreach (WaveSpawner waveSpawner in waveSpawnerArray)
                    {
                        if (!waveSpawner.AreWavesOver())
                        {
                            timeBetweenWaves = timeBetweenWavesMax;

                            finishedWaveList.Clear();

                            state = State.WaitingToSpawnNextWave;
                            break;
                        }
                        else
                        {
                            finishedWaveList.Add(waveSpawner);
                        }
                    }
                }

                if (finishedWaveList.Count == waveSpawnerArray.Length)
                {
                    areWavesOver = true;
                    checkTimer = checkTimerMax;
                    enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");

                    finishedWaveList.Clear();
                }

                if (areWavesOver)
                {
                    if (checkTimer > 0)
                    {
                        checkTimer -= Time.deltaTime;
                        if (checkTimer <= 0)
                        {
                            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");

                            checkTimer = checkTimerMax;
                        }
                    }

                    if (enemiesAlive.Length == 0)
                    {
                        Time.timeScale = 0;
                        GameOverUI.Instance.Show();
                    }
                }
                break;
        }
    }

    public void EndWaveWaitingTime()
    {
        timeBetweenWaves = 0;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public int GetWaveNumberTotal()
    {
        return waveNumberTotal;
    }

    public float GetTimeBetweenWaves()
    {
        return timeBetweenWaves;
    }

    public GridPosition GetEndingGridPosition()
    {
        return endGridPosition;
    }

    public Transform GetBaseTransform()
    {
        return endingPointTransform;
    }
}
