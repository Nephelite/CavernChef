using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<GameObject> path = new List<GameObject>();
    private List<GameObject> enemySpawns;
    private List<GameObject> foodPoints;

    private int xLength, yLength, currentTileIndex;
    private bool hasReachedX = false, hasReachedY = false;
    private GameObject startTile, endTile;

    public List<GameObject> GetGeneratedPath => path;

    // Start is called before the first frame update

    public Path(List<GameObject> enemySpawns, List<GameObject> foodPoints, int xLength, int yLength)
    {
        this.enemySpawns = enemySpawns;
        this.foodPoints = foodPoints;
        this.xLength = xLength;
        this.yLength = yLength;
    }
    
    public void GeneratePath()
    {
        //Coordinates of start and end points (more specifically, the tiles closest to them  on the 32x12 play grid)
        int enemyX = (int) (enemySpawns[0].transform.position.x - 0.5); //use nested loops in future versions
        int enemyY = (int) (enemySpawns[0].transform.position.y - 1.5);
        int foodX = (int) (foodPoints[0].transform.position.x - 0.5);
        int foodY = (int) (foodPoints[0].transform.position.y + 2.5);

        //Tile assignments
        GameObject startTile = GridGenerator.validEnemyTiles[enemyY * xLength + enemyX]; 
        GameObject endTile = GridGenerator.validEnemyTiles[foodY * xLength + foodX];
        GameObject currentTile = startTile;

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
    }

    private void MoveDown(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        int n = currentTileIndex - xLength;
        currentTile = GridGenerator.validEnemyTiles[n];
    }

    private void MoveUp(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        int n = currentTileIndex + xLength;
        currentTile = GridGenerator.validEnemyTiles[n];
    }
    private void MoveLeft(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        currentTileIndex--;
        currentTile = GridGenerator.validEnemyTiles[currentTileIndex];
    }
    private void MoveRight(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GridGenerator.validEnemyTiles.IndexOf(currentTile);
        currentTileIndex++;
        currentTile = GridGenerator.validEnemyTiles[currentTileIndex];
    }
}
