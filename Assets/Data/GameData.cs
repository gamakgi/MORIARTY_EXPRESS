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
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
