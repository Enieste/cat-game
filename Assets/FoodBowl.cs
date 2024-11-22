using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    [SerializeField] private InkDialogueManager dialogueManager;
    [SerializeField] private HungerSystem hungerSystem;

    void Start()
    {
        Debug.Log("Food Bowl initialized, DialogueManager found: " + (dialogueManager != null));
    }


    private void Awake()
    {
        BindInkMethods();
    }

    private void BindInkMethods()
    {
        dialogueManager.GetStory().BindExternalFunction("Feed", DispenseFood);
    }

    void OnMouseDown()
    {
        Debug.Log("Food Bowl clicked!");
        if (dialogueManager != null)
        {
            if (!dialogueManager.dialoguePanel.activeSelf)
            {
                Debug.Log("Starting food bowl dialogue");
                dialogueManager.StartFoodBowlDialogue();
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

    public void DispenseFood()
    {
        var kibble = new Kibble();
        hungerSystem.FeedWith(kibble);
    }

}

public class Kibble : IFood
{
    public int SaturationValue => 70;
    public int ConsumptionTime => 3;
}