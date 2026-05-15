using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public Transform spawnPoint; // Transform para el punto de disparo
    public float reloadTime;
    public int maxAmmo;
    public float currentAmmo;
    public float outAmmo;
    public bool isReloading = false;

    protected float damage;

    public void Initialize(Transform spawn)
    {
        spawnPoint = spawn;
        currentAmmo = maxAmmo;
    }

    public abstract void Shoot();
    protected abstract void Reload();

    public virtual void StartReloading()
    {
        if (!isReloading && outAmmo > 0 && currentAmmo < maxAmmo)
        {
            isReloading = true;
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        float neededAmmo = maxAmmo - currentAmmo;
        float ammoToReload = Mathf.Min(neededAmmo, outAmmo);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo += ammoToReload;
        outAmmo -= ammoToReload;

        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }

        isReloading = false;
        Debug.Log($"Reload Complete: Current Ammo: {currentAmmo}, Out Ammo: {outAmmo}");
    }

    public void AddAmmo(float ammoAmount)
    {
        outAmmo += ammoAmount;
        Debug.Log($"Added ammo. Current outAmmo: {outAmmo}");
    }

    public virtual float GetDamage()
    {
        return damage;
    }
}

