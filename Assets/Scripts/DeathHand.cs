using UnityEngine;

public class DeathHand : MonoBehaviour
{
    public GameObject hand;
    public Transform grabPoint;
    public float holdTimer;
    public int phase = 0;
    private float AnimationTimer = 0;
    //-0.014
    private void Awake()
    {
        hand.transform.position = new Vector3(hand.transform.position.x, -0.026f, hand.transform.position.z);
    }

    private void Update()
    {
        switch (phase)
        {
            case 0:
                hand.transform.position += new Vector3(0, 0.1f * Time.deltaTime, 0);
                if (hand.transform.position.y >= -0.014f) { phase++; AnimationTimer = holdTimer; }
                break;

            case 1:
                AnimationTimer -= Time.deltaTime;
                if (AnimationTimer < 0) { phase++; }
                break;

            case 2:
                hand.transform.position -= new Vector3(0, 0.1f * Time.deltaTime, 0);
                if (hand.transform.position.y <= -0.034f) { phase++;}
                break;
        }
        Player.Instance.transform.position = grabPoint.position;
        Player.Instance.desiredPosition = grabPoint.position;
    }
}
