using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private BoxCollider2D catCollider;
    [SerializeField] private HungerSystem hungerSystem;
    //private InkDialogueManager dialogueManager;
    [SerializeField] private InkDialogueManager dialogueManager;


    public float moveSpeed = 2f;
    public float stoppingDistance = 0.5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float fixedY;
    
    public float playDecayRate = 3f;
    public float petsDecayRate = 3f;

    [Header("Positions")]
    public Transform floorPosition;
    public Transform couchPosition;

    public bool isOnCouch = false;
    private static readonly string IS_LYING = "isLying";
    private static readonly string IS_WALKING = "isWalking";


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // dialogueManager = FindObjectOfType<InkDialogueManager>();
        catCollider = GetComponent<BoxCollider2D>();
        fixedY = transform.position.y;
        targetPosition = transform.position;

        if (Random.value < 0.5f && couchPosition != null)
        {
            PlaceOnCouch();
        }

        if (dialogueManager != null)
        {
            InkStateHandler.InitializeOrGet(dialogueManager.inkJSON);
        }

        SetWalkingAnimation(false);

        hungerSystem.OnActionOccurred += HandleHungerAction;
        
    }

    void Awake()
    {
        BindInkMethods();
    }
    
    private void HandleHungerAction(HungerAction action)
    {
        switch (action)
        {
            case HungerAction.Hungry:
                // Trigger meowing or other hungry behavior
                Debug.Log("Cat is getting hungry!");
                break;
            case HungerAction.Starving:
                // More desperate behavior
                Debug.Log("Cat is starving! Feed me now!");
                break;
        }
    }

    private void BindInkMethods()
    {
        UnbindInkMethods();
        dialogueManager.GetStory().BindExternalFunction("PutOnFloor", PutOnTheFloor);
        InkStateHandler.ReportExternalFunction("PutOnFloor");
    }

    private void UnbindInkMethods()
    {
        if (InkStateHandler.IsExternalFunctionKnown("PutOnFloor"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("PutOnFloor");
        }        

    }

    void Update()
    {
        
        // hungerSystem.ProcessTime(Time.deltaTime, ActivityType.Resting);
        
        // Movement update
        if (isMoving && !isOnCouch)
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
        if (dialogueManager != null)
        {
            if (isOnCouch)
            {
                dialogueManager.StartCouchDialogue();
            }
            else
            {
                dialogueManager.StartDialogue();
            }
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
            animator.SetBool(IS_WALKING, walking);
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void PlaceOnCouch()
    {
        transform.position = couchPosition.position;
        isOnCouch = true;
        //animator.SetBool(IS_LYING, true);
        //animator.SetBool(IS_WALKING, false);
    }

    public void PutOnTheFloor()
    {
        if (floorPosition != null)
        {
            //animator.SetBool(IS_LYING, false);
            isOnCouch = false;
            transform.position = floorPosition.position;
            targetPosition = transform.position;

            // cat walk a bit after being shooed
            Vector3 randomDirection = new Vector3(Random.Range(-2f, 2f), 0, 0);
            MoveTo(transform.position + randomDirection);
        }
    }
}