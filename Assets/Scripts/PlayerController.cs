using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject mapUI;
    
    public InputActionReference moveAction;
    public InputActionReference interactAction;
    public InputActionReference mapAction;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        interactAction.action.Enable();
        mapAction.action.Enable();
        
        interactAction.action.performed += OnInteract;
        mapAction.action.performed += OnToggleMap;
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        interactAction.action.Disable();
        mapAction.action.Disable();
        
        interactAction.action.performed -= OnInteract;
        mapAction.action.performed -= OnToggleMap;
    }

    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in colliders)
        {
            OpenableBehaviour openable = col.GetComponent<OpenableBehaviour>();
            if (openable != null)
            {
                
            }
        }
    }

    private void OnToggleMap(InputAction.CallbackContext context)
    {
        if (mapUI != null)
        {
            mapUI.SetActive(!mapUI.activeSelf);
        }
    }
}