using UnityEngine;

public class Coin : MonoBehaviour
{
    GM GMComponent_ref;
    private void Start()
    {
        GMComponent_ref = GameObject.FindGameObjectWithTag("GM").GetComponent<GM>();
    }

    private void Update()
    {
        transform.Rotate(0, 2f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        GMComponent_ref.IncreasePlayerCoins();
        Destroy(gameObject);
    }
}