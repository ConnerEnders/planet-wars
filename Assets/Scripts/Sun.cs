using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        transform.LookAt(target);
    }
}
