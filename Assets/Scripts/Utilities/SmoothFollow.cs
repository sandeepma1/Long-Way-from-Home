using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] private float cameraSmooth = 0.1f;
    [SerializeField] private bool CameraClamp = false;
    [SerializeField] private float minX, maxX = 0;
    [SerializeField] private float minY, maxY = 0;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.transform.position;
    }

    private void Update()
    {
        if (player)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, cameraSmooth);
            transform.position = player.position + new Vector3(0, 0, -10);
        }
        if (CameraClamp)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                -10);
        }
    }
}