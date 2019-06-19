using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Kick : MonoBehaviour
{
    [SerializeField] float force;
    Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Astronaut"))
        {
            rigidBody.AddForce(collision.transform.forward * force);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Shot"))
        {
            rigidBody.AddForce(collision.transform.forward * force);
        }
    }
}
