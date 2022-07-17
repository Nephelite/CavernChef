using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// So that I can use Math.Floor()
using System;

/* Lab style summary
class Enemy
    Fields
        public EnemyStatus status
        public GameObject waypointPrefab, foodPoint

        public float base_centi_speed
        public float base_hp
        public float base_dmg

        public List<GameObject> waypoints;
        public GameObject nextTileToVisit
        public float distToFood
        public float wayptPathLen

        public float foodOffsetFromGridX, foodOffsetFromGridY, spawnOffsetFromGridX, spawnOffsetFromGridY

        private bool stalled
        private float heldSpeed
    Methods
        public virtual void setup()
            Initializes waypoints, distToFood, wayptPathLen
        
        void stall()
            potat tower stall stuff
        public void checkForStall(GameObject currentTile, GameObject nextTile)
            potat tower stall stuff
        
        public bool isInFrontOf(Enemy enemy0)
            a.isInFrontOf(b) = a.distToFood < b.distToFood
        public void movementUpdate()
            Updates distToFood, this.transform.position, and relevant WaypointInternals
        public void statusUpdate()
            this.status.updateStatus()
        
        public virtual void findNewPath()
            A-star alg to find new path if og path is blocked
        public class AStarEnemyPathfinding
            copy pasta to run A-star (I think)
*/

public abstract class Enemy : MonoBehaviour
{
    public int enemyID;
    public EnemyStatus status;
    public GameObject waypointPrefab, foodPoint;

    // For setting in unity; will be put into status
    public float base_centi_speed;   // units is "centiunits per frame"
    public float base_hp;
    public float base_dmg;   // Dmg dealt to food/stall TRTs

    // Waypoints to path the enemy (from Kevin's A Star pathfinding)
    public List<GameObject> waypoints;

    public GameObject nextTileToVisit;

    // Distance from the end; Also used for determining the priority
    public float distToFood;

    // Total distance along the currently set `waypoints`
    // Used to aid in handling movement
    public float wayptPathLen;

    public float foodOffsetFromGridX, foodOffsetFromGridY, spawnOffsetFromGridX, spawnOffsetFromGridY; //might not need spawn coords

    public bool stalled;
    private float heldSpeed, lastTime;
    public GameObject stallWaypointPrev, stallWaypointNext;


    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void setup()
    {
        RunManager.seenEnemies[enemyID] = true;

        // Initialize the waypoints
        if (Spawner.choice == 0)
        {
            waypoints = new List<GameObject>(Spawner.waypointList);
        }
        else
        {
            waypoints = new List<GameObject>(Spawner.waypointSecondList);
        }
        
        // Initialize distToFood
        if (waypoints.Count == 2) {   // Corner case of there being no waypoints except for the start and end
            Vector2 displacementVectorInThisCase = waypoints[1].transform.position- waypoints[0].transform.position;
            distToFood = displacementVectorInThisCase.magnitude;
        
        } else {   // There's at least 1 grid waypoint
            int n = waypoints.Count;       // Number of waypoints in total
            // *grid spacing is assumed to be 1, doubt this will change
            // If grid spacing is changed, siply do (n-3)*units_per_grid
            float distAlongGridWaypoints = n - 3;   // Distance traveled between grid waypoints

            // Get positions of the first 2 and last 2 waypoints (possible overlap in middle)
            Vector2 spawnPoint = waypoints[0].transform.position;
            Vector2 firstGridWaypoint = waypoints[1].transform.position;
            Vector2 lastGridWaypoint = waypoints[n-2].transform.position;
            Vector2 endPoint = waypoints[n-1].transform.position;

            // Get the first and last segment to travel
            Vector2 firstSegment = firstGridWaypoint - spawnPoint;
            Vector2 lastSegment = endPoint - lastGridWaypoint;

            // Get the magnitude of these two vectors
            float firstSegmentDist = firstSegment.magnitude;
            float lastSegmentDist = lastSegment.magnitude;

            // Add them up to get total dist to food
            distToFood = firstSegmentDist + distAlongGridWaypoints + lastSegmentDist;
        }
        
        // Initialize wayptPathLen
        wayptPathLen = distToFood;
    }

