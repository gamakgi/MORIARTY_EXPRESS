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

        MapLoadingScreen loadingScreen =
            FindFirstObjectByType<MapLoadingScreen>();

        if (loadingScreen == null || loadingScreen.IsTransitioning)
        {
            return;
        }

        // Battle 타일을 클릭했을 때
        if (tile.type == TileType.Battle)
        {
            loadingScreen.LoadBattle();
        }
    }

    public HexTileData GetTileData()
    {
        return tile;
    }
}
