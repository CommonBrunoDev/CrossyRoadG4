using UnityEngine;

public class TestSkipCamera : MonoBehaviour
{
    GameObject CameraController_ref;
    [SerializeField, Range(0f, 10f)] float RepositionTime;

    private void Start()
    {
        CameraController_ref = GameObject.FindGameObjectWithTag("CameraController");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 ActualPosition = transform.position;
            CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(ActualPosition, RepositionTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Relics\\CameraSkipTo.png",true, Color.red);
    }
#endif
}
