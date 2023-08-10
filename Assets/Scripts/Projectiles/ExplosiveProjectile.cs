using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private Transform explosionVfxPrefab;

    private void Start()
    {
        StartCoroutine(Curve(transform.position, targetEnemy));
    }

    private IEnumerator Curve(Vector3 startPosition, Enemy targetEnemy)
    {
        float timePassed = 0f;

        Vector2 endPosition = targetEnemy.transform.position;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;

            float linearT = timePassed / duration;
            float heightT = arcYAnimationCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            float damageRadius = 10f;

            if (timePassed > duration)
            {
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(endPosition, damageRadius);

                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<HealthSystem>(out HealthSystem targetEnemyHealth))
                    {
                        targetEnemyHealth.Damage(damageAmount);
                    }
                }

                Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }

            yield return null;
        }
    }
}
