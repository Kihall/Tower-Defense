using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    private void Start()
    {
        EnemyWaveManager.Instance.OnWaveChanged += EnemyWaveManager_OnWaveChanged;

        UpdateText();
    }

    private void UpdateText()
    {
        waveText.text = "Wave " + EnemyWaveManager.Instance.GetWaveNumber() + "/" + EnemyWaveManager.Instance.GetWaveNumberTotal();
    }

    private void EnemyWaveManager_OnWaveChanged(object sender, EventArgs e)
    {
        UpdateText();
    }
}
