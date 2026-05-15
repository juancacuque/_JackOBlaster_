using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float currentTime;
    private bool active = true;

    void Update()
    {
        if(!active)
            return;

        currentTime -= Time.deltaTime;

        UpdateTimerUI();

        if(currentTime <= 0)
        {
            StopTimer();
            UIController.instance.winningPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StopTimer()
    {
        active = false;
        currentTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (currentTime > 0f && currentTime < 6)
        {
            timerText.color = Color.yellow;
        }
        else if(currentTime < 1)
        {
            timerText.color = Color.red;
        }

        TimeSpan t = TimeSpan.FromSeconds(currentTime);
        timerText.text = t.ToString(@"mm\:ss");
    }
}
