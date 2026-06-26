using UnityEngine;

public class ScannerPickup : MonoBehaviour
{
    public GameObject playerScannerGO;
    public GameObject sphereMinimap;

    public void Pickup()
    {
        if (playerScannerGO != null)
        {
            playerScannerGO.SetActive(true);
            sphereMinimap.SetActive(false);
        }
        GameManager.instance.hasScanner = true;
        GameManager.instance.ShowItemUI("Scanner found !");
        Destroy(gameObject);
    }
}