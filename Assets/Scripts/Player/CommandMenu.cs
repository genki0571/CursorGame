using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMenu : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;
    PlayerAttack playerAttack => PlayerAttack.instance;
    List<Command> commands = new List<Command>();

    [SerializeField] GameObject openCommandBlock;
    [SerializeField] GameObject fallTextCommandBlock;
    [SerializeField] GameObject fireWallCommandBlock;
    [SerializeField] GameObject scanCommandBlock;
    [SerializeField] GameObject deleteCommandBlock;
    [SerializeField] GameObject enemyPasteCommandBlock;
    [SerializeField] GameObject installTxtCommandBlock;
    [SerializeField] GameObject installPngCommandBlock;
    [SerializeField] GameObject installZipCommandBlock;

    const float COMMAND_BLOCK_LOCAL_WIDTH = 30;
    const float COMMNAD_BLOCK_LOCAL_HEIGHT = 3;

    Text[] texts;
    string[] textStr;
    [SerializeField] TextAsset bugTextAsset;
    string bugStr;
    float fontbugTimer = 0;
    const float FONT_BUG_INTERVAL = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerAttack.rightAttackers.Count; i++)
        {
            commands.Add(playerAttack.rightAttackers[i].GetCommandKind());
        }

        texts = GetComponentsInChildren<Text>();
        textStr = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            textStr[i] = texts[i].text;
        }
        bugStr = bugTextAsset.text;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.IsVirus(Virus.FontBug)) 
        {
            fontbugTimer += Time.deltaTime;
            if (fontbugTimer >= FONT_BUG_INTERVAL) 
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].text = "";
                    int randStrNum = Random.Range(3, 8);
                    for (int j = 0; j < randStrNum; j++)
                    {
                        int rand = Random.Range(0, bugStr.Length);
                        texts[i].text += bugStr.Substring(rand);
                    }
                }
                fontbugTimer = 0;
            }
        }
        else 
        {
            if (texts[0].text != textStr[0]) 
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].text = textStr[i];
                }
                fontbugTimer = 0;
            }
        }

        openCommandBlock.SetActive(false);
        fallTextCommandBlock.SetActive(false);
        fireWallCommandBlock.SetActive(false);
        scanCommandBlock.SetActive(false);
        deleteCommandBlock.SetActive(false);
        /*
        enemyPasteCommandBlock.SetActive(false);
        */
        installTxtCommandBlock.SetActive(false);
        installPngCommandBlock.SetActive(false);
        installZipCommandBlock.SetActive(false);

        int commandCnt = 0;
        for (int i = 0; i < commands.Count; i++)
        {
            GameObject commandBlock = null;
            if (commands[i] == Command.EnemyPaste || commands[i] == Command.Delete || commands[i] == Command.Open) 
            {
                //敵を選択しているとき
                if (playerAttack.isSelectEnemy)
                {
                    commandBlock = GetCommandBlock(commands[i]);
                }
            }
            else
            {
                commandBlock = GetCommandBlock(commands[i]);
            }

            if (commandBlock != null) 
            {
                commandBlock.transform.localPosition = new Vector3((-COMMAND_BLOCK_LOCAL_WIDTH / 2), (-COMMNAD_BLOCK_LOCAL_HEIGHT/2) + (commandCnt * -COMMNAD_BLOCK_LOCAL_HEIGHT),0);
                commandCnt += 1;
                commandBlock.SetActive(true);
            }
        }
    }

    GameObject GetCommandBlock(Command command) 
    {
        GameObject commandBlock = null;

        switch (command) 
        {
            case Command.Empty:
                break;
            case Command.Open:
                commandBlock = openCommandBlock;
                break;
            case Command.FallText:
                commandBlock = fallTextCommandBlock;
                break;
            case Command.FireWall:
                commandBlock = fireWallCommandBlock;
                break;
            case Command.Scan:
                commandBlock = scanCommandBlock;
                break;
            case Command.Delete:
                commandBlock = deleteCommandBlock;
                break;
            case Command.EnemyPaste:
                commandBlock = enemyPasteCommandBlock;
                break;
            case Command.InstallTxt:
                commandBlock = installTxtCommandBlock;
                break;
            case Command.InstallPng:
                commandBlock = installPngCommandBlock;
                break;
            case Command.InstallZip:
                commandBlock = installZipCommandBlock;
                break;

        }

        return commandBlock;
    }
}
