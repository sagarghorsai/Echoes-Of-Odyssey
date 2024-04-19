using UnityEngine;

public class Skeleton : Enemy
{
    [Header("Inscribed: Skeleton")]
    public float speed = 2f;
    public float detectionRange = 5f;
    public float detectionAngle = 45f;
    public float wanderingRange = 3f;
    public float chaseDuration = 5f; // Duration of chasing in seconds
    public Transform player;
    private bool isFollowing = false;
    private Vector3 startPos;
    private Vector3 wanderTarget;
    public float attackRange = 1.5f;
    public int attackDamage = 1;
    private float runSpeed;
    private PlayerController playerController;
    private float chaseTimer = 0f; // Timer for tracking chase duration
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
        wanderTarget = GetRandomWanderTarget();
        runSpeed = speed;
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsRunning", false);

        playerController = player.GetComponent<PlayerController>();
    }


   
    private void FixedUpdate()
    {
        if (!isFollowing)
        {
            ScoutForPlayer();
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsRunning", false);
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                FollowPlayer();
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", true);
            }

            // Update chase timer and check if chase duration has been exceeded
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseDuration)
            {
                StopChasing();
            }
        }
    }



    private void ScoutForPlayer()
    {
        if (Vector3.Distance(transform.position, wanderTarget) < 0.1f)
        {
            wanderTarget = GetRandomWanderTarget();
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);

        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < detectionAngle && directionToPlayer.magnitude < detectionRange)
        {
            isFollowing = true;
            chaseTimer = 0f; // Reset chase timer when starting to chase
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);

        transform.position += transform.forward * speed * Time.deltaTime;
        speed = runSpeed * 1.5f;
    }

    private Vector3 GetRandomWanderTarget()
    {
        float randomX = Random.Range(startPos.x - wanderingRange, startPos.x + wanderingRange);
        float randomZ = Random.Range(startPos.z - wanderingRange, startPos.z + wanderingRange);
        return new Vector3(randomX, startPos.y, randomZ);
    }

    private void AttackPlayer()
    {
        anim.SetTrigger("Attack");
    }

    

    public void DealDamageToPlayer()
    {
            playerController.TakeDamage(attackDamage);
    }

    private void StopChasing()
    {
        isFollowing = false;
        chaseTimer = 0f; 
    }

   


}
