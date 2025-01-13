using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float lifePoints;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private bool isDeath = false;

    public float Speed { get => speed; set => speed = value; }
    public bool IsDeath { get => isDeath; set => isDeath = value; }

    public void StartDeath() 
    {
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        isDeath = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(damage);
        }
    }
}
