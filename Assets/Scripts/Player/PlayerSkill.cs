using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RightSkill 
{
    Yet,
    FireWall,
    Command,
    Installation
}

enum LeftSkill 
{
    Yet,
    Hammer,
    ContinuousClick,
    LoadingThunder
}

enum LongPressSkill 
{
    Yet,
    ThunderAndWind,
    FireAndIce,
    RangeSelect
}


public class PlayerSkill : MonoBehaviour
{
    struct Skills 
    {
        public int rightLevel;
        public int leftLevel;
        public int longPressLevel;
        public RightSkill rightSkill;
        public LeftSkill leftSkill;
        public LongPressSkill longPressSkill;
    }

    Skills skills;

    // Start is called before the first frame update
    void Awake()
    {
        SetSkills();
        AddSkills();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSkills() 
    {
        //テスト用
        skills.rightLevel = 0;
        skills.leftLevel = 0;
        skills.longPressLevel = 0;
        skills.rightSkill = RightSkill.Yet;
        skills.leftSkill = LeftSkill.Yet;
        skills.longPressSkill = LongPressSkill.Yet;
    }

    void AddSkills() 
    {
        switch (skills.rightSkill) 
        {
            case RightSkill.Yet:
                if (skills.rightLevel == 0)
                {

                }
                else 
                {
                    Debug.Log("Error");
                }
                break;
            case RightSkill.FireWall:
                if (skills.rightLevel == 1)
                {

                }
                else if (skills.rightLevel == 2)
                {

                }
                else if (skills.rightLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case RightSkill.Command:
                if (skills.rightLevel == 1)
                {

                }
                else if (skills.rightLevel == 2)
                {

                }
                else if (skills.rightLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case RightSkill.Installation:
                if (skills.rightLevel == 1)
                {

                }
                else if (skills.rightLevel == 2)
                {

                }
                else if (skills.rightLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
        }

        switch (skills.leftSkill)
        {
            case LeftSkill.Yet:
                if (skills.leftLevel == 0)
                {

                }
                else if (skills.leftLevel == 1) 
                {
                
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.Hammer:
                if (skills.leftLevel == 2)
                {

                }
                else if (skills.leftLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.ContinuousClick:
                if (skills.leftLevel == 2)
                {

                }
                else if (skills.leftLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LeftSkill.LoadingThunder:
                if (skills.leftLevel == 2)
                {

                }
                else if (skills.leftLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
        }

        switch (skills.longPressSkill)
        {
            case LongPressSkill.Yet:
                if (skills.longPressLevel == 0)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.ThunderAndWind:
                if (skills.longPressLevel == 1)
                {

                }
                else if (skills.longPressLevel == 2)
                {

                }
                else if (skills.longPressLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.FireAndIce:
                if (skills.longPressLevel == 1)
                {

                }
                else if (skills.longPressLevel == 2)
                {

                }
                else if (skills.longPressLevel == 3)
                {

                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case LongPressSkill.RangeSelect:
                if (skills.longPressLevel == 1)
                {

                }
                else if (skills.longPressLevel == 2)
                {

                }
                else if (skills.longPressLevel == 3)
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
