using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleHud enemyHud;

    private void Start()
    {
        SetUpBattle();
    }

    private void SetUpBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.gatto);
        enemyHud.SetData(enemyUnit.gatto);
    }
}
