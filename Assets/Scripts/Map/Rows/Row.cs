using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowIndex;
    public RowType type = RowType.None;
    public Tile[] tiles = new Tile[9];

    //TODO Player movement
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
}
