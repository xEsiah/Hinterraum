using UnityEngine;

public class OpenableBehaviour : MonoBehaviour
{
    public enum StructureType { Chest, Door }
    public StructureType structureType;
    public GameObject contentPrefab;

    private bool isOpened = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isOpened || !collision.gameObject.CompareTag("Player")) return;

        if (structureType == StructureType.Chest && GameManager.instance.hasKeyChest)
        {
            GameManager.instance.hasKeyChest = false;
            if (contentPrefab != null)
            {
                Instantiate(contentPrefab, transform.position, transform.rotation);
            }
            OpenStructure();
        }
        else if (structureType == StructureType.Door && GameManager.instance.hasKeyDoor)
        {
            GameManager.instance.hasKeyDoor = false;
            OpenStructure();
        }
    }

    private void OpenStructure()
    {
        isOpened = true;
        Destroy(gameObject);
    }
}