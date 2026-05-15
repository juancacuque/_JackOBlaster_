using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int currentExperience;

    public ExpPickUp pickup;

    public List<int> expLevels;
    public int currentLevel = 1, levelCount = 100;

    void Start()
    {
        while(expLevels.Count < levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    void Update()
    {
        
    }

    public void GetExp(int amountToGet)
    {
        currentExperience += amountToGet;
        Debug.Log($"Experience: {currentExperience} / {expLevels[currentLevel]}");
        if(currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIController.instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel);
    }

    public void SpawnExp(Vector3 position, int expValue)
    {
        Instantiate(pickup, position, Quaternion.identity).expValue = expValue;
    }

    void LevelUp()
    {
        SoundManager.instance.PlaySound3D("LevelUp", transform.position);
        Debug.Log("Lvl Up");
        currentExperience -= expLevels[currentLevel];

        currentLevel++;

        if(currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        UIController.instance.levelUpPanel.SetActive(true);
        Time.timeScale = 0f;

    }
}
