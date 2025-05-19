using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{

    [SerializeField] Dialog dialog;
    public IEnumerator Interact()
    {
        yield return DialogManager.Instance.ShowDialog(dialog);
    }
}
