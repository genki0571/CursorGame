using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenu : MonoBehaviour
{
    [SerializeField] Player player;
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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < player.rightAttackers.Count; i++)
        {
            commands.Add(player.rightAttackers[i].GetCommandKind());
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        /*
        installZipCommandBlock.SetActive(false);
        */

        int commandCnt = 0;
        for (int i = 0; i < commands.Count; i++)
        {
            GameObject commandBlock = null;
            if (commands[i] == Command.EnemyPaste || commands[i] == Command.Delete || commands[i] == Command.Open) 
            {
                //敵を選択しているとき
                if (player.isSelectEnemy)
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
