using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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

    public bool isMirroredSpawns;

    // Array of the enemies
    public List<GameObject> enemyList = new List<GameObject>();   // SET THIS IN UNITY

    public List<GameObject> displayWaypoints = new List<GameObject>();

    // Time between waves
    public int tBetWaves = 5;
    // Last spawn time
    private float tLastSpawn;
    // Number of waves done so far
    public int nWaves; //FIX NUMBER 4, changed in the inspector to 1

    private int waveCount; // Current wave number

    // Information on what waves will be spawned
    public Wave[] waves = new Wave[1];   // MANUALLY SET THIS HERE (or in start() ig)
    // Wait I can't set this in here due to how classes work, SET THIS IN start()

    // List of all possible spawning points
    // NOTE: This is 0-indexed
    public static List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> displaySpawns = new List<GameObject>();
    public static int choice;

    public static List<GameObject> waypointList = new List<GameObject>();  // MADE STATIC NOW
    public static List<GameObject> waypointSecondList = new List<GameObject>();  // MADE STATIC NOW
    public GameObject foodPoint, waypointPrefab;
    public float offsetFromGridX, offsetFromGridY, foodOffsetFromGridX, foodOffsetFromGridY;

    public static void destroyAndRemoveSpawn(int i)
    {
        Destroy(spawnPoints[i]);
        spawnPoints.RemoveAt(i);
    }

    public void newPath()
    {
        //Pathing generation
        GameObject spawnPoint = waypointList[0];
        GameObject endPoint = waypointList[waypointList.Count - 1];
        GameObject waypointStart = waypointList[1];
        GameObject waypointEnd = waypointList[waypointList.Count - 2];
        waypointList.Clear();
        
        AStarEnemyPathfinding pathFinder = new AStarEnemyPathfinding(ref waypointStart, ref waypointEnd);
        List<GameObject> pathBody = pathFinder.generatePathing();
        waypointList.Add(spawnPoint);
        pathBody.Reverse();
        waypointList.AddRange(pathBody);
        waypointList.Add(endPoint);
        //Pathing done

        //Pathing generation
        spawnPoint = waypointSecondList[0];
        endPoint = waypointSecondList[waypointSecondList.Count - 1];
        waypointStart = waypointSecondList[1];
        waypointEnd = waypointSecondList[waypointSecondList.Count - 2];
        waypointSecondList.Clear();

        AStarEnemyPathfinding pathFinder2 = new AStarEnemyPathfinding(ref waypointStart, ref waypointEnd);
        List<GameObject> pathBody2 = pathFinder2.generatePathing();
        waypointSecondList.Add(spawnPoint);
        pathBody2.Reverse();
        waypointSecondList.AddRange(pathBody2);
        waypointSecondList.Add(endPoint);
        //Pathing done
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawner activated");

        displaySpawns = spawnPoints;

        BlockageTRT.spawner = this.gameObject;

        waypointList.Clear();
        waypointSecondList.Clear();

        //Pathing generation
        GameObject waypointStart = GridGenerator.validEnemyTiles[(int)((spawnPoints[0].transform.position.x - offsetFromGridX) + (spawnPoints[0].transform.position.y - offsetFromGridY) * 32)].transform.Find("WayPointTemplate(Clone)").gameObject;
        GameObject waypointEnd = GridGenerator.validEnemyTiles[(int)((foodPoint.transform.position.x - foodOffsetFromGridX) + (foodPoint.transform.position.y - foodOffsetFromGridY) * 32)].transform.Find("WayPointTemplate(Clone)").gameObject;

        AStarEnemyPathfinding pathFinder1 = new AStarEnemyPathfinding(ref waypointStart, ref waypointEnd);
        List<GameObject> pathBody1 = pathFinder1.generatePathing();
        waypointList.Add(Instantiate(waypointPrefab, spawnPoints[0].transform.position, Quaternion.identity) as GameObject);
        pathBody1.Reverse();
        waypointList.AddRange(pathBody1);
        waypointList.Add(Instantiate(waypointPrefab, foodPoint.transform.position, Quaternion.identity) as GameObject);
        //Pathing done

        if (isMirroredSpawns)
        {
            offsetFromGridX = 1 + (-offsetFromGridX);
        }

        //Debug.Log("Second Waypoint Start: " + (int)((spawnPoints[1].transform.position.x - offsetFromGridX) + (spawnPoints[1].transform.position.y - offsetFromGridY) * 32));
        //Pathing generation
        GameObject waypointStart2 = GridGenerator.validEnemyTiles[(int)((spawnPoints[1].transform.position.x - offsetFromGridX) + (spawnPoints[1].transform.position.y - offsetFromGridY) * 32)].transform.Find("WayPointTemplate(Clone)").gameObject;
        //waypointEnd = GridGenerator.validEnemyTiles[(int)((foodPoint.transform.position.x - foodOffsetFromGridX) + (foodPoint.transform.position.y - foodOffsetFromGridY) * 32)].transform.Find("WayPointTemplate(Clone)").gameObject;

        AStarEnemyPathfinding pathFinder2 = new AStarEnemyPathfinding(ref waypointStart2, ref waypointEnd);
        List<GameObject> pathBody2 = pathFinder2.generatePathing();
        waypointSecondList.Add(Instantiate(waypointPrefab, spawnPoints[1].transform.position, Quaternion.identity) as GameObject);
        pathBody2.Reverse();
        waypointSecondList.AddRange(pathBody2);
        waypointSecondList.Add(Instantiate(waypointPrefab, foodPoint.transform.position, Quaternion.identity) as GameObject);
        //Pathing done

        if (isMirroredSpawns)
        {
            offsetFromGridX = 1 + (-offsetFromGridX);
        }

        displayWaypoints = waypointSecondList;

        // Set up the waves to be done
        waves[0] = new Wave(0, 2.0f, 10);  // Increased number of enemies from 2 to 10 for testing
        tLastSpawn = Time.time;
        // waveManager = GameObject.Find("WaveManagerHolder").GetComponent<WaveManager>();

        //Set up all possible starting points
        //spawnPoints[0] = new int[] {0,11};
        // Maybe this shud just be a field stored within Waves idk
    }

    // Update is called once per frame
    void Update()
    {
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
                choice = Random.Range(0, 2);
                // Update last spawn time
                tLastSpawn = Time.time;

                GameObject newEnemy;

                if (choice == 0)
                {
                    Debug.Log("First Spawn");
                    // Make a new instance of the specified enemy type for the wave
                    newEnemy = (GameObject)Instantiate(enemyList[currWave.enemyInd],
                    spawnPoints[0].transform.position, Quaternion.identity);
                    //newEnemy.GetComponent<Wispy>().waypoints = waypoints.waypoints; //FIX NUMBER 2 (THIS IS OLD VERSION)
                    //newEnemy.GetComponent<Enemy>().waypoints = new List<GameObject>(waypointList);


                    // Set the parent of the enemy in the hierarchy to be the Spawner
                    newEnemy.transform.SetParent(this.transform);
                }
                else
                {
                    Debug.Log("Second Spawn");
                    // Make a new instance of the specified enemy type for the wave
                    newEnemy = (GameObject)Instantiate(enemyList[currWave.enemyInd],
                    spawnPoints[1].transform.position, Quaternion.identity);
                    //newEnemy.GetComponent<Wispy>().waypoints = waypoints.waypoints; //FIX NUMBER 2 (THIS IS OLD VERSION)
                    //newEnemy.GetComponent<Enemy>().waypoints = new List<GameObject>(waypointSecondList);


                    // Set the parent of the enemy in the hierarchy to be the Spawner
                    newEnemy.transform.SetParent(this.transform);
                }
                // Toss the enemy into `aEnemies` for access
                currWave.aEnemies[currWave.enemySpawned] = newEnemy;
                // ???
                // newEnemy.GetComponent<Wispy>().waypoints = waypoints;
                // Update the number of enemies spawned
                GlobalVariables.enemyList.add(newEnemy.GetComponent<Enemy>());
                currWave.enemySpawned++;

                newEnemy.GetComponent<Enemy>().foodOffsetFromGridX = foodOffsetFromGridX;
                newEnemy.GetComponent<Enemy>().foodOffsetFromGridY = foodOffsetFromGridY;
                newEnemy.GetComponent<Enemy>().spawnOffsetFromGridX = offsetFromGridX;
                newEnemy.GetComponent<Enemy>().spawnOffsetFromGridY = offsetFromGridY;
                newEnemy.GetComponent<Enemy>().foodPoint = foodPoint;
            }


            // If wave's enemies all spawned and all enemies dead
            if (currWave.enemySpawned == currWave.enemyCount &&      // If wave's enemies all spawned
                GameObject.FindGameObjectWithTag("Enemy") == null)      // All enemies dead
            {
                waveCount += 1;   // Next wave
                //waveManager.wave++;          // Next wave
                // Gold?
                // enemiesSpawned = 0;          // Reset enemy spawn count
                tLastSpawn = Time.time;   // Reset last spawn time
            }
        }
        else   // Done with all waves
        {
            // Do stuff to end the game
            Debug.Log("Stage Clear");
            Spawner.waypointList.Clear();
            GlobalVariables.lastClearedScene = GlobalVariables.nextSceneToPlay;
            GlobalVariables.nextSceneToPlay = Random.Range(4, 8); //Consider ensuring no 2 consecutive zones are the same
            Debug.Log("Cleared: " + GlobalVariables.lastClearedScene + " Next: " + GlobalVariables.nextSceneToPlay);
            SceneManager.LoadScene(8);
        }
    }
}
//FIX NUMBER 3: Assigned Fix 1 to the WayPoints GameObject in the hierarchy
public class AStarEnemyPathfinding
{
    public GameObject startPoint, endPoint; //Specifically start and ending waypoints
    private int gridSizeX = 32, gridSizeY = 11;

