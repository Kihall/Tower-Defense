using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseHealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI baseHealthNumberText;

    private HealthSystem baseHealth;

    private void Awake()
    {
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<HealthSystem>();
    }

    private void Start()
    {
        baseHealth.OnDamaged += BaseHealth_OnDamaged;
        baseHealth.OnDied += BaseHealth_OnDied;

        UpdateBaseHealthText();
    }

    private void UpdateBaseHealthText()
    {
        baseHealthNumberText.text = " " + baseHealth.GetHealthAmount();
    }

    private void BaseHealth_OnDamaged(object sender, EventArgs e)
    {
        UpdateBaseHealthText();
    }

    private void BaseHealth_OnDied(object sender, EventArgs e)
    {
        UpdateBaseHealthText();
        Time.timeScale = 0;
        GameOverUI.Instance.Show();
    }
}
