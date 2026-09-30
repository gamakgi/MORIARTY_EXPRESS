using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skillbutton : MonoBehaviour
{
    private AttakManager _attakManager;
    private GameObject GameController;
    public ManaManager _ManaManager;
    public int SKillCost;
    private int SkillDamage;
    
    void Start()
    {
        GameController = GameObject.FindWithTag("Attak_Manager");  
        _attakManager = GameController.GetComponent<AttakManager>();
        ManaManager MM = GetComponentInParent<ManaManager>();
        _ManaManager = MM;
    }
    public void SkillPush(Skill skillData)
    {
        SKillCost = skillData.Cost;
        SkillDamage =  skillData.Damage;
    }
    public void Click()
    {
        if (_attakManager.Target != null)
        {
            if (_ManaManager.Mana >= SKillCost)
            {
                _ManaManager.Mana -= SKillCost;
                _ManaManager.ManaTextChange();
                _attakManager.Attak(SkillDamage, 0);
            }
            else
            {
                Debug.LogError("마나 없어여...뜌");
            }
        }
    }
    
}
