using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_Prefab : MonoBehaviour {

    [SerializeField] private Color hoverColor;
    [Header("Optional")]
    [SerializeField] public GameObject Pipe;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    void Start() {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition() {
        Vector3 positionOffset = new Vector3(0f, 0.2f, 0f);
        return transform.position + positionOffset;
    }

    void OnMouseDown() {
        if (!buildManager.CanBuild) {
            Debug.Log("No pipe selected to build!");
            return;
        }

        if (Pipe != null) {
            Debug.Log("Can't build here!");
            return;
        }

        buildManager.BuildPipeOn(this);


    }

    void OnMouseEnter() {
        if (!buildManager.CanBuild) {
            Debug.Log("No pipe selected to build!");
            return;
        }
        rend.material.color = hoverColor;
    }


    void OnMouseExit() {
        rend.material.color = startColor;
    }
}