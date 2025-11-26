using System.Collections;
using UnityEngine;

public class EarthquakeManager : MonoBehaviour
{
    [Header("Waktu Gempa")]
    public float waktuMin = 15f;
    public float waktuMax = 30f;

    [Header("Efek Kamera")]
    public Transform kameraUtama; // Slot untuk Main Camera
    public float durasiGetar = 1.0f;
    public float kekuatanGetar = 0.2f;

    void Start()
    {
        StartCoroutine(LoopingGempa());
    }

    IEnumerator LoopingGempa()
    {
        while (true)
        {
            // 1. Tunggu waktu acak
            yield return new WaitForSeconds(Random.Range(waktuMin, waktuMax));

            // 2. Mulai Gempa (Getar & Rusak Pipa)
            StartCoroutine(GetarkanKamera());
            RusakkanSatuPipa();
        }
    }

    IEnumerator GetarkanKamera()
    {
        Vector3 posisiAsli = kameraUtama.localPosition;
        float waktu = 0f;

        while (waktu < durasiGetar)
        {
            // Acak posisi kamera
            float x = Random.Range(-1f, 1f) * kekuatanGetar;
            float y = Random.Range(-1f, 1f) * kekuatanGetar;

            kameraUtama.localPosition = new Vector3(posisiAsli.x + x, posisiAsli.y + y, posisiAsli.z);

            waktu += Time.deltaTime;
            yield return null;
        }
        // Kembalikan posisi
        kameraUtama.localPosition = posisiAsli;
    }

    void RusakkanSatuPipa()
    {
        Debug.Log("!!! GEMPA TERJADI !!!");

        PipeScript[] semuaPipa = FindObjectsByType<PipeScript>(FindObjectsSortMode.None);

        if (semuaPipa.Length == 0)
        {
            Debug.Log("Tidak ada pipa untuk dirusak.");
            return;
        }

        // Safety untuk mencegah loop tidak berakhir
        int percobaanMaks = 30;

        for (int i = 0; i < percobaanMaks; i++)
        {
            int acak = Random.Range(0, semuaPipa.Length);
            PipeScript target = semuaPipa[acak];

            // Jika bukan Start/End Pipe → rusakkan dan selesai
            if (!target.CompareTag("StartPipe") && !target.CompareTag("EndPipe"))
            {
                target.BreakPipe();
                return;
            }
        }

        Debug.Log("Gempa tidak menemukan pipa yang boleh dirusak (semua Start/End Pipe).");
    }
}