using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{
    public GameObject mapObject;
    public Row normalRow;
    public Row wallRow;
    public Row roadRow;
    public Row trainRow;
    public Row waterPadsRow;
    public Row waterLogsRow;
    public Tile tileRef;

    public List<int> test = new List<int>();
    public int currentTest = 0;
    public List<Row> map = new List<Row>();

    private static MapGenerator instance;
    public static MapGenerator Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 28; i++)
        {
            Row r = null;
            if (i <= 2)
            { r = Instantiate(wallRow); }
            else if (i <= 7)
            { 
                r = Instantiate(normalRow);
                r.type = RowType.Empty;
            }
            else { r = CreateProceduralRow(i); }

            r.transform.position = new Vector3(0, 0, -18 + i * 2);
            r.rowIndex = i;
            map.Add(r);
            r.transform.parent = mapObject.transform;
        }
        Player.Instance.TeleportOnGrid(new Vector2(4,3));
    }

    public int[] GetChances(Row row)
    {
        int[] chances = new int[5];   //Normal,road,train,water pad, water log,

        switch (row.type)
        {
            case RowType.Empty:
                chances[0] = 100;
                chances[1] = 0;
                chances[2] = 0;
                chances[3] = 0;
                chances[4] = 0;
                break;

            case RowType.Normal:
            case RowType.Road:
            case RowType.Train:
                chances[0] = 25;
                chances[1] = 25;
                chances[2] = 24;
                chances[3] = 13; 
                chances[4] = 13; 
                break;

            case RowType.WaterPads:
                chances[0] = 0;
                chances[1] = 0;
                chances[2] = 0;
                chances[3] = 0; 
                chances[4] = 100; 
                break;  

            case RowType.WaterLogs:
                chances[0] = 20;
                chances[1] = 20;
                chances[2] = 20;
                chances[3] = 0;
                chances[4] = 40;
                break;
        }

        return chances;
    }

    public Row CreateProceduralRow(int index)
    {
        Row r = null;
        var chances = GetChances(map[index - 1]);
        var rnd = Random.Range(1, 101);
        //Normal - Road - Train - Pads - Logs
        if (rnd <= chances[0])
        { r = Instantiate(normalRow); }
        else if (rnd <= chances[0] + chances[1])
        { r = Instantiate(roadRow); }
        else if (rnd <= chances[0] + chances[1] + chances[2])
        { r = Instantiate(trainRow); }
        else if (rnd <= chances[0] + chances[1] + chances[2] + chances[3])
        { r = Instantiate(waterPadsRow); }
        else if (rnd <= chances[0] + chances[1] + chances[2] + chances[3] + chances[4])
        { r = Instantiate(waterLogsRow); }
        else
        {
            r = Instantiate(normalRow);
            r.type = RowType.Empty;
        }

        return r;
    }

    public void CheckGenerate(int playerY)
    {
        if (playerY + 28 > map.Count )
        {
            var r = CreateProceduralRow(map.Count);
            r.transform.position = new Vector3(0, 0, -18 + map.Count * 2);
            r.rowIndex = map.Count;
            map.Add(r);
            r.transform.parent = mapObject.transform;

            GM.Instance.IncreasePlayerPoints();
        }
    }
}
