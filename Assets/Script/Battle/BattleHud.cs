using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HealthBar hpBar;

    Gatto _gatto;

    public void SetData(Gatto gatto)
    {
        _gatto = gatto;
        nameText.text = gatto.gattoStats.Name;
        hpBar.SetHP((float) gatto.HP / gatto.MaxHp);
    }

    
    public IEnumerator UpdateHP() {
       yield return hpBar.SetHPSmooth((float) _gatto.HP / _gatto.MaxHp);
    }
}
