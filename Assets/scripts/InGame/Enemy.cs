using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Enemy", menuName = "EnemyManager/Enemy")]
public class Enemy : ScriptableObject
{
    public string EnemyName;
    public Sprite EnemySprite;
    public int EnemyHP;
    public int EnemyMana;
    public List<Skill> EnemyHaveSkill;
}
