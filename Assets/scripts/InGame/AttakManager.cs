using UnityEngine;

public class AttakManager : MonoBehaviour
{
    public GameObject Target;
    public TargetSet _TargetSet;
    [SerializeField] private HPscripts _enemyHP;
    public void Attak(int Damage, int SkillType)
    {
        _enemyHP = Target.GetComponent<HPscripts>();
        //난수
        _enemyHP.HP -= Random.Range(Damage-3, Damage+3);
        _enemyHP.HP_change();
    }

    public void TargetOff()
    {
        _TargetSet.Off();
    }
    
}
