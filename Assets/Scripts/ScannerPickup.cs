using UnityEngine;

public class ScannerPickup : MonoBehaviour
{
    public GameObject playerScannerGO;

    public void Pickup()
    {
        if (playerScannerGO != null)
        {
            playerScannerGO.SetActive(true);
        }
        
        GameManager.instance.ShowItemUI("Scanner found !");
        Destroy(gameObject);
    }
}