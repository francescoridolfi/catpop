using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HealthBar hpBar;

    public void SetData(Gatto gatto)
    {
        nameText.text = gatto.gattoStats.Name;
        hpBar.SetHP((float) gatto.HP / gatto.MaxHp);
    }
}
