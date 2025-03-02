using UnityEngine;

public class Log : MonoBehaviour
{
    public int playerOn = 0; 

    public float PlayerRide(float posX)
    {
        if (posX >= transform.position.x + 1 && posX <= transform.position.x + 3)
        { playerOn = 3; }
        else if (posX >= transform.position.x - 1)
        { playerOn = 2; }
        else if (posX >= transform.position.x - 3)
        { playerOn = 1; }
        else { Debug.LogError("ERROR, PLAYER NOT ON LOG"); }

        if (Player.Instance.currentLog != null)
        { Player.Instance.currentLog.playerOn = 0; }
        Player.Instance.currentLog = this;

        float playerPos = transform.position.x + 2 * (playerOn - 2);
        return playerPos;
    }

    public void MoveLog(Vector3 mov)
    {
        transform.position += mov;
        if (playerOn != 0)
        {Player.Instance.desiredPosition.x = transform.position.x + 2 * (playerOn - 2);}
    }

    public void CheckPlayerPos()
    {
        if (Player.Instance.currentLog == this && playerOn != 0)
        {
            if (Player.Instance.transform.position.x < -8 || Player.Instance.transform.position.x > 8)
            {
                Player.Instance.currentLog = null;
                playerOn = 0;
                Player.Instance.Drown();
            }
        }
    }
}
