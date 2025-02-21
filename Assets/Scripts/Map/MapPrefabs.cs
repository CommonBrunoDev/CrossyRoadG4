using System.Collections.Generic;
using UnityEngine;

public class MapPrefabs : MonoBehaviour
{
    public List<GameObject> groundObstacles;
    public List<GameObject> waterPads;
    public List<GameObject> sideTrees;
    public Car car;
    public Car camion;

    private static MapPrefabs instance;
    public static MapPrefabs Instance
    { get { return instance; } }

    private void Awake()
    {instance = this; }
}
