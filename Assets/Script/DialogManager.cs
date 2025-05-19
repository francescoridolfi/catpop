using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;


    [SerializeField] int lettersPerSecond;


    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance {get; private set;}

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    private void Awake()
    {
        Instance = this; 
    }

    public IEnumerator HandleUpdate()
    {
        if(Physics2D.OverlapCircle(PlayerController.Instance.getInteractPos(), 0.2f, PlayerController.Instance.interactableLayer) != null && !isTyping){
            
            ++currentLine;
            if(currentLine < dialog.Lines.Count) {
                yield return TypeDialog(dialog.Lines[currentLine]);
            } else {
                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
            }
        }
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        currentLine = 0;
        dialogBox.SetActive(true);
        yield return TypeDialog(dialog.Lines[0]);
    }

    public IEnumerator TypeDialog (string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}
