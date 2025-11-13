using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [Tooltip("Jika dicentang, semua pipa akan diputar acak saat game dimulai.")]
    public bool randomizePipesOnStart = true;

    [Tooltip("Lebar grid (informasi opsional, tidak digunakan otomatis).")]
    public int width = 10;

    [Tooltip("Tinggi grid (informasi opsional, tidak digunakan otomatis).")]
    public int height = 10;

    private void Start()
    {
        if (randomizePipesOnStart)
        {
            RandomizeAllPipes();
        }

        Debug.Log("Puzzle siap. Semua pipa diletakkan manual. Klik pipa untuk memutar.");
    }

    /// <summary>
    /// Mencari semua objek dengan script PipeBehavior dan memutar acak di awal.
    /// </summary>
    private void RandomizeAllPipes()
    {
        PipeBehavior[] allPipes = GetComponentsInChildren<PipeBehavior>();

        foreach (PipeBehavior pipe in allPipes)
        {
            // Tentukan jumlah rotasi acak (0–3 kali putaran)
            int randomRotations = Random.Range(0, 4);

            // Lakukan rotasi sebanyak randomRotations
            for (int i = 0; i < randomRotations; i++)
            {
                // Panggil event klik yang memicu rotasi halus
                pipe.SendMessage("OnMouseDown");
            }
        }

        Debug.Log($"Rotasi acak diterapkan ke {allPipes.Length} pipa.");
    }

    // =======================================================================
    // Di masa depan kamu bisa tambahkan fungsi berikut:
    // public void CheckForWin()
    // {
    //     // Logika untuk memeriksa apakah semua pipa sudah tersambung dengan benar.
    // }
    // =======================================================================
}
