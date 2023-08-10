using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : Projectile
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform laserHitVfxPrefab;
    [SerializeField] private Transform fireEffectPrefab;
    [SerializeField] private float fireTimer = 2f;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (targetEnemy != null)
        {
            Vector3 moveDir = (targetEnemy.transform.position - transform.position).normalized;

            float distanceBeforeMoving = Vector3.Distance(transform.position, targetEnemy.transform.position);

            float moveSpeed = 200f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(transform.position, targetEnemy.transform.position);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = targetEnemy.transform.position;

                trailRenderer.transform.parent = null;

                HealthSystem enemyHealthSystem = targetEnemy.GetComponent<HealthSystem>();

                enemyHealthSystem.SetFireTimer(fireTimer);
                enemyHealthSystem.Damage(damageAmount);

                Instantiate(laserHitVfxPrefab, enemyHealthSystem.transform.position, Quaternion.identity);
                Instantiate(fireEffectPrefab, enemyHealthSystem.transform);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
