using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Commenting out the debug logs here cause I also need to check a debug log - Bryce, 2022-6-30

public class Path : MonoBehaviour
{
    private List<GameObject> path = new List<GameObject>();
    private List<int> shortestPath = new List<int> ();
    private List<GameObject> enemySpawns;
    private List<GameObject> foodPoints;

    private int xLength, yLength, currentTileIndex, pathMinLen;
    public float offsetX, offsetY;
    //private bool hasReachedX = false, hasReachedY = false;
    private GameObject startTile, endTile;

    public List<GameObject> GetGeneratedPath => path;

    // Start is called before the first frame update

    public Path(ref List<GameObject> enemySpawns, List<GameObject> foodPoints, int xLength, int yLength, int pathMinLen, float offsetX, float offsetY, bool isMirroredSpawns)
    {
        this.enemySpawns = enemySpawns;
        this.foodPoints = foodPoints;
        this.xLength = xLength;
        this.yLength = yLength;
        this.pathMinLen = pathMinLen;
        if (isMirroredSpawns)
        {
            this.offsetX = 1.0f + (-offsetX);
            // Debug.Log("Enemy offset: " + this.offsetX); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        }
        else
        {
            this.offsetX = offsetX;
        }
        this.offsetY = offsetY;
    }

    public void GeneratePath(int i)
    {
        /*
        //True Random
        chosenSpawns.Add(Random.Range(0, enemySpawns.Count));
        int two = Random.Range(0, enemySpawns.Count - 1);
        if (two < chosenSpawns[0])
        {
            chosenSpawns.Add(two);
        }
        else
        {
            chosenSpawns.Add(two + 1);
        }
        */
        
        // Debug.Log("Path number " + i); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Coordinates of start and end points (more specifically, the tiles closest to them  on the 32x12 play grid)
                int enemyX = (int)(enemySpawns[i].transform.position.x - offsetX);
                int enemyY = (int)(enemySpawns[i].transform.position.y - offsetY);
                int foodX = (int)(foodPoints[0].transform.position.x - 0.5);
                int foodY = (int)(foodPoints[0].transform.position.y + 2.5);
        // Debug.Log("EnemyX: " + enemyX + " EnemyY: " + enemyY); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Tile assignments
                GameObject startTile = GridGenerator.validEnemyTiles[enemyY * xLength + enemyX];
                GameObject endTile = GridGenerator.validEnemyTiles[foodY * xLength + foodX];
                GameObject currentTile = startTile;
                // X and Y distances between enemy and food spawns
                int xDist = (int)Math.Abs(currentTile.transform.position.x - endTile.transform.position.x);
                int yDist = (int)Math.Abs(currentTile.transform.position.y - endTile.transform.position.y);

                //This part generates a random SSSP, but somewhat biased.
                //SSSP is guaranteed because the path is a rectangular grid with no obstacles.
                //Part 1: Generates a list of 0s and 1s, 0 means go horizontal, 1 means go vertical.
                List<int> turns = new List<int>();
                while (true)
                {
                    if (xDist > 0 && yDist > 0)
                    {
                        int sui = Random.Range(0, 2);
                        if (sui == 0)
                        {
                            xDist--;
                        }
                        else
                        {
                            yDist--;
                        }
                        turns.Add(sui);
                    }
                    else if (xDist > 0)
                    {
                        xDist--;
                        turns.Add(0);
                    }
                    else if (yDist > 0)
                    {
                        yDist--;
                        turns.Add(1);
                    }
                    else { break; }
                }
                //Part 2: Converts the list of 0s and 1s into an actual path.
                var safetyBreak = 0;
                while (turns.Count > 0)
                {
                    // Safety break to prevent crash
                    safetyBreak++;
                    if (safetyBreak > 500)
                        break;

                    // Movement
                    if (turns[0] == 0)
                    {
                        if (currentTile.transform.position.x > endTile.transform.position.x)
                            MoveLeft(ref currentTile);
                        else
                            MoveRight(ref currentTile);
                    }
                    else
                    {
                        if (currentTile.transform.position.y > endTile.transform.position.y)
                            MoveDown(ref currentTile);
                        else
                            MoveUp(ref currentTile);
                    }
                    turns.RemoveAt(0);
                }
                path.Add(currentTile);
            
        


            //Now, we check if the path we have is long enough. If not, we randomly add a sidestep to increase the length.
            while (path.Count < pathMinLen) //change to while after confirming this works once
            {
                List<GameObject> pathNextIter = new List<GameObject>();
                int a = Random.Range(0, path.Count - 5); //path must be longer than 3
                int b = a + 5;
                if (a != b)
                {
                    int first = a;
                    int second = b;
                    // Debug.Log(first); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    for (int j = 0; j < path.Count; j++)
                    {
                        if (j == first)
                        {   
                            GameObject currTile = path[j];
                            GameObject end = path[second];
                            pathNextIter.Add(currTile);

                            List<int> directions = checkDirections(ref currTile);

                            if (directions.Count > 0)
                            {
                                int direction = directions[Random.Range(0, directions.Count)];
                                //create a sidestep from tiles associated to first to tiles associated to second
                                int max = maxDistAccesible(direction, ref currTile);
                                // Debug.Log(max + " " + direction); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                    
                                max = Random.Range(1, max);
                                // Debug.Log(max); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                travelUntilCannot(ref currTile, ref pathNextIter, max, direction, ref end); //Adds tiles in direction to pathNextIter until max tiles have been added
                                //pathNextIter.Add(path[j]);
                                j = second; // effectively blanks out the initial path between first and second
                            }
                            else
                            {
                                //Cannot move from here, use another path
                            }
                            
                        }
                        else
                        {
                            pathNextIter.Add(path[j]);
                        }
                    }
                }
                path = pathNextIter;
            }
        
    }





    private void travelUntilCannot (ref GameObject initTile, ref List<GameObject> nextPath, int maxToTravel, int direction, ref GameObject endTile)
    {
        int index, dist;
        GameObject init = initTile;
        switch (direction)
        {
            case 0:
                //Initial sidestep
                while (maxToTravel > 0)
                {
                    index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                    initTile = GridGenerator.validEnemyTiles[index + xLength];
                    nextPath.Add(initTile);
                    maxToTravel--;
                }

                //Returning to main path
                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) % xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) % xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - 1];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + 1];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }
                else
                {
                    //bug: Dead end
                    // Debug.Log("Dead End"); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) / xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) / xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - xLength];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + xLength];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }

                break;

            case 1:
                while (maxToTravel > 0)
                {
                    index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                    initTile = GridGenerator.validEnemyTiles[index - 1];
                    nextPath.Add(initTile);
                    maxToTravel--;
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) / xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) / xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - xLength];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + xLength];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }
                else
                {
                    //bug: Dead end
                    // Debug.Log("Dead End"); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) % xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) % xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - 1];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + 1];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }

                break;

            case 2:
                while (maxToTravel > 0)
                {
                    index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                    initTile = GridGenerator.validEnemyTiles[index - xLength];
                    nextPath.Add(initTile);
                    maxToTravel--;
                }

                //Returning to main path
                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) % xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) % xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - 1];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + 1];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }
                else
                {
                    //bug: Dead end
                    // Debug.Log("Dead End"); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) / xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) / xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - xLength];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + xLength];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }
                break;
            case 3:
                while (maxToTravel > 0)
                {
                    index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                    initTile = GridGenerator.validEnemyTiles[index + 1];
                    nextPath.Add(initTile);
                    maxToTravel--;
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) / xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) / xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - xLength];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + xLength];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }
                else
                {
                    //bug: Dead end
                    // Debug.Log("Dead End"); ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }

                dist = (GridGenerator.validEnemyTiles.IndexOf(initTile) % xLength) - (GridGenerator.validEnemyTiles.IndexOf(endTile) % xLength);
                if (dist > 0)
                {
                    while (dist > 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index - 1];
                        nextPath.Add(initTile);
                        dist--;
                    }
                }
                else if (dist < 0)
                {
                    while (dist < 0)
                    {
                        index = GridGenerator.validEnemyTiles.IndexOf(initTile);
                        initTile = GridGenerator.validEnemyTiles[index + 1];
                        nextPath.Add(initTile);
                        dist++;
                    }
                }

                break;
        }
    }

    private int maxDistAccesible (int direction, ref GameObject initTile)
    {
        GameObject currentTile = initTile;
        int count = 0;
        switch (direction)
        {
            case 0:
                do
                {
                    currentTile = GridGenerator.validEnemyTiles[GridGenerator.validEnemyTiles.IndexOf(currentTile) + xLength];
                    count++;
                } while (!path.Contains(currentTile) && currentTile.transform.position.y < yLength - 1);
                break;
            case 1:
                do
                {
                    currentTile = GridGenerator.validEnemyTiles[GridGenerator.validEnemyTiles.IndexOf(currentTile) - 1];
                    count++;
                } while (!path.Contains(currentTile) && currentTile.transform.position.x > 0);
                break;
            case 2:
                do
                {
                    currentTile = GridGenerator.validEnemyTiles[GridGenerator.validEnemyTiles.IndexOf(currentTile) - xLength];
                    count++;
                } while (!path.Contains(currentTile) && currentTile.transform.position.y > 0);
                break;
            case 3:
                do
                {
                    currentTile = GridGenerator.validEnemyTiles[GridGenerator.validEnemyTiles.IndexOf(currentTile) + 1];
                    count++;
                } while (!path.Contains(currentTile) && currentTile.transform.position.x < xLength - 1);
                break;
        }
        return count;
    }

    private bool isDirectionAccessible(ref GameObject currentTile, int direction) //directions: 0 - up, 1 - left, 2 - down, 3 - right
    {

        int curr = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        if (direction == 0)
        {
            return curr / xLength != yLength && !path.Contains(GridGenerator.validEnemyTiles[curr + xLength]);
        }
        else if (direction == 1)
        {
            return curr % xLength != 0 && !path.Contains(GridGenerator.validEnemyTiles[curr - 1]);
        }
        else if (direction == 2)
        {
            return curr / xLength != 0 && !path.Contains(GridGenerator.validEnemyTiles[curr - xLength]);
        }
        else if (direction == 3) 
        {
            return curr % xLength != xLength - 1 && !path.Contains(GridGenerator.validEnemyTiles[curr + 1]);
        }
        else
        {
            return false;
        }
    }

    private List<int> checkDirections(ref GameObject currentTile)
    {
        List<int> directionsAccessible = new List<int>();
        int curr = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        if (curr / xLength != yLength - 1 && !path.Contains(GridGenerator.validEnemyTiles[curr + xLength]))
        {
            directionsAccessible.Add(0); // Can go up
        }
        if (curr % xLength != 0 && !path.Contains(GridGenerator.validEnemyTiles[curr - 1]))
        {
            directionsAccessible.Add(1); // Can go left
        }
        if (curr / xLength != 0 && !path.Contains(GridGenerator.validEnemyTiles[curr - xLength]))
        {
            directionsAccessible.Add(2); // Can go down
        }
        if (curr % xLength != xLength - 1 && !path.Contains(GridGenerator.validEnemyTiles[curr + 1]))
        {
            directionsAccessible.Add(3); // Can go right
        }
        return directionsAccessible;
    }

    private void MoveDown(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        //shortestPath.Add(currentTileIndex);
        int n = currentTileIndex - xLength;
        currentTile = GridGenerator.validEnemyTiles[n];
    }

    private void MoveUp(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        //shortestPath.Add(currentTileIndex);
        int n = currentTileIndex + xLength;
        currentTile = GridGenerator.validEnemyTiles[n];
    }
    private void MoveLeft(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        //shortestPath.Add(currentTileIndex);
        currentTileIndex--;
        currentTile = GridGenerator.validEnemyTiles[currentTileIndex];
    }
    private void MoveRight(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        //shortestPath.Add(currentTileIndex);
        currentTileIndex++;
        currentTile = GridGenerator.validEnemyTiles[currentTileIndex];
    }
}

