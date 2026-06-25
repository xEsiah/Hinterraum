using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject mapUI;
    public float interactRange = 3f;
    
    public InputActionReference moveAction;
    public InputActionReference interactAction;
    public InputActionReference mapAction;

    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        interactAction.action.Enable();
        mapAction.action.Enable();
        
        interactAction.action.started += OnInteract;
        mapAction.action.started += OnToggleMap;
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        interactAction.action.Disable();
        mapAction.action.Disable();
        
        interactAction.action.started -= OnInteract;
        mapAction.action.started -= OnToggleMap;
    }

    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        CheckInteractables();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        float currentSpeed = mapUI != null && mapUI.activeSelf ? moveSpeed * 0.5f : moveSpeed;
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void UpdateAnimations()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsRunning", isMoving);
    }

    private void CheckInteractables()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange, Physics.AllLayers, QueryTriggerInteraction.Collide);
        bool canInteract = false;

        foreach (Collider col in colliders)
        {
            if (col.GetComponentInParent<KeyPickup>() != null || col.GetComponentInParent<OpenableBehaviour>() != null || col.GetComponentInParent<ScannerPickup>() != null)
            {
                GameManager.instance.ShowInteractPrompt("Press E to interact");
                canInteract = true;
                break;
            }
        }

        if (!canInteract)
        {
            GameManager.instance.HideInteractPrompt();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange, Physics.AllLayers, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            KeyPickup key = col.GetComponentInParent<KeyPickup>();
            if (key != null)
            {
                key.Pickup();
                return; 
            }

            OpenableBehaviour openable = col.GetComponentInParent<OpenableBehaviour>();
            if (openable != null)
            {
                openable.TryOpen();
                return;
            }

            ScannerPickup scanner = col.GetComponentInParent<ScannerPickup>();
            if (scanner != null)
            {
                scanner.Pickup();
                return;
            }
        }
    }

    private void OnToggleMap(InputAction.CallbackContext context)
    {
        if (mapUI != null) mapUI.SetActive(!mapUI.activeSelf);
    }

    public void TriggerNoClipFall()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}