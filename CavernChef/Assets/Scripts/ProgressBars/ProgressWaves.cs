using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressWaves : MonoBehaviour
{
    public Spawner spawner;
    public ProgressBar bar;

    void Start()
    {
        bar.maximum = spawner.enemiesPerWave * spawner.nWaves;
    }

    // Update is called once per frame
    void Update()
    {
        bar.current = spawner.enemiesSpawned;
    }
}
