using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Path
{
    private List<GameObject> path = new List<GameObject>();
    private List<int> shortestPath = new List<int> ();
    private List<GameObject> enemySpawns;
    private List<GameObject> foodPoints;

    private int xLength, yLength, currentTileIndex, sectionsToTraverse;
    //private bool hasReachedX = false, hasReachedY = false;
    private GameObject startTile, endTile;

    public List<GameObject> GetGeneratedPath => path;

    // Start is called before the first frame update

    public Path(List<GameObject> enemySpawns, List<GameObject> foodPoints, int xLength, int yLength, int sectionsToTraverse)
    {
        this.enemySpawns = enemySpawns;
        this.foodPoints = foodPoints;
        this.xLength = xLength;
        this.yLength = yLength;
        this.sectionsToTraverse = sectionsToTraverse;
    }
    
    public void GeneratePath()
    {
        for (int i = 0; i < enemySpawns.Count; i++)
        {
            //Coordinates of start and end points (more specifically, the tiles closest to them  on the 32x12 play grid)
            int enemyX = (int)(enemySpawns[i].transform.position.x - 0.5); 
            int enemyY = (int)(enemySpawns[i].transform.position.y - 1.5);
            int foodX = (int)(foodPoints[i].transform.position.x - 0.5);
            int foodY = (int)(foodPoints[i].transform.position.y + 2.5);

            //Tile assignments
            GameObject startTile = GridGenerator.validEnemyTiles[enemyY * xLength + enemyX];
            GameObject endTile = GridGenerator.validEnemyTiles[foodY * xLength + foodX];
            GameObject currentTile = startTile;
            // X and Y distances between enemy and food spawns
            int xDist = (int) Math.Abs(currentTile.transform.position.x - endTile.transform.position.x); 
            int yDist = (int) Math.Abs(currentTile.transform.position.y - endTile.transform.position.y); 

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
        }
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
