using UnityEngine;
public class SceneChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoMap()
    {
        GameData gameData = FindFirstObjectByType<GameData>();
        if (gameData != null)
        {
            gameData.ClearMapRun();
        }

        SceneTransitionManager.LoadScene("Map");
    }
}
