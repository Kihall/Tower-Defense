using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDied;
    public event EventHandler OnHealed;
    public event EventHandler OnDamaged;

    [SerializeField] private float healthAmountMax;

    private float healthAmount;
    private float healTimer;
    private float healRegenAmount = 1f;
    private float fireTimer;
    private float fireDamage = 2f;
    private bool isOnfire;
    private bool isDead;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    private void Update()
    {
        OnFire();

        if (isOnfire) return;
        Heal();
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
        {
            if (isDead) return;
            OnDied?.Invoke(this, EventArgs.Empty);
            isDead = true;
        }
    }

    private bool IsDead()
    {
        return healthAmount == 0;
    }

    private void OnFire()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            healthAmount -= fireDamage * Time.deltaTime;
            OnDamaged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            isOnfire = false;
        }
    }

    private void Heal()
    {
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;
            healthAmount += healRegenAmount * Time.deltaTime;
            OnHealed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetHealTimer(float healTimer)
    {
        this.healTimer = healTimer;
    }

    public void SetFireTimer(float fireTimer)
    {
        this.fireTimer = fireTimer;
        isOnfire = true;
    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }

    public float GetHealthAmount()
    {
        return healthAmount;
    }
}
