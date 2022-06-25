using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/* Info of a wave */
public class Wave {
    // fsr C# says this is a compile error so tossing this back over to the Spawner class
    // public static nWaves = 0;   // Number of waves done so far
    public int enemyInd;   // Enemy type index
    public float spawnInterval;   // Time interval between individual enemies of a wave
    public int enemyCount;   // Number of enemies for the wave
    public int enemySpawned;   // Number of enemies spawned for this current round
    public GameObject[] aEnemies;   // For storing a reference to all the enemies

    public Wave(int enemyInd, float spawnInterval, int enemyCount) {
        this.enemyInd = enemyInd;
        this.spawnInterval = spawnInterval;
        this.enemyCount = enemyCount;
        this.enemySpawned = 0;
        this.aEnemies = new GameObject[enemyCount];
    }
}