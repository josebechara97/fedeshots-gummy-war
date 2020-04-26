using UnityEngine;

public class ExamplePlayerController : MonoBehaviour
{
    public float drunkPercentage = 0;
    public Color materialColor;
    public GameObject body;
    public GameObject bodyRenderer;
    public GameObject shotSFX;
    public GameObject shotPoint;
    public GameObject shotPrefab;
    private Rigidbody m_rigidbody;
    private Animator anim;
    public bool dead;
    public enum GummyBearAnimationStates
    {
        Idle,
        Shooting,
        Walking,
        Jumping,
        Falling,
        DrunkWalk0,
        DrunkWalk1,
        DrunkWalk2,
        DrunkWalk3,
        DrunkWalk4,
        DrunkWalk5,
        Dead
    }

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string jumpButton = "Jump";
    public string shootButton = "Jump2";
    public float speed = 20f;

    private float inputHorizontal;
    private float inputVertical;

    private GummyBearAnimationStates GetWalkAnimation()
    {
        if (drunkPercentage >= 15 * 6)
            return GummyBearAnimationStates.DrunkWalk5;
        if (drunkPercentage >= 15 * 5)
            return GummyBearAnimationStates.DrunkWalk4;
        if (drunkPercentage >= 15 * 4)
            return GummyBearAnimationStates.DrunkWalk3;
        if (drunkPercentage >= 15 * 3)
            return GummyBearAnimationStates.DrunkWalk2;
        if (drunkPercentage >= 15 * 2)
            return GummyBearAnimationStates.DrunkWalk1;
        if (drunkPercentage >= 15 * 1)
            return GummyBearAnimationStates.DrunkWalk0;

        return GummyBearAnimationStates.Walking;
    }

    void Awake()
    {
        bodyRenderer.GetComponent<Renderer>().material.color = materialColor;
        m_rigidbody = GetComponent<Rigidbody>();
        anim = body.GetComponent<Animator>();
        anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Idle);
    }

    void Update()
    {
        if (drunkPercentage > 99)
        {
            dead = true;
        }

        if (!dead)
        {

            //movement
            inputHorizontal = SimpleInput.GetAxis(horizontalAxis);
            inputVertical = SimpleInput.GetAxis(verticalAxis);
            MovementByArrows();

            Vector3 movementVector = new Vector3(-inputHorizontal, 0f, -inputVertical);
            transform.LookAt(movementVector.normalized + transform.position);


            if ((SimpleInput.GetButtonDown(jumpButton) && IsGrounded()) ||
                (Input.GetKeyDown(KeyCode.X) && IsGrounded()))
            {
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Jumping);
                m_rigidbody.AddForce(0f, 10f, 0f, ForceMode.Impulse);
            }
            //animations
            if (!IsGrounded())
            {
                transform.Translate(Vector3.forward * movementVector.magnitude * ((speed - (speed * (drunkPercentage / 100))) / 2) * Time.deltaTime);
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Falling);
            }
            else if (SimpleInput.GetButton(shootButton))
            {
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Shooting);
                shotSFX.SetActive(true);
                Instantiate(shotPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
            }
            else if (movementVector.magnitude > 0)
            {
                anim.SetInteger("AnimationState", (int)GetWalkAnimation());
                transform.Translate(Vector3.forward * movementVector.magnitude * (speed - (speed * (drunkPercentage / 100))) * Time.deltaTime);
            }
            else
            {
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Idle);
            }
            if (SimpleInput.GetButtonUp(shootButton))
            {
                shotSFX.SetActive(false);
            }
        }
        else
        {
            m_rigidbody.velocity = Vector3.zero;
            anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Dead);
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().LevelLost();
        }

    }

    void MovementByArrows()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputHorizontal = 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputVertical = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputVertical = -1;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            m_rigidbody.AddForce(collision.contacts[0].normal * 10f, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.75f);
    }
}