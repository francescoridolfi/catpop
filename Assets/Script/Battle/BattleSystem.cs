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
        yield return new WaitForSeconds(1.5f);
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

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerMove());
         });
    }
    public void HandleUpdate()
    {
        if (state == BattleState.PlayerMove)
        {
            // Do Nothing, waiting for player input
            // Doom regna
        }
    }



    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
        

    }
    private bool isPerformingAction = false;
    IEnumerator PerformPlayerMove()
    {

        if (isPerformingAction)
            yield break;

        isPerformingAction = true;
        var move = playerUnit.gatto.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} usa {move.Base.Name}");

        yield return new WaitForSeconds(1f);

        if (move.Base.Description != "")
        {
            yield return dialogBox.TypeDialog(move.Base.Description);
            yield return new WaitForSeconds(1f);
        }

        if (move.Base.IsAttack)
            {
                playerUnit.PlayAttackAnimation();
                yield return new WaitForSeconds(1f);

                enemyUnit.PlayHitAnimation();
            }

        bool isFainted = enemyUnit.gatto.TakeDamage(move, playerUnit.gatto);

        switch (move.Base.MoveType)
        {
            case MoveType.ATTACK_BOOST:
                playerUnit.gatto.Attack += (int) ((float) move.Base.Power / 100 * playerUnit.gatto.Attack);
                yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} ha aumentato il suo attacco!");
                break;
            case MoveType.DEFENSE_BOOST:
                playerUnit.gatto.Defense += (int) ((float) move.Base.Power / 100 * playerUnit.gatto.Defense);
                yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} ha aumentato la sua difesa!");
                break;
            case MoveType.REGEN:
                playerUnit.gatto.HP += (int) ((float) move.Base.Power / 100 * playerUnit.gatto.HP);
                if (playerUnit.gatto.HP > playerUnit.gatto.MaxHp)
                    playerUnit.gatto.HP = playerUnit.gatto.MaxHp;
                yield return dialogBox.TypeDialog($"{playerUnit.gatto.Name} si è rigenerato!");
                break;
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
        isPerformingAction = false;
    }

    IEnumerator EnemyMove() {
        state = BattleState.EnemyMove;
        var move = enemyUnit.gatto.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} usa {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        if (move.Base.IsAttack) {
            enemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);

            playerUnit.PlayHitAnimation();
        }

        bool isFainted = playerUnit.gatto.TakeDamage(move, enemyUnit.gatto);

        switch (move.Base.MoveType)
        {
            case MoveType.ATTACK_BOOST:
                enemyUnit.gatto.Attack += (int) ((float) move.Base.Power / 100 * enemyUnit.gatto.Attack);
                yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} ha aumentato il suo attacco!");
                break;
            case MoveType.DEFENSE_BOOST:
                enemyUnit.gatto.Defense += (int) ((float) move.Base.Power / 100 * enemyUnit.gatto.Defense);
                yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} ha aumentato la sua difesa!");
                break;
            case MoveType.REGEN:
                enemyUnit.gatto.HP += (int) ((float) move.Base.Power / 100 * enemyUnit.gatto.HP);
                if (enemyUnit.gatto.HP > enemyUnit.gatto.MaxHp)
                    enemyUnit.gatto.HP = enemyUnit.gatto.MaxHp;
                yield return dialogBox.TypeDialog($"{enemyUnit.gatto.Name} si è rigenerato!");
                break;
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



    
}
