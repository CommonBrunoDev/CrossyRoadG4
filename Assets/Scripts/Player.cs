using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {get { return instance; }}

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

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            xMove = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            xMove = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            yMove = -1;
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            yMove = 1;

        if (xMove != 0)
        {
            transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
            desiredPosition.x += xMove * tileWidth;
        }
        else if (yMove != 0)
        {
            transform.position = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.y);
            desiredPosition.y += yMove * tileWidth;
        }
    }

    void HandleMovement()
    {
        if (desiredPosition.x != transform.position.x) //X movement
        {
            if (desiredPosition.x > transform.position.x)
            {
                if (transform.position.x + animationSpeed > desiredPosition.x)
                    transform.position = desiredPosition;
                else
                    transform.position = new Vector3(transform.position.x + animationSpeed, transform.position.y, transform.position.z);
            }
            else
            {
                if (transform.position.x - animationSpeed < desiredPosition.x)
                    transform.position = desiredPosition;
                else
                    transform.position = new Vector3(transform.position.x - animationSpeed, transform.position.y, transform.position.z);
            }
        }
        if (desiredPosition.y != transform.position.z) //X movement
        {
            if (desiredPosition.y > transform.position.z)
            {
                if (transform.position.z + animationSpeed > desiredPosition.y)
                    transform.position = desiredPosition;
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + animationSpeed);
            }
            else
            {
                if (transform.position.z - animationSpeed < desiredPosition.y)
                    transform.position = desiredPosition;
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - animationSpeed);
            }
        }
    }
}
