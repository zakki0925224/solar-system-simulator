using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Camera Camera;

    private readonly float scrollSpeed = 100f;
    private readonly float moveSpeed = 100f;
    private readonly float lookSpeed = 100f;
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
    }

    void Update()
    {
        // scroll
        var scroll = this.scrollAction.ReadValue<Vector2>().y;
        var zoom = scroll * this.scrollSpeed;
        this.Camera.transform.position += this.Camera.transform.forward * zoom;

        // move
        var move = this.moveAction.ReadValue<Vector2>();
        var moveDirection = this.Camera.transform.TransformDirection(new Vector3(move.x, 0, move.y));
        this.Camera.transform.position += moveDirection * this.moveSpeed * Time.deltaTime;

        // look
        if (this.rightClickAction.IsPressed())
        {
            this.lookDelta = this.lookAction.ReadValue<Vector2>();
            if (this.lookDelta != Vector2.zero)
            {
                var yaw = this.lookDelta.x * this.lookSpeed * Time.deltaTime;
                var pitch = -this.lookDelta.y * this.lookSpeed * Time.deltaTime;
                this.Camera.transform.eulerAngles += new Vector3(pitch, yaw, 0);
            }
        }
    }

    void OnDestroy()
    {
        this.scrollAction.Dispose();
        this.moveAction.Dispose();
        this.lookAction.Dispose();
    }
}
