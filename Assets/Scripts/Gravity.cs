using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    public float gravity = -9.8f;
    public bool fixRotation = true;
    [SerializeField] Transform planet;
    Rigidbody rigidBody;
    Quaternion wantedRotation;
    SmoothCamera cameraScript;
    PlayerController playerController;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        if (fixRotation)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (planet == null)
        {
            planet = GameObject.FindWithTag("Planet").GetComponent<Transform>();
        }
    }

    void FixedUpdate()
    {
        Vector3 gravityUp = (rigidBody.position - planet.position).normalized;

        rigidBody.AddForce(gravityUp * gravity);

        if (fixRotation)
        {
            wantedRotation = Quaternion.FromToRotation(transform.up, gravityUp) * rigidBody.rotation;
            rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, wantedRotation, 0.1f);
        }
    }
}