using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject trtTile;

    public GameObject enemyTile;
    
    private int gridSizeX = 32;
    
    private int gridSizeY = 11;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector2 pos = new Vector2(i, j);
                GameObject block = Instantiate(trtTile, pos, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
            }
        }
    }
}
