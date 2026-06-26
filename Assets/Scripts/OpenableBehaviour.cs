using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class OpenableBehaviour : MonoBehaviour
{
    public enum StructureType { Chest, Door }
    public StructureType structureType;
    public GameObject contentPrefab;
    public float spawnYOffset = 1.0f;

    private bool isOpened = false;
    private Animator animator;
    private GameObject spawnedContent;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TryOpen()
    {
        if (isOpened) return;

        if (structureType == StructureType.Chest)
        {
            if (GameManager.instance.hasKeyChest)
            {
                GameManager.instance.hasKeyChest = false;
                GameManager.instance.ShowItemUI("Chest unlocked !"); 
                
                OpenStructure();
                
                if (contentPrefab != null)
                {
                    StartCoroutine(SpawnContentRoutine());
                }
            }
            else
            {
                GameManager.instance.ShowItemUI("You need the chest key !");
            }
        }
        else if (structureType == StructureType.Door)
        {
            if (GameManager.instance.hasKeyDoor)
            {
                GameManager.instance.hasKeyDoor = false;
                GameManager.instance.ShowItemUI("Door unlocked !"); 
                
                OpenStructure();

                if (contentPrefab != null)
                {
                    StartCoroutine(LoadCreditsRoutine());
                }
            }
            else
            {
                GameManager.instance.ShowItemUI("You need the door key !");
            }
        }
    }

    private void OpenStructure()
    {
        isOpened = true;
        animator.SetTrigger("Opening");
    }

    private IEnumerator SpawnContentRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        
        Vector3 spawnPosition = transform.position + new Vector3(0f, spawnYOffset, 0f);
        spawnedContent = Instantiate(contentPrefab, spawnPosition, transform.rotation);
    }

    private IEnumerator LoadCreditsRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Credits");
    }

    public void ResetState()
    {
        isOpened = false;

        if (spawnedContent != null)
        {
            Destroy(spawnedContent);
        }
        
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }
}