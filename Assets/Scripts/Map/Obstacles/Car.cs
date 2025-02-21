using UnityEngine;

public class Car : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {collision.gameObject.GetComponent<Player>().Squish(); }
    }
}
