using UnityEngine;
public class SceneChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoMap()
    {
        SceneTransitionManager.LoadScene("Map");
    }
}
