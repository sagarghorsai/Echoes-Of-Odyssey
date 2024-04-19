using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float count;
    [SerializeField] private GameObject spawnPoint;
    public Wave[] waves;


    public int currWave = 0;
    private bool readyToCountDown;
    private void Start()
    {
        readyToCountDown = true;
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemy.Length;
        }
    }
    private void Update()
    {
        if (currWave >= waves.Length)
        {
            Destroy(gameObject);            
            return;
        }

        if (readyToCountDown == true)
        {
        count -= Time.deltaTime;

        }


        if (count <= 0)
        {
            readyToCountDown = false;
            count = waves[currWave].timeToNextWave;
            StartCoroutine(SpawnWave());

        }
        if (waves[currWave].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currWave++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currWave < waves.Length)
        {
            for (int i = 0; i < waves[currWave].enemy.Length; i++)
            {
                Enemy enemy = Instantiate(waves[currWave].enemy[i], spawnPoint.transform);
                enemy.transform.SetParent(spawnPoint.transform);
                yield return new WaitForSeconds(waves[currWave].timeToNextEnemy);
            }
        }

    }

}

[System.Serializable]
public class Wave
{
    public Enemy[] enemy;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}