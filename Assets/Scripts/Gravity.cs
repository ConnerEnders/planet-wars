using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    readonly float gravity = -9.8f;
    [SerializeField] Transform planet;
    Rigidbody rigidBody;
    Quaternion wantedRotation;
    SmoothCamera cameraScript;
    PlayerController playerController;

    void Start()
    {
        cameraScript = Camera.main.GetComponent<SmoothCamera>();
        playerController = this.gameObject.GetComponent<PlayerController>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        Vector3 gravityUp = (rigidBody.position - planet.position).normalized;
        Vector3 localUp = transform.up;

        // Apply downwards gravity
        rigidBody.AddForce(gravityUp * gravity);

        wantedRotation = Quaternion.FromToRotation(localUp, gravityUp) * rigidBody.rotation;
        rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, wantedRotation, 0.1f);
    }
}