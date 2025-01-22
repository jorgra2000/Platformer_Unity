using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hunter : Boss
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private int numberShoots;
    [SerializeField] Transform firePoint;
    [SerializeField] Projectile bulletPrefab;

    private Transform[] waypoints;
    private Animator animator;

    private bool isWalking;
    private bool isVertical = false;
    private bool hasJumped = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = Waypoints[positionIndex].position;
        StartCoroutine(StartFight());
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("a");
    }

    IEnumerator StartFight() 
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("start", true);
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(1f);
        while (!IsDeath)
        {
            
            while (transform.position != targetPosition && !IsDeath)
            {
                if (!isVertical)
                {
                    animator.SetBool("run", true);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
                }
                else 
                {
                    if (!hasJumped) 
                    {
                        animator.SetTrigger("jump");
                        hasJumped = true;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpSpeed * Time.deltaTime);
                }

                yield return null;
            }
            NewDestination();
            animator.SetBool("run", false);
            hasJumped = false;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < numberShoots; i++) 
            {
                ShootAnimation();
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForSeconds(1.5f);
            
        }
    }

    void ShootAnimation() 
    {
        animator.SetTrigger("shoot");
    }

    void Shoot() 
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }

    

    void NewDestination()
    {
        positionIndex++;
        if (positionIndex >= Waypoints.Length)
        {
            positionIndex = 0;
        }
        targetPosition = Waypoints[positionIndex].position;

        if((positionIndex % 2) == 0) 
        {
            isVertical = false;
        }
        else 
        {
            isVertical = true;
        }
    }
}
