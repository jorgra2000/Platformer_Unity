using System.Collections;
using UnityEngine;

public class GhostBoss : Boss
{
    private Hero player;
    private Animator animator;

    private void Start()
    {
        player =GameObject.Find("Hero").GetComponent<Hero>();
        animator = GetComponent<Animator>();
        StartCoroutine(GoToWayPoint());
        StartCoroutine(PlayAnimationBoss());
    }

    private void Update()
    {
    }

    IEnumerator GoToWayPoint() 
    {
        yield return new WaitForSeconds(1f);

        while (transform.position != Waypoints[0].position && player.IsAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[0].position, Speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        GameObject.Find("GameManager").GetComponent<GameManager>().StartChangeLevel();
    }

    IEnumerator PlayAnimationBoss() 
    {
        while (transform.position != Waypoints[0].position) 
        {
            animator.SetTrigger("shout");
            yield return new WaitForSeconds(5f);
        }
    }
}
