using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateSpeed);
    }
}
