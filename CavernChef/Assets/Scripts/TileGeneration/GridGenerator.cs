using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject trtTile;

    public GameObject enemyTile;

    // public GameObject WayPointTemplate;

    public List<GameObject> enemySpawn; //Demo version only has 1 point

    public List<GameObject> foodPoint; //Demo version only has 1 point

    public static List<GameObject> validEnemyTiles = new List<GameObject>(); //So uh... I will get around to renaming this, but this is actually all tiles in the play grid.

    private int gridSizeX = 32, gridSizeY = 11, currentTileIndex;
    public float offsetX, offsetY;

    // Start is called before the first frame update
    void Start()
    {
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
        Path pathGeneration = new Path(enemySpawn, foodPoint, gridSizeX, gridSizeY, 70, offsetX, offsetY);
        pathGeneration.GeneratePath();

        List<GameObject> actualGrid = new List<GameObject>();
        for (int i = 0; i < validEnemyTiles.Count; i++)
        {
            if (pathGeneration.GetGeneratedPath.Contains(validEnemyTiles[i]))
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
