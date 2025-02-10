using UnityEngine;

public class Row : MonoBehaviour
{
    public RowType type = RowType.None;
    [HideInInspector] private Tile[] tiles = new Tile[9];
}
