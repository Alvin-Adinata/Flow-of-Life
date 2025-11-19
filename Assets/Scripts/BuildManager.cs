using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    [SerializeField] public GameObject Straight_Tile_Prefab;
    [SerializeField] public GameObject Curved_Tile_Prefab;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }



    private GameObject PipeToBuild;

    public GameObject GetPipeToBuild()
    {
        return PipeToBuild;
    }

    public void setPipeToBuild(GameObject pipe)
    {
        PipeToBuild = pipe;
    }
}
