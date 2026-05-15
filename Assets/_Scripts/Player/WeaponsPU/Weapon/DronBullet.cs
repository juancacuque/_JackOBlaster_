using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronBullet : MonoBehaviour
{
    public float speed; 
    private float damage = 30; 
    private Vector2 direction;

    void Start()
    {
        direction = direction.normalized;
    }

    void Update()
    {
    }

    public float GetDamage()
    {
        return damage; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy1"))
        {
            Enemy1 enemy = collision.GetComponent<Enemy1>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"La bala hizo {damage} puntos de daño al enemigo.");
                Destroy(this.gameObject);
            }
        }
    }
}

