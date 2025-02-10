using UnityEngine;

public class RowWaterLogs : Row
{
    public float minLogTime = 1.0f;
    public float maxLogTime = 4.0f;
    private float logTimer;

    public int minLogLenght = 1;
    public int maxLogLenght = 3;
    private int lastLogLenght = 0;

    public bool direction = false; //False = L to R | True = R to L
    public float speed = 2;
    public float tileOffset = 0;
}
