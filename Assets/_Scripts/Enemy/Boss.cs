using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 2f;
    public float jumpCooldown = 6f;
    public float jumpForce = 5f;
    public float meleeDamage = 10f;
    public float lifeStealAmount = 10f;
    public float damageRadius = 1.5f;
    public LayerMask playerLayer;

    public GameObject bulletPrefab; 
    public float bulletSpeed = 3f;
    public int bulletCount = 8;

    private bool isJumping = false;
    private bool canJump = true;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isMeleeActive = false;

    public Animator animator;

    public float health = 500f;
    public int maxHealth = 500;
    private void Start()
    {
        SoundManager.instance.PlaySound3D("BossEntrance", transform.position);
        health = 500;
        rb = GetComponent<Rigidbody2D>();
        playerTransform = PlayerController.instance.transform; 
        if (playerTransform == null)
        {
            Debug.LogError("No se encontró al jugador en la escena.");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        LookAtPlayer();

        if (canJump)
        {
            JumpAtPlayer();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        animator.SetBool("isAttacking", false);
    }

    private void JumpAtPlayer()
    {
        if (!canJump || isJumping) return;

        isJumping = true;
        canJump = false;

        Vector2 jumpDirection = (playerTransform.position - transform.position).normalized;
        rb.velocity = Vector2.zero; 
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f); 
        DetectPlayerOnLanding();
        isJumping = false;

        yield return new WaitForSeconds(jumpCooldown - 0.5f);
        canJump = true;
    }

    private void DetectPlayerOnLanding()
    {
        rb.velocity = Vector2.zero;

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer);
        if (hitPlayer != null)
        {
            PlayerController playerController = hitPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(20);

                Debug.Log("El jugador recibió daño del salto.");
            }
        }
        else
        {
            Debug.Log("El jugador no estaba en el radio del salto.");
            FireBullets(); 
        }
    }

    private void FireBullets()
    {
        SoundManager.instance.PlaySound3D("BossZap", transform.position);
        Debug.Log("Disparando balas en círculo.");
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0f) - transform.position;
            Vector2 bulletDir = new Vector2(bulletMoveVector.x, bulletMoveVector.y).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = bulletDir * bulletSpeed;

            angle += angleStep;
        }

        animator.SetBool("isAttacking", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMeleeActive = true;
            StartCoroutine(MeleeAttack(collision.GetComponent<PlayerController>()));
            
        }

        if (collision.CompareTag("PlBullet"))
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            controller.TakeDamage(20);
            Vector3 directionToPlayer = (transform.position - collision.transform.position).normalized;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMeleeActive = false;
        }
    }

    private IEnumerator MeleeAttack(PlayerController playerController)
    {
        while (isMeleeActive && playerController != null)
        {
            SoundManager.instance.PlaySound3D("BossBite", transform.position);
            playerController.TakeDamage((int)meleeDamage);

            HealBoss(lifeStealAmount);

            Debug.Log("Melee Attack: daño al jugador y absorción de vida.");
            yield return new WaitForSeconds(1f); 
        }
        animator.SetBool("isAttacking", false);
    }

    private void HealBoss(float amount)
    {
        Debug.Log($"Boss healed for {amount} health.");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took damage: {damage}. Current health: {health}");
        Die();
    }

    public void Die()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);

            SoundManager.instance.PlaySound3D("Splash", transform.position);
        }
    }
    
}


