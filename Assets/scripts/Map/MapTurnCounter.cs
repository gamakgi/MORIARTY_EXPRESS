using System.Globalization;
using TMPro;
using UnityEngine;

public class MapTurnCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;

    public void SetTurn(float turnCount, int recommendedNumberOfTurns)
    {
        if (turnText != null)
        {
            string formattedTurn = turnCount.ToString("0.##", CultureInfo.InvariantCulture);
            turnText.text = formattedTurn + " / " + recommendedNumberOfTurns;
        }
    }

}
