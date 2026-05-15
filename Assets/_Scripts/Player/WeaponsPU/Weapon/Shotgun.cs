using UnityEngine;

public class Shotgun : WeaponBase
{
    public GameObject bulletPrefab;
    public float fireRate = 0.7f;
    private float nextFireTime = 0f;
    public float coneAngle = 45f;

    private void Start()
    {
        maxAmmo = 999999;
        reloadTime = 2f;
        damage = 25;
        outAmmo = 20f; 
        currentAmmo = maxAmmo;
    }

    public override void Shoot()
    {
        if (!isReloading)
        {
            if (Time.time < nextFireTime || currentAmmo < 3) return;

            nextFireTime = Time.time + fireRate;

            float halfCone = coneAngle / 2f;

            for (int i = 0; i < 3; i++)
            {
                float randomAngle = Random.Range(-halfCone, halfCone);

                Quaternion rotation = Quaternion.Euler(0, 0, spawnPoint.rotation.eulerAngles.z + randomAngle);

                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 9f;
                bullet.GetComponent<Player_Bullet>().Initialize(bullet.transform.forward, 4f, this);
            }

            currentAmmo -= 3;
            Debug.Log($"Shotgun current ammo: {currentAmmo}");

            if (currentAmmo <= 0)
            {
                Debug.Log("Out of ammo, starting reload...");
                StartReloading(); 
            }
        }
    }

    public override void StartReloading()
    {
        base.StartReloading(); 

        if (currentAmmo < maxAmmo && outAmmo > 0)
        {
            AddAmmo(4); 
        }
    }

    protected override void Reload()
    {
        StartReloading();
    }
}

