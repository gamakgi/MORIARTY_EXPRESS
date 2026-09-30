using UnityEngine;
using System.Collections.Generic;
public class GameData : MonoBehaviour
{
    public int HolmesHP;
    public int HolmesMaxHP;
    public int watsonHP;
    public int watsonMaxHP;
    public List<Skill> HaveSkill;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
