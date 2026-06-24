using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerReference;
    
    public bool hasKeyChest;
    public bool hasKeyDoor;

    public int deathCount = 0;
    public event Action<int> OnDeathCountChanged;

    private Coroutine timerCoroutine;

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
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(LayoutTimerRoutine());
    }

    private IEnumerator LayoutTimerRoutine()
    {
        yield return new WaitForSeconds(105f);
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

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        yield return new WaitForSeconds(3f);

        playerReference.transform.position = new Vector3(2f, 0.15f, 0f);

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        RegisterDeath();
    }

    public void RegisterDeath()
    {
        deathCount++;
        OnDeathCountChanged?.Invoke(deathCount);

        if (LayoutManager.instance != null)
        {
            LayoutManager.instance.ChangeLayoutRandomly();
        }

        StartLayoutTimer();
    }
}