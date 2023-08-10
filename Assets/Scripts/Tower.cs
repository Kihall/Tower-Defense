using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootTimerMax;

    private float shootTimer;
    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;
    private Vector3 projectileSpawnPosition;
    private Projectile.ProjectileType projectileType;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Is a enemy!
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (enemy.GetEnemyType() == Enemy.EnemyType.Priest && projectileType == Projectile.ProjectileType.LaserProjectile)
                    {
                        targetEnemy = enemy;
                    }

                    if (Vector3.Distance(EnemyWaveManager.Instance.GetBaseTransform().position, enemy.transform.position) <
                        Vector3.Distance(EnemyWaveManager.Instance.GetBaseTransform().position, targetEnemy.transform.position))
                    {
                        // Closer!
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;

            if (targetEnemy != null)
            {
                Projectile.Create(projectileSpawnPosition, targetEnemy, projectileType);
            }
        }
    }

    public void SetProjectileType(Projectile.ProjectileType projectileType)
    {
        this.projectileType = projectileType;
    }
}
