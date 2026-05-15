using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    public float speed=20;
    private float damage;
    private WeaponBase originWeapon;
    private Vector2 direction;

    public void Initialize(Vector2 direction, float speed, WeaponBase originWeapon)
    {
        this.direction = direction.normalized;
        this.speed = speed=20;
        this.originWeapon = originWeapon;

        if (originWeapon != null)
        {
            this.damage = originWeapon.GetDamage();

            if (this.damage == 0)
            {
                this.damage = 30;
            }
        }
    }

    void Update()
    {
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}"); 

        if (collision.gameObject.CompareTag("Enemy1"))
        {
            Enemy1 enemy = collision.GetComponent<Enemy1>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                Debug.Log("Enemy hit, damage applied: " + damage);
                Debug.Log($"{damage}");
                Destroy(this.gameObject);
            }
        }
    }
}
