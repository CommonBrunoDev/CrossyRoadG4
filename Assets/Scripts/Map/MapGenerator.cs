using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{
    public Row normalRow;
    public Row wallRow;
    public Row roadRow;
    public Row trainRow;
    public Row waterPadsRow;
    public Row waterLogsRow;

    public List<Row> map = new List<Row>();
    [SerializeField] private float startingZ;

    private void Start()
    {
        for (int i = 0; i < 26; i++)
        {
            Row r;
            if (i == 0)
            { r = Instantiate(normalRow); }
            else
            { r = Instantiate(normalRow); }
            r.transform.position = new Vector3(0, 0, -14 + i * 2);
            map.Add(r);
        }
    }
    void Update()
    {
        
    }
}
