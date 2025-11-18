using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public GameObject UIPrefab;
    public Simulator Simulator;
    public CameraControl CameraControl;
    public SurfaceCameraControl SurfaceCameraControl;

    private UIDocument uIDocument;
    private Label dateTimeLabel;
    private DropdownField speedScaleDropdown;
    private DropdownField cameraTargetDropdown;
    private Toggle surfaceCameraModeToggle;

    void Start()
    {
        var uiObject = Instantiate(this.UIPrefab);
        this.uIDocument = uiObject.GetComponent<UIDocument>();
        this.dateTimeLabel = this.uIDocument.rootVisualElement.Q<Label>("DateTimeLabel");
        this.speedScaleDropdown = this.uIDocument.rootVisualElement.Q<DropdownField>("SpeedScaleDropdown");
        this.cameraTargetDropdown = this.uIDocument.rootVisualElement.Q<DropdownField>("CameraTargetDropdown");
        this.surfaceCameraModeToggle = this.uIDocument.rootVisualElement.Q<Toggle>("SurfaceCameraModeToggle");

        this.speedScaleDropdown.RegisterValueChangedCallback(evt =>
        {
            this.Simulator.SpeedScaleFactor = evt.newValue switch
            {
                "Pause" => 0f,
                "Real time" => 1f,
                "1min/s" => 60f,
                "30min/s" => 1800f,
                "1h/s" => 3600f,
                "12h/s" => 43200f,
                "1d/s" => 86400f,
                "5d/s" => 432000f,
                "1w/s" => 604800f,
                "1mon/s" => 2592000f,
                "6mon/s" => 15552000f,
                "1y/s" => 31536000f,
                "5y/s" => 157680000f,
                "10y/s" => 315360000f,
                "50y/s" => 1576800000f,
                "100y/s" => 3153600000f,
                _ => 0f
            };
        });

        this.cameraTargetDropdown.RegisterValueChangedCallback(evt =>
        {
            var obj = GameObject.Find(evt.newValue);
            this.CameraControl.FollowerObject = obj;
        });

        this.surfaceCameraModeToggle.RegisterValueChangedCallback(evt =>
        {
            this.SurfaceCameraControl.SetActive(evt.newValue);

            if (evt.newValue)
            {
                this.cameraTargetDropdown.value = "Earth";
                this.cameraTargetDropdown.SetEnabled(false);

                var earthObj = GameObject.Find("Earth");
                if (earthObj != null)
                {
                    this.CameraControl.FollowerObject = earthObj;
                }
            }
            else
            {
                this.cameraTargetDropdown.SetEnabled(true);
            }
        });
    }

    void Update()
    {
        this.dateTimeLabel.text = this.Simulator.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
