using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour,IButton
{
    public void Pushed() 
    {
        Debug.Log("押しました");
    }
}
