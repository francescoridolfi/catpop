using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogText;

    [SerializeField] GameObject moveSelector;

    [SerializeField] List<Button> moveButtons;

    private bool isWriting = false;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        if (isWriting) yield break;
        isWriting = true;
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isWriting = false;
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
    }

    public void ConfigureButtonCallback(Action<int> callback)
    {
        for (int i = 0; i < moveButtons.Count; i++)
        {
            int index = i; 
            moveButtons[i].onClick.AddListener(() => callback(index));
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i < moves.Count)
            {
                moveButtons[i].GetComponentInChildren<Text>().text = moves[i].Base.Name;
                moveButtons[i].GetComponentInChildren<RawImage>().color = Color.white;
                moveButtons[i].GetComponentInChildren<RawImage>().texture = moves[i].Base.Image;
            }
            else
                moveButtons[i].GetComponentInChildren<Text>().text = "-";
        }
    }

    public void UpdateMoveSelection(int currentMove)
    {
        for (int i = 0; i < moveButtons.Count; ++i)
        {
            if (i == currentMove)
                moveButtons[i].GetComponentInChildren<Text>().color = Color.yellow;
            else
                moveButtons[i].GetComponentInChildren<Text>().color = Color.black;
        }
    }
}
