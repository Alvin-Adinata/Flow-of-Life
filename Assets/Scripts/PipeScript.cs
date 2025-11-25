using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public bool isBroken = false;
    public bool isFilled = false; // Status apakah ada air

    [Header("Material Pipa")]
    public Material matNormal; // Pipa putih/kering
    public Material matWater;    // Pipa Warna Biru
    public Material matBroken;  // Pipa Hitam/Pecah

    private MeshRenderer meshRenderer;

    void Awake() // Ganti Start jadi Awake agar lebih cepat dimuat
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        UpdateMaterial(); // Set warna awal
    }

    // Fungsi dipanggil saat Gempa
    public void BreakPipe()
    {
        if (!isBroken)
        {
            isBroken = true;
            UpdateMaterial();
        }
    }

    // Fungsi untuk Memperbaiki (Dipanggil saat player mengganti pipa)
    public void RepairPipe()
    {
        isBroken = false;
        isFilled = false; // Reset jadi kosong dulu
        UpdateMaterial();
    }

    // Fungsi BARU: Mengatur apakah pipa ini kena air
    public void SetWater(bool state)
    {
        // Jika pipa rusak, air tidak bisa mengalir (tetap dianggap tidak terisi visualnya)
        if (isBroken) return; 

        // Hanya update jika status berubah biar hemat performa
        if (isFilled != state)
        {
            isFilled = state;
            UpdateMaterial();
        }
    }

    // Satu fungsi pusat untuk mengatur warna
    private void UpdateMaterial()
    {
        if (meshRenderer == null) return;

        if (isBroken)
        {
            meshRenderer.material = matBroken; // Hitam
        }
        else if (isFilled)
        {
            meshRenderer.material = matWater;   // Biru (Mengalir)
        }
        else
        {
            meshRenderer.material = matNormal; // Putih (Kering)
        }
    }
}