    public AStarEnemyPathfinding(ref GameObject start, ref GameObject end)
    {
        startPoint = start;
        endPoint = end;
    }

    public class Node
    {
        public int costToStart, costToEnd, totalCost;
        private int gridSizeX = 32, gridSizeY = 11;
        public GameObject associatedWaypoint;
        public Node parent;

        public Node(int costToStart, GameObject currPoint, GameObject endPoint) //x and y are coordinates of the Node on the grid
        {
            this.costToStart = costToStart;
            this.costToEnd = Mathf.Abs(currPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX - endPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX)
                                + Mathf.Abs(currPoint.GetComponent<WaypointInternals>().tileIndex / gridSizeX - endPoint.GetComponent<WaypointInternals>().tileIndex / gridSizeX); //works cuz coords are always >= 0
            this.totalCost = costToStart + costToEnd;
            this.associatedWaypoint = currPoint;
        }

        public void setParent(Node parent)
        {
            this.parent = parent;
        }

        public void updateNode(int costToStart, GameObject currPoint, GameObject endPoint)
        {
            int newCostToEnd = Mathf.Abs(currPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX - endPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX)
                                + Mathf.Abs(currPoint.GetComponent<WaypointInternals>().tileIndex / gridSizeX - endPoint.GetComponent<WaypointInternals>().tileIndex / gridSizeX); //Manhatten distance
            if (costToEnd > newCostToEnd)
            {
                this.costToStart = costToStart;
                this.costToEnd = newCostToEnd; //works cuz coords are always >= 0
                this.totalCost = costToStart + newCostToEnd;
            }
            else
            {
                //The previous node is at least as good as this one
            }
        }
    }


