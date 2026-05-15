using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float health;
    public int maxHealth = 100;

    private CHangeWeapon weaponManager;

    private bool weaponSelected = false;

    public Slider healthSlider;
    public static PlayerController instance;

    public float pickUpRange = 1.5f;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        health = maxHealth;
        weaponManager = FindObjectOfType<CHangeWeapon>();

        if (weaponManager == null)
        {
            Debug.LogError("WeaponManager not found!");
        }

        ShowWeaponSelection();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            SoundManager.instance.PlaySound3D("Shoot", transform.position);
            Debug.Log("Left mouse button pressed.");
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ReloadWeapon();
        }
    }

    private void Shoot()
    {
        WeaponBase activeWeapon = weaponManager.GetCurrentWeapon();

        if (activeWeapon == null)
        {
            Debug.LogError("No weapon is currently active.");
            return;
        }

        Debug.Log($"Shooting with weapon: {activeWeapon.name}");
        activeWeapon.Shoot();
    }

    private void ShowWeaponSelection()
    {
        Debug.Log("Select your weapon: Press 1 for pistola, 2 for escopeta, 3 for cuchilla.");
    }

    public void SelectWeapon(int weaponNumber)
    {
        if (weaponManager != null)
        {
            weaponManager.SelectWeapon(weaponNumber);
            weaponSelected = true;
            Debug.Log("Weapon selected and ready to use.");
        }
        else
        {
            Debug.LogError("WeaponManager not available.");
        }
    }

    private void ReloadWeapon()
    {
        WeaponBase activeWeapon = weaponManager.GetCurrentWeapon();

        if (activeWeapon != null)
        {
            activeWeapon.StartReloading();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"Current health: {health}");
        healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log($"Healed: Current health: {health}");
        healthSlider.value = health;
    }

    private void Die()
    {
        Debug.Log("Game Over.");
        Destroy(gameObject);
        UIController.instance.losingPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}

