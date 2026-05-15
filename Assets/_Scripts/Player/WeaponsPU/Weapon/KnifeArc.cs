using UnityEngine;
using System.Collections;
public class KnifeArc : MonoBehaviour
{
    public float speed; // Velocidad del cuchillo
    public float radius; // Radio del movimiento en arco
    public float damage; // Daño del cuchillo

    private WeaponBase originWeapon; // El arma que dispara el cuchillo
    private bool isLeftDirection; // Dirección del movimiento: izquierda o derecha

    private Vector2 startPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        // Inicialización del daño, por ejemplo
        damage = 40;
    }

    // Método Initialize modificado para aceptar los parámetros correctos
    public void Initialize(bool isLeft, float knifeSpeed, float knifeRadius, WeaponBase weapon)
    {
        isLeftDirection = isLeft;  // Define si el cuchillo va hacia la izquierda o derecha
        speed = knifeSpeed;        // Establece la velocidad
        radius = knifeRadius;      // Establece el radio
        originWeapon = weapon;     // Asocia el arma de origen

        // Calcula las posiciones de inicio y final del movimiento en arco
        startPosition = transform.position;
        targetPosition = new Vector2(isLeft ? -radius : radius, 0);  // Define la dirección del movimiento

        // Empieza el movimiento
        StartCoroutine(MoveKnifeInArc());
    }

    private IEnumerator MoveKnifeInArc()
    {
        // Aquí nos encargamos del movimiento en arco
        Vector2 currentPosition = startPosition;
        Vector2 target = targetPosition;

        float journeyLength = Vector2.Distance(currentPosition, target);
        float startTime = Time.time;

        while (Vector2.Distance(currentPosition, target) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            currentPosition = Vector2.Lerp(startPosition, target, fractionOfJourney);
            transform.position = currentPosition;

            yield return null;  
        }

        transform.position = targetPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy1 enemy = collision.GetComponent<Enemy1>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);  
            }
        }
    }
}


