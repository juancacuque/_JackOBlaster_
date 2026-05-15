using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronBehaviour : SawBehaviour
{
    private Vector3 targetPosition;
    private float correctionRadius = 2.5f;
    private float currentSpeed;
    private Coroutine randomPointCoroutine;
    private bool isMovingToRandomPoint = false;
    private float dronRadius = 1.5f;

    public GameObject bulletPref;
    public Transform spawnPoint;
    public float bulletSpeed = 10;
    public float shootingRange = 3f; 
    private float fireRate = 1f; 
    private float lastFireTime = 0f;

    private Transform closestEnemy;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        radius = 2f;
        speed = 1.5f;
        currentSpeed = speed;
        StartRandomPointRoutine();
    }

    protected override void ExecuteBehaviour()
    {
       

        float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);

        if (distanceToPlayer > radius)
        {
            if (isMovingToRandomPoint)
            {
                if (distanceToPlayer > correctionRadius)
                {
                    currentSpeed = speed * 3;
                }
                else
                {
                    currentSpeed = speed * 2;
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
                {
                    isMovingToRandomPoint = false;
                }
            }
            else
            {
                if (distanceToPlayer > correctionRadius)
                {
                    currentSpeed = speed * 3;
                }
                else
                {
                    currentSpeed = speed * 2;
                }
                if (distanceToPlayer < dronRadius)
                {
                    NewRandomPoint();
                }
                transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                NewRandomPoint();
            }
        }

        FindClosestEnemy();
        if (closestEnemy != null)
        {
            RotateTowardsEnemy();
            ShootAtEnemyIfInRange();
        }
    }

    private void StartRandomPointRoutine()
    {
        if (randomPointCoroutine == null)
        {
            randomPointCoroutine = StartCoroutine(NewRandomPointRoutine());
        }
    }

    private IEnumerator NewRandomPointRoutine()
    {
        while (true)
        {
            NewRandomPoint();
            yield return new WaitForSeconds(1f);
        }
    }

    private void NewRandomPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * dronRadius;
        targetPosition = playerController.transform.position + new Vector3(randomPoint.x, randomPoint.y, -1);
        isMovingToRandomPoint = true;
    }

    private void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy1"))
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
    }

    private void RotateTowardsEnemy()
    {
        if (closestEnemy == null) return;

        Vector3 direction = (closestEnemy.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ShootAtEnemyIfInRange()
    {
        if (closestEnemy == null) return;

        float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.position);

        if (distanceToEnemy <= shootingRange && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time;

            shoot();
        }
    }
    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPref, spawnPoint.position, spawnPoint.rotation);
        Vector2 shootDirection = transform.up;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;

        Destroy(bullet, 4f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.5f);
    }
}
