using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {FreeRoam, Dialog, Battle}
public class GameController : MonoBehaviour
{
        [SerializeField] PlayerController playerController;
        [SerializeField] BattleSystem battleSystem;

        [SerializeField] Camera worldCamera;

        [SerializeField] Canvas worldDialogBox;
        GameState state;

    public void Start()
    {
        worldDialogBox.gameObject.SetActive(false);

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            worldDialogBox.gameObject.SetActive(true);
        };

        DialogManager.Instance.OnHideDialog += () =>
       {
           if (state == GameState.Dialog)
           {
               state = GameState.FreeRoam;
               worldDialogBox.gameObject.SetActive(false);
           }
       }
           ;
        playerController.OnBattle += () =>
        {
            StartBattle();
        };
        
        battleSystem.OnBattleOver += (isOver) =>
        {
            if (isOver)
            {
                state = GameState.FreeRoam;
                battleSystem.gameObject.SetActive(false);
                worldCamera.gameObject.SetActive(true);
                worldDialogBox.gameObject.SetActive(false);
                playerController.hasWin = true;
            }
        };
    }

    private void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        worldDialogBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

        }
        else if (state == GameState.Dialog)
        {
            
            DialogManager.Instance.HandleUpdate();

        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
