using System.Collections;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private float timeDeath;

    private bool canMove = false;

    void Start()
    {
        StartCoroutine(StartMoving());
        StartCoroutine(CountdownDeath());
    }

    void Update()
    {
        Movement();
    }

    void Movement() 
    {
        if (canMove && !IsDeath)
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
    }

    IEnumerator StartMoving() 
    {
        yield return new WaitForSeconds(0.8f);
        canMove = true;
    }

    IEnumerator CountdownDeath() 
    {
        yield return new WaitForSeconds(timeDeath);
        StartCoroutine(Death());
    }

}
