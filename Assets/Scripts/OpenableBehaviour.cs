using UnityEngine;

public class OpenableBehaviour : MonoBehaviour
{
    public enum StructureType { Chest, Door }
    public StructureType structureType;
    public GameObject contentPrefab;

    private bool isOpened = false;

    public void TryOpen()
    {
        if (isOpened) return;

        if (structureType == StructureType.Chest && GameManager.instance.hasKeyChest)
        {
            GameManager.instance.hasKeyChest = false;
            GameManager.instance.ShowItemUI("Chest unlocked !"); 
            
            if (contentPrefab != null)
            {
                Instantiate(contentPrefab, transform.position, transform.rotation);
            }
            OpenStructure();
        }
        else if (structureType == StructureType.Door && GameManager.instance.hasKeyDoor)
        {
            GameManager.instance.hasKeyDoor = false;
            GameManager.instance.ShowItemUI("Door unlocked !"); 
            
            OpenStructure();
        }
    }

    private void OpenStructure()
    {
        isOpened = true;
        Destroy(gameObject);
    }
}