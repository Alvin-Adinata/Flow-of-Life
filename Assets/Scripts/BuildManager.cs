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

    public bool CanBuild { get { return PipeToBuild != null; } }

    public void BuildPipeOn(Tile_Prefab tile)
    {
        if (PlayerStat.Money < PipeToBuild.cost)
        {
            Debug.Log("Not enough money to build that pipe!");
            return;
        }

        PlayerStat.Money -= PipeToBuild.cost;

        GameObject pipe = (GameObject)Instantiate(PipeToBuild.prefab, tile.GetBuildPosition(), Quaternion.identity);
        tile.Pipe = pipe;

        Debug.Log("Pipe built! Money left: " + PlayerStat.Money);
    }

    private PipeBlueprint PipeToBuild;

    public void SelectPipeToBuild(PipeBlueprint pipe)
    {
        PipeToBuild = pipe;
    }
}
