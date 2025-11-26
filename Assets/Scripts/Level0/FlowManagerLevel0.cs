using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowManagerLevel0 : MonoBehaviour
{
    // Agar mudah diakses dari mana saja
    public static FlowManagerLevel0 instance;

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
        // 1. Kumpulkan semua pipa yang ada di scene
        PipeBehavior[] pipes = FindObjectsByType<PipeBehavior>(FindObjectsSortMode.None);

        // 2. Reset semua pipa jadi KERING (Putih)
        foreach (PipeBehavior pipe in pipes)
        {
            PipeScript visual = pipe.GetComponent<PipeScript>();
            if (visual != null) visual.SetWater(false);
        }

        // 3. Cari Pipa START
        GameObject startPipeObj = GameObject.FindGameObjectWithTag("StartPipe");
        if (startPipeObj != null)
        {
            PipeBehavior startBehavior = startPipeObj.GetComponent<PipeBehavior>();
            if (startBehavior != null)
            {
                // Mulai penelusuran
                FillPipe(startBehavior);
            }
        }
    }

    // Fungsi Rekursif aliran air
    void FillPipe(PipeBehavior currentPipe)
    {
        PipeScript visual = currentPipe.GetComponent<PipeScript>();

        // Stop jika pipa rusak atau sudah terisi
        if (visual.isBroken || visual.isFilled) return;

        // Isi pipa dengan air
        visual.SetWater(true);

        // Jika ini EndPipe → panggil LevelCompleted()
        if (currentPipe.CompareTag("EndPipe"))
        {
            if (GameLevel0Manager.instance != null)
            {
                GameLevel0Manager.instance.LevelCompleted();
            }
            return;
        }

        // Cek koneksi pipa (Atas,Kanan,Bawah,Kiri)
        bool[] myConnections = currentPipe.GetConnections();
        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        for (int i = 0; i < 4; i++)
        {
            if (myConnections[i])
            {
                Debug.DrawRay(currentPipe.transform.position, directions[i] * pipeDistance, Color.red, 0.1f);

                RaycastHit hit;
                if (Physics.Raycast(currentPipe.transform.position, directions[i], out hit, pipeDistance))
                {
                    PipeBehavior neighborPipe = hit.collider.GetComponent<PipeBehavior>();
                    if (neighborPipe != null)
                    {
                        int oppositeDir = (i + 2) % 4;
                        bool[] neighborConnections = neighborPipe.GetConnections();

                        if (neighborConnections[oppositeDir])
                        {
                            FillPipe(neighborPipe);
                        }
                    }
                }
            }
        }
    }
}
