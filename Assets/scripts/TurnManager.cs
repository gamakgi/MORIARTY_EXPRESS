using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject NowTurn;
    //AllyTurn이 true면 아군턴, false면 적군턴.
    public bool AllyTurn;

    public void TurnEnd()
    {
        NowTurn = transform.Find("NowTurn").gameObject;
        AllyTurn = false;
    }
}