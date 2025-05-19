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

    [SerializeField] List<Text> moveTexts;

    public void SetDialog (string dialog)
    {
        dialogText.text=dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i=0; i<moveTexts.Count; i++)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.name;
            else
                moveTexts[i].text = "-";
        }
    }

    public void UpdateMoveSelection(int currentMove)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == currentMove)
                moveTexts[i].color = Color.yellow;
            else
                moveTexts[i].color = Color.black;
        }
    }
}