/* Old code
var safetyBreakX = 0;
while (!hasReachedX)
{
    //Safety break to prevent crash
    safetyBreakX++;
    if (safetyBreakX > 500)
        break;

    //Movement
    if (currentTile.transform.position.x > endTile.transform.position.x)
        MoveLeft(ref currentTile);
    else if (currentTile.transform.position.x < endTile.transform.position.x)
        MoveRight(ref currentTile);
    else
        hasReachedX = true;
}

var safteyBreakY = 0;
while (!hasReachedY)
{
    //Safety break to prevent crash
    safteyBreakY++;
    if (safteyBreakY > 500)
        break;

    //Movement
    if (currentTile.transform.position.y > endTile.transform.position.y)
        MoveDown(ref currentTile);
    else if (currentTile.transform.position.y < endTile.transform.position.y)
        MoveUp(ref currentTile);
    else
        hasReachedY = true;
}
shortestPath.Add(GridGenerator.validEnemyTiles.IndexOf(endTile));






for (int j = 0; j < shortestPath.Count; j++)
{
    path.Add(GridGenerator.validEnemyTiles[shortestPath[j]]);
}
*/














/*
This is where I explain the random path genmeration I have come up with

The playable area is 32x12, so let's split it up into 12 even pieces, each being 8x3. 

We start from the spawn point in one of these 12 sections. We first generate a sequence for which the sections must be traversed. 

e.g: 

        |1col   |2col   |3col   |4col
-----------------------------------------------
Arow    |0      |7      |8      |9
-----------------------------------------------
Brow    |1      |6      |5      |10
-----------------------------------------------
Crow    |2      |3      |4      |11

Numbers show the pathing taken. Start at 0, end at 11.

We now randomly generate a point the path must go to within the sections during traversal.

Perhaps in higher difficulties, we can reduce the number of sections they need to go through. However, we need more points travelled than not to have an interesting path.

Grid indices:

8  9  10 11
4  5  6  7
0  1  2  3



int currentSection = (foodY / 3) * 4 + (foodX / 4); //Starting
int endingSection = (enemyY / 3) * 4 + (enemyX / 4);
List<int> sectionsTraversed = new List<int>();


List<int> 
int pathLength = 0;

BFS bfs = new BFS(sectionsToTraverse);

for (int i = 0; i < 12; i++)
{
    if (i > 3)
    {
        bfs.AddEdge(i, i - 4);
    }
    if (i % 4 < 3)
    {
        bfs.AddEdge(i, i + 1); 
    }
    if (i < 8)
    {
        bfs.AddEdge(i, i + 4); 
    }
    if (i % 4 > 0)
    {
        bfs.AddEdge(i, i - 1)
    }
}   


while (pathLength < sectionsToTraverse)
{
    sectionsTraversed.Add(currentSection);
    pathLength++;

    List<int> possibleDirections = new List<int>();
    if (currentSection > 3)
    {
        possibleDirections.Add(0); // 0 means can go up
    }
    if (currentSection % 4 < 3)
    {
        possibleDirections.Add(1); // 1 means can go left
    }
    if (currentSection < 8)
    {
        possibleDirections.Add(2); // 2 means can go down
    }
    if (currentSection % 4 > 0)
    {
        possibleDirections.Add(3); // 3 means can go right
    }

    if (possibleDirections.Count == 0)
    {
        if (currentSection == endingSection && pathLength >= sectionsToTraverse)
        {
            break; // Done
        }
        else
        {

        }
    } 
    else
    {

    }


}
*/

