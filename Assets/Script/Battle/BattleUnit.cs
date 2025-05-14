using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] GattoStats gattoStats;
    [SerializeField] bool isPlayerUnit;
    
    public Gatto gatto { get; set;}

    public void Setup()
    {
        gatto = new Gatto(gattoStats);
        if(isPlayerUnit)
        GetComponent<Image>().sprite = gatto.gattoStats.FrontSprite;
    }
}
