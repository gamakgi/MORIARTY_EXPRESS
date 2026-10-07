using UnityEngine;
public class EnemyDead : MonoBehaviour
{
    public int Alive_Enemy_Count;
    private PlayerHPManager _PlayerHPManager;


    public void IfEnemyDead()
    {
        Alive_Enemy_Count -= 1;
        if (Alive_Enemy_Count <= 0)
        {
                GameObject holmes = GameObject.FindWithTag("Holmes");
                _PlayerHPManager = holmes.GetComponent<PlayerHPManager>();
                GameData.HolmesHP = _PlayerHPManager.HP;
                GameData.HolmesMaxHP = _PlayerHPManager.MaxHP;
                GameObject watson = GameObject.FindWithTag("Watson");
                _PlayerHPManager = watson.GetComponent<PlayerHPManager>();
                GameData.WatsonHP = _PlayerHPManager.HP;
            GameData.WatsonMaxHP = _PlayerHPManager.MaxHP;
            SceneTransitionManager.LoadScene("Map");
        }
    }
}
