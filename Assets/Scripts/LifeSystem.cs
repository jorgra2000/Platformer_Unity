using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private float lifes;

    public void GetDamaged(float damage) 
    {
        lifes -= damage;
        if (lifes <= 0) 
        {
            Destroy(this.gameObject);
        }
    }
}
