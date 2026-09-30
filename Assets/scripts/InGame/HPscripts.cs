using UnityEngine;
using UnityEngine.UI;
public class HPscripts : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    [SerializeField] private Image HPbar;
    private RectTransform _RectTransform;
    private AttakManager _attakManager;
    private GameObject GameController;
    

    void OnEnable()
    {
        GameController = GameObject.FindWithTag("Attak_Manager");  
        _attakManager = GameController.GetComponent<AttakManager>();
        _RectTransform = HPbar.GetComponent<RectTransform>();
        _RectTransform.localScale =new Vector3((float)HP / (float)MaxHP, 1f, 1f);
        
    }

    public void HP_change()
    {
        if (((float)HP / (float)MaxHP) <= 0)
        {
            _attakManager.Target = null;
            Destroy(gameObject);
        }
        else
        {
            _RectTransform.localScale =new Vector3((float)HP / (float)MaxHP, 1f, 1f);
        }
    }
}
