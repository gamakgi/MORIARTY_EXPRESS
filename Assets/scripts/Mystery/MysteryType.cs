using UnityEngine;

[CreateAssetMenu(fileName = "MysteryType", menuName = "MysteryManager/MysteryType")]
public class MysteryType : ScriptableObject
{
    public string MysteryExplanation;
    public Sprite MysteryImage;
    public int MysteryChoices_Count;
    public int MysteryChoices_Eeffect;
}
