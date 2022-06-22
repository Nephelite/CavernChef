using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject trtTile;

    public GameObject enemyTile;

    // public GameObject WayPointTemplate;

    public List<GameObject> enemySpawn; 

    public List<GameObject> foodPoint; //Demo version only has 1 point

    public static List<GameObject> validEnemyTiles = new List<GameObject>(); //So uh... I will get around to renaming this, but this is actually all tiles in the play grid.

    private int gridSizeX = 32, gridSizeY = 11, currentTileIndex;
    public float offsetX, offsetY;
    public bool isMirroredSpawns;

    // Start is called before the first frame update
    void Start()
    {
        List<int> chosenSpawns = new List<int>();
        //Random once from left and once from right
        chosenSpawns.Add(Random.Range(0, 2));
        chosenSpawns.Add(Random.Range(2, 4));
        for (int i = 0; i < enemySpawn.Count; i++)
        {
            if (chosenSpawns.Contains(i))
            {
                Destroy(enemySpawn[i]);
                enemySpawn.RemoveAt(i);
                chosenSpawns.Remove(i);
                if (chosenSpawns.Count > 0)
                    chosenSpawns[0]--;
                i--;
            }
        }

        validEnemyTiles.Clear();
        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                Vector2 pos = new Vector2(j, i);
                GameObject block = Instantiate(trtTile, pos, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
                validEnemyTiles.Add(block);
            }
        }

        Path pathGeneration1 = new Path(ref enemySpawn, foodPoint, gridSizeX, gridSizeY, 60, offsetX, offsetY, false);
        pathGeneration1.GeneratePath(0);
        Path pathGeneration2 = new Path(ref enemySpawn, foodPoint, gridSizeX, gridSizeY, 60, offsetX, offsetY, isMirroredSpawns);
        pathGeneration2.GeneratePath(1);
        Spawner.spawnPoints = enemySpawn;
        Debug.Log("Path Generated with " + enemySpawn.Count + " spawn points");
        
        List<GameObject> actualGrid = new List<GameObject>();
        for (int i = 0; i < validEnemyTiles.Count; i++)
        {
            if (pathGeneration1.GetGeneratedPath.Contains(validEnemyTiles[i]) || pathGeneration2.GetGeneratedPath.Contains(validEnemyTiles[i]))
            {
                validEnemyTiles[i].SetActive(false);
                Vector2 pos = new Vector2(i % 32, i / 32);
                GameObject block = Instantiate(enemyTile, pos, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
                actualGrid.Add(block);
            }
            else
            {
                actualGrid.Add(validEnemyTiles[i]);
            }
        }
        validEnemyTiles = actualGrid;
    }
}
