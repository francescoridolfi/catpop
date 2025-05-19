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

    BattleState state;
    int currentMove;
    private void Start()
    {
        StartCoroutine (SetUpBattle());
    }

    private IEnumerator SetUpBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.gatto);
        enemyHud.SetData(enemyUnit.gatto);

        

        yield return (dialogBox.TypeDialog($"A wild {enemyUnit.gatto.gattoStats.Name} appeared"));
        yield return new WaitForSeconds(1f);
        PlayerMove();
        StartCoroutine (dialogBox.TypeDialog("Choose and action"));
        yield return new WaitForSeconds(1f);
        StartCoroutine (dialogBox.TypeDialog(""));

        dialogBox.SetMoveNames(playerUnit.gatto.Moves);
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
        yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} used {move.Base.name}");

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
            yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} fainted");
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
        yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} used {move.Base.name}");
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
            yield return dialogBox.TypeDialog("You loose (Emanuela Orlandi ti osserva)");
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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.gatto.Moves.Count - 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.gatto.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }

    }


    
}
