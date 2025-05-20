using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{

    [SerializeField] Dialog dialog;
    [SerializeField] GattoStats enemyGattoStats;

    public IEnumerator Interact()
    {
        yield return DialogManager.Instance.ShowDialog(dialog);
    }

    public GattoStats GetGattoStats()
    {
        return enemyGattoStats;
    }
}
