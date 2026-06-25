using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenableBehaviour : MonoBehaviour
{
    public enum StructureType { Chest, Door }
    public StructureType structureType;
    public GameObject contentPrefab;
    public float spawnYOffset = 1.0f;

    private bool isOpened = false;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TryOpen()
    {
        if (isOpened) return;

        if (structureType == StructureType.Chest && GameManager.instance.hasKeyChest)
        {
            GameManager.instance.hasKeyChest = false;
            GameManager.instance.ShowItemUI("Chest unlocked !"); 
            
            OpenStructure();
            
            if (contentPrefab != null)
            {
                StartCoroutine(SpawnContentRoutine());
            }
        }
        else if (structureType == StructureType.Door && GameManager.instance.hasKeyDoor)
        {
            GameManager.instance.hasKeyDoor = false;
            GameManager.instance.ShowItemUI("Door unlocked !"); 
            
            OpenStructure();

            if (contentPrefab != null)
            {
                StartCoroutine(SpawnContentRoutine());
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
        Instantiate(contentPrefab, spawnPosition, transform.rotation);
    }
}