using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppWindowManager : MonoBehaviour
{
    public static bool IsOpenwindow { get; private set; }

    [SerializeField] LobbyDivision lobbyDivision;
    [SerializeField] LobbyManager lobbyManager;
    [SerializeField] PlayerInput playerInput;

    [Header("アプリケーションウィンドウ")]
    [Tooltip("ゴミ箱")]
    [SerializeField] private GameObject dustbinWindow;
    [Tooltip("ブラウザ")]
    [SerializeField] private GameObject browserWindow;
    [Tooltip("メール")]
    [SerializeField] private GameObject emailWindow;
    [Tooltip("設定")]
    [SerializeField] private GameObject settingWindow;

    LobbyPosition crickPosition;
    LobbyItemType crickedItemType;

    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数
    private void Start()
    {
        CloseWindow();
    }
    private void Update()
    {
        if (!IsOpenwindow)
        {
            if (Input.GetKeyDown(KeyCode.O))// アプリを開くときの動作をここに記述する(ダブルクリックなど).
            {
                crickPosition = lobbyDivision.CursorPartitioning(transform.position);
                crickedItemType = lobbyManager.GetItemType(crickPosition);
                if (OpenWindow(crickedItemType)) IsOpenwindow = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector3.forward);
                if (hit2d)
                {
                    clickedGameObject = hit2d.transform.gameObject;
                    if (clickedGameObject.CompareTag("CloseButton"))
                    {
                        CloseWindow();
                    }
                }
            }
        }
    }
    private bool OpenWindow(LobbyItemType itemType)
    {
        switch (itemType)
        {
            case LobbyItemType.Dustbin:
                dustbinWindow.SetActive(true);
                break;
            case LobbyItemType.Browser:
                browserWindow.SetActive(true);
                break;
            case LobbyItemType.Email:
                emailWindow.SetActive(true);
                break;
            case LobbyItemType.Setting:
                settingWindow.SetActive(true);
                break;
            default:
                return false;
        }
        return true;
    }
    public void CloseWindow()
    {
        dustbinWindow.SetActive(false);
        browserWindow.SetActive(false);
        emailWindow.SetActive(false);
        settingWindow.SetActive(false);
        IsOpenwindow = false;
    }
}
