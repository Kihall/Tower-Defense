using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPriest : Enemy
{
    [SerializeField] private float healTimer = 1f;
    [SerializeField] private Transform healVfx;

    private State state;
    private float movingTimer = 7f;
    private float waitForHealTimer = 2f;

    private float movingTimerMax;
    private float waitForHealTimerMax;
    private bool alreadyInstantiated;

    private enum State
    {
        WaitingToHeal,
        Move
    }

    private void Start()
    {
        movingTimerMax = movingTimer;
        waitForHealTimerMax = waitForHealTimer;

        state = State.Move;
    }

    protected override void Update()
    {
        HandleMovement();

        switch (state)
        {
            case State.Move:
                Move();
                animator.SetBool("walking", true);
                movingTimerMax -= Time.deltaTime;
                if (movingTimerMax < 0)
                {
                    movingTimerMax = movingTimer;
                    state = State.WaitingToHeal;
                }
                break;
            case State.WaitingToHeal:
                if (!alreadyInstantiated)
                {
                    StopMoving();
                    animator.SetBool("walking", false);
                    Instantiate(healVfx, transform.position, Quaternion.identity);
                    HealOtherEnemies();
                    alreadyInstantiated = true;
                }

                waitForHealTimerMax -= Time.deltaTime;
                if (waitForHealTimerMax < 0)
                {
                    waitForHealTimerMax = waitForHealTimer;
                    alreadyInstantiated = false;
                    state = State.Move;
                }
                break;
        }
    }

    private void HealOtherEnemies()
    {
        float targetMaxRadius = 5f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                HealthSystem health = enemy.GetComponent<HealthSystem>();
                health.SetHealTimer(healTimer);
            }
        }
    }
}
