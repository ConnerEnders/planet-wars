using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Image fuelBarImage;

    void Start()
    {
        fuelBarImage = (Image)GetComponent("Image");
        Color color = fuelBarImage.material.color;
        fuelBarImage.material.color = new Color(color.r, color.g, color.b, 1f);
    }

    private void Update()
    {
        Color color = fuelBarImage.material.color;
        fuelBarImage.material.color = new Color(color.r, color.g, color.b, Mathf.Sin(Time.time * 2f) / 16f + 15f / 16f);
    }

    private void OnDestroy()
    {
        Color color = fuelBarImage.material.color;
        fuelBarImage.material.color = new Color(color.r, color.g, color.b, 1f);
    }
}
