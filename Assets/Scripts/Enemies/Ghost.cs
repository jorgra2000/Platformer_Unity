using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private Transform[] waypoints;

    private Vector3 targetPosition;
    private int positionIndex = 0;

    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Transform[] Waypoints { get => waypoints; set => waypoints = value; }

    void Start()
    {
        targetPosition = waypoints[positionIndex].position;
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol() 
    {
        yield return new WaitForSeconds(1f);
        while (!IsDeath)
        {
            while (transform.position != targetPosition && !IsDeath)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
                yield return null;
            }
            NewDestination();
        }
    }

    void NewDestination() 
    {
        positionIndex++;
        if(positionIndex >= waypoints.Length) 
        {
            positionIndex = 0;
        }
        targetPosition = waypoints[positionIndex].position;
    }
}
