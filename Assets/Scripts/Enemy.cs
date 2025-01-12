using UnityEngine;
using static UnityEditor.Progress;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float lifePoints;
    [SerializeField] private float speed;

    public float Speed { get => speed; set => speed = value; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(1);
        }
    }
}
