using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplosiveBehaviour : MonoBehaviour
{
    UnlockableWeapons uW;
    private Vector3 c4Position;
    private bool Active=false;
    public GameObject c4;
    private float liveTime = 3;
    private float spawnCooldown =2;
    private float spawnTimer = 0f;
    private float radius;
    void Start()
    {
        radius = 2;
        uW = FindObjectOfType<UnlockableWeapons>();
    }

    void Update()
    {
        if (uW.bombLvl == 1)
        {
            Active = true;
        }
        if (uW.bombLvl == 2) { spawnCooldown = 1.5f; Debug.Log($"{spawnCooldown}"); }
        if (uW.bombLvl == 3) { spawnCooldown = 1f; Debug.Log($"{spawnCooldown}"); }
        if (Active == true)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnCooldown)
            {
                SpawnC4();
                spawnTimer = 0f;
            }  
        }
    }

    private void SpawnC4()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * radius;
        c4Position = transform.position + new Vector3(randomPoint.x, randomPoint.y, -1);

        GameObject newC4 = Instantiate(c4, c4Position, Quaternion.identity);
        Destroy(newC4, liveTime);
    }
}
