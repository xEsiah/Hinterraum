using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public enum KeyType { Chest, Door }
    public KeyType keyType;

    public void Pickup()
    {
        if (keyType == KeyType.Chest) 
        {
            GameManager.instance.hasKeyChest = true;
            GameManager.instance.ShowItemUI("Chest key picked up");
        }
        else if (keyType == KeyType.Door)
        {
            GameManager.instance.hasKeyDoor = true;
            GameManager.instance.ShowItemUI("Door key picked up");
        }
        
        Destroy(gameObject);
    }
}