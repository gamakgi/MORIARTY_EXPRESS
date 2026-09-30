using System;
using UnityEngine;

public class TargetSet : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void On()
    {
        gameObject.SetActive(true);
    }
    public void Off()
    {
        gameObject.SetActive(false);
    }
}
