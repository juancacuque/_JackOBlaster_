using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy1
{
    private bool isShooting = false;
    public float bulletSpeed = 1.5f;
    public GameObject bulletPref;
    public Transform spawnBullet;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target == null)
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'. Asegúrate de que el jugador tenga ese tag.");
        }
    }

    void Update()
    {
        Behaviour();
    }

    public override void Behaviour()
    {
        if (Vector3.Distance(transform.position, target.position) > 5f)
        {
            lookChasePlayer();
        }
        else
        {
            RotateToPlayer();
        }
    }

    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPref, spawnBullet.position, spawnBullet.rotation);
        Vector2 shootDirection = transform.up;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;

        Destroy(bullet, 4f);
    }

    private void RotateToPlayer()
    {
        var lookPlayer = target.position - transform.position;
        lookPlayer.z = 0; 

        float angle = Mathf.Atan2(lookPlayer.y, lookPlayer.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Si no está disparando, empezar el ciclo de disparo
        if (!isShooting)
        {
            Debug.Log("starting PEW");
            StartCoroutine(shootRep());
        }
    }

    IEnumerator shootRep()
    {
        isShooting = true;
        shoot();
        Debug.Log("PEW");
        yield return new WaitForSeconds(3f);
        isShooting = false;
    }
}

