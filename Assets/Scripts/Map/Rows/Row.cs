using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowIndex;
    public RowType type = RowType.None;
    public Tile[] tiles = new Tile[9];

    private void Awake()
    {
        var availablePaths = 9;
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile t = Instantiate(MapGenerator.Instance.tileRef);

            switch (type)
            {
                case RowType.Normal: case RowType.Empty: case RowType.Road: case RowType.Train:
                    t.type = TileType.Ground; break;

                case RowType.WaterPads: case RowType.WaterLogs:
                    t.type = TileType.Water; break;
            }

            if (type == RowType.Normal)
            {
                if (availablePaths > 5)
                {
                    var rnd = Random.Range(0, 5);
                    if (rnd == 0)
                    {
                        t.hasObstacle = true;
                        if (rowIndex > 0)
                        {
                            if (MapGenerator.Instance.map[rowIndex - 1].tiles[i].hasObstacle)
                            { availablePaths--; }
                        }
                    }
                }
            }
            t.transform.parent = this.transform;
            t.transform.position = new Vector3(-8 + i * 2, 0, transform.position.z);
            tiles[i] = t;
        }

        if (type == RowType.WaterPads)
        {
            var waterPaths = 5;
            while (waterPaths > 0)
            {
                var rnd = Random.Range(1,8);
                if (!tiles[rnd].hasObstacle)
                {
                    tiles[rnd].hasObstacle = true;
                    waterPaths--;
                }
            }
        }
    }

    private void Start()
    {
        if (type == RowType.Empty || type == RowType.Normal || type == RowType.WaterPads)
        {
            var counter = 0;
            while (counter < 17)
            {
                var rnd = Random.Range(0, 5);
                if (counter == 3 || counter == 13) { rnd = 0; }
                if (rnd < 2)
                {
                    var tree = Instantiate(MapPrefabs.Instance.sideTrees[0]);
                    tree.transform.position = new Vector3(-16 + 2 * counter, 0, transform.position.z);
                }
                counter++;
                if (counter == 4) { counter += 9; }
            }
        }



        if (type == RowType.Normal || type == RowType.Road || type == RowType.Train)
        {
            if (Random.Range(0, 8) == 0)
            {
                var rnd = Random.Range(0, 9);
                if (!tiles[rnd].hasObstacle)
                {
                    var coin = Instantiate(MapPrefabs.Instance.coin);
                    coin.transform.position = tiles[rnd].transform.position;
                }
            }
        }
    }
}
