using UnityEngine;

public class Coin : MonoBehaviour
{
    GM GMComponent_ref;
    private void Start()
    {
        GMComponent_ref = GameObject.FindGameObjectWithTag("GM").GetComponent<GM>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GMComponent_ref.IncreasePlayerCoins();
        Destroy(gameObject);
    }
}