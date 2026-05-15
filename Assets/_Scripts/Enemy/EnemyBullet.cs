using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Player_Bullet
{
    PlayerController controller;
    void Start()
    {
        speed = 2f;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (collision.gameObject.CompareTag("Player"))
        {
            controller.TakeDamage(10);
            Destroy(this.gameObject);
        }
        else
        {
            
        }
    }
}
