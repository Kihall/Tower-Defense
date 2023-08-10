using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Create(Vector3 position, Enemy targetEnemy, ProjectileType projectileType)
    {
        Transform pfProjectile;

        switch (projectileType)
        {
            default:
            case ProjectileType.ArrowProjectile: pfProjectile = Resources.Load<Transform>("pfArrowProjectile"); break;
            case ProjectileType.ExplosiveProjectile: pfProjectile = Resources.Load<Transform>("pfExplosiveProjectile"); break;
            case ProjectileType.LaserProjectile: pfProjectile = Resources.Load<Transform>("pfLaserProjectile"); break;
        }

        Transform projectileTransform = Instantiate(pfProjectile, position, Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.SetTarget(targetEnemy);

        return projectile;
    }

    public enum ProjectileType
    {
        ArrowProjectile,
        ExplosiveProjectile,
        LaserProjectile
    }

    [SerializeField] protected int damageAmount;

    protected Enemy targetEnemy;

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }
}
