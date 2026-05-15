using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    public float ammoAmount = 30f;  

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el arma activa
            WeaponBase currentWeapon = FindObjectOfType<CHangeWeapon>().GetCurrentWeapon();

            if (currentWeapon != null)
            {
                currentWeapon.AddAmmo(ammoAmount);
                Destroy(gameObject);  
            }
        }
    }
}