    public void checkForStall(GameObject currentTile, GameObject nextTile)
    {
        if (stalled)
        {
            if (nextTile.transform.parent != null && nextTile.transform.parent.Find("StallTRT(Clone)") != null && Time.time >= lastTime + 1)
            {
                nextTile.transform.parent.Find("StallTRT(Clone)").gameObject.GetComponent<StallTRT>().decrementHP(base_dmg);
                lastTime = Time.time;
            }
            else if (currentTile.transform.parent != null && currentTile.transform.parent.Find("StallTRT(Clone)") != null && Time.time >= lastTime + 1)
            {
                currentTile.transform.parent.Find("StallTRT(Clone)").gameObject.GetComponent<StallTRT>().decrementHP(base_dmg);
                lastTime = Time.time;
            }
            else if (nextTile.transform.parent != null && nextTile.transform.parent.Find("StallTRT(Clone)") == null
                && currentTile.transform.parent != null && currentTile.transform.parent.Find("StallTRT(Clone)") == null)
            {
                status.restoreSpeed(base_centi_speed);
                stalled = false;
            }
            else if (nextTile.transform.parent != null && nextTile.transform.parent.Find("StallTRT(Clone)") == null
                && currentTile.transform.parent == null)
            {
                status.restoreSpeed(base_centi_speed);
                stalled = false;
            }
        }
        else
        {
            if (nextTile.transform.parent != null && nextTile.transform.parent.Find("StallTRT(Clone)") != null)
            {
                //nextTile.transform.parent.Find("StallTRT(Clone)").gameObject.GetComponent<StallTRT>().decrementHP(base_dmg);
                Debug.Log("Stalling");
                heldSpeed = status.stop();
                stalled = true;
            }

            if (currentTile.transform.parent != null && currentTile.transform.parent.Find("StallTRT(Clone)") != null)
            {
                //currentTile.transform.parent.Find("StallTRT(Clone)").gameObject.GetComponent<StallTRT>().decrementHP(base_dmg);
                Debug.Log("Stalling");
                heldSpeed = status.stop();
                stalled = true;
            }
        }
    }

    // For relative comparison of priorities
    //     enemy1.goFirsterThan(enemy2)
    // returns True iff enemy1 is strictly closer than enemy2
    public bool isInFrontOf(Enemy enemy0)
    {
        return this.distToFood < enemy0.distToFood;
    }

    // Updates the position of the enemy based on waypoints and distToFood
    public void movementUpdate()
    {
        // Get the speed given the current status
        float curr_speed = status.currSpeed() * SpeedChanger.speedMultiplier; //speedMultiplier is the global time scale multiplier

        // Update dist to food
        distToFood -= curr_speed;

        // Get dist from start of the waypoints
        float distFromStart = wayptPathLen - distToFood;

        // Get dist along first and last segments
        Vector2 vector_i_0 = waypoints[0].transform.position;   // Start of first segment
        Vector2 vector_i_1 = waypoints[1].transform.position;   // End of first segment
        Vector2 vector_f_0 = waypoints[waypoints.Count-2].transform.position;   // Start of last segment
        Vector2 vector_f_1 = waypoints[waypoints.Count-1].transform.position;   // End of last segment

        Vector2 vector_i = vector_i_1 - vector_i_0;   // Vector of first segment
        Vector2 vector_f = vector_f_1 - vector_f_0;   // Vector of last segment

        float dist_i = vector_i.magnitude;   // Length of first segment
        float dist_f = vector_f.magnitude;   // Length of last segment

        // Initialize the segment the enemy is in, which is [prevWaypoint, nextWaypoint)
        // All intialized to anything cause of stupid "Use of unassigned local variable" error
        GameObject prevWaypoint = null;
        GameObject nextWaypoint = null;
        float fracOfWayToNextWaypoint = 0;   // Initialize this too

        // If at the food
        if (distToFood <= 0)
        {
            FoodBehaviour.FoodHP -= base_dmg; // TODO Deal base_dmg to the food TODO
            GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());
            Destroy(gameObject);
            return ;
        }

        // Elif at the last segment
        else if (distToFood <= dist_f)
        {
            prevWaypoint = waypoints[waypoints.Count-2];
            nextWaypoint = waypoints[waypoints.Count-1];
            fracOfWayToNextWaypoint = 1 - distToFood/dist_f;
        }

