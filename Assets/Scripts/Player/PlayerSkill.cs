using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RightSkill 
{
    Yet,
    FireWall,
    Command,
    Installation
}

public enum LeftSkill 
{
    Yet,
    Hammer,
    ContinuousClick,
    LoadThunder
}

public enum LongPressSkill 
{
    Yet,
    ThunderAndWind,
    FireAndIce,
    RangeSelect
}


public class PlayerSkill : MonoBehaviour
{
    static public PlayerSkill instance;

    public int rightLevel;
    public int leftLevel;
    public int longPressLevel;
    public RightSkill rightSkill;
    public LeftSkill leftSkill;
    public LongPressSkill longPressSkill;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        SetSkills();
        AddSkills();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSkills() 
    {

    }

    void AddSkills() 
    {

        gameObject.AddComponent<OpenAttack>();
        gameObject.AddComponent<FallTextAttack>();
        switch (rightSkill) 
        {
            case RightSkill.Yet:
                break;
            case RightSkill.FireWall:
                if (rightLevel >= 1)
                {
                    var rightAttack = gameObject.AddComponent<FireWallAttack>();
                    rightAttack.level = 0;
                }
                if (rightLevel >= 2)
                {
                    var rightAttack = gameObject.AddComponent<FireWallAttack>();
                    rightAttack.level = 1;
                }
                if (rightLevel >= 3)
                {
                    var rightAttack = gameObject.AddComponent<FireWallAttack>();
                    rightAttack.level = 2;
                }
                break;
            case RightSkill.Command:
                if (rightLevel >= 1)
                {
                    var rightAttack = gameObject.AddComponent<ScanWeakPoint>();
                }
                if (rightLevel >= 2)
                {
                    var rightAttack = gameObject.AddComponent<DeleteAttack>();
                }
                if (rightLevel >= 3)
                {

                }
                break;
            case RightSkill.Installation:
                if (rightLevel >= 1)
                {
                    var rightAttack = gameObject.AddComponent<InstallTxtTallet>();
                }
                if (rightLevel >= 2)
                {
                    var rightAttack = gameObject.AddComponent<InstallPngBuster>();
                }
                if (rightLevel >= 3)
                {
                    var rightAttack = gameObject.AddComponent<InstallZipFile>();
                }
                break;
        }

        switch (leftSkill)
        {
            case LeftSkill.Yet:
                if (leftLevel == 0)
                {
                    var leftAttacker = gameObject.AddComponent<SingleClickAttack>();
                }
                else if (leftLevel == 1)
                {
                    var leftAttacker = gameObject.AddComponent<DoubleClickAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.Hammer:
                if (leftLevel == 2)
                {
                    var leftAttacker = gameObject.AddComponent<HammerFirstAttack>();
                }
                else if (leftLevel == 3)
                {
                    var leftAttacker = gameObject.AddComponent<HammerFirstAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.ContinuousClick:
                if (leftLevel == 2)
                {
                    var leftAttacker = gameObject.AddComponent<TripleClickAttack>();
                }
                else if (leftLevel == 3)
                {
                    var leftAttacker = gameObject.AddComponent<TripleClickAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.LoadThunder:
                if (leftLevel == 2)
                {
                    var leftAttacker = gameObject.AddComponent<LoadThunderFirstAttack>();
                }
                else if (leftLevel == 3)
                {
                    var leftAttacker = gameObject.AddComponent<LoadThunderSecondAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
        }

        switch (longPressSkill)
        {
            case LongPressSkill.Yet:
                if (longPressLevel == 0)
                {
                    var holdAttacker = gameObject.AddComponent<NormalRangeAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.ThunderAndWind:
                if (longPressLevel == 1)
                {
                    var holdAttacker = gameObject.AddComponent<ThunderRangeAttack>();
                }
                else if (longPressLevel == 2)
                {
                    var holdAttacker = gameObject.AddComponent<ThunderAndWindRangeAttack>();
                }
                else if (longPressLevel == 3)
                {
                    var holdAttacker = gameObject.AddComponent<ThunderAndWindMixRangeAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.FireAndIce:
                if (longPressLevel == 1)
                {
                    var holdAttacker = gameObject.AddComponent<FireRangeAttack>();
                }
                else if (longPressLevel == 2)
                {
                    var holdAttacker = gameObject.AddComponent<FireAndIceRangeAttack>();
                }
                else if (longPressLevel == 3)
                {
                    var holdAttacker = gameObject.AddComponent<FireAndIceMixRangeAttack>();
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.RangeSelect:
                if (longPressLevel == 1)
                {
                    var holdAttacker = gameObject.AddComponent<RangeSelect>();
                }
                else if (longPressLevel == 2)
                {

                }
                else if (longPressLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
        }
    }
}
