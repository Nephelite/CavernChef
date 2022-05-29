using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]   // Lets you edit values in the Unity editor
public class Wave //: MonoBehaviour
{
    /* Key for enemy idnex
    0 - Wispy
    * /
    public int enemyInd;    // Enemy type index
    public float spawnInterval = 2;   // Time bet individual enemies
    public int maxEnemies = 20;       // Number of enemies for the wave
    public GameObject[] aEnemies;   // Store a reference to all the enemies

    // Constructor
    public Wave(int enemyInd, float spawnInterval, int maxEnemies) {
        this.enemyInd = enemyInd;
        this.spawnInterval = spawnInterval;
        this.maxEnemies = maxEnemies;
        aEnemies = new GameObject[maxEnemies];
    }
}
*/



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



[System.Serializable]
public class Spawner : MonoBehaviour
{
    /* No an array can't hold [int float int] ffs strongly typed
    // waves[i] returns [enemyInd, spawnInt, maxEnemies]
    // [Index of the enemy type, interval between spawned enemies, number of enemies for the round]
    // Edit to change the waves being made
    public int[][] waves = new int[][] {new int[] {0, 2, 20}};
    */
    // Number of enemies spawned for the current wave
    // private int nEnemies = 0;
    // private GameManagerBehavior gameManager;  // TODO?
    // private WaveManager waveManager;   // Holds relevant wave related info

    // Array of the enemies
    public List<GameObject> enemyList = new List<GameObject>();   // SET THIS IN UNITY

    public WayPoints waypoints; //FIX NUMBER 1

    // Time between waves
    public int tBetWaves = 5;
    // Last spawn time
    private float tLastSpawn;
    // Number of waves done so far
    public int nWaves; //FIX NUMBER 4, changed in the inspector to 1

    // Information on what waves will be spawned
    public Wave[] waves = new Wave[1];   // MANUALLY SET THIS HERE (or in start() ig)
    // Wait I can't set this in here due to how classes work, SET THIS IN start()

    // List of all possible spawning points
    // NOTE: This is 0-indexed
    public List<int[]> spawnPoints = new List<int[]>(1);

    // Start is called before the first frame update
    void Start()
    {
        // Set up the waves to be done
        waves[0] = new Wave(0, 2.0f, 20);
        tLastSpawn = Time.time;
        // waveManager = GameObject.Find("WaveManagerHolder").GetComponent<WaveManager>();

        //Set up all possible starting points
        spawnPoints[0] = new int[] {0,11};
        // Maybe this shud just be a field stored within Waves idk
    }

    // Update is called once per frame
    void Update()
    {

        // Current wave number
        int waveCount = 0;

        if (waveCount < nWaves)   // Not yet done with all waves
        {
            // Time since last spawned enemy
            float timeInterval = Time.time - tLastSpawn;
            // Retrieve the current wave
            Wave currWave = waves[waveCount];


            // If (new wave starting OR next enemy in wave) AND (wave is not yet done)
            if ( ((currWave.enemySpawned == 0 && timeInterval > tBetWaves) ||
                timeInterval > currWave.spawnInterval) &&                         //      next enemy in wave ) AND
                currWave.enemySpawned < waves[waveCount].enemyCount )         //    wave isn't done yet
            {
                // Update last spawn time
                tLastSpawn = Time.time;
                // Make a new instance of the specified enemy type for the wave
                GameObject newEnemy = (GameObject) Instantiate(enemyList[currWave.enemyInd], 
                    gameObject.transform.position, Quaternion.identity);
                newEnemy.GetComponent<Wispy>().waypoints = waypoints.waypoints; //FIX NUMBER 2
                // Set the parent of the enemy in the hierarchy to be the Spawner
                newEnemy.transform.SetParent(this.transform);
                // Toss the enemy into `aEnemies` for access
                currWave.aEnemies[currWave.enemySpawned] = newEnemy;
                // ???
                // newEnemy.GetComponent<Wispy>().waypoints = waypoints;
                // Update the number of enemies spawned
                GlobalVariables.enemyList.Add(newEnemy);
                currWave.enemySpawned++;
            }


            // If wave's enemies all spawned and all enemies dead
            if (currWave.enemySpawned == currWave.enemyCount &&      // If wave's enemies all spawned
                GameObject.FindGameObjectWithTag("Enemy") == null)      // All enemies dead
            {
                nWaves += 1;   // Next wave
                //waveManager.wave++;          // Next wave
                // Gold?
                // enemiesSpawned = 0;          // Reset enemy spawn count
                tLastSpawn = Time.time;   // Reset last spawn time
            }

        }
        else   // Done with all waves
        {
            // Do stuff to end the game
        }
    }
}
//FIX NUMBER 3: Assigned Fix 1 to the WayPoints GameObject in the hierarchy
