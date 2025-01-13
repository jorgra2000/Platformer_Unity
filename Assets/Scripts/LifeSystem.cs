using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private float lifes;

    public float Lifes { get => lifes; set => lifes = value; }

    public void GetDamaged(float damage) 
    {
        lifes -= damage;
        if (lifes <= 0) 
        {
            if (this.gameObject.CompareTag("Enemy"))
            {
                GetComponent<Enemy>().StartDeath();
            }
            else 
            {
                GetComponent<Hero>().GetDamaged();
                Debug.Log("Muerto");
            }
            
        }
        else 
        {
            if (this.gameObject.CompareTag("Enemy"))
            {
                //Animación daño
            }
            else
            {
                GetComponent<Hero>().GetDamaged();
            }
        }
    }
}
