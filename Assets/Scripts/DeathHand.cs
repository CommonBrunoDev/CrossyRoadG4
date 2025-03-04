using UnityEngine;

public class DeathHand : MonoBehaviour
{
    public GameObject hand;
    public Transform grabPoint;
    public float holdTimer;
    public int phase = 0;
    private float AnimationTimer = 0;
    public float moveSpeed = 1f;
    //-0.014
    private void Awake()
    {
        Vector3 pos = transform.position; pos.y = 0;
        transform.position = pos;
        hand.transform.position = new Vector3(hand.transform.position.x, -5, hand.transform.position.z);
    }

    private void Update()
    {
        switch (phase)
        {
            case 0:
                hand.transform.position += new Vector3(0, moveSpeed * 10 * Time.deltaTime, 0);
                if (hand.transform.position.y >= 0.25f) { phase++; AnimationTimer = holdTimer; }
                break;

            case 1:
                AnimationTimer -= Time.deltaTime;
                if (AnimationTimer < 0) { phase++; }
                break;

            case 2:
                hand.transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                if (hand.transform.position.y <= -5F) { phase++;}
                break;
        }
        Player.Instance.transform.position = grabPoint.position + new Vector3(0.7f,0, 0);
        Player.Instance.desiredPosition = grabPoint.position + new Vector3(0.7f, 0, 0);
        Player.Instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 88));
    }
}
