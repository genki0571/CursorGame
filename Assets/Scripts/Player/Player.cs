using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command 
{
    Empty,
    Opem,
    FallText,
    FireWall,
    Scan,
    Delete,
    EnemyPaste,
    InstallTxt,
    InstallPng,
    InstallZip
}


public class Player : MonoBehaviour
{
    ILeftAttacker leftAttacker;
    ILongPressAttacker longPressAttacker;
    public List<IRightAttacker> rightAttackers = new List<IRightAttacker>();

    Transform cursorTrans;
    Vector2 cursorPos;

    //
    public bool isLeftClick;
    public bool isRightClick;
    public bool isLongPressDown;
    public bool isLongPress;

    bool isRange;

    bool isGrabbing;
    IGrabbable grabbable;
    int grabCnt = 0;
    const int GRAB_FRAME_INTERVAL = 15;

    bool beforeLongPress;
    bool LongPressed;
    Vector2 longPressStartPos;
    public List<IDamagable> withinRangeEnemies;

    public List<IDamagable> selectingEnemies;

    [SerializeField] GameObject longPressDisplay;
    [SerializeField] GameObject commandMenu;

    // Start is called before the first frame update
    void Start()
    {
        //仮
        var leftAttack = gameObject.AddComponent<DoubleClickAttack>();
        var longPressAttack = gameObject.AddComponent<NormalRangeAttack>();
        var rightAttack = gameObject.AddComponent<FallTextAttack>();
        leftAttacker = leftAttack.GetComponent<ILeftAttacker>();
        longPressAttacker = longPressAttack.GetComponent<ILongPressAttacker>();
        rightAttackers.Add(rightAttack);

        cursorTrans = GetComponent<Transform>();

        withinRangeEnemies = longPressDisplay.GetComponent<LongPressDisplay>().withinRangeEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = cursorTrans.position;

        Ray2D ray = new Ray2D(transform.position, transform.up);
        RaycastHit2D hit = new RaycastHit2D();
        if (isRightClick || isLeftClick || isLongPress || beforeLongPress) 
        {
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1f);
        }

        //左クリックしたとき
        if (isLeftClick) 
        {
            if (hit.collider)
            {
                IButton button = hit.transform.GetComponent<IButton>();
                if (button != null)
                {
                    Command command = button.Pushed();
                    ActionCommand(command);
                }
            }

            commandMenu.SetActive(false);

            //攻撃を使用
            leftAttacker.Attack(hit.transform);
        }

        //右クリックした時
        if (isRightClick)
        {
            if (hit.collider)
            {
                IButton button = hit.transform.GetComponent<IButton>();
                if (button != null)
                {
                    Command command = button.Pushed();
                    ActionCommand(command);
                }

                IDamagable damagable = hit.transform.GetComponent<IDamagable>();
                if (damagable != null) 
                {

                }

                commandMenu.SetActive(false);
            }
            else 
            {
                commandMenu.transform.position = cursorPos;
                commandMenu.SetActive(true);
            }
        }

        if (isLongPressDown)
        {
            longPressStartPos = cursorTrans.position;
            LongPressed = true;
            grabCnt = 0;
        }

        if (isLongPress)
        {

            if (!isRange && !isGrabbing) 
            {
                if (hit.collider)
                {
                    grabbable = hit.transform.GetComponent<IGrabbable>();
                    if (grabbable != null)
                    {
                        grabCnt += 1;
                        if (grabCnt >= GRAB_FRAME_INTERVAL) 
                        {
                            isGrabbing = true;
                        }
                    }
                    else
                    {
                        isRange = true;
                        isGrabbing = false;
                    }
                }
                else
                {
                    isRange = true;
                    isGrabbing = false;
                }
            }

            //範囲選択
            if (isRange) 
            {
                float width = Mathf.Abs(cursorPos.x - longPressStartPos.x);
                float height = Mathf.Abs(cursorPos.y - longPressStartPos.y);
                Vector2 centerPos = longPressStartPos + ((cursorPos - longPressStartPos) / 2);

                longPressDisplay.transform.position = centerPos;
                longPressDisplay.transform.localScale = new Vector2(width, height);
                longPressDisplay.SetActive(true);
            }

            //つかむ
            if (isGrabbing)
            {
                grabbable.Grabbing(cursorTrans);
            }
        }
        else 
        {
            if (beforeLongPress)
            {
                Debug.Log("長押し解除");

                //範囲選択攻撃
                for (int i = 0; i < withinRangeEnemies.Count; i++)
                {
                    longPressAttacker.Attack(withinRangeEnemies[i]);
                }

                //コマンド選択
                if (hit.collider)
                {
                    IButton button = hit.transform.GetComponent<IButton>();
                    if (button != null)
                    {
                        Command command = button.Pushed();
                        ActionCommand(command);
                    }
                }
                commandMenu.SetActive(false);

                if (isGrabbing) 
                {
                    grabbable.Putting();
                }
            }
            LongPressed = false;
            isRange = false;
            isGrabbing = false;

            longPressDisplay.SetActive(false);
        }
        beforeLongPress = isLongPress;
    }

    void ActionCommand(Command command) 
    {
        for (int i = 0; i < rightAttackers.Count; i++)
        {
            if (command == rightAttackers[i].GetCommandKind()) 
            {
                rightAttackers[i].Command(cursorTrans);
            }
        }
    }
}
