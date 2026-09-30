using System;
using UnityEngine;
using TMPro;

public class ManaManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ManaText;
    public int Mana;

    private void Start()
    {
        ManaTextChange();
    }

    public void ManaTextChange()
    {
        ManaText.text = Mana.ToString();
    }
}
