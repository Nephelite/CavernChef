using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    public EnemyStatus status;
    public GameObject waypointPrefab, foodPoint;

    // For setting in unity; will be put into status
    public float base_centi_speed;   // units is "centiunits per frame"
    public float base_hp;
    public float base_dmg;   // Dmg dealt to food

    // Distance from the start; higher val -> higher prio for turrets
    public float priority;

    // Waypoints to path the enemy (from Kevin's A Star pathfinding)
    public List<GameObject> waypoints;

    public float foodOffsetFromGridX, foodOffsetFromGridY, spawnOffsetFromGridX, spawnOffsetFromGridY; //might not need spawn coords

    public GameObject nextTileToVisit;

    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void setup()
    {
        // Initialize the waypoints and priority
        waypoints = new List<GameObject>(Spawner.waypointList);
        priority = 0;
    }

    //Only called if, in a list of waypoints, an obstacle is placed on a tile with a waypoint in the list. All spawns using this list, and enemies that are still
    //not past the point of blockage will then call this method to generate the new path.
    public virtual void findNewPath() 
    {
        //Pathing generation
        GameObject initialPosition = Instantiate(waypointPrefab, gameObject.transform.position, Quaternion.identity) as GameObject; //Create a waypoint from its current position
        GameObject waypointStart = nextTileToVisit;
        //The enemy will look for a path that first finishes its initial movement towards nextTileToVisit, then look for a new route to waypointEnd.
        GameObject waypointEnd = waypoints[waypoints.Count - 2];
        GameObject destination = waypoints[waypoints.Count - 1];
        waypoints.Clear();

        AStarEnemyPathfinding pathFinder = new AStarEnemyPathfinding(ref waypointStart, ref waypointEnd);
        List<GameObject> pathBody = pathFinder.generatePathing();
        waypoints.Add(initialPosition);
        pathBody.Reverse();
        waypoints.AddRange(pathBody);
        waypoints.Add(destination);
        priority = 0;
    }




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
