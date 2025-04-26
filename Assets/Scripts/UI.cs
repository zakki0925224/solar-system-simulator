using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public GameObject UIPrefab;
    public Simulator Simulator;

    private UIDocument uIDocument;

    void Start()
    {
        var uiObject = Instantiate(this.UIPrefab);
        this.uIDocument = uiObject.GetComponent<UIDocument>();
    }

    void Update()
    {
        var dateTime = this.Simulator.DateTime;
        var dateTimeLabel = this.uIDocument.rootVisualElement.Q<Label>("DateTimeLabel");
        dateTimeLabel.text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
