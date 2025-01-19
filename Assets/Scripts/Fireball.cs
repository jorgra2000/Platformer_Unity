using UnityEngine;

public class Fireball : MonoBehaviour
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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(this.gameObject);
                LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
                lifeSystem.GetDamaged(3);
            }
        }
        else 
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
                lifeSystem.GetDamaged(1);
            }
        }
    }
}
