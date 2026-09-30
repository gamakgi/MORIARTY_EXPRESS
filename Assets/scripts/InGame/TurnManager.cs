using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject NowTurn;
    private RectTransform Canvas;
    [SerializeField] private int turn = 1;
    private SkillButtonCreat _SkillButtonCreat;
    [SerializeField] private GameObject TurnendButton;
    //AllyTurn이 true면 아군턴, false면 적군턴
    public bool AllyTurn;
    public List<GameObject> Turn_order = new List<GameObject>();
    public void Input_Enemt_Turn(GameObject Enemy)
    {
        Turn_order.Add(Enemy);
    }
    public void TurnStart(GameObject nowturn)
    {
        TurnendButton.SetActive(true);
        NowTurn = nowturn;
        _SkillButtonCreat = nowturn.GetComponent<SkillButtonCreat>();
        Canvas = _SkillButtonCreat.buttonParent;
        if (_SkillButtonCreat)
        {
            _SkillButtonCreat.SkillCreate();
        }
        else
        {
            Canvas.gameObject.SetActive(true);
        }
        
    }

    public void TurnEnd()
    {
        Canvas.gameObject.SetActive(false);
        TurnChange();
    }

    void TurnChange()
    {
        NowTurn = Turn_order[turn-1 % Turn_order.Count];
        turn += 1;
        if (NowTurn.tag == "Holmes" || NowTurn.tag == "Watson")
        {
            TurnStart(NowTurn);
        }
        else
        {
            TurnendButton.SetActive(false);
        }
    }
}