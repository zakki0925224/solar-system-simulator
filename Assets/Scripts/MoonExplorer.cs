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
    public float flashlightRange = 15f;
    public float flashlightIntensity = 3f;
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
        }

        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
            groundCheck = groundCheckObj.transform;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
            {
                playerCamera = FindFirstObjectByType<Camera>();

                if (playerCamera != null)
                {
                    if (playerCamera.gameObject.tag != "MainCamera")
                    {
                        playerCamera.gameObject.tag = "MainCamera";
                    }
                }
            }
        }

        if (playerCamera != null)
        {
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = cameraOffset;
            playerCamera.transform.localRotation = Quaternion.identity;

            MoonMouseLook mouseLook = playerCamera.GetComponent<MoonMouseLook>();
            if (mouseLook == null)
            {
                mouseLook = playerCamera.gameObject.AddComponent<MoonMouseLook>();
                mouseLook.playerBody = transform;
            }

            SetupFlashlight();
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
            SetupFlashlight();
        }

        SetupDarkEnvironment();
    }

    private void SetupDarkEnvironment()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0.08f);

        Light[] directionalLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in directionalLights)
        {
            if (light.type == LightType.Directional)
            {
                light.intensity = 0.1f;
                light.color = new Color(0.7f, 0.7f, 0.8f);
            }
        }

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.02f, 0.02f, 0.03f);
        RenderSettings.fogDensity = 0.01f;
    }

    private void SetupFlashlight()
    {
        if (flashlight == null && playerCamera != null)
        {
            flashlight = playerCamera.GetComponent<Light>();
            if (flashlight == null)
            {
                flashlight = playerCamera.gameObject.AddComponent<Light>();
            }

            flashlight.type = LightType.Spot;
            flashlight.range = flashlightRange;
            flashlight.intensity = flashlightIntensity;
            flashlight.spotAngle = flashlightSpotAngle;
            flashlight.color = flashlightColor;
            flashlight.shadows = LightShadows.Soft;
            flashlight.enabled = true;
            flashlight.cullingMask = ~0;
        }
        else if (flashlight != null)
        {
            flashlight.type = LightType.Spot;
            flashlight.range = flashlightRange;
            flashlight.intensity = flashlightIntensity;
            flashlight.spotAngle = flashlightSpotAngle;
            flashlight.color = flashlightColor;
            flashlight.enabled = true;
            flashlight.cullingMask = ~0;
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
