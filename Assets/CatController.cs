using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private InkDialogueManager dialogueManager;
    private BoxCollider2D catCollider;

    public float moveSpeed = 2f;
    public float stoppingDistance = 0.5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float fixedY;

    public float foodDecayRate = 5f;
    public float playDecayRate = 3f;
    public float petsDecayRate = 3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogueManager = FindObjectOfType<InkDialogueManager>();
        catCollider = GetComponent<BoxCollider2D>();
        fixedY = transform.position.y;
        targetPosition = transform.position;

        if (dialogueManager != null)
        {
            InkStateHandler.Initialize(dialogueManager.inkJSON);
        }

        SetWalkingAnimation(false);
    }

    void Update()
    {
        // Update stats
        float deltaMinutes = Time.deltaTime / 60f;
        float currentFood = InkStateHandler.GetFood();
        float currentPlay = InkStateHandler.GetPlay();
        float currentPets = InkStateHandler.GetPets();
        int currentDaylight = InkStateHandler.GetDaylight();

        InkStateHandler.SetFood(currentFood - foodDecayRate * deltaMinutes);
        InkStateHandler.SetPlay(currentPlay - playDecayRate * deltaMinutes);
        InkStateHandler.SetPets(currentPets - petsDecayRate * deltaMinutes);

        int newDaylight = currentDaylight - 1;
        InkStateHandler.SetDaylight(newDaylight);

        // Movement update
        if (isMoving)
        {
            Vector3 currentTarget = new Vector3(targetPosition.x, fixedY, targetPosition.z);
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

            if (distanceToTarget > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

                if (targetPosition.x > transform.position.x)
                    spriteRenderer.flipX = false;
                else if (targetPosition.x < transform.position.x)
                    spriteRenderer.flipX = true;

                SetWalkingAnimation(true);
            }
            else
            {
                isMoving = false;
                SetWalkingAnimation(false);
            }
        }

        //prevent cat clicks if dialogue is active 
        if (dialogueManager != null && catCollider != null)
        {
            catCollider.enabled = !dialogueManager.dialoguePanel.activeSelf;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Cat clicked!");
        if (!isMoving && dialogueManager != null)
        {
            Debug.Log("Starting dialogue...");
            dialogueManager.StartDialogue();
        }
    }

    public void MoveTo(Vector3 position)
    {
        if (dialogueManager != null && !dialogueManager.dialoguePanel.activeSelf)
        {
            targetPosition = new Vector3(position.x, fixedY, position.z);
            isMoving = true;
        }
    }

    private void SetWalkingAnimation(bool walking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", walking);
        }
    }
}