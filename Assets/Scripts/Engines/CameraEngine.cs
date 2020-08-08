using UnityEngine;

public class CameraEngine : MonoBehaviour
{
    private Camera mainCamera;

    private Vector2 cameraOriginalPosition;
    private float cameraOriginalOrthographicSize;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        cameraOriginalPosition = transform.position;
        cameraOriginalOrthographicSize = mainCamera.orthographicSize;
    }

    public void Foo_1()
    {
        
    }

    public void Foo_2()
    {
        
    }
}