        // Elif at the first segment
        else if (distFromStart < dist_i)
        {
            prevWaypoint = waypoints[0];
            nextWaypoint = waypoints[1];
            fracOfWayToNextWaypoint = distFromStart/dist_i;
        }

        // Elif between two grid waypoints
        else
        {
            float gridTilesTraversed = distFromStart - dist_i;                                // Distance traveled along grid waypoints
            int floorGridTilesTraversed = (int)Math.Floor(gridTilesTraversed);                // Number grid waypoints passed
            float decimalGridTilesTraversed = gridTilesTraversed - floorGridTilesTraversed;   // Fraction of way along current segment

            prevWaypoint = waypoints[floorGridTilesTraversed+1];   // +1 to include first non grid waypoint
            nextWaypoint = waypoints[floorGridTilesTraversed+2];
            fracOfWayToNextWaypoint = decimalGridTilesTraversed;
        }

        checkForStall(prevWaypoint, nextWaypoint);
        if (stalled)
        {
            stallWaypointNext = nextWaypoint;
            stallWaypointPrev = prevWaypoint;
        }
        else
        {
            // Get the position between above waypoints
            Vector3 startPos = prevWaypoint.transform.position;
            Vector3 endPos = nextWaypoint.transform.position;
            nextTileToVisit = nextWaypoint;

            if (prevWaypoint != storedWaypoint)
            {
                if (storedWaypoint != null)
                {
                    storedWaypoint.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.Remove(gameObject);
                }
                storedWaypoint = prevWaypoint;
                nextWaypoint.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.Add(gameObject);
            }

            gameObject.transform.position = Vector2.Lerp(startPos, endPos, fracOfWayToNextWaypoint);
        }
    }

    GameObject storedWaypoint = null;

    // Update frame count for effects
    public void statusUpdate()
    {
        this.status.updateStatus();
    }


    //Below this will be methods for finding a path when a blockage TRT is placed.
    //Only called if, in a list of waypoints, an obstacle is placed on a tile with a waypoint in the list. All spawns using this list, and enemies that are still
    //not past the point of blockage will then call this method to generate the new path.

    private AStarEnemyPathfinding pathFinder;

    public virtual bool findNewPath() 
    {
        //Pathing generation
        //GameObject initialPosition = Instantiate(waypointPrefab, gameObject.transform.position, Quaternion.identity) as GameObject; //Create a waypoint from its current position
        GameObject waypointStart = nextTileToVisit;
        //The enemy will look for a path that first finishes its initial movement towards nextTileToVisit, then look for a new route to waypointEnd.
        GameObject waypointEnd = waypoints[waypoints.Count - 2];
        //GameObject destination = waypoints[waypoints.Count - 1];

        pathFinder = new AStarEnemyPathfinding(ref waypointStart, ref waypointEnd);
        return pathFinder.generateAndCheck();
    }

    public virtual void assignNewPath()
    {
        GameObject initialPosition = Instantiate(waypointPrefab, gameObject.transform.position, Quaternion.identity) as GameObject; //Create a waypoint from its current position
        GameObject destination = waypoints[waypoints.Count - 1];
        waypoints.Clear();

        List<GameObject> pathBody = pathFinder.waypoints;
        waypoints.Add(initialPosition);
        pathBody.Reverse();
        waypoints.AddRange(pathBody);
        waypoints.Add(destination);

        // Update distToFood
        if (waypoints.Count == 2)   // Corner case of no grid tile waypoints in between
        {
            Vector2 cornerCaseStart = waypoints[0].transform.position;          // Starting pos
            Vector2 cornerCaseEnd = waypoints[1].transform.position;            // Ending pos
            Vector2 cornerCaseDisplacement = cornerCaseEnd - cornerCaseStart;   // Vector from start to end
            float cornerCaseDist = cornerCaseDisplacement.magnitude;            // Dist bet start and end
            distToFood = cornerCaseDist;
        }
        else   // There exists at least one grid tile waypoint in between (copy pasted from above)
        {
            int n = waypoints.Count;       // Number of waypoints in total
            // *grid spacing is assumed to be 1, doubt this will change
            // If grid spacing is changed, siply do (n-3)*units_per_grid
            float distAlongGridWaypoints = n - 3;   // Distance traveled between grid waypoints

            // Get positions of the first 2 and last 2 waypoints (possible overlap in middle)
            Vector2 spawnPoint = waypoints[0].transform.position;
            Vector2 firstGridWaypoint = waypoints[1].transform.position;
            Vector2 lastGridWaypoint = waypoints[n-2].transform.position;
            Vector2 endPoint = waypoints[n-1].transform.position;

            // Get the first and last segment to travel
            Vector2 firstSegment = firstGridWaypoint - spawnPoint;
            Vector2 lastSegment = endPoint - lastGridWaypoint;

            // Get the magnitude of these two vectors
            float firstSegmentDist = firstSegment.magnitude;
            float lastSegmentDist = lastSegment.magnitude;

            // Add them up to get total dist to food
            distToFood = firstSegmentDist + distAlongGridWaypoints + lastSegmentDist;
        }

        // Update wayptPathLen
        wayptPathLen = distToFood;
    }


    public class AStarEnemyPathfinding
    {
        public GameObject startPoint, endPoint; //Specifically start and ending waypoints
        private int gridSizeX = 32, gridSizeY = 11;
        public List<GameObject> waypoints;

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

        public bool generateAndCheck()
        {
            waypoints = generatePathing();
            return waypoints.Count > 0;
        }

        private List<GameObject> generatePathing() // Returns a list of Waypoints for an enemy to follow. Can be from a spawn point or an enemy.
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
                            continue;
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
}




