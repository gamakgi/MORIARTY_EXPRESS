using UnityEngine;

public class EnemyDataLoad : MonoBehaviour
{
    private Sprite EnemySprite1;
    private SpriteRenderer _SpriteRenderer;
    private HPscripts _HPscripts;
    private EnemySkillManager _EnemtSkillManager;
    private EnemyManaManager _EnemyManaManager;

    public void Load (Enemy EnemyData)
    {
        _SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = EnemyData.EnemySprite;
        _HPscripts = gameObject.GetComponent<HPscripts>();
        _HPscripts.MaxHP = EnemyData.EnemyHP;
        _HPscripts.HP = EnemyData.EnemyHP;
        _EnemtSkillManager = gameObject.GetComponent<EnemySkillManager>();
        _EnemtSkillManager.HaveSkill.AddRange(EnemyData.EnemyHaveSkill);
        _EnemyManaManager = gameObject.GetComponent<EnemyManaManager>();
        _EnemyManaManager.Enemy_Have_Mana = EnemyData.EnemyMana;
    }
}