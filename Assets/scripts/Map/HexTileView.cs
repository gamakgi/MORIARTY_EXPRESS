using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Battle 타일을 클릭했을 때
        if (tile.type == TileType.Battle)
        {
            SceneManager.LoadScene("Battle");
        }
    }

    public HexTileData GetTileData()
    {
        return tile;
    }
}