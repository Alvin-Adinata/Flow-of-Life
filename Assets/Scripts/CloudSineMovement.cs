using UnityEngine;

public class CloudSineMovement : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            startPos.x + Mathf.Sin(Time.time * speed) * amplitude,
            transform.position.y,
            transform.position.z
        );
    }
}
