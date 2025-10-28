using UnityEngine;

public class LensFlareEffect : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject TargetObject;
    public Texture2D FlareTexture;
    public float FlareSize = 100f;
    public float MaxBrightness = 1f;
    public Color FlareColor = Color.white;

    private bool isVisible = false;
    private Vector2 screenPosition;
    private float brightness = 0f;

    void Start()
    {
        if (this.MainCamera == null)
        {
            this.MainCamera = Camera.main;
        }

        if (this.TargetObject == null)
        {
            this.TargetObject = this.gameObject;
        }
    }

    void Update()
    {
        if (this.MainCamera == null || this.TargetObject == null)
        {
            return;
        }

        Vector3 worldPos = this.TargetObject.transform.position;
        Vector3 screenPos = this.MainCamera.WorldToScreenPoint(worldPos);

        // カメラの視界内にあるか確認
        this.isVisible = screenPos.z > 0 &&
                        screenPos.x >= 0 && screenPos.x <= Screen.width &&
                        screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (this.isVisible)
        {
            this.screenPosition = new Vector2(screenPos.x, screenPos.y);

            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            float distanceFromCenter = Vector2.Distance(this.screenPosition, screenCenter);
            float maxDistance = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 2f;

            this.brightness = Mathf.Clamp01(1f - (distanceFromCenter / maxDistance)) * this.MaxBrightness;

            Vector3 directionToTarget = worldPos - this.MainCamera.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (Physics.Raycast(this.MainCamera.transform.position, directionToTarget.normalized, out RaycastHit hit, distanceToTarget))
            {
                if (hit.collider.gameObject != this.TargetObject)
                {
                    this.brightness = 0f;
                }
            }
        }
        else
        {
            this.brightness = 0f;
        }
    }

    void OnGUI()
    {
        if (!this.isVisible || this.brightness <= 0 || this.FlareTexture == null)
        {
            return;
        }

        Color originalColor = GUI.color;

        float size = this.FlareSize * this.brightness * 1.5f;
        GUI.color = new Color(this.FlareColor.r, this.FlareColor.g, this.FlareColor.b, this.brightness * 0.9f);
        Rect flareRect = new Rect(
            this.screenPosition.x - size / 2f,
            Screen.height - this.screenPosition.y - size / 2f,
            size,
            size
        );
        GUI.DrawTexture(flareRect, this.FlareTexture);

        float coreSize = size * 0.4f;
        GUI.color = new Color(1f, 1f, 1f, this.brightness * 1.2f);
        Rect coreRect = new Rect(
            this.screenPosition.x - coreSize / 2f,
            Screen.height - this.screenPosition.y - coreSize / 2f,
            coreSize,
            coreSize
        );
        GUI.DrawTexture(coreRect, this.FlareTexture);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 directionToCenter = (screenCenter - this.screenPosition).normalized;

        for (int i = 1; i <= 5; i++)
        {
            float offset = i * 80f * this.brightness;
            Vector2 flarePos = this.screenPosition + directionToCenter * offset;
            float smallSize = this.FlareSize * 0.4f * this.brightness / i;

            Rect smallFlareRect = new Rect(
                flarePos.x - smallSize / 2f,
                Screen.height - flarePos.y - smallSize / 2f,
                smallSize,
                smallSize
            );

            GUI.color = new Color(this.FlareColor.r, this.FlareColor.g, this.FlareColor.b, this.brightness * 0.7f / i);
            GUI.DrawTexture(smallFlareRect, this.FlareTexture);
        }

        Color[] rainbowColors = new Color[]
        {
            new Color(1f, 0.3f, 0.3f, 1f),
            new Color(0.3f, 1f, 0.3f, 1f),
            new Color(0.3f, 0.3f, 1f, 1f),
            new Color(1f, 1f, 0.3f, 1f)
        };

        for (int i = 0; i < rainbowColors.Length; i++)
        {
            float offset = (i + 2) * 120f * this.brightness;
            Vector2 flarePos = this.screenPosition + directionToCenter * offset;
            float colorSize = this.FlareSize * 0.25f * this.brightness;

            Rect colorFlareRect = new Rect(
                flarePos.x - colorSize / 2f,
                Screen.height - flarePos.y - colorSize / 2f,
                colorSize,
                colorSize
            );

            GUI.color = new Color(rainbowColors[i].r, rainbowColors[i].g, rainbowColors[i].b, this.brightness * 0.5f);
            GUI.DrawTexture(colorFlareRect, this.FlareTexture);
        }

        GUI.color = originalColor;
    }
}
