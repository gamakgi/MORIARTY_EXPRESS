using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MysteryDataLoad : MonoBehaviour
{
    [SerializeField] private GameObject MysteryImage;
    [SerializeField] private TMP_Text MysteryExplanation;
    [SerializeField] private GameObject MysteryChoices;

    public void MysteryDataInput()
    {
        
    }
    void Start()
    {
        Image MysteryImageComponent = MysteryImage.GetComponent<Image>();
        MysteryImageComponent.sprite = null;
        MysteryExplanation.text = "";
    }
}
