using UnityEngine;
using System.Collections; // Diperlukan untuk IEnumerator

public class PipeBehavior : MonoBehaviour
{
    // Tipe pipa, di-set di Inspector
    public enum PipeType { Straight, Curve }
    public PipeType type;

    [Header("Random Settings")]
    public bool randomizeOnStart = true;

    [Header("Lock Settings")]
    public bool isLocked = false; // Jika true, pipa tidak bisa diputar

    // Arah koneksi [Atas, Kanan, Bawah, Kiri]
    private bool[] connections = new bool[4];

    // Variabel rotasi
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 300f;
    private Quaternion targetRotation;
    private bool isRotating = false;

    // Pelacak rotasi logis (0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°)
    private int rotationSteps = 0;

    void Start()
    {
        // KHUSUS START & END PIPE — jangan acak, tapi hitung rotasi awal
        if (CompareTag("StartPipe") || CompareTag("EndPipe"))
        {
            float currentY = transform.eulerAngles.y;
            rotationSteps = Mathf.RoundToInt(currentY / 90f) % 4;
            targetRotation = transform.rotation;

            UpdateConnections();
            return;
        }

        // PIPE YANG BOLEH DIACAK
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
            // PIPE BARU (PLAYER BUILD)
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
                    // CEGAH START PIPE DAN END PIPE DIHAPUS
                    if (CompareTag("StartPipe") || CompareTag("EndPipe"))
                    {
                        Debug.Log("StartPipe dan EndPipe TIDAK bisa dihapus!");
                        return;
                    }

                    // Hapus pipa lain
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

        // Menentukan rotasi target 90 derajat selanjutnya
        targetRotation *= Quaternion.Euler(0f, rotationAngle, 0f);

        // Perbarui pelacak rotasi logis
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

        // Setelah rotasi selesai, update koneksi
        UpdateConnections();
        isRotating = false;
    }

    /// <summary>
    /// Memperbarui status koneksi berdasarkan tipe dan rotasi.
    /// [0] = Atas (+Z), [1] = Kanan (+X), [2] = Bawah (-Z), [3] = Kiri (-X)
    /// </summary>
    private void UpdateConnections()
    {
        connections = new bool[4] { false, false, false, false };

        if (type == PipeType.Straight)
        {
            if (rotationSteps == 0 || rotationSteps == 2) // Vertikal
            {
                connections[0] = true; // Atas
                connections[2] = true; // Bawah
            }
            else // Horizontal
            {
                connections[1] = true; // Kanan
                connections[3] = true; // Kiri
            }
        }
        else if (type == PipeType.Curve)
        {
            switch (rotationSteps)
            {
                case 0: connections[0] = true; connections[1] = true; break; // Atas + Kanan
                case 1: connections[1] = true; connections[2] = true; break; // Kanan + Bawah
                case 2: connections[2] = true; connections[3] = true; break; // Bawah + Kiri
                case 3: connections[3] = true; connections[0] = true; break; // Kiri + Atas
            }
        }
    }

    public bool[] GetConnections()
    {
        return connections;
    }
}
