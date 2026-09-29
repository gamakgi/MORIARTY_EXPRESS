using UnityEngine;

public class HexTileView : MonoBehaviour
{
    private HexTileData tile;

    public void SetTile(HexTileData data)
    {
        tile = data;
    }

    private void OnMouseDown()
    {
        if (tile == null)
        {
            return;
        }
    }

    public HexTileData GetTileData()
    {
        return tile;
    }
}