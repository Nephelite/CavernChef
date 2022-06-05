using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
public class AStarEnemyPathfindingTemplate : MonoBehaviour
{
    public GameObject startPoint, endPoint; //Specifically start and ending waypoints
    private int gridSizeX = 32, gridSizeY = 11;

    public AStarEnemyPathfindingTemplate(ref GameObject start, ref GameObject end)
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
            int newCostToEnd =  Mathf.Abs(currPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX - endPoint.GetComponent<WaypointInternals>().tileIndex % gridSizeX) 
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

        while(activeNodes.Count > 0)
        {
            Node checkNode = activeNodes.OrderBy(x => x.totalCost).First();
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
                var adjWaypoints = checkNode.associatedWaypoint.GetComponent<WaypointInternals>().adjWaypoints;
                foreach(var adjWaypoint in adjWaypoints)
                {
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
        while (finalNode.parent != null)
        {
            waypointPathing.Add(finalNode.associatedWaypoint);
            finalNode = finalNode.parent;
        }

        return waypointPathing;
    }
}
*/