using UnityEngine;

public class TestSkipCamera : MonoBehaviour
{
    GameObject CameraController_ref;
    [SerializeField, Range(0f, 10f)] float RepositionTime;

    private void Awake()
    {
        CameraController_ref = GameObject.FindGameObjectWithTag("CameraController");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraController_ref.GetComponent<I_CameraReaction>().CameraReaction(transform, RepositionTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawIcon(transform.position, "Relics\\CameraSkipTo.png");
    }
#endif
}
