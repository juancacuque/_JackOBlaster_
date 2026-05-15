using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : MonoBehaviour
{
    public float explosionRadius = 3f;
    public float explosionDamage = 50f; 


    public void Detonate()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy1"))
            {
                Enemy1 enemy = collider.GetComponent<Enemy1>();
                if (enemy != null)
                {
                    enemy.TakeDamage(explosionDamage);
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Enemy1"))
        {
            Detonate();
        }
    }
}

