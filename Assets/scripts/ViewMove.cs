using UnityEngine;
using UnityEngine.InputSystem;

public class ViewMove : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset MapInput;

    [Header("Camera")]
    public Camera TargetCamera;
    public float MoveSpeed = 8f;
    public float ZoomSpeed = 1f;
    public float MinZoom = 2f;
    public float MaxZoom = 20f;

    InputAction moveAction;
    InputAction zoomAction;

    void Awake()
    {
        if (TargetCamera == null)
        {
            TargetCamera = GetComponent<Camera>();
        }

        if (TargetCamera == null)
        {
            TargetCamera = GetComponentInChildren<Camera>();
        }

        if (MapInput == null)
        {
            return;
        }

        InputActionMap mapView = MapInput.FindActionMap("MapView");
        if (mapView == null)
        {
            return;
        }

        moveAction = mapView.FindAction("Move");
        zoomAction = mapView.FindAction("Zoom");
    }

    void OnEnable()
    {
        moveAction?.Enable();
        zoomAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        zoomAction?.Disable();
    }

    void Update()
    {
        if (TargetCamera == null)
        {
            return;
        }

        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        if (moveAction == null)
        {
            return;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude <= 0f)
        {
            return;
        }

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        TargetCamera.transform.Translate(input * MoveSpeed * Time.deltaTime, Space.World);
    }

    void ZoomCamera()
    {
        if (zoomAction == null)
        {
            return;
        }

        float scroll = zoomAction.ReadValue<Vector2>().y;
        if (Mathf.Approximately(scroll, 0f))
        {
            return;
        }

        float zoomStep = Mathf.Sign(scroll) * ZoomSpeed;

        if (TargetCamera.orthographic)
        {
            TargetCamera.orthographicSize = Mathf.Clamp(
                TargetCamera.orthographicSize - zoomStep,
                MinZoom,
                MaxZoom);
            return;
        }

        TargetCamera.fieldOfView = Mathf.Clamp(
            TargetCamera.fieldOfView - zoomStep,
            MinZoom,
            MaxZoom);
    }
}
