using UnityEngine;

public class Knife : WeaponBase
{
    public GameObject wavePrefab;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private void Start()
    {
        damage = 20;
    }

    public override void Shoot()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        GameObject wave = Instantiate(wavePrefab, spawnPoint.position, spawnPoint.rotation);
        wave.GetComponent<Rigidbody2D>().velocity = spawnPoint.up * 50f;
        wave.GetComponent<Player_Bullet>().Initialize(wave.transform.forward, 6f, this);
    }

    protected override void Reload()
    {
    }
}







