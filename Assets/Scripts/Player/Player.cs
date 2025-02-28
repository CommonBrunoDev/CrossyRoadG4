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
    public Vector2 desiredPosition;
    [SerializeField] float animationSpeed = 0;
    [SerializeField] float tileWidth = 2;
    GameObject CameraController_ref;
    GameObject GM_ref;

    public bool immoble = false;
    public bool drowning = false;
    public bool lost;
    public float loseTimer = 0;

    public Log currentLog;

    private void Awake()
    {
        instance = this;
        desiredPosition = new Vector2(transform.position.x,transform.position.z);
    }

    private void Start()
    {
        CameraController_ref = GameObject.FindGameObjectWithTag("CameraController");
        GM_ref = GameObject.FindGameObjectWithTag("GM");
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleLimits();

        if (lost)
        { 
            loseTimer -= Time.deltaTime; 
            if (loseTimer < 0)
            {
                lost = false;
                //HUD.Instance.gameoverButton.setActive();
            }
        }
    }
    void HandleInput()
    {
        var xMove = 0;
        var yMove = 0;


        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) { xMove = -1; transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0)); }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) { xMove = 1; transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0)); }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) { yMove = -1; transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) { yMove = 1; transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0)); }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GM_ref.GetComponent<GM>().b_IsResuming)
            {
                if (!GM_ref.GetComponent<GM>().b_IsInPause) GM_ref.GetComponent<GM>().PauseGame();
                else GM_ref.GetComponent<GM>().WaitResumeGame();
            }
        }

        if (immoble) { xMove = 0; yMove = 0; }

        //Checks movement on x
        if (xMove != 0)
        {
            //Stops movement if on the edges of the map
            if ((gridPosition.x <= 0 && xMove < 0) || (gridPosition.x >= 8 && xMove > 0))
            { xMove = 0;}

            //Checks if the player is on a log
            if (MapGenerator.Instance.map[(int)(gridPosition.y + 3)].type == RowType.WaterLogs)
            { 
                if((xMove >= 1 && currentLog.playerOn == 3) || (xMove <= -1 && currentLog.playerOn == 1))
                {
                    desiredPosition.x += xMove * tileWidth;
                    Drown();
                }
                else
                { currentLog.playerOn += xMove; }
            }
            else
            { 
                var tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x) + xMove];
                if (tileCheck.type == TileType.Ground)
                {
                    if (!tileCheck.hasObstacle)
                    {
                        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                        desiredPosition.x += xMove * tileWidth;
                        MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x)].hasPlayer = false;
                        gridPosition.x += xMove;
                        tileCheck.hasPlayer = true;
                    }
                }
                else if (tileCheck.type == TileType.Water)
                {
                    if (tileCheck.hasObstacle)
                    {
                        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                        desiredPosition.x += xMove * tileWidth;
                        MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x)].hasPlayer = false;
                        gridPosition.x += xMove;
                        tileCheck.hasPlayer = true;
                    }
                    else 
                    {
                        desiredPosition.x += xMove * tileWidth;
                        Drown(); 
                    }
                }
            }
           
        }
        else if (yMove != 0 && MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)] != null)
        {
            //Checks if the player is gonna step on a log
            if (MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].type == RowType.WaterLogs)
            {
                Log l = (MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)] as RowWaterLogs).GetLog(transform.position.x);
                if (l != null)
                {
                    l.PlayerRide(transform.position.x);
                    desiredPosition.y += yMove * tileWidth;
                    gridPosition.y += yMove;
                }
                else
                {
                    desiredPosition.x += xMove * tileWidth;
                    Drown(); 
                }
            }
            else
            {
                //Checks if the player is currently standing on a log
                if (MapGenerator.Instance.map[(int)(gridPosition.y + 3)].type == RowType.WaterLogs)
                {
                    currentLog.playerOn = 0;
                    currentLog = null;

                    int gPosX = (int)(transform.position.x / 2) + 4;
                    if (transform.position.x % 2 >= 1) 
                    {gPosX++;}

                    gridPosition.x = gPosX;

                }

                var tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].tiles[(int)(gridPosition.x)];
                if (MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].type == RowType.WaterLogs)
                {
                    RowWaterLogs row = MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)] as RowWaterLogs;
                    if (row.tileOffset != Mathf.Clamp(row.tileOffset, -0.5f, 0.5f))
                    {
                        if (row.tileOffset > 1f) { tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].tiles[(int)(gridPosition.x - 1)]; }
                        if (row.tileOffset < -1f) { tileCheck = MapGenerator.Instance.map[(int)(gridPosition.y + 3 + yMove)].tiles[(int)(gridPosition.x + 1)]; }
                    }
                }

                if (tileCheck.type == TileType.Ground)
                {
                    if (!tileCheck.hasObstacle)
                    {
                        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                        desiredPosition.y += yMove * tileWidth;
                        desiredPosition.x = -8 + gridPosition.x * tileWidth;
                        MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x)].hasPlayer = false;
                        gridPosition.y += yMove;
                        tileCheck.hasPlayer = true;
                    }
                }
                else if (tileCheck.type == TileType.Water)
                {
                    if (tileCheck.hasObstacle)
                    {
                        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                        desiredPosition.y += yMove * tileWidth;
                        desiredPosition.x = -8 + gridPosition.x * tileWidth;
                        if (tileCheck.GetComponentInParent<Row>().type == RowType.WaterLogs) { desiredPosition.x += tileCheck.transform.parent.GetComponent<RowWaterLogs>().tileOffset; }
                        tileCheck.hasPlayer = true;
                        MapGenerator.Instance.map[(int)(gridPosition.y + 3)].tiles[(int)(gridPosition.x)].hasPlayer = false;
                        gridPosition.y += yMove;
                    }
                    else 
                    { 
                        desiredPosition.y += yMove * tileWidth;
                        Drown(); 
                    }
                }

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
                if (transform.position.x + animationSpeed * Time.deltaTime >= desiredPosition.x)
                {transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);}
                else
                {
                    transform.position = new Vector3(transform.position.x + animationSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                    CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(new Vector3(transform.position.x, 0, 0), 0.4f);
                }
            }
            else
            {
                if (transform.position.x - animationSpeed * Time.deltaTime <= desiredPosition.x)
                {transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);}
                else
                {
                    transform.position = new Vector3(transform.position.x - animationSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                    CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(new Vector3(transform.position.x, 0, 0), 0.4f);
                }
            }
        }
        if (desiredPosition.y != transform.position.z) //Z movement
        {
            if (desiredPosition.y > transform.position.z)
            {
                if (transform.position.z + animationSpeed * Time.deltaTime >= desiredPosition.y)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + animationSpeed * Time.deltaTime);
            }
            else
            {
                if (transform.position.z - animationSpeed * Time.deltaTime <= desiredPosition.y)
                    transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - animationSpeed * Time.deltaTime);
            }
        }

        if (desiredPosition == new Vector2(transform.position.x,transform.position.z) && loseTimer < 0 && drowning)
        { transform.position -= new Vector3(0, 3f * Time.deltaTime,0); }
    }

    public void TeleportOnGrid(Vector2 newPos)
    {
        gridPosition = new Vector2(newPos.x,newPos.y);
        desiredPosition = new Vector2(-8 + newPos.x * tileWidth, -12 + newPos.y * tileWidth);
        transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y); 
    }

    public void HandleLimits()
    {
        if (transform.position.x >= 10 || transform.position.x <= -10)
        { 
            desiredPosition = new Vector2 (transform.position.x,transform.position.z);
            Drown(); 
        }
    }
    public void Drown()
    {
        drowning = true;
        immoble = true;
        loseTimer = 0.3f;
        lost = true;
    }
    public void Squish()
    {
        immoble = true;
        loseTimer = 0.6f;
        transform.localScale = new Vector3(1, 0.3f, 1);
    }
    public void Dragged()
    {
        //Death by distance
        Debug.Log("Hand");
    }
}
