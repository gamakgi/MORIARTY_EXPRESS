using UnityEngine;

// 기존 HexTileView에서 턴 중 클릭을 막고 Battle 씬으로 이동할 때 사용
public class MapLoadingScreen : MonoBehaviour
{
    public bool IsTransitioning
    {
        get { return SceneTransitionManager.IsTransitioning; }
    }

    public void LoadBattle()
    {
        SceneTransitionManager.LoadScene("Battle");
    }
}
