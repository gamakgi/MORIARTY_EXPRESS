using UnityEngine;
using System.Collections.Generic;
public class StartSkillLoad : MonoBehaviour
{
    [SerializeField] int StartHolmesHP;
    [SerializeField] int StartHolmesMaxHP;
    [SerializeField] int StartWatsonHP;
    [SerializeField] int StartWatsonMaxHP;
    [SerializeField] List<Skill> StartHolmesHaveSkill;
    [SerializeField] List<Skill> StartWatsonHaveSkill;
    void Start()
    {
        GameData.HolmesHP = StartHolmesHP;
        GameData.HolmesMaxHP = StartHolmesMaxHP;
        GameData.WatsonHP = StartWatsonHP;
        GameData.WatsonMaxHP = StartWatsonMaxHP;
        GameData.HolmesHaveSkill = StartHolmesHaveSkill;
        GameData.WatsonHaveSkill = StartWatsonHaveSkill;
    }
}
