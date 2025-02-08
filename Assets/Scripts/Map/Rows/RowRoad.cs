using UnityEngine;

public class RowRoad : Row
{
    public float minCarTime = 3.0f;
    public float maxCarTime = 8.0f;
    private float carTimer;
    public bool direction = false; //False = L to R | True = R to L
}
