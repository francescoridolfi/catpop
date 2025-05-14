using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleHud enemyHud;

    [SerializeField] BattleDialogBox dialogBox;

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
        PlayerAction();
        StartCoroutine (dialogBox.TypeDialog("Choose and action"));
        yield return new WaitForSeconds(1f);
        StartCoroutine (dialogBox.TypeDialog(""));
        dialogBox.SetMoveNames(playerUnit.gatto.Moves);
    }
   
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
    }

    void PlayerMove() 
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }
    /*
    if (state == BattleState.PlayerMove)
    {
        HandleMoveSelection();
    }

    void HandleMoveSelection()
    {

    } */
}
