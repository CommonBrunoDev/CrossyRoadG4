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
    private Log[] logs = new Log[5];

    private void Start()
    {
        direction = Random.Range(0, 2) == 0 ? true : false;
        speed = Random.Range(minSpeed, maxSpeed);

        for (int i = 0; i < logs.Length; i++)
        {
            Log l = Instantiate(logRef);
            l.gameObject.SetActive(false);
            l.transform.parent = transform;
            logs[i] = l;
        }
    }

    private void Update()
    {
        //Log spawning
        tileOffset += speed * (direction ? 1 : -1) * Time.deltaTime ;
        if (tileOffset > 2 || tileOffset < -2)
        {
            tileOffset = tileOffset % 2;
            logDistance--;
            if (logDistance <= 0)
            {SpawnLog(); }
        }

        //Removes any logs that go over the limit
        foreach (Log log in logs) 
        {
            log.MoveLog(new Vector3(speed * (direction ? 1 : -1) * Time.deltaTime, 0, 0));
            log.CheckPlayerPos();
            if (log.transform.position.x > 20 || log.transform.position.x < -20)
            { log.gameObject.SetActive(false) ; }
        }
    }

    public void SpawnLog()
    {
        var tilesDis = Random.Range(minTilesBetween, maxTilesBetween + 1);
        logDistance = tilesDis + 3;

        var check = 0;
        while (check < 30)
        {
            var rnd = Random.Range(0, logs.Length);
            if (!logs[rnd].gameObject.activeSelf)
            {
                logs[rnd].GetComponent<NightmareMesher>().SetMesh(GM.Instance.nightmareMode);
                logs[rnd].gameObject.SetActive(true);
                logs[rnd].gameObject.transform.position = new Vector3(16 * (direction ? -1 : 1), 1, transform.position.z);
                check = 30;
            }
            else { check++; }
        }
    }

    public Log GetLog(float posX)
    {
        for (int i = 0; i < logs.Length; i++)
        {
            if (logs[i].transform.position.x - 3 < posX && logs[i].transform.position.x + 3 > posX)
            {return logs[i]; }
        }
        return null;
    }
}