/* Old code
int numberOfPointsToAvoid = 12 - sectionsToTraverse;

List<int> pointsToPass = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

for (int j = 0; j < numberOfPointsToAvoid; j++)
{
    pointsToPass.RemoveAt(Random.Range(0, pointsToPass.Count));
}
ListShuffler.Shuffle<int>(pointsToPass);

List<int> indexOfPoint = new List<int>();
for (int j = 0; j < pointsToPass.Count; j++)
{
    indexOfPoint.Add(Random.Range(0, 24)); //Change 24 if size of area to randomise changes
    Debug.Log(pointsToPass[j]);
}
*/



/*
var safetyBreakX = 0;
while (!hasReachedX)
{
    //Safety break to prevent crash
    safetyBreakX++;
    if (safetyBreakX > 500)
        break;

    //Movement
    if (currentTile.transform.position.x > endTile.transform.position.x)
        MoveLeft(ref currentTile);
    else if (currentTile.transform.position.x < endTile.transform.position.x)
        MoveRight(ref currentTile);
    else
        hasReachedX = true;
}

var safteyBreakY = 0;
while (!hasReachedY)
{
    //Safety break to prevent crash
    safteyBreakY++;
    if (safteyBreakY > 500)
        break;

    //Movement
    if (currentTile.transform.position.y > endTile.transform.position.y)
        MoveDown(ref currentTile);
    else if (currentTile.transform.position.y < endTile.transform.position.y)
        MoveUp(ref currentTile);
    else
        hasReachedY = true;
}
path.Add(endTile);
*/