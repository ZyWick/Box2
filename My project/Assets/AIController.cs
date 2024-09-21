using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private Transform player; // Reference to the player character

    [SerializeField]
    private CharacterController aiController;

    private Animator anim;
    private Vector3 moveDirection;

    // AI parameters
    public float moveSpeed = 20f;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float health = 100f; // Current health
    public float maxHealth = 100f; // Maximum health
    public float stamina = 100f; // Current stamina
    public float maxStamina = 100f; // Maximum stamina
    public float retreatThreshold = 20f; // Health threshold to start retreating
    public float staminaCostPerAttack = 20f;
    public float staminaRegenRate = 10f; // Stamina regeneration per second

    private bool isInAttackRange = false;
    private bool canAttack = true;
    private bool isRetreating = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform; // Automatically find player if not assigned
        }
    }

    void Update()
    {
        // if (health <= 0)
        // {
        //     Die(); // Handle AI death
        //     return;
        // }

        // // Regenerate stamina over time
        // RegenerateStamina();

        // // Decide behavior based on health and stamina
        // DecideBehavior();

        // // Animate the AI
        // Animate();
    }

    private void DecideBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Determine if AI should be aggressive, defensive, or retreating
        if (health <= retreatThreshold && !isRetreating)
        {
            StartCoroutine(Retreat()); // Retreat if health is low
        }
        else if (distanceToPlayer < detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer(); // Follow player when in detection range but not in attack range
        }
        else if (distanceToPlayer <= attackRange && canAttack && stamina >= staminaCostPerAttack)
        {
            StartCoroutine(PerformAttack()); // Attack when in range and can attack
        }
        else
        {
            moveDirection = Vector3.zero; // Idle if not in any range
        }
    }

    private void FollowPlayer()
    {
        // Move towards player
        Vector3 direction = (player.position - transform.position).normalized;
        moveDirection = new Vector3(direction.x, 0, direction.z);
        aiController.Move(moveDirection * moveSpeed * Time.deltaTime);
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        // Make AI face the player
        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false; // Prevent immediate re-attacks

        // Deduct stamina for the attack
        stamina -= staminaCostPerAttack;

        // Choose a random attack
        int attackType = Random.Range(0, 2); // Assuming we have two attack types: jab (0) and cross (1)

        if (attackType == 0)
        {
            anim.SetTrigger("jab");
        }
        else
        {
            anim.SetTrigger("cross");
        }

        yield return new WaitForSeconds(1f); // Cooldown before next attack

        canAttack = true; // Re-enable attack after cooldown
    }

    private IEnumerator Retreat()
    {
        isRetreating = true;
        float retreatTime = 3f; // Retreat duration

        while (retreatTime > 0)
        {
            // Move away from the player
            Vector3 retreatDirection = (transform.position - player.position).normalized;
            moveDirection = new Vector3(retreatDirection.x, 0, retreatDirection.z);
            aiController.Move(moveDirection * moveSpeed * Time.deltaTime);

            retreatTime -= Time.deltaTime;
            yield return null;
        }

        isRetreating = false;
    }

    private void RegenerateStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina); // Keep stamina within bounds
    }

    private void Animate()
    {
        // Pass the input values to the animator to control blend tree
        anim.SetFloat("x", moveDirection.x);
        anim.SetFloat("y", moveDirection.z);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Keep health within bounds
    }

    private void Die()
    {
        anim.SetTrigger("die");
        // Add any other death-related logic here, such as disabling AI controls
    }
}