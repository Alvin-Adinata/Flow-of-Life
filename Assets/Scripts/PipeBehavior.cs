using UnityEngine;
using System.Collections;

public class PipeBehavior : MonoBehaviour
{
    public enum PipeType { Straight, Curve }
    public PipeType type;

    [Header("Random Settings")]
    public bool randomizeOnStart = true;

    [Header("Lock Settings")]
    public bool isLocked = false;

    // FLAG BARU → hanya pipa yang rusak (dari gempa) yang bisa dihancurkan
    public bool isBreakable = false;

    private bool[] connections = new bool[4];

    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 300f;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private int rotationSteps = 0;

    void Start()
    {
        // START & END PIPE → tidak acak
        if (CompareTag("StartPipe") || CompareTag("EndPipe"))
        {
            float currentY = transform.eulerAngles.y;
            rotationSteps = Mathf.RoundToInt(currentY / 90f) % 4;
            targetRotation = transform.rotation;

            UpdateConnections();
            return;
        }

        if (randomizeOnStart)
        {
            int randomRotations = Random.Range(0, 4);
            transform.Rotate(0f, rotationAngle * randomRotations, 0f);
            targetRotation = transform.rotation;
            rotationSteps = randomRotations;

            UpdateConnections();
        }
        else
        {
            targetRotation = transform.rotation;
            UpdateConnections();
        }
    }

    void Update()
    {
        // --- LOGIKA PENGHAPUSAN PIPA DENGAN TOMBOL 'E' ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // CEGAH Start & End Pipe
                    if (CompareTag("StartPipe") || CompareTag("EndPipe"))
                    {
                        Debug.Log("StartPipe & EndPipe tidak bisa dihancurkan!");
                        return;
                    }

                    // CEGAH pipa yang belum rusak
                    if (!isBreakable)
                    {
                        Debug.Log("Pipa ini belum rusak, jadi tidak bisa dihancurkan!");
                        return;
                    }

                    // HANCURKAN
                    Destroy(gameObject);
                }
            }
        }
        // --- AKHIR LOGIKA PENGHAPUSAN ---
    }

    private void OnMouseDown()
    {
        if (isLocked)
        {
            Debug.Log("Pipa terkunci, tidak bisa diputar.");
            return;
        }

        if (isRotating) return;

        targetRotation *= Quaternion.Euler(0f, rotationAngle, 0f);
        rotationSteps = (rotationSteps + 1) % 4;

        StartCoroutine(RotateSmooth());
    }

    private IEnumerator RotateSmooth()
    {
        isRotating = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRotation;
        UpdateConnections();
        isRotating = false;
    }

    private void UpdateConnections()
    {
        connections = new bool[4];

        if (type == PipeType.Straight)
        {
            if (rotationSteps == 0 || rotationSteps == 2)
            {
                connections[0] = true;
                connections[2] = true;
            }
            else
            {
                connections[1] = true;
                connections[3] = true;
            }
        }
        else if (type == PipeType.Curve)
        {
            switch (rotationSteps)
            {
                case 0: connections[0] = true; connections[1] = true; break;
                case 1: connections[1] = true; connections[2] = true; break;
                case 2: connections[2] = true; connections[3] = true; break;
                case 3: connections[3] = true; connections[0] = true; break;
            }
        }
    }

    public bool[] GetConnections()
    {
        return connections;
    }
}
