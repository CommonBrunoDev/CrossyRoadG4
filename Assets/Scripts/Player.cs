using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;

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
    GameObject CameraController_ref;
    GameObject GM_ref;
    GM GMCompontent_ref;

    private void Awake()
    {
        instance = this;
        desiredPosition = new Vector2(transform.position.x,transform.position.z);
    }

    private void Start()
    {
        CameraController_ref = GameObject.FindGameObjectWithTag("CameraController");
        GM_ref = GameObject.FindGameObjectWithTag("GM");
        GMCompontent_ref = GM_ref.GetComponent<GM>();
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
        if (GMCompontent_ref.b_IsStarted)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) xMove = -1;
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) xMove = 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) yMove = -1;
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) yMove = 1;
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!GMCompontent_ref.b_IsResuming)
                {
                    GMCompontent_ref.UpdatePlayerStats();
                    if (!GMCompontent_ref.b_IsInPause) GMCompontent_ref.PauseGame();
                    else GMCompontent_ref.WaitResumeGame();
                }
            }

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
    }

        void HandleMovement()
    {
        if (desiredPosition.x != transform.position.x) //X movement
        {
            if (desiredPosition.x > transform.position.x)
            {
                if (transform.position.x + animationSpeed >= desiredPosition.x)
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + animationSpeed, transform.position.y, transform.position.z);
                    CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(new Vector3(transform.position.x, 0, 0), 0.4f);
                }
            }
            else
            {
                if (transform.position.x - animationSpeed <= desiredPosition.x)
                {
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - animationSpeed, transform.position.y, transform.position.z);
                    CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(new Vector3(transform.position.x, 0, 0), 0.4f);
                }
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
        Debug.Log("Blocked");
    }
    public void Drown()
    {
        //Death + water animation
        Debug.Log("Drowned");
    }
    public void Squish()
    {
        //Death by car
        Debug.Log("Squished");
    }
}
