using UnityEngine;

public class Couch : MonoBehaviour
{
    [SerializeField] private InkDialogueManager dialogueManager;
    [SerializeField] private HungerSystem hungerSystem;

    private void Awake()
    {
        BindInkMethods();
    }

    private void BindInkMethods()
    {
        UnbindInkMethods();
        dialogueManager.GetStory().BindExternalFunction("CandyWrapperPlay", CandyWrapperPlay);
        InkStateHandler.ReportExternalFunction("CandyWrapperPlay");
    }

    private void UnbindInkMethods()
    {
        if (InkStateHandler.IsExternalFunctionKnown("CandyWrapperPlay"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("CandyWrapperPlay");
        }
    }


    void OnMouseDown()
    {
        if (dialogueManager != null)
        {
            if (!dialogueManager.dialoguePanel.activeSelf)
            {
                dialogueManager.StartToyUnderTheCouchDialogue();
            }
            else
            {
                Debug.Log("Dialogue panel is already active");
            }
        }
        else
        {
            Debug.LogError("No DialogueManager found!");
        }
    }

    public void CandyWrapperPlay()
    {
        var candyWrapper = new CandyWrapper();
        hungerSystem.PlayWith(candyWrapper);
    }

}

public class CandyWrapper : IToy
{
    public int Fun => 20;
    public int PlayTime => 10;
}
