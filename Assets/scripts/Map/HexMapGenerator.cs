using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Battle,
    EliteBattle,
    Shop,
    Event,
    Rest,
    Mystery,
    Boss,
    Null
}

[Serializable] public class TileWeight
{
    public TileType type;
    public float weight;
}

[Serializable]
public class HexTileData
{
    public Vector2Int coord;
    public TileType type;
    public GameObject tileObject;

    public HexTileData(Vector2Int coord, TileType type)
    {
        this.coord = coord;
        this.type = type;
    }
}

public class HexMapGenerator : MonoBehaviour
{
    // ========================================
    // 맵 설정
    // ========================================

    [Header("Map")]
    public int MapSize = 5;
    public int Recommended_number_of_turns = 20;
    [Min(0.01f)] public float NullTileTurnCost = 0.5f;

    [Header("Hex")]
    public float HexSize = 1.0f;
    public float TileSpacing = 0.1f;
    public float TileScale = 1.0f;


    // ========================================
    // 타일 프리팹
    // ========================================

    [Header("Tile Prefabs")]
    public GameObject BattlePrefab;
    public GameObject EliteBattlePrefab;
    public GameObject ShopPrefab;
    public GameObject EventPrefab;
    public GameObject RestPrefab;
    public GameObject TresurePrefab;
    public GameObject BossPrefab;
    public GameObject NullPrefab;
    public GameObject PlayerSpritePrefab;


    // ========================================
    // 타일 확률
    // ========================================

    [Header("Tile Weights")]
    public List<TileWeight> TileWeights = new List<TileWeight>()
    {
        new TileWeight()
        {
            type = TileType.Battle,
            weight = 5
        },

        new TileWeight()
        {
            type = TileType.EliteBattle,
            weight = 2
        },

        new TileWeight()
        {
            type = TileType.Shop,
            weight = 1.5f
        },

        new TileWeight()
        {
            type = TileType.Event,
            weight = 3
        },

        new TileWeight()
        {
            type = TileType.Rest,
            weight = 2
        },

        new TileWeight()
        {
            type = TileType.Mystery,
            weight = 1.5f
        }
    };


    // ========================================
    // 랜덤
    // ========================================

    [Header("Random")]
    public int Seed = 12345;
    public bool RandomSeed = false;


    // ========================================
    // 수리
    // ========================================

    [Header("Repair")]
    public int MaxRepairCount = 50;


    // ========================================
    // 시작 설정
    // ========================================

    [Header("Generate")]
    public bool GenerateOnStart = true;


    // ========================================
    // 타일 부모
    // ========================================

    [Header("Hierarchy")]
    public Transform TileParent;


    // ========================================
    // 타일 데이터
    // ========================================

    public Dictionary<Vector2Int, HexTileData> Tiles =
        new Dictionary<Vector2Int, HexTileData>();

    private Vector2Int playerCoord;
    private GameObject playerMarker;
    private GameData gameData;
    private MapTurnCounter mapTurnCounter;
    private float turnCount;


    private System.Random random;


    // ========================================
    // 시작
    // ========================================

    private void Start()
    {
        if (GenerateOnStart)
        {
            GenerateMap();
        }
    }


    // ========================================
    // 맵 생성
    // ========================================

    public void GenerateMap()
    {
        ClearMap();

        if (TileParent == null)
        {
            TileParent = transform;
        }

        if (RandomSeed)
        {
            Seed = UnityEngine.Random.Range(-100000, 100000);
        }

        random = new System.Random(Seed);
        gameData = FindFirstObjectByType<GameData>();
        mapTurnCounter = GetComponent<MapTurnCounter>();
        if (mapTurnCounter == null)
            mapTurnCounter = FindFirstObjectByType<MapTurnCounter>();

        if (gameData != null && gameData.HasSavedMap)
        {
            foreach (HexTileData savedTile in gameData.SavedMapTiles)
                Tiles.Add(savedTile.coord, new HexTileData(savedTile.coord, savedTile.type));
            playerCoord = gameData.SavedMapPlayerCoord;
            turnCount = gameData.SavedMapTurnCount;
        }
        else
        {
            for (int x = -MapSize; x <= MapSize; x++)
            {
                for (int y = -MapSize; y <= MapSize; y++)
                {
                    if (Mathf.Abs(x + y) > MapSize)
                        continue;
                    Vector2Int coord = new Vector2Int(x, y);
                    Tiles.Add(coord, new HexTileData(coord, GetRandomTileType()));
                }
            }
            RepairTiles();
            SetRandomBossTile();
            playerCoord = Vector2Int.zero;
            Tiles[playerCoord].type = TileType.Null;
            turnCount = 0;
        }

        if (mapTurnCounter != null)
            mapTurnCounter.SetTurn(turnCount, Recommended_number_of_turns);
        CreateTiles();
        CreatePlayerMarker();
        UpdateAvailableMoves();
        SaveMapRun();
        Debug.Log("Hex Map 생성 완료 / 타일 수 : " + Tiles.Count);
    }


