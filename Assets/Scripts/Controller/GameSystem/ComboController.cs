using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{
    static public ComboController instance;

    [SerializeField] Text comboCountText;
    [SerializeField] Text hitText;

    int comboCnt = 0;
    float comboTimer = 0;
    const float COMBO_INTERVAL = 3;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ResetCombo();
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTimer <= COMBO_INTERVAL && comboCnt != 0) 
        {
            comboTimer += Time.deltaTime;
            comboCountText.enabled = true;
            comboCountText.text = "" + comboCnt;
            hitText.enabled = true;
        }
        if (comboTimer >= COMBO_INTERVAL) 
        {
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        comboTimer = 0;
        comboCnt = 0;
        hitText.enabled = false;
        comboCountText.enabled = false;
    }

    public void AddCombo() 
    {
        comboCnt += 1;
        comboTimer = 0;
    }
}
