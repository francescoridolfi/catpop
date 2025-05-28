
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public enum GameState {FreeRoam, Dialog, Battle}
public class GameController : MonoBehaviour
{
        [SerializeField] PlayerController playerController;
        [SerializeField] BattleSystem battleSystem;

        [SerializeField] Camera worldCamera;

        [SerializeField] GameObject worldDialogBox;

        [SerializeField] GameObject joyStick;

        [SerializeField] GameObject pauseButton;

        [SerializeField] VideoPlayer faintedVideoPlayer;
        GameState state;
        
        

    public void Start()
    {
        worldDialogBox.gameObject.SetActive(false);

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            worldDialogBox.gameObject.SetActive(true);
            joyStick.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
        };

        DialogManager.Instance.OnHideDialog += () =>
       {
           if (state == GameState.Dialog)
           {
               state = GameState.FreeRoam;
               worldDialogBox.gameObject.SetActive(false);
               joyStick.gameObject.SetActive(true);
               pauseButton.gameObject.SetActive(true);
           }
       }
           ;
        playerController.OnBattle += (GattoStats gattoStats) =>
        {
            Debug.Log($"Start Battle with {gattoStats}");
            
            battleSystem.enemyGattoStats = gattoStats;
            joyStick.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            StartBattle();
        };

        battleSystem.OnBattleOver += (isOver) =>
        {   
            state = GameState.FreeRoam;
            battleSystem.gameObject.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            worldDialogBox.gameObject.SetActive(false);

            if (isOver)
            {
                joyStick.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(true);
                playerController.hasWin = true;
            }
            else
            {
                playerController.hasWin = true;
                faintedVideoPlayer.loopPointReached += OnFaintedVideoFinished;
                faintedVideoPlayer.gameObject.SetActive(true);
                faintedVideoPlayer.Play();
            }
        };
    }

    public void OnFaintedVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }

    private void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        worldDialogBox.gameObject.SetActive(false);
        battleSystem.Start();
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