// Summary of EnemyStatus at the top
// MOVED INTO ITS OWN FILE (2022-6-17)

/*
public class EnemyStatus
{
    // FIELDS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Base stats of the enemy
    public float base_hp;
    public float base_speed;
    public float base_dmg;

    // Current hp of the enemy
    public float hp;

    // Frame counter for various status effects
    public int snow_frames;
    public int stun_frames;
    public int wet_frames;

    // Multipliers of status effects
    // THESE CAN BE EDITED
    public const float snow_spd_mult = 0.5f;
    public const float water_spd_mult = 0.9f;
    public const float snow_fire_dmg_mult = 1.5f;
    public const float water_fire_dmg_mult = 0.9f;
    public const float electric_water_dmg_mult = 2.0f;




    // Utility function, -= 1 if >0, =0 if =0
    // Pass by reference cause yeh
    public void decrementRef(ref int x) {
        if (x > 0) {
            x -= 1; 
        } else {
            x = 0;
        }
    }



    // Constructor
    public EnemyStatus(float hp, float centi_speed, float dmg) {
        this.base_hp = hp;
        this.base_speed = centi_speed / 100;
        this.base_dmg = dmg;

        this.hp = hp;
        this.snow_frames = 0;
        this.stun_frames = 0;
        this.wet_frames = 0;
    }


    // SPEED AND STATUS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Get current speed; call every framr
    public float currSpeed() {
        float speed = base_speed;
        // Stun => 0 speed
        if (stun_frames > 0) {
            speed *= 0;
        } else if (isWet()) {
            speed *= water_spd_mult;
        }
        if (isSnowed()) {
            speed *= snow_spd_mult;
        }
        return speed;
    }
    public float currHp() {
        return hp;
    }
    // Update counters relating to the enemy; call every frame
    public void updateStatus() {
        decrementRef(ref snow_frames);
        decrementRef(ref stun_frames);
        decrementRef(ref wet_frames);
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // BOOLEAN CHECKS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Ded?
    public bool isDead() { return hp <= 0; }
    // Snowed?
    public bool isSnowed() { return snow_frames > 0; }
    // Stunned?
    public bool isStunned() { return stun_frames > 0; }
    // Wet?
    public bool isWet() { return wet_frames > 0; }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // APPLYING STATUS EFFECTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // (In general, durations don't stack but can overlap) ~~~~~~~~~~~~~~~~~~~~
    // Snow the enemy; yes snow is a verb now
    public void snow(int frame_count) {
        snow_frames = Math.Max(snow_frames, frame_count);
    }
    // Stun the enemy
    public void stun(int frame_count) {
        stun_frames = Math.Max(stun_frames, frame_count);
    }
    // Wet the enemy
    public void wet(int frame_count) {
        wet_frames = Math.Max(wet_frames, frame_count);
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // DEALING DAMAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Deal basic dmg to an enemy
    public void basicDmg(float bullet_dmg) {
        hp -= bullet_dmg;
    }
    // Deal earth dmg to an enemy
    public void earthDmg(float bullet_dmg) {
        // No special dmg interaction for now
        hp -= bullet_dmg;
    }
    // Deal electric dmg to an enemy
    public void electricDmg(float bullet_dmg) {
        float dmg = bullet_dmg;
        if (isWet()) {
            dmg *= electric_water_dmg_mult;
        }
        hp -= dmg;
    }
    // Deal fire damage to an enemy
    public void fireDmg(float bullet_dmg) {
        float dmg = bullet_dmg;
        if (isSnowed()) {
            dmg *= snow_fire_dmg_mult;
            snow_frames = 0;
        }
        if (isWet()) {
            dmg *= water_fire_dmg_mult;
            wet_frames = 0;
        }
        hp -= dmg;
    }
    // Deal snow dmg to an enemy
    public void snowDmg(float bullet_dmg) {
        // No special interaction for now
        hp -= bullet_dmg;
    }
    // Deal water dmg to an enemy
    public void waterDmg(float bullet_dmg) {
        // No special interaction for now
        hp -= bullet_dmg;
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
}
*/







