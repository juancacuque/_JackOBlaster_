using System.Collections.Generic;
using UnityEngine;

public class UnlockableWeapons : MonoBehaviour
{
    public GameObject Saw;
    public GameObject Drone;
    public GameObject areaForce;
    public GameObject Bomb;

    public bool LevelUp = false;
    public float sawLvl = 0;
    public float bombLvl = 0;
    public float droneLvl = 0;
    public float shieldLvl = 0;

    public GameObject[] currentChoices;

    void Start()
    {
        currentChoices = new GameObject[4];  // Ahora hay 4 opciones posibles
    }

    void Update()
    {
        if (LevelUp)
        {
            LevelUpWeapon();
        }

        if (areaForce != null)
        {
            if (shieldLvl >= 1 && !areaForce.activeSelf)
            {
                areaForce.SetActive(true);
            }
            else if (shieldLvl == 0 && areaForce.activeSelf)
            {
                areaForce.SetActive(false);
            }
        }
    }

    public void LevelUpWeapon()
    {
        Time.timeScale = 0;

        // Llenar las opciones de mejora con las 4 opciones disponibles
        currentChoices[0] = Saw;
        currentChoices[1] = Drone;
        currentChoices[2] = areaForce;
        currentChoices[3] = Bomb;

        // Llamar a la función en UIController para mostrar las opciones
        UIController.instance.ShowUpgradeChoices(currentChoices[0], currentChoices[1], currentChoices[2], currentChoices[3]);

        LevelUp = false;
    }

    public void ApplyUpgrade(GameObject upgrade)
    {
        if (upgrade == Saw)
        {
            AddSaw();
        }
        else if (upgrade == Drone)
        {
            AddDrone();
        }
        else if (upgrade == areaForce)
        {
            IncreaseShieldDamage();
        }
        else if (upgrade == Bomb)
        {
            BombUp();
        }

        Time.timeScale = 1;  // Reseteamos el tiempo del juego después de la mejora
    }

    public void AddSaw()
    {
        Debug.Log("Add Saw");
        if (sawLvl < 20)
        {
            sawLvl++;
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController == null) return;

            GameObject newSaw = Instantiate(Saw, playerController.transform.position, Quaternion.identity);
            SawBehaviour sawScript = newSaw.GetComponent<SawBehaviour>();
            sawScript.SetPlayerReference(playerController);
            sawScript.weapon = this;

            float angleOffset = (360f / sawLvl) * (sawLvl - 1);
            sawScript.SetInitialPosition(playerController.transform.position, angleOffset);

            sawScript.speed += 0.1f * sawLvl;
        }
        else
        {
            Debug.Log("Maximum Saw Level reached.");
        }
    }

    public void AddDrone()
    {
        Debug.Log("Drone add");
        if (droneLvl < 10)
        {
            droneLvl++;
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController == null) return;

            GameObject newDron = Instantiate(Drone, playerController.transform.position, Quaternion.identity);
            DronBehaviour dronScript = newDron.GetComponent<DronBehaviour>();
        }
    }

    public void IncreaseShieldDamage()
    {
        shieldLvl++;
        Debug.Log("Shield damage increased");
    }

    public void BombUp()
    {
        if (bombLvl < 8)
        {
            bombLvl++;
            Debug.Log("Bomb cooldown reduced");
        }
    }
}

