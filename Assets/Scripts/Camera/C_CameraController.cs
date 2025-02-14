using System;
using UnityEngine;

//Ideal camera position: X = 5 / Y = 9 / Xrot = 40 / Yrot = -25 / Ortographic camera size = 8
public class C_CameraController : MonoBehaviour, I_CameraReaction
{
    [Serializable]
    enum CameraState {MovingForward, SkipToPosition}
    [SerializeField] CameraState CameraStateType;

    [SerializeField] Transform PlayerTransform_ref;

    private void Awake()
    {
        StartPosition = transform.position;
        ActualCameraPosition = transform.position;
        StartCameraSpeed = CameraSpeed;
    }

    void LateUpdate()
    {
        if (CameraStateType == CameraState.MovingForward)
        {
            CameraMovingForward();
        }
        else if (CameraStateType == CameraState.SkipToPosition)
        {
            CameraSkipToPosition();
        }

    }

    #region CameraStateTypesFunctions
    //Qui sono definite le funzioni che modificano il comportamento della camera in base allo stato.
    [SerializeField] Vector3 CameraDirection;
    [SerializeField, Range(0f, 100f)] float CameraSpeed;
    float StartCameraSpeed;
    [SerializeField] float ModifiedCameraSpeed;
    [SerializeField] float MaxCameraDistance;
    Vector3 ActualCameraPosition;

    void CameraMovingForward()
    {
        //Funzione che permette alla camera di muoversi continuamente in una direzione con una determinata velocità.
        transform.Translate(CameraDirection.normalized * CameraSpeed * Time.deltaTime, Space.World);
        if (PlayerTransform_ref.position.z - ActualCameraPosition.z  > MaxCameraDistance)
        {
            ActualCameraPosition = transform.position;
            CameraSpeed = ModifiedCameraSpeed;
        }
        else
        {
            ActualCameraPosition = transform.position;
            CameraSpeed = StartCameraSpeed;
        }
    }

    void CameraSkipToPosition()
    {
        //Funzione che permette di muovere la camera in un punto specifico nel tempo desiderato.
        if (transform.position.x != CameraDestination.x)
        {

            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(StartPosition, new Vector3(CameraDestination.x, transform.position.y, ActualCameraPosition.z), Timer / CameraRepositionTime);
            if (PlayerTransform_ref.position.z - ActualCameraPosition.z > MaxCameraDistance)
            {
                CameraSpeed = ModifiedCameraSpeed;
            }
            else
            {
                CameraSpeed = StartCameraSpeed;
            }
            ActualCameraPosition.z += 1 * CameraSpeed * Time.deltaTime;
        }
        else
        {
            Timer = 0f;
            CameraStateType = CameraState.MovingForward;
        }
    }
    #endregion

    #region CameraReactionInterface
    //Definizione della funzione lanciabile tramite interfaccia da altri script che vogliono controllare la camera.
    float Timer = 0f;
    Vector3 CameraDestination;
    Vector3 StartPosition;
    float CameraRepositionTime;

    public void CameraReaction(Vector3 Destination, float RepositionTime)
    {
        StartPosition = transform.position;
        CameraDestination = Destination;
        CameraRepositionTime = RepositionTime;
        Timer = 0;
        CameraStateType = CameraState.SkipToPosition;
    }
    #endregion

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Relics\\CameraObj_Gizmo.png");
    }
#endif
    #endregion
}