// Enemy class before comment cleaning on 2022-6-14 night/2022-6-15 morning

/*
public abstract class Enemy : MonoBehaviour
{
    public EnemyStatus status;

    // For setting in unity; will be put into status
    public float base_speed;
    public float base_hp;
    public float base_dmg;   // Dmg dealt to food when in contact

    // Basically how far it is from the start; higher val -> higher prio for turrets
    // Is also equal to the number of tiles traversed
    public float priority;
    // Measured by raw speed * number of frames

    /*
    // Unit distance between tiles cause I'm confused with how speed works
    public float unitDist;
    // Number of tiles traversed per frame
    public float tilesPerFrame;
    public float tileUnitsTraversed;
    * /

    // All enemies probably have the same waypoints
    public List<GameObject> waypoints; // = Spawner.waypointList;
    // 

    // SET IN UNITY; used so that a reference to the waypoint prefab exists

    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void setup()
    {
        // Initialize the waypoints and priority
        waypoints = Spawner.waypointList;
        priority = 0;

        /*
        // Initialize the 4 latter fields; to be called in each individual enemy's code too
        waypoints = Spawner.waypointList;
        Vector2 pos0 = waypoints[0].transform.position;
        Vector2 pos1 = waypoints[1].transform.position;
        unitDist = Vector2.Distance(pos0, pos1);
        tilesPerFrame = speed/unitDist;
        tileUnitsTraversed = 0;
        * /
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        // waypoints = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    * /
}
*/





// Deleted 2022-6-13 (comments from a few days ago for EnemyStatus) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

/* TODO
Implement class to encapsulate enemy status (name to be decided)
Include stuff like
    hp
    dmg
    speed
    status effects
So that it'll be easier to handle possible status effects

Fix up the 
*/

// Deleted 2022-6-13 (old comments from when I implemented EnemyList about a week ago) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

/* TODO
Main goals:
1.) Make speed system consistent (aka change the copy pasted code in Wispy)
2.) Implement priority queue for enemyList
3.) Implement range for turret

- Copy the idea for bullet movement over for enemies
- Implement `EnemyList` class
- Implement abstract `Enemy` class
- Implement abstract 'AtkTower' class

abstract class Enemy extends MonoBehaviour
    Fields
        static float speed
        float hp
        static GameObject[] waypoints (Maybe WayPoints[]?)
        float priority
            Basically distance to walk to get to the food
            Lower value => Higher prio for turrets if in range
    Methods
        Start()
        Update()



(Impl details moved to comment in GlobalVariables.cs)
class EnemyList   (to be placed in GlobalVariables.cs)
    Fields
        Enemy[100] enemies = [null for i in range(100)]
        int numEnemies     = 0
        int numDead        = 0
    Methods
        add(Enemy nextEnemy)
        kill(Enemy deadEnemy)
        rearrange()
        findTarget(Vector2 towerPos, float range)
        reset()
        


// Wait why was I planning this again what
abstract class AtkTower extends MonoBehaviour
*/
