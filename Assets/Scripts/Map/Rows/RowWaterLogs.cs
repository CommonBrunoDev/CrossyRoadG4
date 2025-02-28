using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RowWaterLogs : Row
{
    public int minTilesBetween = 1;
    public int maxTilesBetween = 4;
    private int logDistance = 1;

    public float minSpeed = 0.02f;
    public float maxSpeed = 0.04f;
    private float speed;

    public bool direction = false; //False = L to R | True = R to L
    public float tileOffset = 0;

    public Log logRef;
    private List<Log> logs = new List<Log>();

    private void Start()
    {
        direction = Random.Range(0, 2) == 0 ? true : false;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        //Log spawning
        tileOffset += speed * (direction ? 1 : -1) * Time.deltaTime;
        if (tileOffset > 2 || tileOffset < -2)
        {
            tileOffset = tileOffset % 2;
            logDistance--;
            if (logDistance <= 0)
            {SpawnLog(); }
        }

        //Removes any logs that go over the limit
        bool removeLog = false;
        foreach (Log log in logs) 
        {
            log.MoveLog(new Vector3(speed * (direction ? 1 : -1) * Time.deltaTime, 0, 0)); 
            if (log.transform.position.x > 20 || log.transform.position.x < -20)
            { removeLog = true; }
        }
        if (removeLog)
        {
            var l = logs[0];
            logs.Remove(logs[0]);
            Destroy(l);
        }
    }

    public void SpawnLog()
    {
        var tilesDis = Random.Range(minTilesBetween, maxTilesBetween + 1);
        logDistance = tilesDis + 3;
        var l = Instantiate(logRef);
        l.transform.position = new Vector3(16 * (direction ? -1 : 1), 1, transform.position.z); 
        logs.Add(l);
    }

    public Log GetLog(float posX)
    {
        for (int i = 0; i < logs.Count; i++)
        {
            if (logs[i].transform.position.x - 3 < posX && logs[i].transform.position.x + 3 > posX)
            {return logs[i]; }
        }
        return null;
    }
}
