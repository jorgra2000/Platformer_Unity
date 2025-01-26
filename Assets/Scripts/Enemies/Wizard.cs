using System.Collections;
using UnityEngine;

public class Wizard : Enemy
{
    [SerializeField] private Projectile fireballPrefab;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private Transform spawnPosition;

    private Animator animator;
    private bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackRoutine() 
    {
        while (true) 
        {
            while (canAttack) 
            {
                animator.SetTrigger("fire");
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            yield return null;
        }
    }

    void ThrowAttack() 
    {
        Instantiate(fireballPrefab, spawnPosition.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(Damage);
        }
        else if (collision.gameObject.CompareTag("PlayerDetection")) 
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            canAttack = false;
        }
    }
}
