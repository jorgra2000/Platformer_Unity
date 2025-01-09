using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Skeleton enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            enemy.gameObject.SetActive(true);
            enemy.gameObject.transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }
}
