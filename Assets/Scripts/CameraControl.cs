using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject FollowerObject;
    public bool IsDisabled = false;

    private readonly float scrollSpeed = 50f;
    private readonly float scrollSmoothTime = 0.1f;
    private readonly float moveSpeed = 100f;
    private readonly float lookSpeed = 100f;
    private readonly float minDistanceWithFollower = 5f;
    private float distanceWithFollower = 5f;
    private float targetDistanceWithFollower = 5f;
    private float yaw = 0f;
    private float pitch = 0f;
    private InputAction scrollAction;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction rightClickAction;
    private Vector2 lookDelta;

    void Awake()
    {
        if (this.MainCamera == null)
        {
            Debug.LogWarning("CameraControl: MainCamera is not assigned. Attempting to find Camera.main");
            this.MainCamera = Camera.main;
            if (this.MainCamera == null)
            {
                Debug.LogError("CameraControl: Could not find MainCamera! Camera control will not work.");
                enabled = false;
                return;
            }
        }

        // scroll
        this.scrollAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/scroll");
        this.scrollAction.Enable();

        // move
        this.moveAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/w,<Keyboard>/s,<Keyboard>/a,<Keyboard>/d");
        this.moveAction.AddCompositeBinding("2DVector")
            .With("up", "<Keyboard>/w")
            .With("down", "<Keyboard>/s")
            .With("left", "<Keyboard>/a")
            .With("right", "<Keyboard>/d");
        this.moveAction.Enable();

        // look
        this.lookAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/delta");
        this.lookAction.Enable();

        // right click
        this.rightClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/rightButton");
        this.rightClickAction.Enable();

        // rotation angles
        var currentEuler = this.MainCamera.transform.eulerAngles;
        this.yaw = currentEuler.y;
        this.pitch = currentEuler.x;

        Debug.Log("CameraControl: Initialized successfully");
    }

    private void HandleFreeMode()
    {
        var baseTransform = this.MainCamera.transform;

        // scroll
        var scroll = this.scrollAction.ReadValue<Vector2>().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            var zoom = scroll * this.scrollSpeed;
            baseTransform.position += baseTransform.forward * zoom;
        }

        // move
        var move = this.moveAction.ReadValue<Vector2>();
        var moveDirection = baseTransform.TransformDirection(new Vector3(move.x, 0, move.y));
        baseTransform.position += moveDirection * this.moveSpeed * Time.deltaTime;

        // look
        if (this.rightClickAction.IsPressed())
        {
            this.lookDelta = this.lookAction.ReadValue<Vector2>();
            if (this.lookDelta != Vector2.zero)
            {
                var yawDelta = this.lookDelta.x * this.lookSpeed * Time.deltaTime;
                var pitchDelta = -this.lookDelta.y * this.lookSpeed * Time.deltaTime;

                this.yaw += yawDelta;
                this.pitch += pitchDelta;
                this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);

                baseTransform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0);
            }
        }
    }

    private void HandleFollowMode()
    {
        var baseTransform = this.MainCamera.transform;
        var followerPosition = this.FollowerObject.transform.position;

        // scroll
        var scroll = this.scrollAction.ReadValue<Vector2>().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            var zoom = scroll * this.scrollSpeed;
            this.targetDistanceWithFollower += zoom;
            this.targetDistanceWithFollower = Mathf.Clamp(this.targetDistanceWithFollower, this.minDistanceWithFollower, float.MaxValue);
        }

        // smooth interpolation
        this.distanceWithFollower = Mathf.Lerp(this.distanceWithFollower, this.targetDistanceWithFollower, this.scrollSmoothTime);

        // look
        if (this.rightClickAction.IsPressed())
        {
            this.lookDelta = this.lookAction.ReadValue<Vector2>();
            if (this.lookDelta != Vector2.zero)
            {
                var yawDelta = this.lookDelta.x * this.lookSpeed * Time.deltaTime;
                var pitchDelta = -this.lookDelta.y * this.lookSpeed * Time.deltaTime;

                this.yaw += yawDelta;
                this.pitch += pitchDelta;
                this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
            }
        }

        var rotation = Quaternion.Euler(this.pitch, this.yaw, 0);
        var offset = rotation * new Vector3(0, 5f, Math.Abs(this.distanceWithFollower));
        baseTransform.position = followerPosition + offset;
        baseTransform.LookAt(followerPosition);
    }

    void Update()
    {
        if (IsDisabled)
            return;

        if (this.FollowerObject != null)
        {
            HandleFollowMode();
        }
        else
        {
            HandleFreeMode();
        }
    }

    public void DisableInput()
    {
        this.scrollAction?.Disable();
        this.moveAction?.Disable();
        this.lookAction?.Disable();
        this.rightClickAction?.Disable();
    }

    public void EnableInput()
    {
        this.scrollAction?.Enable();
        this.moveAction?.Enable();
        this.lookAction?.Enable();
        this.rightClickAction?.Enable();
    }

    void OnDestroy()
    {
        this.scrollAction.Dispose();
        this.moveAction.Dispose();
        this.lookAction.Dispose();
    }
}
