using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaForce : MonoBehaviour
{
    [SerializeField] private float baseDamage = 15f;
    [SerializeField] private float maxDamage = 40f;
    [SerializeField] private float damageInterval = 1f;

    private UnlockableWeapons weapon;
    private PlayerController playerController;
    private float currentDamage;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            weapon = playerController.GetComponent<UnlockableWeapons>();
            if (weapon == null)
            {
                Debug.LogError("UnlockableWeapons no encontrado en el PlayerController.");
            }
        }
        else
        {
            Debug.LogError("PlayerController no encontrado.");
        }
    }

    void Update()
    {
        if (weapon == null || weapon.shieldLvl < 1)
        {
            currentDamage = 0;
            return;
        }

        float level = weapon.shieldLvl;
        currentDamage = Mathf.Lerp(baseDamage, maxDamage, (level - 1) / 4f);
        if (weapon == null || weapon.shieldLvl < 1)
        {
            currentDamage = 0;
            return;
        }

        if (playerController != null)
        {
            Vector3 playerPosition2D = playerController.transform.position;

            transform.position = new Vector3(playerPosition2D.x, playerPosition2D.y, transform.position.z);
        }
    }

    public float GetDamage()
    {
        return currentDamage;
    }

    public float GetDamageInterval()
    {
        return damageInterval;
    }
}



