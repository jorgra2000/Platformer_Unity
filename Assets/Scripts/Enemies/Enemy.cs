using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Color32 hitColor = new Color32(255, 117, 117, 255);

    private bool isDeath = false;

    public float Speed { get => speed; set => speed = value; }
    public bool IsDeath { get => isDeath; set => isDeath = value; }
    public float Damage { get => damage; set => damage = value; }

    public void StartDeath() 
    {
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        isDeath = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<BoxCollider2D>().enabled = false;
        if (this.gameObject.CompareTag("Boss")) 
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().StartChangeLevel();
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    public void GetDamaged()
    {
         GetComponent<SpriteRenderer>().color = hitColor;
         Invoke(nameof(RestoreColor), 0.35f);
    }

    private void RestoreColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LifeSystem lifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(damage);
        }
    }
}
