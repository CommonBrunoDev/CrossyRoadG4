using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;

public class RowRoad : Row
{
    [Range(0.5f, 4f)] public float minCarTime = 2.0f;
    [Range(1f, 8f)] public float maxCarTime = 6.0f;
    [Range(1f, 5f)] public float minBusTime = 3.0f;
    [Range(1.5f, 10f)] public float maxBusTime = 8.0f;

    [Range(0.1f, 2f)] public float minCarSpeed = 3.0f;
    [Range(0.1f, 2f)] public float maxCarSpeed = 8.0f;
    [Range(0.1f, 2f)] public float minBusSpeed = 2.0f;
    [Range(0.1f, 2f)] public float maxBusSpeed = 5.0f;

    private float spawnTimer = 0;
    private float speed = 0;
    Car chosenCar;
    private Car[] cars = new Car[6];

    private bool direction; //False = L to R | True = R to L
    [SerializeField] private float spawnDistance = 0;

    public void Start()
    {
        var rnd = Random.Range(1, 6);
        if (rnd <= 3)
        {
            chosenCar = MapPrefabs.Instance.cars[0];
            spawnTimer = Random.Range(minCarTime, maxCarTime);
            speed = Random.Range(minCarSpeed,maxCarSpeed);
        }
        else
        {
            chosenCar = MapPrefabs.Instance.camions[rnd - 4];
            spawnTimer = Random.Range(minBusTime, maxBusTime);
            speed = Random.Range(minBusSpeed, maxBusSpeed);
        }
        direction = Random.Range(0,2) == 0 ? true : false;

        for (int i = 0; i < cars.Length; i++)
        {
            Car c = Instantiate(chosenCar);
            c.gameObject.SetActive(false);
            c.transform.parent = transform;
            cars[i] = c;
        }
    }

    public void Update()
    {
        foreach (Car car in cars)
        {
            if (car.gameObject.activeSelf)
            {
                var dir = direction ? -1 : 1;
                car.transform.position = new Vector3(car.transform.position.x + speed * dir, 1, transform.position.z);

                if ((car.transform.position.x < -spawnDistance && direction)
                || (car.transform.position.x > spawnDistance && !direction))
                { car.gameObject.SetActive(false); }
            }
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            SpawnCar();
            spawnTimer = chosenCar.name == "car" ? Random.Range(minCarTime, maxCarTime) : Random.Range(minBusTime, maxBusTime);
        }
    }

    void SpawnCar()
    {
        var check = 0;
        while (check < 30)
        {
            var rnd = Random.Range(0, cars.Length);
            if (!cars[rnd].gameObject.activeSelf)
            {
                cars[rnd].gameObject.SetActive(true);
                cars[rnd].gameObject.transform.position = new Vector3(direction ? spawnDistance : -spawnDistance, 1, transform.position.z);
                if(!direction)
                {cars[rnd].transform.rotation = Quaternion.Euler(0, 180, 0); }
                check = 30;
            }
            else { check++; }
        }
    }
}
