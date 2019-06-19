using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Gravity))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject shot;
    [SerializeField] GameObject shotOrbit;
    [SerializeField] Image fuelBar;
    [SerializeField] ParticleSystem jetPackFire;
    [SerializeField] ParticleSystem jetPackSmoke;
    [SerializeField] Light jetPackLight;
    [SerializeField] float lookSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float backSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float chargeSpeed;
    [SerializeField] float shotSpeed;
    [SerializeField] string horizontalAxis;
    [SerializeField] string verticalAxis;
    [SerializeField] string jumpButton;
    [SerializeField] string fireButton;

    float xVelocity;
    float zVelocity;
    bool onGround;
    Vector3 moveAmount;
    Vector3 sideMoveAmount;
    Vector3 smoothMoveVelocity;
    Rigidbody rigidBody;
    Animator animator;
    Vector2 touchDeltaPosition;
    bool charging = false;

    void Awake()
    {
        Cursor.visible = false;
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputY = Input.GetAxisRaw(verticalAxis);
        float inputX = Input.GetAxisRaw(horizontalAxis);

        Vector3 moveDir = new Vector3(0, 0, inputY).normalized;
        float moveSpeed;
        if (inputY >= 0)
        {
            moveSpeed = walkSpeed;
        }
        else
        {
            moveSpeed = backSpeed;
        }
        Vector3 targetMoveAmount = moveDir * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButton(jumpButton) && fuelBar.fillAmount > .01f)
        {
            rigidBody.AddForce(transform.up * jumpForce / 2f + inputY * transform.forward * jumpForce / 2f);

            if (!jetPackFire.isPlaying || !jetPackSmoke.isPlaying)
            {
                jetPackFire.Play();
                jetPackSmoke.Play();
            }
            jetPackLight.enabled = true;
            fuelBar.fillAmount -= .01f;
        }
        else
        {
            jetPackFire.Stop();
            jetPackSmoke.Stop();
            jetPackLight.enabled = false;
        }

        if (Input.GetAxis(fireButton) > 0f && fuelBar.fillAmount > .6f)
        {
            fuelBar.fillAmount -= .6f;
            GameObject newShot = Instantiate(shot);
            GameObject newShotOrbit = Instantiate(shotOrbit);
            Transform shotTransform = newShot.GetComponent<Transform>();
            //Rigidbody shotRigidbody = newShot.GetComponent<Rigidbody>();

            shotTransform.position = transform.position + transform.up * 2f + transform.forward * 2f;
            newShotOrbit.transform.LookAt(newShot.transform, transform.up);
            newShot.transform.SetParent(newShotOrbit.transform);
            //shotTransform.forward = transform.forward;
            //shotRigidbody.AddForce(shotTransform.forward * shotSpeed);
        }

        animator.SetFloat("Forward", inputY, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", inputX, 0.1f, Time.deltaTime);

        if (charging && fuelBar.fillAmount < 1f)
        {
            fuelBar.fillAmount += chargeSpeed;
        }
    }

    void FixedUpdate()
    {
        //Quaternion wantedRotation = rigidBody.rotation * Quaternion.Euler(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);
        Quaternion wantedRotation = rigidBody.rotation * Quaternion.Euler(0, Input.GetAxis(horizontalAxis) * rotateSpeed, 0);
        rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, wantedRotation, 0.1f);

        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + localMove);
        //Vector3 localSideMove = transform.TransformDirection(sideMoveAmount) * Time.fixedDeltaTime;
        //rigidBody.MovePosition(rigidBody.position + localSideMove);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Light"))
        {
            charging = true;
        }
        if (collision.gameObject.CompareTag("Shot"))
        {
            Debug.Log("Hit");
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            charging = false;
        }
    }
}