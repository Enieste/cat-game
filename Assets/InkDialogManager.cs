using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    private bool shouldStartNightDialogue = false;
    private bool nightStarted = false;

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

        // Check if we should start night dialogue
        if (GameManager.ShouldStartNightDialogue &&
            SceneManager.GetActiveScene().name == "Bedroom")
        {
            StartCoroutine(StartNightDialogueDelayed());
            GameManager.ShouldStartNightDialogue = false;
        }
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

    public void StartToyUnderTheCouchDialogue()
    {
        dialoguePanel.SetActive(true);

        canProcessChoices = false;
        StartCoroutine(EnableChoicesAfterDelay());

        story.ChoosePathString("toy_under_couch");
        ContinueStory();
    }

    public void StartTVDialogue()
    {
        dialoguePanel.SetActive(true);

        canProcessChoices = false;
        StartCoroutine(EnableChoicesAfterDelay());

        story.ChoosePathString("watch_tv");
        ContinueStory();
    }

    public void StartNightDialogue()
    {
        Debug.Log("StartNightDialogue called");
        dialoguePanel.SetActive(true);
        Debug.Log($"DialoguePanel activated: {dialoguePanel.activeSelf}");
        isNightMode = true;
        int currentDay = InkStateHandler.GetDate();
        Debug.Log($"Current day: {currentDay}");

        if (dayPanel != null)
        {
            dayPanel.SetActive(false);
            Debug.Log("Day panel deactivated");
        }
        if (nightPanel != null)
        {
            nightPanel.SetActive(true);
            Debug.Log("Night panel activated");
        }
        if (catPortrait != null)
        {
            catPortrait.gameObject.SetActive(false);
            Debug.Log("Cat portrait deactivated");
        }

        Debug.Log($"Choosing path: night_{currentDay}");
        story.ChoosePathString($"night_{currentDay}");
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
                text += "\n\n<color=#808080>Click the panel to continue</color>";
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
                InkStateHandler.SetNewDay();
                nightModeManager.EndNightMode();
                GameManager.NightStarted = false;
            }
            int currentDay = InkStateHandler.GetDate();
            if (currentDay <= 3)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    void Update()
    {
        if (InkStateHandler.IsNight() && !GameManager.NightStarted)
        {
            GameManager.NightStarted = true;
            StartNight();
        }

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

    private IEnumerator StartNightDialogueDelayed()
    {
        // Wait for a frame or two to ensure scene is fully setup
        yield return new WaitForSeconds(0.1f);
        shouldStartNightDialogue = false;
        StartNightDialogue();
    }

    public void StartNight()
    {
        if (InkStateHandler.IsNight())
        {
            GameManager.ShouldStartNightDialogue = true;
            if (LoadingScreen.Instance != null)
            {
                LoadingScreen.Instance.LoadScene("Bedroom");
            }
            else
            {
                SceneManager.LoadScene("Bedroom");
            }
        }
    }

}