using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isHardMode;

    public GameObject playerReference;
    public GameObject scannerPlayerReference;
    public TextMeshProUGUI itemTextUI;
    
    public bool hasKeyChest;
    public bool hasKeyDoor;
    public bool hasScanner;

    private Coroutine timerCoroutine;
    public float playthroughDuration = 90f;
    private bool isShowingPickup = false;
    public float fallDuration = 5f;
    
    public AudioClip getUpSound;
    private AudioSource sfxSource;

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

        isHardMode = GameSettings.isHardMode;
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.spatialBlend = 0f;
    }

    void Start()
    {
        StartLayoutTimer();
        sfxSource.PlayOneShot(getUpSound);
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
        scannerPlayerReference.SetActive(false);
        Collider playerCollider = playerReference.GetComponent<Collider>();
        Rigidbody playerRb = playerReference.GetComponent<Rigidbody>();
        Animator playerAnimator = playerReference.GetComponent<Animator>();
        PlayerController pc = playerReference.GetComponent<PlayerController>();

        if (pc != null) {
            pc.CloseMap();
            pc.enabled = false;
        }

        if (playerAnimator != null) playerAnimator.SetBool("NoClip", true);

        yield return new WaitForSeconds(fallDuration);

        if (AtmosphereManager.instance != null) AtmosphereManager.instance.ResetAtmosphere();

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

        if (getUpSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(getUpSound);
        }

        yield return new WaitForSeconds(2.5f);

        if (hasScanner)
        {
            scannerPlayerReference.SetActive(true);
        }
        if (pc != null) pc.enabled = true;

        RegisterDeath();
    }

    public void RegisterDeath()
    {
        ResetInventory();

        if (AtmosphereManager.instance != null) AtmosphereManager.instance.ResetAtmosphere();
        
        if (LayoutManager.instance != null) 
        {
            LayoutManager.instance.ResetMapElements();
            LayoutManager.instance.ChangeLayoutRandomly();
        }

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

    public void ResetInventory()
    {
        hasKeyChest = false;
        hasKeyDoor = false;
    }
}