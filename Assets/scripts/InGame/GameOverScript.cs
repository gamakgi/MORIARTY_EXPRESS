using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScript : MonoBehaviour
{
    static public void Game_OVER()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
