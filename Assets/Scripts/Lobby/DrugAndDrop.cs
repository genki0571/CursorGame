using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugAndDrop : MonoBehaviour
{
    [SerializeField] LobbyDivision lobbyDivision;
    [SerializeField] LobbyManager lobbyManager;
    [SerializeField] PlayerInput playerInput;

    [Header("ドラッグ&ドロップのアイコンの表示用")]
    [SerializeField] GameObject dustbinApp;
    [SerializeField] GameObject browserApp;
    [SerializeField] GameObject emailApp;
    [SerializeField] GameObject settingApp;
    [SerializeField] GameObject emptyFolder;
    [SerializeField] GameObject nonEmptyFolder;
    [SerializeField] GameObject item1;
    [SerializeField] GameObject item2;
    [SerializeField] GameObject item3;

    LobbyPosition crickPosition;
    LobbyPosition releasePosition;
    LobbyItemType crickedItemType;
    LobbyItemType releasedItemType;

    // Start is called before the first frame update
    void Start()
    {
        ClearCursorIcon(LobbyItemType.None);
    }

    // Update is called once per frame
    void Update()
    {
        if (AppWindowManager.IsOpenwindow == false)//ウィンドウが開いていないなら
        {
            if (playerInput.IsHoldStart())
            {
                crickPosition = lobbyDivision.CursorPartitioning(transform.position);
                crickedItemType = lobbyManager.GetItemType(crickPosition);
                lobbyManager.SetItemType(crickPosition, LobbyItemType.None);
                lobbyManager.RefreshScreenIcons();
                ClearCursorIcon(crickedItemType);

            }
            if (playerInput.IsHoldEnd())
            {
                releasePosition = lobbyDivision.CursorPartitioning(transform.position);
                releasedItemType = lobbyManager.GetItemType(releasePosition);
                if (releasedItemType == LobbyItemType.EmptyFolder || releasedItemType == LobbyItemType.NonEmptyFolder)
                {
                    if (crickedItemType == LobbyItemType.Item1 || crickedItemType == LobbyItemType.Item2 || crickedItemType == LobbyItemType.Item3)
                    {
                        //空きのあるフォルダーにアイテムを移動させようとした時の処理.
                        lobbyManager.SetItemType(crickPosition, releasedItemType);
                        lobbyManager.SetItemType(releasePosition, crickedItemType);
                    }
                    else
                    {
                        //空きのあるフォルダーにフォルダーやアプリを移動させようとした時の処理.
                        lobbyManager.SetItemType(crickPosition, releasedItemType);
                        lobbyManager.SetItemType(releasePosition, crickedItemType);
                    }
                }
                else
                {
                    //フォルダー以外のアイテムの上に移動させた時の処理.
                    lobbyManager.SetItemType(crickPosition, releasedItemType);
                    lobbyManager.SetItemType(releasePosition, crickedItemType);
                }

                if (crickedItemType != LobbyItemType.None)
                {
                    ClearCursorIcon(LobbyItemType.None);
                    lobbyManager.RefreshScreenIcons();
                }

            }
        }
    }

    private void ClearCursorIcon(LobbyItemType itemType)
    {
        dustbinApp.SetActive(false);
        browserApp.SetActive(false);
        emailApp.SetActive(false);
        settingApp.SetActive(false);
        /*
        emptyFolder.SetActive(false);
        nonEmptyFolder.SetActive(false);
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);*/

        switch (itemType)
        {
            case LobbyItemType.Dustbin:
                dustbinApp.SetActive(true);
                break;
            case LobbyItemType.Browser:
                browserApp.SetActive(true);
                break;
            case LobbyItemType.Email:
                emailApp.SetActive(true);
                break;
            case LobbyItemType.Setting:
                settingApp.SetActive(true);
                break;
            case LobbyItemType.EmptyFolder:
                emptyFolder.SetActive(true);
                break;
            case LobbyItemType.NonEmptyFolder:
                nonEmptyFolder.SetActive(true);
                break;
            case LobbyItemType.Item1:
                item1.SetActive(true);
                break;
            case LobbyItemType.Item2:
                item2.SetActive(true);
                break;
            case LobbyItemType.Item3:
                item3.SetActive(true);
                break;
        }
    }
}
