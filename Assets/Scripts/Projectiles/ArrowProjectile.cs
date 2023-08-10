using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : Projectile
{
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    private void Update()
    {
        HandleMovement();
        DieAfterSeconds();
    }

    private void HandleMovement()
    {
        Vector3 moveDir;

        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float radians = Mathf.Atan2(moveDir.y, moveDir.x);
        float degrees = radians * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, degrees);
    }

    private void DieAfterSeconds()
    {
        timeToDie -= Time.deltaTime;
        if (timeToDie < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
