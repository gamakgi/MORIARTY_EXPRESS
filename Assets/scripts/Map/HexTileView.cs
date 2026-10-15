using UnityEngine;

public class HexTileView : MonoBehaviour
{
    private HexTileData tile;
    private HexMapGenerator mapGenerator;
    private bool moveAvailable;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public void SetTile(HexTileData data, HexMapGenerator generator)
    {
        tile = data;
        mapGenerator = generator;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void SetMoveAvailable(bool available)
    {
        moveAvailable = available;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = available ? new Color(1f, 1f, 0.65f, 1f) : originalColor;
    }

    private void OnMouseDown()
    {
        if (tile == null || !moveAvailable || mapGenerator == null)
            return;

        MapLoadingScreen loadingScreen = FindFirstObjectByType<MapLoadingScreen>();
        if (loadingScreen == null || loadingScreen.IsTransitioning)
            return;

        mapGenerator.MovePlayer(tile);
    }

    public HexTileData GetTileData()
    {
        return tile;
    }
}
