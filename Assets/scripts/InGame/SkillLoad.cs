using UnityEngine;
using System.Collections.Generic;
public class SkillLoad : MonoBehaviour
{
    public SkillButtonCreat _SkillButtonCreat;
    public PlayerHPManager _PlayerHPManager;
    void Awake()
    {
        _SkillButtonCreat = gameObject.GetComponent<SkillButtonCreat>();
        _PlayerHPManager = gameObject.GetComponent<PlayerHPManager>();
        if (gameObject.tag == "Holmes")
        {
            _PlayerHPManager.HP = GameData.HolmesHP;
            _PlayerHPManager.MaxHP = GameData.HolmesMaxHP;
            _SkillButtonCreat.HaveSkill = GameData.HolmesHaveSkill;
        }
        if (gameObject.tag == "Watson")
        {
            _PlayerHPManager.HP = GameData.WatsonHP;
            _PlayerHPManager.MaxHP = GameData.WatsonMaxHP;
            _SkillButtonCreat.HaveSkill = GameData.WatsonHaveSkill;
        }
    }
}