    // ========================================
    // 랜덤 타일 종류
    // ========================================

    private TileType GetRandomTileType()
    {
        float totalWeight = 0f;

        for (int i = 0; i < TileWeights.Count; i++)
        {
            if (TileWeights[i].weight > 0)
            {
                totalWeight += TileWeights[i].weight;
            }
        }

        if (totalWeight <= 0f)
        {
            return TileType.Battle;
        }

        float randomValue =
            (float)random.NextDouble() * totalWeight;

        float currentWeight = 0f;

        for (int i = 0; i < TileWeights.Count; i++)
        {
            if (TileWeights[i].weight <= 0)
            {
                continue;
            }

            currentWeight += TileWeights[i].weight;

            if (randomValue <= currentWeight)
            {
                return TileWeights[i].type;
            }
        }

        return TileType.Battle;
    }


    // ========================================
    // Boss 배치
    // ========================================

    private void SetRandomBossTile()
    {
        List<HexTileData> edgeTiles =
            new List<HexTileData>();

        foreach (HexTileData tile in Tiles.Values)
        {
            int x = tile.coord.x;
            int y = tile.coord.y;

            if (
                Mathf.Abs(x) == MapSize ||
                Mathf.Abs(y) == MapSize ||
                Mathf.Abs(x + y) == MapSize
            )
            {
                edgeTiles.Add(tile);
            }
        }

        if (edgeTiles.Count == 0)
        {
            Debug.LogWarning(
                "Boss를 배치할 외곽 타일이 없습니다."
            );

            return;
        }

        int randomNumber =
            random.Next(0, edgeTiles.Count);

        HexTileData bossTile =
            edgeTiles[randomNumber];

        bossTile.type = TileType.Boss;

        Debug.Log(
            "Boss 위치 : (" +
            bossTile.coord.x +
            ", " +
            bossTile.coord.y +
            ")"
        );
    }


    // ========================================
    // 타일 수리
    // ========================================

    private void RepairTiles()
    {
        for (
            int count = 0;
            count < MaxRepairCount;
            count++
        )
        {
            bool changed = false;

            foreach (HexTileData tile in Tiles.Values)
            {
                if (tile.type == TileType.Boss)
                {
                    continue;
                }

                List<HexTileData> neighbors =
                    GetNeighbors(tile.coord);

                if (neighbors.Count == 0)
                {
                    continue;
                }

                TileType repeatedType = neighbors[0].type;
                List<HexTileData> repeatedNeighbors = new List<HexTileData>();

                for (int i = 0; i < neighbors.Count; i++)
                {
                    List<HexTileData> sameTypeNeighbors = new List<HexTileData>();

                    for (int j = 0; j < neighbors.Count; j++)
                    {
                        if (neighbors[j].type == neighbors[i].type)
                        {
                            sameTypeNeighbors.Add(neighbors[j]);
                        }
                    }

                    if (sameTypeNeighbors.Count >= 5)
                    {
                        repeatedType = neighbors[i].type;
                        repeatedNeighbors = sameTypeNeighbors;
                        break;
                    }
                }

                if (repeatedNeighbors.Count >= 5)
                {
                    TileType newType = GetOtherTileType(repeatedType);

                    if (newType != repeatedType)
                    {
                        int randomNumber = random.Next(0, repeatedNeighbors.Count);
                        HexTileData target = repeatedNeighbors[randomNumber];
                        target.type = newType;
                        changed = true;
                    }
                }
            }

            if (!changed)
            {
                break;
            }
        }
    }


    // ========================================
    // 다른 타일 종류
    // ========================================

    private TileType GetOtherTileType(TileType oldType)
    {
        List<TileType> possibleTypes =
            new List<TileType>();

        for (int i = 0; i < TileWeights.Count; i++)
        {
            if (
                TileWeights[i].weight > 0 &&
                TileWeights[i].type != oldType &&
                TileWeights[i].type != TileType.Boss
            )
            {
                possibleTypes.Add(
                    TileWeights[i].type
                );
            }
        }

        if (possibleTypes.Count == 0)
        {
            return oldType;
        }

        int randomNumber =
            random.Next(
                0,
                possibleTypes.Count
            );

        return possibleTypes[randomNumber];
    }


    // ========================================
    // 주변 타일
    // ========================================

