using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    void FixedUpdate()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}