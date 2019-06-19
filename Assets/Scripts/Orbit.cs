using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float rotateSpeed;

    void FixedUpdate()
    {
        transform.Rotate(rotateSpeed, 0, 0);
    }
}
