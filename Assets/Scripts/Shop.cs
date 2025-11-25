using UnityEngine;

public class Shop : MonoBehaviour
{

    public PipeBlueprint straightPipe;
    public PipeBlueprint curvedPipe;

    BuildManager buildManager;

    void Start() {
        buildManager = BuildManager.instance;
    }

    public void SelectStraightPipe() {
        Debug.Log("Purchased Straight Pipe");
        buildManager.SelectPipeToBuild(straightPipe);
    }

    public void SelectCurvedPipe() {
        Debug.Log("Purchased Curved Pipe");
        buildManager.SelectPipeToBuild(curvedPipe);
    }
}
