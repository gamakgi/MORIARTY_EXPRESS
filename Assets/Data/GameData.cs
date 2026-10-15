using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static int HolmesHP;
    public static int HolmesMaxHP;
    public static int WatsonHP;
    public static int WatsonMaxHP;
    public static List<Skill> HolmesHaveSkill;
    public static List<Skill> WatsonHaveSkill;

    public static List<MysteryType> MysteryList;

    // 씬을 바꿔도 이어지는 현재 육각 맵 정보
    public List<HexTileData> SavedMapTiles = new List<HexTileData>();
    public Vector2Int SavedMapPlayerCoord;
    public float SavedMapTurnCount;
    public bool HasSavedMap;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveMapRun(
        Dictionary<Vector2Int, HexTileData> tiles,
        Vector2Int playerCoord,
        float turnCount)
    {
        SavedMapTiles.Clear();

        foreach (HexTileData tile in tiles.Values)
        {
            SavedMapTiles.Add(
                new HexTileData(tile.coord, tile.type)
            );
        }

        SavedMapPlayerCoord = playerCoord;
        SavedMapTurnCount = turnCount;
        HasSavedMap = true;
    }

    public void ClearMapRun()
    {
        SavedMapTiles.Clear();
        SavedMapPlayerCoord = Vector2Int.zero;
        SavedMapTurnCount = 0;
        HasSavedMap = false;
    }
}
