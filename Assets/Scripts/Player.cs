using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {get { return instance; }}

    public Vector2 tilePosition = Vector2.zero;
    public Vector2 gridPosition;
    [SerializeField] private Vector2 desiredPosition;
    [SerializeField] float animationSpeed = 0;
    [SerializeField] float tileWidth = 2;

    private void Awake()
    {
        instance = this;
        desiredPosition = new Vector2(transform.position.x,transform.position.z);
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
    }
    void HandleInput()
    {
        var xMove = 0;
        var yMove = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) xMove = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) xMove = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) yMove = -1;
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) yMove = 1;

        if (xMove != 0)
        {
            if ((gridPosition.x <= 0 && xMove < 0) || (gridPosition.x >= 8 && xMove > 0))
            { 
                xMove = 0;
                Debug.Log("Stopped movement");
            }

            var tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x) + xMove];
            if (tileCheck.type == TileType.Ground)
            {
                if (tileCheck.hasObstacle)
                { Blocked(); }
                else
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                    desiredPosition.x += xMove * tileWidth;
                    gridPosition.x += xMove;
                }
            }
            else if (tileCheck.type == TileType.Water || tileCheck.type == TileType.Log)
            {
                if (tileCheck.hasObstacle)
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                    desiredPosition.x += xMove * tileWidth;
                    gridPosition.x += xMove;
                }
                else { Drown(); }
            }
        }
        else if (yMove != 0 && MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)] != null)
        {
            var tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].tiles[(int)(gridPosition.x)];
            if (tileCheck.type == TileType.Ground)
            {
                if (tileCheck.hasObstacle)
                { Blocked(); }
                else
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                    desiredPosition.y += yMove * tileWidth;
                    gridPosition.y += yMove;
                }
            }
            else if (tileCheck.type == TileType.Water || tileCheck.type == TileType.Log)
            {
                if (tileCheck.hasObstacle)
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                    desiredPosition.y += yMove * tileWidth;
                    if (tileCheck.type == TileType.Log) { desiredPosition.x += tileCheck.transform.parent.GetComponent<Row>().xOffset; }
                    gridPosition.y += yMove;
                }
                else { Drown(); }
            }

            MapGenerator.Instance.CheckGenerate((int)gridPosition.y + 3);
        }
    }
        
    void HandleMovement()
    {
        if (desiredPosition.x != transform.position.x) //X movement
        {
            if (desiredPosition.x > transform.position.x)
            {
                if (transform.position.x + animationSpeed >= desiredPosition.x)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x + animationSpeed, transform.position.y, transform.position.z);
            }
            else
            {
                if (transform.position.x - animationSpeed <= desiredPosition.x)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x - animationSpeed, transform.position.y, transform.position.z);
            }
        }
        if (desiredPosition.y != transform.position.z) //Z movement
        {
            if (desiredPosition.y > transform.position.z)
            {
                if (transform.position.z + animationSpeed >= desiredPosition.y)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + animationSpeed);
            }
            else
            {
                if (transform.position.z - animationSpeed <= desiredPosition.y)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - animationSpeed);
            }
        }
    }

    public void TeleportOnGrid(Vector2 newPos)
    {
        gridPosition = new Vector2(newPos.x,newPos.y);
        desiredPosition = new Vector2(-8 + newPos.x * tileWidth, -12 + newPos.y * tileWidth);
        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y); 
    }
    public void Blocked()
    {
        //Animation in case you try to go somewhere you cant
    }
    public void Drown()
    {
        //Death + water animation
    }
}
