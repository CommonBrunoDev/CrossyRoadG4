using UnityEngine;

public class NightmareMesher : MonoBehaviour
{
    public GameObject meshNormal;
    public GameObject meshNightmare;

    public void Start()
    {
        if (GM.Instance.nightmareMode)
        { meshNormal.SetActive(false); }
        else
        { meshNightmare.SetActive(false); }
    }

    public void SetMesh(bool nightmare)
    {
        meshNormal.SetActive(!nightmare);
        meshNightmare.SetActive(nightmare);
    }
}
