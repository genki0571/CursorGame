using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScroll : MonoBehaviour
{
    static public PlayerScroll instace;

    const float MAX_SCROLL_AMOUNT = 360;
    const float SCROLL_AMOUNT_SPEED = 5;
    public int scrollLevel;
    public float scrollAmount = 0;

    private void Awake()
    {
        instace = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollAmount += Input.mouseScrollDelta.y * SCROLL_AMOUNT_SPEED;
        if (scrollAmount >= MAX_SCROLL_AMOUNT) 
        {
            scrollAmount = 0;
        }
        else if(scrollAmount < 0)
        {
            scrollAmount = MAX_SCROLL_AMOUNT + scrollAmount;
        }
    }
}
