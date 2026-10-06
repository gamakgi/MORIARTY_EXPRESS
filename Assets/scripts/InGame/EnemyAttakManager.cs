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
    [SerializeField] private GameObject Holmes;
    [SerializeField] private GameObject Watson;
    [SerializeField] private PlayerHPManager _playerHPManager;
    public void HolmesObjectInput(GameObject holmes)
    { 
        Holmes = holmes;
    }
    public void WatsonObjectInput(GameObject watson)
    { 
        Watson = watson;
    }
    
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
        // 0 = 홈즈, 왓슨  |  1 = 홈즈  |  2 = 왓슨
        int random_targeting = 0;
        if (Watson == null)
        {
            random_targeting =  1;
        }
        if (Holmes == null)
        {
            random_targeting =  2;
        }

        if (random_targeting == 0)
        {
            int _Watson =  Random.Range(0, 2);
            switch (_Watson)
            {
                case 0:
                    _playerHPManager = Holmes.GetComponent<PlayerHPManager>();
                    _playerHPManager.HP_change(useSkill.Damage);
                    break;
                case 1:
                    _playerHPManager = Watson.GetComponent<PlayerHPManager>();
                    _playerHPManager.HP_change(useSkill.Damage);
                    break;
            }
        }
        else if (random_targeting == 1)
        {
            _playerHPManager = Holmes.GetComponent<PlayerHPManager>();
            _playerHPManager.HP_change(useSkill.Damage);
        }
        else if (random_targeting == 2)
        {
            _playerHPManager = Watson.GetComponent<PlayerHPManager>();
            _playerHPManager.HP_change(useSkill.Damage);
        }
    }
}
