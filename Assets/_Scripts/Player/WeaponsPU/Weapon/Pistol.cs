using UnityEngine;

public class Pistol : WeaponBase
{
    public GameObject bulletPrefab;
    public float fireRate = 0.4f;
    private float nextFireTime = 0f;
    public float coneAngle = 5f;

    private void Start()
    {
        maxAmmo = 9999;
        reloadTime = 1.5f;
        damage = 50;
        outAmmo = 35f;  
        currentAmmo = maxAmmo;  
    }

    public override void Shoot()
    {
        Debug.Log("Attempting to shoot..."); 
        if (isReloading || Time.time < nextFireTime || currentAmmo <= 0)
        {
            Debug.Log("Cannot shoot: Reloading, waiting for fire rate, or out of ammo.");
            return;
        }

        nextFireTime = Time.time + fireRate;

        ShootBullet();

        currentAmmo--;
        Debug.Log($"Pistol current ammo: {currentAmmo}");

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo, starting reload...");
            StartReloading();
        }
    }

    private void ShootBullet()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is missing in Pistol.");
            return;
        }

        float halfCone = coneAngle / 2f;
        float randomAngle = Random.Range(-halfCone, halfCone);

        Quaternion rotation = Quaternion.Euler(0, 0, spawnPoint.rotation.eulerAngles.z + randomAngle);

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, rotation);
        if (bullet == null)
        {
            Debug.LogError("Bullet prefab instantiation failed.");
            return;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.up * 15f;
        }
        else
        {
            Debug.LogError("Bullet prefab is missing Rigidbody2D.");
        }

        Player_Bullet bulletScript = bullet.GetComponent<Player_Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(bullet.transform.forward, 4f, this);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing Player_Bullet script.");
        }

        Debug.Log("Bullet fired!");
    }

    protected override void Reload()
    {
        StartReloading();
    }

    public override void StartReloading()
    {
        base.StartReloading();
        if (currentAmmo < maxAmmo && outAmmo > 0)
        {
            AddAmmo(4);
        }
    }
}






