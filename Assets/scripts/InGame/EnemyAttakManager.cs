using UnityEngine;
using System.Collections.Generic;

public class EnemyAttakManager : MonoBehaviour
{
    [SerializeField] private GameObject TurnEnemy;
    [SerializeField] private EnemySkillManager _EnemySkillManager;
    [SerializeField] private List<Skill> TurnEmemy_Have_skills =  new List<Skill>();
    [SerializeField] private Skill Use_Skill;
    [SerializeField] private EnemyManaManager _EnemyManaManager;
    [SerializeField] private TurnManager _TurnManager;
    public void TurnEnemyInput(GameObject EnemyObject)
    {
        TurnEnemy = EnemyObject;
        _EnemySkillManager = TurnEnemy.GetComponent<EnemySkillManager>();
        _EnemyManaManager = TurnEnemy.GetComponent<EnemyManaManager>();
        TurnEmemy_Have_skills.Clear();
        TurnEmemy_Have_skills.AddRange(_EnemySkillManager.HaveSkill);
        ChoseSkill();
    }

    void ChoseSkill()
    {
        List<Skill> remainingSkills = new List<Skill>(TurnEmemy_Have_skills);
        while (remainingSkills.Count > 0)
        {
            List<Skill> affordableSkills = new List<Skill>();
            foreach (Skill skill in remainingSkills)
            {
                if (skill.Cost <= _EnemyManaManager.Enemy_Have_Mana)
                {
                    affordableSkills.Add(skill);
                }
            }

            if (affordableSkills.Count == 0)
            {
                break;
            }

            Use_Skill = affordableSkills[Random.Range(0, affordableSkills.Count)];
            remainingSkills.Remove(Use_Skill);
            _EnemyManaManager.Enemy_Have_Mana -= Use_Skill.Cost;
            EnemyAttak(Use_Skill);
        }

        Use_Skill = null;
        _TurnManager.TurnChange();
    }

    private void EnemyAttak(Skill useSkill)
    {
        Debug.Log(useSkill.SkillName);
    }
}
