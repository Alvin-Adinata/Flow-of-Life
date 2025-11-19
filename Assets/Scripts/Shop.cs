using UnityEngine;

public class Shop : MonoBehaviour
{

    BuildManager buildManager;

    void Start() {
        buildManager = BuildManager.instance;
    }

    public void PurchaseStraightPipe() {
        Debug.Log("Purchased Straight Pipe");
        buildManager.setPipeToBuild(buildManager.Straight_Tile_Prefab);
    }

    public void PurchaseCurvedPipe() {
        Debug.Log("Purchased Curved Pipe");
        buildManager.setPipeToBuild(buildManager.Curved_Tile_Prefab);
    }
}
