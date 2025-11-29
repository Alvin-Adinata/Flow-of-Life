using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowManager : MonoBehaviour
{
    // Agar mudah diakses dari mana saja
    public static FlowManager instance;

    // Jarak pengecekan antar pipa (sesuaikan dengan ukuran Grid kamu, misal 1 unit)
    public float pipeDistance = 1.0f; 

    // Daftar semua pipa di scene
    private List<PipeBehavior> allPipes = new List<PipeBehavior>();

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Kita cek aliran setiap frame
        CheckFlow();
    }

    // Fungsi utama pengecekan
    public void CheckFlow()
    {
        // 1. Kumpulkan semua pipa yang ada di scene (karena pipa bisa dihancurkan/dibangun ulang)
        //    Note: FindObjectsByType agak berat, untuk optimasi nanti bisa pakai List statis.
        //    Tapi untuk sekarang ini aman.
        PipeBehavior[] pipes = FindObjectsByType<PipeBehavior>(FindObjectsSortMode.None);
        
        // 2. Reset semua pipa jadi KERING (Putih) dulu
        foreach (PipeBehavior pipe in pipes)
        {
            PipeScript visual = pipe.GetComponent<PipeScript>();
            if (visual != null) visual.SetWater(false);
        }

        // 3. Cari Pipa START dan mulai alirkan air dari sana
        GameObject startPipeObj = GameObject.FindGameObjectWithTag("StartPipe");
        if (startPipeObj != null)
        {
            PipeBehavior startBehavior = startPipeObj.GetComponent<PipeBehavior>();
            if (startBehavior != null)
            {
                // Mulai penelusuran rekursif
                FillPipe(startBehavior);
            }
        }
    }

    // Fungsi Rekursif: "Isi pipa ini, lalu cek tetangganya"
    void FillPipe(PipeBehavior currentPipe)
    {
        // Ambil visual scriptnya
        PipeScript visual = currentPipe.GetComponent<PipeScript>();
        
        // Jika pipa ini Rusak atau Sudah Terisi Air sebelumnya, BERHENTI.
        // (Pencegahan infinite loop)
        if (visual.isBroken || visual.isFilled) return;

        // 1. Ubah warna jadi BIRU
        visual.SetWater(true);

        // Jika pipa yang baru saja dialiri air adalah "EndPipe"
        if (currentPipe.CompareTag("EndPipe"))
        {
            // Panggil fungsi di GameManager
            if (GameManager.instance != null)
            {
                // Kita bungkus di Invoke agar ada jeda sedikit (opsional) atau langsung panggil
                GameManager.instance.LevelCompleted();
            }
            return; // Selesai, tidak perlu cek tetangga lagi dari EndPipe
        }

        // 2. Cek koneksi pipa ini [Atas, Kanan, Bawah, Kiri]
        bool[] myConnections = currentPipe.GetConnections();

        // Arah vektor Unity: [Atas (Z+), Kanan (X+), Bawah (Z-), Kiri (X-)]
        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        // Loop ke 4 arah
        for (int i = 0; i < 4; i++)
        {
            // Jika pipa ini punya lubang ke arah 'i'
            if (myConnections[i])
            {
                // TAMBAHKAN BARIS INI UNTUK MELIHAT SINARNYA DI SCENE VIEW
                Debug.DrawRay(currentPipe.transform.position, directions[i] * pipeDistance, Color.red, 0.1f);
                
                // Tembakkan Raycast (sinar tak terlihat) ke arah itu untuk cari tetangga
                RaycastHit hit;
                // Start ray sedikit digeser agar tidak kena diri sendiri
                if (Physics.Raycast(currentPipe.transform.position, directions[i], out hit, pipeDistance))
                {
                    // Cek apakah yang kena adalah Pipa?
                    PipeBehavior neighborPipe = hit.collider.GetComponent<PipeBehavior>();
                    
                    if (neighborPipe != null)
                    {
                        // KITA KETEMU TETANGGA!
                        // Sekarang cek: Apakah tetangga punya lubang yang menghadap ke kita?
                        // Arah lawan: 0(Atas) vs 2(Bawah), 1(Kanan) vs 3(Kiri). Rumusnya: (i + 2) % 4
                        int oppositeDir = (i + 2) % 4;
                        bool[] neighborConnections = neighborPipe.GetConnections();

                        if (neighborConnections[oppositeDir])
                        {
                            // HORE! Sambungan cocok. Lanjut alirkan air ke tetangga.
                            FillPipe(neighborPipe);
                        }
                    }
                }
            }
        }
    }
}