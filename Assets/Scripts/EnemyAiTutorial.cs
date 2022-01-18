using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    
    public int health;

    [SerializeField]private HealthBar heal;
    [SerializeField]private GameObject changeHeal;
    [SerializeField]private Transform posInstanceChangeHeal;
    [SerializeField]private Collider coll;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private Animator anim;
    //private Collider coll;
    private enum States{Idle,Walk,Attack,Hurt,Death};
    private States state;
    private bool isHurt=false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        heal.SetMaxHealth(health);
        anim=GetComponentInChildren<Animator>();
        //coll=GetComponentInChildren<Collider>();
        coll.enabled=false;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!isHurt)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            state=States.Walk;
            anim.SetInteger("States",(int)state);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        state=States.Walk;
        anim.SetInteger("States",(int)state);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            /*Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);*/
            coll.enabled=true;
            state=States.Attack;
            anim.SetInteger("States",(int)state);
            ///End of attack code
            Invoke(nameof(ResetState), 1f);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void ResetState()
    {
        state=States.Idle;
        anim.SetInteger("States",(int)state);
        coll.enabled=false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        heal.SetHealth(health);
        if (health <= 0)
        {
            state=States.Death;anim.SetInteger("States",(int)state);
            Invoke(nameof(DestroyEnemy), 3f);
        }
        else
        {
            StartCoroutine(Hurt());
        }
        GameObject dame= Instantiate(changeHeal, posInstanceChangeHeal.position, Quaternion.identity);
        dame.GetComponent<DameUI>().dgText.text=(0-damage).ToString();
    }
    private IEnumerator Hurt()
    {
        state=States.Hurt;anim.SetInteger("States",(int)state);isHurt=true;
        yield return new WaitForSeconds(0.5f);
        state=States.Idle;
        anim.SetInteger("States",(int)state);isHurt=false;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
