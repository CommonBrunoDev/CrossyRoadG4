using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class Tile : MonoBehaviour
{
    public TileType type;
    public bool hasObstacle = false;
    public bool hasPlayer = false;
    //private bool hasCoin;

    private void Start()
    {
        if (hasObstacle)
        {
            if (type == TileType.Ground)
            {
                if (transform.parent.GetComponent<Row>().type != RowType.Empty)
                {
                    var map = transform.parent.parent.GetComponent<MapPrefabs>();
                    int rnd = Random.Range(0, map.groundObstacles.Count);
                    var obstacle = Instantiate(map.groundObstacles[rnd]);
                    obstacle.transform.parent = transform;
                    obstacle.transform.position = transform.position;
                }
                else { hasObstacle = false; }
            }
            else if (type == TileType.Water)
            {
                if (transform.parent.GetComponent<Row>().type == RowType.WaterPads)
                {
                    var map = transform.parent.parent.GetComponent<MapPrefabs>();
                    int rnd = Random.Range(0, map.waterPads.Count);
                    var obstacle = Instantiate(map.waterPads[rnd]);
                    obstacle.transform.parent = transform;
                    obstacle.transform.position = transform.position;
                }
                else { hasObstacle = false; }
            }
        }
    }
}