    public List<HexTileData> GetNeighbors(
        Vector2Int coord
    )
    {
        List<HexTileData> neighbors =
            new List<HexTileData>();

        Vector2Int[] directions =
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1)
        };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborCoord =
                coord + directions[i];

            if (Tiles.ContainsKey(neighborCoord))
            {
                neighbors.Add(
                    Tiles[neighborCoord]
                );
            }
        }

        return neighbors;
    }


    // ========================================
    // 타일 전체 생성
    // ========================================

    private void CreateTiles()
    {
        if (TileParent == null)
        {
            TileParent = transform;
        }

        foreach (HexTileData tile in Tiles.Values)
        {
            CreateOneTile(tile);
        }
    }


    // ========================================
    // 타일 하나 생성
    // ========================================

    private void CreateOneTile(HexTileData tile)
    {
        GameObject prefab =
            GetPrefab(tile.type);

        if (prefab == null)
        {
            Debug.LogWarning(
                tile.type +
                " 프리팹이 없습니다."
            );

            return;
        }


        // ====================================
        // Flat-Top Hex 좌표
        // ====================================

        float horizontal =
            HexSize *
            1.5f;

        float vertical =
            HexSize *
            Mathf.Sqrt(3f);


        float x =
            tile.coord.x *
            horizontal;

        float y =
            (
                tile.coord.y +
                tile.coord.x * 0.5f
            ) *
            vertical;


        // ====================================
        // 타일 간격
        // ====================================

        float spacing =
            1f + TileSpacing;

        x *= spacing;
        y *= spacing;


        Vector3 localPosition =
            new Vector3(
                x,
                y,
                0f
            );


        // ====================================
        // 생성
        // ====================================

        GameObject tileObject =
            Instantiate(
                prefab,
                TileParent
            );


        // 부모 기준 위치로 지정
        tileObject.transform.localPosition =
            localPosition;

        tileObject.transform.localRotation =
            Quaternion.identity;

        tileObject.transform.localScale =
            Vector3.one * TileScale;


        // ====================================
        // 이름
        // ====================================

        tileObject.name =
            "Tile_" +
            tile.coord.x +
            "_" +
            tile.coord.y +
            "_" +
            tile.type;


        // ====================================
        // HexTileView
        // ====================================

        HexTileView tileView =
            tileObject.GetComponent<HexTileView>();

        if (tileView == null)
        {
            tileView =
                tileObject.AddComponent<HexTileView>();
        }

        tileView.SetTile(tile, this);


        // 데이터에 오브젝트 저장
        tile.tileObject =
            tileObject;


        // ====================================
        // 위치 확인용 로그
        // ====================================

        Debug.Log(
            "Tile " +
            tile.coord +
            " / Position = " +
            localPosition
        );
    }


    // ========================================
    // 프리팹 가져오기
    // ========================================

    private GameObject GetPrefab(TileType type)
    {
        switch (type)
        {
            case TileType.Battle:
                return BattlePrefab;

            case TileType.EliteBattle:
                return EliteBattlePrefab;

            case TileType.Shop:
                return ShopPrefab;

            case TileType.Event:
                return EventPrefab;

            case TileType.Rest:
                return RestPrefab;

            case TileType.Mystery:
                return TresurePrefab;

            case TileType.Boss:
                return BossPrefab;

            case TileType.Null:
                return NullPrefab;
        }

        return null;
    }

    private Vector3 GetTileLocalPosition(Vector2Int coord)
    {
        float horizontal = HexSize * 1.5f;
        float vertical = HexSize * Mathf.Sqrt(3f);
        float spacing = 1f + TileSpacing;
        return new Vector3(coord.x * horizontal * spacing,
            (coord.y + coord.x * 0.5f) * vertical * spacing, 0f);
    }

    private void CreatePlayerMarker()
    {
        if (playerMarker != null)
            Destroy(playerMarker);

        playerMarker = new GameObject("MapPlayer");
        playerMarker.transform.SetParent(TileParent, false);
        playerMarker.transform.localPosition = GetTileLocalPosition(playerCoord) + new Vector3(0f, 0f, -0.1f);
        playerMarker.transform.localScale = Vector3.one * TileScale / 3f;

        SpriteRenderer playerRenderer = playerMarker.AddComponent<SpriteRenderer>();
        SpriteRenderer sourceRenderer = PlayerSpritePrefab != null ? PlayerSpritePrefab.GetComponent<SpriteRenderer>() : null;
        if (sourceRenderer != null)
        {
            playerRenderer.sprite = sourceRenderer.sprite;
            playerRenderer.color = sourceRenderer.color;
            playerRenderer.flipX = sourceRenderer.flipX;
        }
        playerRenderer.sortingOrder = 10;
    }

    public void MovePlayer(HexTileData destination)
    {
        if (destination == null)
            return;

        bool isNeighbor = false;
        List<HexTileData> neighbors = GetNeighbors(playerCoord);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] == destination)
            {
                isNeighbor = true;
                break;
            }
        }
        if (!isNeighbor)
            return;

        TileType destinationType = destination.type;
        Debug.Log("밟은 타일 종류: " + destinationType);
        playerCoord = destination.coord;
        if (destinationType == TileType.Null)
            turnCount += NullTileTurnCost;
        else
            turnCount += 1f;
        if (mapTurnCounter != null)
            mapTurnCounter.SetTurn(turnCount, Recommended_number_of_turns);
        ChangeTileToNull(destination);
        playerMarker.transform.localPosition = GetTileLocalPosition(playerCoord) + new Vector3(0f, 0f, -0.1f);
        UpdateAvailableMoves();
        SaveMapRun();

        if (destinationType == TileType.Battle)
            SceneTransitionManager.LoadScene("Battle");
        if (destinationType == TileType.Mystery)
            SceneTransitionManager.LoadScene("Mystery");
        //여기에여
        
    }

    private void ChangeTileToNull(HexTileData tile)
    {
        tile.type = TileType.Null;
        if (tile.tileObject != null && NullPrefab != null)
        {
            SpriteRenderer currentRenderer = tile.tileObject.GetComponent<SpriteRenderer>();
            SpriteRenderer nullRenderer = NullPrefab.GetComponent<SpriteRenderer>();
            if (currentRenderer != null && nullRenderer != null)
            {
                currentRenderer.sprite = nullRenderer.sprite;
                currentRenderer.color = nullRenderer.color;
            }
            for (int i = 0; i < tile.tileObject.transform.childCount; i++)
                tile.tileObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        HexTileView view = tile.tileObject != null ? tile.tileObject.GetComponent<HexTileView>() : null;
        if (view != null)
            view.SetTile(tile, this);
    }

    private void UpdateAvailableMoves()
    {
        foreach (HexTileData tile in Tiles.Values)
        {
            HexTileView view = tile.tileObject != null ? tile.tileObject.GetComponent<HexTileView>() : null;
            if (view != null)
                view.SetMoveAvailable(false);
        }
        List<HexTileData> neighbors = GetNeighbors(playerCoord);
        for (int i = 0; i < neighbors.Count; i++)
        {
            HexTileView view = neighbors[i].tileObject != null ? neighbors[i].tileObject.GetComponent<HexTileView>() : null;
            if (view != null)
                view.SetMoveAvailable(true);
        }
    }

    private void SaveMapRun()
    {
        if (gameData != null)
            gameData.SaveMapRun(Tiles, playerCoord, turnCount);
    }


    // ========================================
    // 맵 삭제
    // ========================================

    public void ClearMap()
    {
        if (TileParent == null)
        {
            TileParent = transform;
        }

        for (
            int i = TileParent.childCount - 1;
            i >= 0;
            i--
        )
        {
            GameObject child =
                TileParent.GetChild(i).gameObject;

            DestroyImmediate(child);
        }

        Tiles.Clear();
    }


    // ========================================
    // 맵 재생성
    // ========================================

    public void RegenerateMap()
    {
        GenerateMap();
    }


    // ========================================
    // 맵 정보
    // ========================================

    public void PrintMapInfo()
    {
        Debug.Log(
            "현재 타일 개수 : " +
            Tiles.Count
        );

        int battle = 0;
        int elite = 0;
        int shop = 0;
        int eventCount = 0;
        int rest = 0;
        int mystery = 0;
        int boss = 0;

        foreach (HexTileData tile in Tiles.Values)
        {
            switch (tile.type)
            {
                case TileType.Battle:
                    battle++;
                    break;

                case TileType.EliteBattle:
                    elite++;
                    break;

                case TileType.Shop:
                    shop++;
                    break;

                case TileType.Event:
                    eventCount++;
                    break;

                case TileType.Rest:
                    rest++;
                    break;

                case TileType.Mystery:
                    mystery++;
                    break;

                case TileType.Boss:
                    boss++;
                    break;
            }
        }

        Debug.Log("Battle : " + battle);
        Debug.Log("EliteBattle : " + elite);
        Debug.Log("Shop : " + shop);
        Debug.Log("Event : " + eventCount);
        Debug.Log("Rest : " + rest);
        Debug.Log("Mystery : " + mystery);
        Debug.Log("Boss : " + boss);
    }
}
