using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class SkillButtonCreat : MonoBehaviour
{
    public List<Skill> HaveSkill;
    public GameObject _Skillbutton;
    [SerializeField] private Vector2 spawnPosition;
    public RectTransform buttonParent;
    public bool ifSkillCreat = true;
    public void SkillCreate()
    {
        int Skillcount = 0;

        foreach (Skill skill in HaveSkill)
        {
            GameObject instance = Instantiate(_Skillbutton, buttonParent);
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.anchoredPosition = spawnPosition + new Vector2(Skillcount * 70f + 50, 50);
            Skillbutton button = instance.GetComponent<Skillbutton>();
            button.SkillPush(skill);
            Skillcount++;
        }
        ifSkillCreat =  false;
    }
}

