using UnityEngine;

public class Car : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { other.gameObject.GetComponent<Player>().Squish(); }
    }
}
