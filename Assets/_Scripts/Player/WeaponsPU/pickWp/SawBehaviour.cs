using UnityEngine;

public class SawBehaviour : MonoBehaviour
{
    [SerializeField] public float radius = 3;
    [SerializeField] public float speed = 100f; 
    [SerializeField] public float initialAngleOffset ;

    private float angle;
    public PlayerController playerController;
    public UnlockableWeapons weapon;
    public void SetPlayerReference(PlayerController controller)
    {
        playerController = controller;
    }

    private void Start()
    {
        if (playerController != null)
        {
            weapon = playerController.GetComponent<UnlockableWeapons>();
            if (weapon == null)
            {
                Debug.LogError("UnlockableWeapons no encontrado en el PlayerController.");
            }
        }
        else
        {
            Debug.LogError("PlayerController no asignado en SawBehaviour.");
        }

        angle = initialAngleOffset;
    }

    public void Update()
    {
        ExecuteBehaviour();
    }

    public void SetInitialPosition(Vector3 playerPosition, float angleOffset)
    {
        angle = angleOffset; 
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        transform.position = playerPosition + new Vector3(x, y, -1);
    }

    protected virtual void ExecuteBehaviour()
    {
        if (playerController == null)
            return;

        angle += speed * Time.deltaTime * 100;

        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

        transform.position = playerController.transform.position + new Vector3(x, y, -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy1 enemy = collision.GetComponent<Enemy1>();
        if (collision.gameObject.CompareTag("Enemy1"))
        {
            float damage =10 *weapon.sawLvl;
            if (damage > 10)
            {
                damage = 10;
            }
            enemy.TakeDamage(damage);
            Debug.Log("Hit");
        }
    }
}
