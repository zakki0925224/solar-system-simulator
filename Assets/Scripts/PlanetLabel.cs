using UnityEngine;
using UnityEngine.UIElements;

public class PlanetLabel : MonoBehaviour
{
    public Camera MainCamera;
    public UIDocument UIDocument;
    public Transform TargetPlanet;
    public string PlanetName;
    public Vector3 Offset = new Vector3(0, 1.5f, 0);
    public SurfaceCameraControl SurfaceCameraControl;

    private Label label;
    private VisualElement labelContainer;

    void Start()
    {
        if (this.UIDocument == null)
        {
            return;
        }

        if (this.MainCamera == null)
        {
            this.MainCamera = Camera.main;
        }

        this.labelContainer = this.UIDocument.rootVisualElement.Q<VisualElement>("PlanetLabelsContainer");
        if (this.labelContainer == null)
        {
            this.labelContainer = new VisualElement
            {
                name = "PlanetLabelsContainer",
                style =
                {
                    position = Position.Absolute,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                    unityBackgroundImageTintColor = Color.clear
                }
            };
            this.labelContainer.pickingMode = PickingMode.Ignore;
            this.UIDocument.rootVisualElement.Add(this.labelContainer);
        }

        this.label = new Label(this.PlanetName)
        {
            style =
            {
                position = Position.Absolute,
                color = Color.white,
                fontSize = 14,
                unityTextOutlineWidth = 1,
                unityTextOutlineColor = Color.black,
                unityTextAlign = TextAnchor.MiddleCenter,
                backgroundColor = new Color(0, 0, 0, 0.5f),
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 2,
                paddingBottom = 2,
                borderTopLeftRadius = 3,
                borderTopRightRadius = 3,
                borderBottomLeftRadius = 3,
                borderBottomRightRadius = 3
            }
        };
        this.label.pickingMode = PickingMode.Ignore;
        this.labelContainer.Add(this.label);
    }

    void Update()
    {
        if (this.MainCamera == null || this.TargetPlanet == null || this.label == null)
            return;

        if (this.SurfaceCameraControl != null && this.SurfaceCameraControl.IsActive)
        {
            this.label.style.display = DisplayStyle.None;
            return;
        }

        var worldPosition = this.TargetPlanet.position + this.Offset;
        var screenPosition = this.MainCamera.WorldToScreenPoint(worldPosition);

        if (screenPosition.z < 0)
        {
            this.label.style.display = DisplayStyle.None;
            return;
        }

        var panelHeight = this.UIDocument.rootVisualElement.resolvedStyle.height;
        var uiPosition = new Vector2(screenPosition.x, panelHeight - screenPosition.y);

        this.label.style.display = DisplayStyle.Flex;
        this.label.style.left = uiPosition.x - this.label.resolvedStyle.width / 2;
        this.label.style.top = uiPosition.y - this.label.resolvedStyle.height / 2;
    }

    void OnDestroy()
    {
        if (this.label != null && this.labelContainer != null)
        {
            this.labelContainer.Remove(this.label);
        }
    }
}
