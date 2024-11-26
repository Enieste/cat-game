using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;

public class InkDialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public GameObject dayPanel;
    public GameObject nightPanel;
    public TextMeshProUGUI dialogueText;
    public Image catPortrait;

    [Header("Stats UI")]
    public TextMeshProUGUI statsText;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    [Header("Night Mode")]
    public NightModeManager nightModeManager;
    public bool isNightMode = false;

    [System.Serializable]
    public class CatExpression
    {
        public string tagName;
        public Sprite sprite;
    }

    [Header("Cat Expressions")]
    public CatExpression[] catExpressions;


    private Story story;
    private bool canProcessChoices = false;

    // TODO figure out how to incapsulate binding callbacks
    public Story GetStory()
    {
        return InkStateHandler.InitializeOrGet(inkJSON);
    }
    

    void Awake()
    {
        story = GetStory();
    }

    void Start()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    public void InitAndStartDialogue(string knot)
    {
        //if (story == null)
        //{

        //    story = GetStory();
        //}

        dialoguePanel.SetActive(true);
        story.ChoosePathString(knot);
        ContinueStory();
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);

        canProcessChoices = false;
        StartCoroutine(EnableChoicesAfterDelay());

        story.ChoosePathString("cat_dialogue");
        ContinueStory();
    }

    public void StartCouchDialogue()
    {
        dialoguePanel.SetActive(true);
        story.ChoosePathString("cat_couch");
        ContinueStory();
    }

    public void StartFoodBowlDialogue()
    {
        dialoguePanel.SetActive(true);

        canProcessChoices = false;
        StartCoroutine(EnableChoicesAfterDelay());

        story.ChoosePathString("food_bowl");
        ContinueStory();
    }

    public void StartNightDialogue()
    {
        dialoguePanel.SetActive(true);
        isNightMode = true;

        if (dayPanel != null) dayPanel.SetActive(false);
        if (nightPanel != null) nightPanel.SetActive(true);
        if (catPortrait != null) catPortrait.gameObject.SetActive(false);

        story.ChoosePathString("night_1");
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (story.canContinue)
        {
            string text = story.Continue();
            HandleTags(story.currentTags);

            if (story.currentChoices.Count > 0)
            {
                text += "\n";
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    text += $"<link=\"choice_{i}\"><color=#FFD700>• {choice.text}</color></link>\n";
                }
            }
            else if (!story.canContinue)
            {
                text += "\n\n<color=#808080>Click anywhere to close</color>";
            }

            dialogueText.text = text;

            // Start delay after updating text
            canProcessChoices = false;
            StartCoroutine(EnableChoicesAfterDelay());
        }
        else if (story.currentChoices.Count == 0)
        {
            EndDialogue();
        }
    }

    private void HandleTags(List<string> tags)
    {
        if (tags.Count > 0 && catPortrait != null)
        {
            foreach (string tag in tags)
            {
                if (tag == "hide_cat")  
                {
                    catPortrait.gameObject.SetActive(false);
                    return;
                }
                else if (tag == "show_cat")
                {
                    catPortrait.gameObject.SetActive(true);
                }

                foreach (CatExpression expression in catExpressions)
                {
                    if (tag == expression.tagName)
                    {
                        catPortrait.sprite = expression.sprite;
                        catPortrait.gameObject.SetActive(true);
                        Debug.Log($"Changed cat expression to: {tag}");
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator EnableChoicesAfterDelay()
    {
        // if cursor is over an option when dialog is opened, it'll handle click on this option
        yield return new WaitForSeconds(0.2f); // Increased delay slightly
        canProcessChoices = true;
    }

    public void HandleClick()
    {
        if (!canProcessChoices) return;

        Vector3 mousePosition = Input.mousePosition;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(dialogueText, mousePosition, Camera.main);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = dialogueText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            if (linkID.StartsWith("choice_"))
            {
                string choiceStr = linkID.Substring(7);
                if (int.TryParse(choiceStr, out int choiceIndex))
                {
                    canProcessChoices = false; // Disable choices while processing
                    MakeChoice(choiceIndex);
                }
            }
        }
        else
        {
            if (!story.canContinue && story.currentChoices.Count == 0)
            {
                EndDialogue();
            }
            else if (story.canContinue && story.currentChoices.Count == 0)
            {
                canProcessChoices = false; // Disable choices while continuing
                ContinueStory();
            }
        }
    }

    private void MakeChoice(int choiceIndex)
    {
        //if (story.currentChoices[choiceIndex].text.Contains("Shoo away"))
        //{
        //    CatController cat = FindObjectOfType<CatController>();
        //    if (cat != null)
        //    {
        //        cat.ShooAway();
        //    }
        //    EndDialogue();
        //}
        //else
        //{
        //    story.ChooseChoiceIndex(choiceIndex);
        //    ContinueStory();
        //}
        story.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        IntroSceneManager introManager = FindObjectOfType<IntroSceneManager>();
        if (introManager != null)
        {
            Debug.Log("Found IntroSceneManager, calling OnDialogueEnd");
            introManager.OnDialogueEnd();
        }

        if (isNightMode)
        {
            isNightMode = false;
            if (dayPanel != null) dayPanel.SetActive(true);
            if (nightPanel != null) nightPanel.SetActive(false);
            if (catPortrait != null) catPortrait.gameObject.SetActive(true);
            if (nightModeManager != null)
            {
                nightModeManager.EndNightMode();
            }
        }
    }

    void Update()
    {
        if (statsText != null)
        {
            statsText.text = string.Format(
                "Food: {0:0}\nPlay: {1:0}\nPets: {2:0}\nDaylight: {3:0}", 
                Mathf.Round(InkStateHandler.GetFood()), 
                Mathf.Round(InkStateHandler.GetPlay()), 
                Mathf.Round(InkStateHandler.GetPets()),
                InkStateHandler.GetDaylight()
            );
        }
    }
}