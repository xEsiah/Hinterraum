using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public float mouseSensitivity = 20f;
    public Transform playerBody;
    public float minLookAngle = -20f;
    public float maxLookAngle = 20f;

    private float xRotation = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialRotation = transform.localRotation;
        xRotation = 85f;
    }

    void LateUpdate()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        transform.localRotation = initialRotation * Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}