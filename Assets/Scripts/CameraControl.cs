using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Camera Camera;
    public GameObject FollowerObject;

    private readonly float scrollSpeed = 50f;
    private readonly float moveSpeed = 100f;
    private readonly float lookSpeed = 100f;
    private readonly float minDistanceWithFollower = 5f;
    private float distanceWithFollower = 5f;
    private Quaternion currentRotation;
    private InputAction scrollAction;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction rightClickAction;
    private Vector2 lookDelta;

    void Awake()
    {
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

        this.currentRotation = this.Camera.transform.rotation;
    }

    private void HandleFreeMode()
    {
        var baseTransform = this.Camera.transform;

        // scroll
        var scroll = this.scrollAction.ReadValue<Vector2>().y;
        var zoom = scroll * this.scrollSpeed;
        baseTransform.position += baseTransform.forward * zoom;

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
                var yaw = this.lookDelta.x * this.lookSpeed * Time.deltaTime;
                var pitch = -this.lookDelta.y * this.lookSpeed * Time.deltaTime;
                baseTransform.rotation *= Quaternion.Euler(pitch, yaw, 0);
            }
        }
    }

    private void HandleFollowMode()
    {
        var baseTransform = this.Camera.transform;
        var followerPosition = this.FollowerObject.transform.position;

        // scroll
        var scroll = this.scrollAction.ReadValue<Vector2>().y;
        var zoom = scroll * this.scrollSpeed;
        this.distanceWithFollower += zoom;
        this.distanceWithFollower = Mathf.Clamp(this.distanceWithFollower, this.minDistanceWithFollower, float.MaxValue);

        // look
        if (this.rightClickAction.IsPressed())
        {
            this.lookDelta = this.lookAction.ReadValue<Vector2>();
            if (this.lookDelta != Vector2.zero)
            {
                var yaw = this.lookDelta.x * this.lookSpeed * Time.deltaTime;
                var pitch = -this.lookDelta.y * this.lookSpeed * Time.deltaTime;
                this.currentRotation *= Quaternion.Euler(pitch, yaw, 0);
            }
        }

        var offset = this.currentRotation * new Vector3(0, 5f, Math.Abs(this.distanceWithFollower));
        baseTransform.position = followerPosition + offset;
        baseTransform.LookAt(followerPosition);
    }

    void Update()
    {
        if (this.FollowerObject != null)
        {
            HandleFollowMode();
        }
        else
        {
            HandleFreeMode();
        }
    }

    void OnDestroy()
    {
        this.scrollAction.Dispose();
        this.moveAction.Dispose();
        this.lookAction.Dispose();
    }
}
