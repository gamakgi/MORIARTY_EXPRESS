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
    [SerializeField] private EnemyAttakManager _EnemyAttakManager;
    private EnemyManaManager _EnemyManaManager;
    private ManaManager _ManaManager;
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

        Canvas.gameObject.SetActive(true);
        
    }

    public void TurnEnd()
    {
        Canvas.gameObject.SetActive(false);
        TurnChange();
    }

    public void TurnChange()
    {
        for (int i = Turn_order.Count - 1; i >= 0; i--)
        {
            if (Turn_order[i] == null)
            {
                Turn_order.RemoveAt(i);
            }
        }

        if (Turn_order.Count == 0)
        {
            return;
        }

        NowTurn = Turn_order[(turn - 1) % Turn_order.Count];
        turn += 1;
        if (NowTurn.tag == "Holmes" || NowTurn.tag == "Watson")
        {
            TurnStart(NowTurn);
            _ManaManager = NowTurn.GetComponent<ManaManager>();
            _ManaManager.Mana = 3;
            _ManaManager.ManaTextChange();
        }
        else
        {
            if (TurnendButton.activeSelf)
            {
                TurnendButton.SetActive(false);
            }
            _EnemyManaManager = NowTurn.GetComponent<EnemyManaManager>();
            _EnemyManaManager.Enemy_Have_Mana = 3;
            _EnemyAttakManager.TurnEnemyInput(NowTurn);
        }
    }
}
