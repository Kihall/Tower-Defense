using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position, EnemyType enemyType)
    {
        Transform pfEnemy;

        switch (enemyType)
        {
            default:
            case EnemyType.Thief: pfEnemy = Resources.Load<Transform>("pfEnemyThief"); break;
            case EnemyType.Priest: pfEnemy = Resources.Load<Transform>("pfEnemyPriest"); break;
            case EnemyType.Knight: pfEnemy = Resources.Load<Transform>("pfEnemyKnight"); break;
        }

        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.SetType(enemyType);
        return enemy;
    }

    public enum EnemyType
    {
        Thief,
        Priest,
        Knight,
    }

    [SerializeField] private int goldValue = 10;
    [SerializeField] private float moveSpeedMax = 3f;
    [SerializeField] protected Animator animator;

    private List<Vector3> positionList;
    private GridPosition gridPosition;
    private int currentPositionIndex;
    private Vector3 moveDirection;
    private HealthSystem healthSystem;
    private EnemyType enemyType;

    private float moveSpeed;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;

        moveSpeed = moveSpeedMax;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        MoveToGridPosition(EnemyWaveManager.Instance.GetEndingGridPosition());
    }

    protected virtual void Update()
    {
        HandleMovement();
        FlipSprite();
    }

    protected void HandleMovement()
    {
        Vector3 targetPosition = positionList[currentPositionIndex];
        moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                StopMoving();
            }
        }
    }

    private void FlipSprite()
    {
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void MoveToGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(this.gridPosition, gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        StopMoving();

        CurrencyManager.Instance.AddGold(goldValue);
        CurrencyManager.Instance.UpdateGoldText();

        animator.SetTrigger("isDead");

        float timeForDeathAnimation = 0.75f;
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length + timeForDeathAnimation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            other.GetComponent<HealthSystem>().Damage(1);
            Destroy(gameObject);
        }
    }

    private void SetType(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    protected void StopMoving()
    {
        moveSpeed = 0f;
    }

    protected void Move()
    {
        moveSpeed = moveSpeedMax;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}
