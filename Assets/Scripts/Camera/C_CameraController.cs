//using Unity.VisualScripting;
using UnityEngine;

public class C_CameraController : MonoBehaviour, I_CameraReaction
{
    enum CameraState {MovingForward, SkipToPosition }
    CameraState CameraStateType;


    private void Awake()
    {
        StartPosition = transform.position;
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

    void CameraMovingForward()
    {
        //Funzione che permette alla camera di muoversi continuamente in una direzione con una determinata velocità.
        transform.Translate(CameraDirection.normalized * CameraSpeed * Time.deltaTime,Space.World);
    }

    void CameraSkipToPosition()
    {
        //Funzione che permette di muovere la camera in un punto specifico nel tempo desiderato.
        if (transform.position != CameraDestination.position)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(StartPosition, CameraDestination.position, Timer / CameraRepositionTime);
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
    Transform CameraDestination;
    Vector3 StartPosition;
    float CameraRepositionTime;

    void I_CameraReaction.CameraReaction(Transform Destination, float RepositionTime)
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
