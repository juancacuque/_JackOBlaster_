using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlPUUp : MonoBehaviour
{
    UnlockableWeapons uW;
    private void Start()
    {
        uW = FindObjectOfType<UnlockableWeapons>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uW.LevelUp = true;
            Destroy(gameObject);
        }
    }


}
