using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public enum KeyType { Chest, Door }
    public KeyType keyType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyType == KeyType.Chest) GameManager.instance.hasKeyChest = true;
            else if (keyType == KeyType.Door) GameManager.instance.hasKeyDoor = true;
            
            Destroy(gameObject);
        }
    }
}   