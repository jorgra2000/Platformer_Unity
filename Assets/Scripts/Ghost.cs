using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private Transform[] waypoints;

    private Vector3 targetPosition;
    private int positionIndex = 0;

    void Start()
    {
        targetPosition = waypoints[positionIndex].position;
        StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Patrol() 
    {
        while (true) 
        {
            while (transform.position != targetPosition)
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
