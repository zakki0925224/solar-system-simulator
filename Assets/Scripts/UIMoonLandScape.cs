using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIMoonLandScape : MonoBehaviour
{
    public GameObject UIPrefab;

    private UIDocument uIDocument;
    private Button solarSystemModeButton;

    void Start()
    {
        var uiObject = Instantiate(this.UIPrefab);
        this.uIDocument = uiObject.GetComponent<UIDocument>();
        this.solarSystemModeButton = this.uIDocument.rootVisualElement.Q<Button>("SolarSystemModeButton");

        this.solarSystemModeButton.clicked += () =>
        {
            SceneManager.LoadScene("Scenes/Main");
        };
    }
}
