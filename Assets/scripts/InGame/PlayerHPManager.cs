using UnityEngine;
using UnityEngine.UI;
public class PlayerHPManager : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    [SerializeField] private Image HPbar;
    private RectTransform _RectTransform;
    

    void OnEnable()
    {
        _RectTransform = HPbar.GetComponent<RectTransform>();
        _RectTransform.localScale =new Vector3((float)HP / (float)MaxHP, 1f, 1f);
    }

    public void HP_change(int Damage)
    {
        HP -= Damage;
        if (((float)HP / (float)MaxHP) <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _RectTransform.localScale =new Vector3((float)HP / (float)MaxHP, 1f, 1f);
        }
    }
}