    public List<GameObject> generatePathing() // Returns a list of Waypoints for an enemy to follow. Can be from a spawn point or an enemy.
    {
        //List<GameObject> openPoints = new List<GameObject>(WaypointGenerator.allWaypoints); //All unexplored waypoints
        List<Node> activeNodes = new List<Node>(); //The nodes that are being investigated
        List<Node> closedPoints = new List<Node>(); //Nodes that have already been explored
        Node start = new Node(0, startPoint, endPoint);
        activeNodes.Add(start);
        Node finalNode = null;
        Debug.Log("Begin pathfinding");
        while (activeNodes.Count > 0)
        {
            Node checkNode = activeNodes.OrderBy(x => x.totalCost).First();
            Debug.Log("Pathfinding " + checkNode.associatedWaypoint.GetComponent<WaypointInternals>().adjWaypoints.Count);

            if (checkNode.associatedWaypoint.GetComponent<WaypointInternals>().tileIndex == endPoint.GetComponent<WaypointInternals>().tileIndex)
            {
                Debug.Log("Enemies have found da wei");
                finalNode = checkNode;
                break; //continued outside while loop for clarity
            }
            else
            {
                closedPoints.Add(checkNode);
                activeNodes.Remove(checkNode);
                List<GameObject> adjWaypoints = checkNode.associatedWaypoint.GetComponent<WaypointInternals>().adjWaypoints;
                Debug.Log("Num of adj Waypoints: " + adjWaypoints.Count);
                foreach (var adjWaypoint in adjWaypoints)
                {
                    Debug.Log("checking adjacent waypoints");
                    if (adjWaypoint.transform.parent.GetComponent<EnemyTile>().isBlockage) // If the enemy tile is a blockage, skip that tile.
                    {
                        Debug.Log("SKIP");
                        continue;
                    }
                    Node adjNode = new Node(checkNode.costToStart + 1, adjWaypoint, endPoint);
                    if (closedPoints.Any(x => x.associatedWaypoint.GetComponent<WaypointInternals>().tileIndex == adjWaypoint.GetComponent<WaypointInternals>().tileIndex))
                        continue;
                    if (activeNodes.Any(x => x.associatedWaypoint.GetComponent<WaypointInternals>().tileIndex == adjWaypoint.GetComponent<WaypointInternals>().tileIndex))
                    {
                        Node existingNode = activeNodes.First(x => x.associatedWaypoint.GetComponent<WaypointInternals>().tileIndex == adjWaypoint.GetComponent<WaypointInternals>().tileIndex);
                        if (existingNode.totalCost > adjNode.totalCost)
                        {
                            activeNodes.Remove(existingNode);
                            activeNodes.Add(adjNode);
                            adjNode.setParent(checkNode);
                        }
                    }
                    else
                    {
                        activeNodes.Add(adjNode);
                        adjNode.setParent(checkNode);
                    }
                }
            }
        }
        List<GameObject> waypointPathing = new List<GameObject>();
        while (finalNode != null && finalNode.parent != null)
        {
            waypointPathing.Add(finalNode.associatedWaypoint);
            finalNode = finalNode.parent;
            Debug.Log("finding parent");
        }
        if (finalNode != null)
        {
            waypointPathing.Add(finalNode.associatedWaypoint);
        }

        return waypointPathing;
    }
}