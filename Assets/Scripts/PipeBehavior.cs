using UnityEngine;
using System.Collections; // Diperlukan untuk IEnumerator

public class PipeBehavior : MonoBehaviour
{
    // Tipe pipa, di-set di Inspector
    public enum PipeType { Straight, Curve }
    public PipeType type;

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
        // Rotasi acak awal
        int randomRotations = Random.Range(0, 4);
        transform.Rotate(0f, rotationAngle * randomRotations, 0f);
        targetRotation = transform.rotation;
        rotationSteps = randomRotations;

        // Hitung koneksi awal
        UpdateConnections();
    }

    void Update()
    {
        // --- LOGIKA PENGHAPUSAN PIPA DENGAN TOMBOL 'E' ---
        
        // Cek apakah tombol 'E' ditekan
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Lakukan Raycast dari posisi mouse di layar
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Gunakan Physics.Raycast untuk melihat apakah kursor mouse mengenai objek
            // Collider pipa (perlu Collider terpasang!)
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Periksa apakah objek yang terkena raycast adalah game object ini
                if (hit.collider.gameObject == gameObject)
                {
                    // Hapus objek pipa
                    Destroy(gameObject);
                    
                    // Anda dapat menambahkan kode di sini untuk:
                    // - Memainkan suara atau efek visual penghapusan
                    // - Memberi tahu Puzzle/Game Manager bahwa sebuah pipa telah hilang
                }
            }
        }
        // --- AKHIR LOGIKA PENGHAPUSAN ---
    }
    
    private void OnMouseDown()
    {
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
        // Reset semua koneksi
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
                case 0: // Atas & Kanan
                    connections[0] = true;
                    connections[1] = true;
                    break;
                case 1: // Kanan & Bawah
                    connections[1] = true;
                    connections[2] = true;
                    break;
                case 2: // Bawah & Kiri
                    connections[2] = true;
                    connections[3] = true;
                    break;
                case 3: // Kiri & Atas
                    connections[3] = true;
                    connections[0] = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Fungsi publik yang dibaca oleh PuzzleManager.
    /// </summary>
    /// <returns>Array boolean [Up, Right, Down, Left]</returns>
    public bool[] GetConnections()
    {
        return connections;
    }
}