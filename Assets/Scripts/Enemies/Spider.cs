using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{

    private bool canAttack = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (canAttack) 
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(Damage);
        }
        else if(collision.gameObject.CompareTag("PlayerDetection"))
        {
            animator.SetBool("run", true);
            canAttack = true;
        }
    }

}
