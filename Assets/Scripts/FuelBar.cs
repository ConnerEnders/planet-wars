using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Image fuelBarImage;
    [SerializeField] Material dimFuelMaterial;
    [SerializeField] Material brightFuelMaterial;

    void Start()
    {
        fuelBarImage = (Image)GetComponent("Image");
        Color color = fuelBarImage.material.color;
        fuelBarImage.material.color = new Color(color.r, color.g, color.b, 1f);
    }

    private void Update()
    {
        fuelBarImage.material = dimFuelMaterial;
        Color color = fuelBarImage.material.color;
        fuelBarImage.material.color = new Color(color.r, color.g, color.b, Mathf.Sin(Time.time * 2f) / 32f + 31f / 32f);
    }
}
