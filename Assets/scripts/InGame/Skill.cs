using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillManager/Skill")]
public class Skill : ScriptableObject
{
    public string SkillName;
    public Sprite SkillIcon;
    public int Damage;
    public int Cost;
}
