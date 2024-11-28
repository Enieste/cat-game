using UnityEngine;

public class TVobject : MonoBehaviour
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
        dialogueManager.GetStory().BindExternalFunction("Idle", WatchTV);
        InkStateHandler.ReportExternalFunction("Idle");
    }

    private void UnbindInkMethods()
    {
        if (InkStateHandler.IsExternalFunctionKnown("Idle"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("Idle");
        }
    }

    void OnMouseDown()
    {
        if (dialogueManager != null)
        {
            if (!dialogueManager.dialoguePanel.activeSelf)
            {
                dialogueManager.StartTVDialogue();
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

    public void WatchTV()
    {
        hungerSystem.Idle();
    }
}