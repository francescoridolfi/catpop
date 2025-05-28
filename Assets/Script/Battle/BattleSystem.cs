using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleHud enemyHud;

    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    public GattoStats enemyGattoStats;

    BattleState state;
    int currentMove;
    public void Start()
    {
        enemyUnit.gattoStats = enemyGattoStats; 
        StartCoroutine (SetUpBattle());
    }

    private IEnumerator SetUpBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.gatto);
        enemyHud.SetData(enemyUnit.gatto);



        yield return (dialogBox.TypeDialog($"Il micio {enemyUnit.gatto.gattoStats.Name} scende in campo!"));
        yield return new WaitForSeconds(1f);
        PlayerMove();
        StartCoroutine(dialogBox.TypeDialog("Ora cosa farai?"));
        yield return new WaitForSeconds(1f);
        StartCoroutine(dialogBox.TypeDialog(""));

        dialogBox.SetMoveNames(playerUnit.gatto.Moves);
        dialogBox.ConfigureButtonCallback((int moveIndex) =>
        {
            if (moveIndex >= playerUnit.gatto.Moves.Count)
                return;

            currentMove = moveIndex;

            dialogBox.UpdateMoveSelection(currentMove);
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerMove());
         });
    }
    public void HandleUpdate()
    {
        if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }



    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
        

    }
    
    IEnumerator PerformPlayerMove()
    {
        var move = playerUnit.gatto.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} usa {move.Base.name}");

        yield return new WaitForSeconds(1f);

        if(!move.Base.IsRegen) {
            playerUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);

            enemyUnit.PlayHitAnimation();
        }

        bool isFainted = enemyUnit.gatto.TakeDamage(move, playerUnit.gatto);
        if(move.Base.IsRegen)
        {
            playerUnit.gatto.HP += move.Base.Power;
            if (playerUnit.gatto.HP > playerUnit.gatto.MaxHp)
                playerUnit.gatto.HP = playerUnit.gatto.MaxHp;
        }
        yield return playerHud.UpdateHP();
        yield return enemyHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} ti ha ridato la tua vita!");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove() {
        state = BattleState.EnemyMove;
        var move = enemyUnit.gatto.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} usa {move.Base.name}");
        yield return new WaitForSeconds(1f);

        if (!move.Base.IsRegen) {
            enemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);

            playerUnit.PlayHitAnimation();
        }

        bool isFainted = playerUnit.gatto.TakeDamage(move, enemyUnit.gatto);
        if (move.Base.IsRegen)
        {
            enemyUnit.gatto.HP += move.Base.Power;
            if (enemyUnit.gatto.HP > enemyUnit.gatto.MaxHp)
                enemyUnit.gatto.HP = enemyUnit.gatto.MaxHp;
        }

        yield return playerHud.UpdateHP();
        yield return enemyHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog("Ritenta... sarai piu fortunato!");
            playerUnit.PlayFaintAnimation();
            
            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerMove();
        }
    }
   
    void HandleMoveSelection()    
    {

        
        

    }


    
}
