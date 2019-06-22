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
    [SerializeField] Light flashlight;
    [SerializeField] float rotateSpeed;
    [SerializeField] float backSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float chargeSpeed;
    [SerializeField] float dashMultiplier;
    [SerializeField] float dashEnergy;
    [SerializeField] float shotEnergy;
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
    float moveMultiplier = 1f;

    void Awake()
    {
        Cursor.visible = false;
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        move();
        shoot();
        dash();
        charge();
    }

    void FixedUpdate()
    {
        Quaternion wantedRotation = rigidBody.rotation * Quaternion.Euler(0, Input.GetAxis(horizontalAxis) * rotateSpeed * (moveMultiplier > 1 ? 2 : 1), 0);
        rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, wantedRotation, 0.1f);

        if (onGround)
        {
            Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + localMove);
        }
    }

    private void move()
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
        Vector3 targetMoveAmount = moveDir * moveSpeed * moveMultiplier;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        animator.SetFloat("Forward", inputY * (moveMultiplier > 1 ? 2 : 1), 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", inputX, 0.1f, Time.deltaTime);
    }

    private void shoot()
    {
        if (Input.GetAxis(fireButton) > 0f && fuelBar.fillAmount > .6f)
        {
            fuelBar.fillAmount -= shotEnergy;
            GameObject newShot = Instantiate(shot);
            GameObject newShotOrbit = Instantiate(shotOrbit);
            Transform shotTransform = newShot.GetComponent<Transform>();

            shotTransform.position = transform.position + transform.up * 2f + transform.forward * 2f;
            newShotOrbit.transform.LookAt(newShot.transform, transform.up);
            newShot.transform.SetParent(newShotOrbit.transform);
        }
    }

    private void dash()
    {
        if (Input.GetButton(jumpButton) && fuelBar.fillAmount > .005f)
        {
            rigidBody.AddForce(-transform.up * 50);
            moveMultiplier = dashMultiplier;

            if (!jetPackFire.isPlaying || !jetPackSmoke.isPlaying)
            {
                jetPackFire.Play();
                jetPackSmoke.Play();
            }
            jetPackLight.enabled = true;
            fuelBar.fillAmount -= dashEnergy;
        }
        else
        {
            moveMultiplier = 1f;
            jetPackFire.Stop();
            jetPackSmoke.Stop();
            jetPackLight.enabled = false;
        }
    }

    private void charge()
    {
        if (charging && fuelBar.fillAmount < 1f)
        {
            fuelBar.fillAmount += chargeSpeed;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            charging = true;
            flashlight.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Shot"))
        {
            rigidBody.AddForce((transform.position + transform.up * 3f - collision.transform.position) * 200);
        }
        if (collision.gameObject.CompareTag("Planet"))
        {
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            charging = false;
            flashlight.gameObject.SetActive(true);
        }
        if (collision.gameObject.CompareTag("Planet"))
        {
            onGround = false;
        }
    }
}