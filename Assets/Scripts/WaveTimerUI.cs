using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private Button startTheWaveButton;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        startTheWaveButton.onClick.AddListener(() =>
        {
            EnemyWaveManager.Instance.EndWaveWaitingTime();
        });
    }

    private void Update()
    {
        timerText.text = "" + String.Format("{0:0}", EnemyWaveManager.Instance.GetTimeBetweenWaves());
    }
}
