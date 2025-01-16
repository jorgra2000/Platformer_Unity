using System.Collections;
using UnityEngine;

public class Wizard : Enemy
{
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private Transform spawnPosition;

    private Animator animator;

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
            animator.SetTrigger("fire");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    void ThrowAttack() 
    {
        Instantiate(fireballPrefab, spawnPosition.position, transform.rotation);
    }
}
