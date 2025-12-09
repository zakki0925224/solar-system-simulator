using UnityEngine;
using UnityEngine.InputSystem;

public class MoonExplorer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = 1.62f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public Vector3 cameraOffset = new Vector3(0, 0.6f, 0);

    [Header("Flashlight Settings")]
    public Light flashlight;
    public float flashlightRange = 5f;
    public float flashlightIntensity = 2f;
    public float flashlightSpotAngle = 60f;
    public Color flashlightColor = Color.white;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            Debug.Log("MoonExplorer: CharacterController was automatically added.");
        }

        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
            groundCheck = groundCheckObj.transform;
            Debug.Log("MoonExplorer: GroundCheck was automatically created.");
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
            {
                playerCamera = FindObjectOfType<Camera>();

                if (playerCamera != null)
                {
                    Debug.Log($"MoonExplorer: Camera found by FindObjectOfType: {playerCamera.gameObject.name}");
                    if (playerCamera.gameObject.tag != "MainCamera")
                    {
                        playerCamera.gameObject.tag = "MainCamera";
                        Debug.Log("MoonExplorer: Set MainCamera tag to camera.");
                    }
                }
                else
                {
                    Debug.LogError("MoonExplorer: No camera found in scene!");
                }
            }
            else
            {
                Debug.Log("MoonExplorer: Camera.main was found and will be positioned.");
            }
        }

        if (playerCamera != null)
        {
            Debug.Log($"MoonExplorer: Setting up camera on {playerCamera.gameObject.name}");

            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = cameraOffset;
            playerCamera.transform.localRotation = Quaternion.identity;

            MoonMouseLook mouseLook = playerCamera.GetComponent<MoonMouseLook>();
            if (mouseLook == null)
            {
                mouseLook = playerCamera.gameObject.AddComponent<MoonMouseLook>();
                mouseLook.playerBody = transform;
                Debug.Log("MoonExplorer: MoonMouseLook was automatically added to camera.");
            }

            SetupFlashlight();
        }
        else
        {
            Debug.LogError("MoonExplorer: playerCamera is null! Flashlight cannot be setup.");
        }

        moveAction = new InputAction(type: InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("up", "<Keyboard>/w")
            .With("down", "<Keyboard>/s")
            .With("left", "<Keyboard>/a")
            .With("right", "<Keyboard>/d");
        moveAction.Enable();

        jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        jumpAction.Enable();

        sprintAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/shift");
        sprintAction.Enable();
    }

    void Start()
    {
        if (flashlight == null && playerCamera != null)
        {
            Debug.Log("MoonExplorer: Start() - Attempting to setup flashlight again.");
            SetupFlashlight();
        }
    }

    private void SetupFlashlight()
    {
        Debug.Log($"MoonExplorer: SetupFlashlight() called. playerCamera: {playerCamera != null}, flashlight: {flashlight != null}");

        if (flashlight == null && playerCamera != null)
        {
            Debug.Log($"MoonExplorer: Adding light to camera GameObject: {playerCamera.gameObject.name}");

            flashlight = playerCamera.GetComponent<Light>();
            if (flashlight == null)
            {
                flashlight = playerCamera.gameObject.AddComponent<Light>();
                Debug.Log("MoonExplorer: Light component added to camera.");
            }
            else
            {
                Debug.Log("MoonExplorer: Light component already exists on camera.");
            }

            flashlight.type = LightType.Spot;
            flashlight.range = flashlightRange;
            flashlight.intensity = flashlightIntensity;
            flashlight.spotAngle = flashlightSpotAngle;
            flashlight.color = flashlightColor;
            flashlight.shadows = LightShadows.Soft;
            flashlight.enabled = true;
            flashlight.cullingMask = ~0;

            Debug.Log($"MoonExplorer: Flashlight created - Type:{flashlight.type}, Range:{flashlightRange}, Intensity:{flashlightIntensity}, Enabled:{flashlight.enabled}");
        }
        else if (flashlight != null)
        {
            Debug.Log("MoonExplorer: Configuring existing flashlight.");
            flashlight.type = LightType.Spot;
            flashlight.range = flashlightRange;
            flashlight.intensity = flashlightIntensity;
            flashlight.spotAngle = flashlightSpotAngle;
            flashlight.color = flashlightColor;
            flashlight.enabled = true;
            flashlight.cullingMask = ~0;

            Debug.Log($"MoonExplorer: Existing flashlight configured - Range:{flashlightRange}, Intensity:{flashlightIntensity}");
        }
        else
        {
            Debug.LogWarning("MoonExplorer: Cannot setup flashlight - playerCamera is null!");
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (!isGrounded)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.1f, groundMask);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        float currentSpeed = sprintAction.IsPressed() ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDestroy()
    {
        moveAction?.Dispose();
        jumpAction?.Dispose();
        sprintAction?.Dispose();
    }
}
