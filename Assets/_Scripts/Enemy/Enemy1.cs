using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    public int rotationSpeed = 8;
    PlayerController controller;

    public float health;
    public int maxHealth;
    public float damage;

    private bool isRecoiling = false;
    private float recoilDuration = 0.5f;
    private float recoilTimer = 0f;
    private Vector3 recoilDirection;
    private bool isInZone = false;
    private float damageInterval;
    private float nextDamageTime = 0f;
    private float currentDamage = 0f;
    public int expToGive;

    public Animator animator;

    void Start()
    {
        health = 100;
        target = PlayerController.instance.transform;
    }

    void Update()
    {
        Behaviour();
        if (isInZone && Time.time >= nextDamageTime)
        {
            TakeDamage(currentDamage);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    public void lookChasePlayer()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += directionToPlayer * speed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.TakeDamage(20);
            Vector3 directionToPlayer = (transform.position - other.transform.position).normalized;
            recoilDirection = directionToPlayer;

            isRecoiling = true;
            recoilTimer = recoilDuration;
        }

        if (other.CompareTag("Zone"))
        {
            AreaForce areaForce = other.GetComponent<AreaForce>();
            if (areaForce != null)
            {
                currentDamage = areaForce.GetDamage();
                damageInterval = areaForce.GetDamageInterval();
                isInZone = true;  
            }
        }
    }

    public virtual void Behaviour()
    {
        if (isRecoiling)
        {
            transform.position += recoilDirection * speed * Time.deltaTime;
            recoilTimer -= Time.deltaTime;
            if (recoilTimer <= 0)
            {
                isRecoiling = false;
            }
        }
        else
        {
            lookChasePlayer();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took damage: {damage}. Current health: {health}");
        Die();
    }

    public void Die()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            ExperienceLevelController.Instance.SpawnExp(transform.position, expToGive);
            SoundManager.instance.PlaySound3D("Splash", transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = false;
        }
    }
}

