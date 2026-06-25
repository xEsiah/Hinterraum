using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerReference;
    public TextMeshProUGUI itemTextUI;
    
    public bool hasKeyChest;
    public bool hasKeyDoor;

    private Coroutine timerCoroutine;
    public float playthroughDuration = 105f;
    private bool isShowingPickup = false;
    public float fallDuration = 5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartLayoutTimer();
    }

    public void StartLayoutTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(LayoutTimerRoutine());
    }

    private IEnumerator LayoutTimerRoutine()
    {
        yield return new WaitForSeconds(playthroughDuration);
        ForceDeath();
    }

    public void ForceDeath()
    {
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        Collider playerCollider = playerReference.GetComponent<Collider>();
        Rigidbody playerRb = playerReference.GetComponent<Rigidbody>();
        Animator playerAnimator = playerReference.GetComponent<Animator>();
        PlayerController playerController = playerReference.GetComponent<PlayerController>();

        if (playerController != null) playerController.enabled = false;
        if (playerAnimator != null) playerAnimator.SetBool("NoClip", true);

        yield return new WaitForSeconds(fallDuration);

        playerReference.transform.position = new Vector3(2f, 0.1f, 0f);
        
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        if (playerCollider != null) playerCollider.enabled = true;
        if (playerAnimator != null) 
        {
            playerAnimator.SetBool("NoClip", false);
            playerAnimator.SetTrigger("GetUp");
        }

        yield return new WaitForSeconds(2.5f);

        if (playerController != null) playerController.enabled = true;

        RegisterDeath();
    }

    public void RegisterDeath()
    {
        if (LayoutManager.instance != null) LayoutManager.instance.ChangeLayoutRandomly();

        StartLayoutTimer();
    }

    public void ShowItemUI(string itemName)
    {
        if (itemTextUI != null)
        {
            itemTextUI.text = itemName;
            isShowingPickup = true;
            StopCoroutine("ClearUIRoutine");
            StartCoroutine("ClearUIRoutine");
        }
    }

    private IEnumerator ClearUIRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        isShowingPickup = false;
        if (itemTextUI != null) itemTextUI.text = "";
    }

    public void ShowInteractPrompt(string message)
    {
        if (itemTextUI != null && !isShowingPickup)
        {
            itemTextUI.text = message;
        }
    }

    public void HideInteractPrompt()
    {
        if (itemTextUI != null && !isShowingPickup)
        {
            itemTextUI.text = "";
        }
    }
}