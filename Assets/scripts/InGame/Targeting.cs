using UnityEngine;
using UnityEngine.InputSystem;
public class Targeting : MonoBehaviour
{
    public AttakManager _attakManager;
    public GameObject Attak_ManagerObject;
    public GameObject TargetSign;
    public TargetSet _TargetSet;
    private bool _isMouseOver;
    


    void Start()
    {
        _TargetSet = TargetSign.GetComponent<TargetSet>();
        Attak_ManagerObject = GameObject.FindWithTag("Attak_Manager");  
        _attakManager = Attak_ManagerObject.GetComponent<AttakManager>();
    }
    private void targeting()
    {
        if (_attakManager._TargetSet != null)
        {
            _attakManager.TargetOff();
        }
        _attakManager._TargetSet = _TargetSet;
        _attakManager._TargetSet.On();
        _attakManager.Target = gameObject;
    }
    public void OnMouseEnter()
    {
        _isMouseOver = true;
    }

    public void OnMouseExit()
    {
        _isMouseOver = false;
    }

    private void Update()
    {
        if (_isMouseOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            targeting();
        }
    }


}
