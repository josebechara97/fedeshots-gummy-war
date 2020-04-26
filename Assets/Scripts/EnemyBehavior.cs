using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    public float drunkPercentage = 0;
    public float distanceToTarget;
    public float distanceToAttack;
    public Color color;
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

    public float speed = 10f;

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
        m_rigidbody = GetComponent<Rigidbody>();
        anim = body.GetComponent<Animator>();
        anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Idle);
        int r = Random.Range(0, 255);
        int g = Random.Range(0, 255);
        int b = Random.Range(0, 255);
        print(r + " " + g + " " + b);

        color = Random.ColorHSV();
        bodyRenderer.GetComponent<Renderer>().material.SetColor("_Color",color);
    }

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (drunkPercentage > 99)
        {
            dead = true;
        }

        if (!dead)
        {
            Vector3 targetPosition = target.transform.position;
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

            //animations
            if (!IsGrounded())
            {
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Falling);
            }
            else if (distanceToTarget <= distanceToAttack)
            {
                anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Shooting);
                shotSFX.SetActive(true);
                Instantiate(shotPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
            }
            else
            {
                shotSFX.SetActive(false);
                anim.SetInteger("AnimationState", (int)GetWalkAnimation());
                if (IsGrounded())
                    transform.Translate(Vector3.forward * (speed - (speed * (drunkPercentage / 100))) * Time.deltaTime);
                else transform.Translate(Vector3.forward * ((speed - (speed * (drunkPercentage / 100))) / 2) * Time.deltaTime);
            }

        }
        else
        {
            shotSFX.SetActive(false);
            m_rigidbody.velocity = Vector3.zero;
            anim.SetInteger("AnimationState", (int)GummyBearAnimationStates.Dead);
            Destroy(gameObject, 3f);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().EnemyDestroyed();
    }
}