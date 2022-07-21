using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressWaves : MonoBehaviour
{
    public Spawner spawner;
    public ProgressBar bar;
    public TMP_Text enemyCount;

    void Start()
    {
        enemyCount.text = "Zone Progress: " + spawner.enemiesSpawned + " / " + (spawner.enemiesPerWave * spawner.nWaves);
        bar.maximum = spawner.enemiesPerWave * spawner.nWaves;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount.text = "Zone Progress: " + spawner.enemiesSpawned + " / " + (spawner.enemiesPerWave * spawner.nWaves);
        bar.current = spawner.enemiesSpawned;
    }
}
