using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Virus 
{
    Empty,
    ClickInvert,
    MoveInvert,
    CannotLeftClick,
    CannotRightClick,
    CannnotHold,
    CannotGrab,
    FontBug,
    Unvisible,
    SpeedUp,
    SpeedDown
}

public class PlayerState : MonoBehaviour
{
    static public PlayerState instance;

    public int cursorHp;
    public int maxCursorHp;
    public bool isDead;

    float damageTimer = 0;
    float damageInterval = 0.1f;
    bool damaged = false;

    public List<Virus> playerViruses = new List<Virus>();
    [SerializeField] SpriteRenderer renderer;

    public bool isStan;
    private float stanTimer = 0;
    private float stanInterval = 0;

    public string[] codeStrs = new string[6] { "using UnityEngine;", "public class Cursor{", "void Start(){", "Revival();", "}", "}" };
    int lineNum = 0;
    public int[] codeNums = new int[6];

    Server server => Server.instance;
    PlayerMovement playerMovement => PlayerMovement.instance;


    //EXP
    int expNum = 0;
    List<Transform> expTransforms = new List<Transform>();

    void Awake()
    {
        instance = this;
        cursorHp = maxCursorHp;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetCursorSpeed();

        if (isStan) 
        {
            stanTimer += Time.deltaTime;
            if (stanTimer >= stanInterval) 
            {
                isStan = false;
                stanTimer = 0;
            }
        }

        if (IsVirus(Virus.Unvisible)) 
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
        }


        if (cursorHp <= 0 && !isDead) 
        {
            isDead = true;

            lineNum = 0;
            codeNums = new int[6];
            for (int i = 0; i < codeNums.Length; i++)
            {
                codeNums[i] = 0;
            }
        }

        if (damaged) 
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval) 
            {
                damaged = false;
                damageTimer = 0;
            }
        }

        if (isDead) 
        {
            if (server.isConnection)
            {
                if (Input.anyKeyDown && lineNum < 6)
                {
                    codeNums[lineNum]++;
                    if (codeNums[lineNum] >= codeStrs[lineNum].Length) 
                    {
                        lineNum += 1;
                    }
                }

                if (lineNum >= 6) 
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Revival();
                    }
                }
            }
        }
    }

    public void GetVirus(Virus virus)
    {
        playerViruses.Add(virus);
    }

    public void RemoveVirus(Virus virus) 
    {
        playerViruses.Remove(virus);
    }

    public bool IsVirus(Virus virus) 
    {
        bool isVirus = false;
        for (int i = 0; i < playerViruses.Count; i++)
        {
            if (virus == playerViruses[i]) 
            {
                isVirus = true;
                break;
            }
        }

        return isVirus;
    }

    public void GetStan(float interval) 
    {
        isStan = true;
        stanTimer = 0;
        stanInterval = interval;
    }

    public void Revival() 
    {
        cursorHp = maxCursorHp;
        isDead = false;
    }

    public void AddDamage(int damage) 
    {
        if (!isDead && !damaged)
        {
            cursorHp -= damage;
            damaged = true;
        }
    }

    public void Recovery(int recoveryAmount)
    {
        if (!isDead)
        {
            cursorHp += recoveryAmount;
            if (cursorHp >= maxCursorHp) 
            {
                cursorHp = maxCursorHp;
            }
        }
    }

    private void SetCursorSpeed ()
    {
        playerMovement.speed = playerMovement.cursorNormalSpeed;
        if (isStan)
        {
            playerMovement.speed = 0;
        }
        else if (IsVirus(Virus.SpeedUp))
        {
            playerMovement.speed *= 8;
        }
        else if (IsVirus(Virus.SpeedDown))
        {
            playerMovement.speed /= 10;
        }

        if (IsVirus(Virus.MoveInvert))
        {
            playerMovement.speed *= -1;
        }

        if (isDead)
        {
            playerMovement.speed = playerMovement.cursorNormalSpeed / 30;
        }
    }

}
