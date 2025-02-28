using System.Collections.Generic;
using UnityEngine;

public class MapPrefabs : MonoBehaviour
{
    public List<GameObject> groundObstacles;
    public List<GameObject> waterPads;
    public List<GameObject> sideTrees;
    public List<Car> cars;
    public List<Car> camions;
    public Coin coin;

    private static MapPrefabs instance;
    public static MapPrefabs Instance
    { get { return instance; } }

    private void Awake()
    {instance = this; }
}
