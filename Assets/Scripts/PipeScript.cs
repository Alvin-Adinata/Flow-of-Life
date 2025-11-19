using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public bool isBroken = false;

    [Header("Warna Pipa")]
    public Material matNormal; // Slot warna biru
    public Material matRusak;  // Slot warna hitam

    private MeshRenderer meshRenderer;

    void Start()
    {
        // PENTING: Pakai 'InChildren' agar bisa menemukan Mesh di dalam anak objek
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        // Cek keamanan supaya tidak error merah
        if (meshRenderer == null)
        {
            Debug.LogError("Gagal menemukan MeshRenderer di objek " + gameObject.name);
        }
        else
        {
            RepairPipe(); // Set warna awal
        }
    }

    // Fungsi dipanggil saat Gempa
    public void BreakPipe()
    {
        if (!isBroken && meshRenderer != null)
        {
            isBroken = true;
            meshRenderer.material = matRusak; // Ubah jadi Hitam
            Debug.Log("Pipa Pecah!");
        }
    }

    // Fungsi untuk Memperbaiki
    public void RepairPipe()
    {
        if (meshRenderer != null)
        {
            isBroken = false;
            meshRenderer.material = matNormal; // Ubah jadi Biru
        }
    }
}