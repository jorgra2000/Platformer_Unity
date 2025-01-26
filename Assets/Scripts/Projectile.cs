using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeDestroy;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool isAlly;

    void Start()
    {
        Destroy(this.gameObject, timeDestroy);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlly) 
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
            {
                LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
                lifeSystem.GetDamaged(3);
                Destroy(this.gameObject);
            }
        }
        else 
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
                lifeSystem.GetDamaged(1);
                Destroy(this.gameObject);
            }
        }
    }
}
