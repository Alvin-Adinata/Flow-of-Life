using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_Prefab : MonoBehaviour {

    [SerializeField] private Color hoverColor;
    private GameObject Pipe;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    void Start() {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    void OnMouseDown() {
        if (buildManager.GetPipeToBuild() == null) {
            Debug.Log("No pipe selected to build!");
            return;
        }

        if (Pipe != null) {
            Debug.Log("Can't build here!");
            return;
        }

        GameObject PipeToBuild = BuildManager.instance.GetPipeToBuild();
        Pipe = (GameObject)Instantiate(PipeToBuild, transform.position, transform.rotation);
    }

    void OnMouseEnter() {
        if (buildManager.GetPipeToBuild() == null) {
            Debug.Log("No pipe selected to build!");
            return;
        }
        rend.material.color = hoverColor;
    }


    void OnMouseExit() {
        rend.material.color = startColor;
    }
}