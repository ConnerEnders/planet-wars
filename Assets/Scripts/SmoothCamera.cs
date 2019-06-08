using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] float distance;
    [SerializeField] float speed;
    [SerializeField] Transform target;
    Vector3 moveVelocity;

    void Start()
    {
        transform.position = target.position - target.forward * distance + target.up * height;
        transform.LookAt(target, target.up);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position - target.forward * distance + target.up * height, ref moveVelocity, speed);
        transform.LookAt(target, target.up);
    }
}