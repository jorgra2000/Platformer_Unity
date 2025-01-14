using UnityEngine;

public class HellGato : Ghost
{
    void Update()
    {
        LookAtPoint();
    }

    void LookAtPoint() 
    {
        if (TargetPosition.x > transform.position.x) 
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else 
        {
            transform.rotation = Quaternion.Euler(0f,0f,0f);
        }
    }
}
