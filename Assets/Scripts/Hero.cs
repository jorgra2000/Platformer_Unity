using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask floor;

    [Header("Other")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject borderBoss;
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Combat")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float radiusAttack;
    [SerializeField] private LayerMask damageLayer;

    private bool isAlive = true;
    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;
    private Color32 hitColor = new Color32(255, 117, 117, 255);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAlive) 
        {
            Movement();
            Jump();
            Attack();
        }
    }

    void Movement() 
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputH * speed, rb.velocity.y);

        if (inputH != 0)
        {
            anim.SetBool("running", true);
            if(inputH > 0) 
            {
                transform.eulerAngles = Vector3.zero;
            }
            else 
            {
                transform.eulerAngles = new Vector3(0, 180f, 0);
            }
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    void Jump() 
    {
        if (Input.GetKeyDown(KeyCode.X) && OntTheFloor())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    bool OntTheFloor()
    {
        return Physics2D.Raycast(feet.position, Vector3.down, 0.15f, floor) || 
            Physics2D.Raycast(feet.position + new Vector3(0.1f, 0f, 0f), Vector3.down, 0.15f, floor) ||
            Physics2D.Raycast(feet.position - new Vector3(0.1f, 0f, 0f), Vector3.down, 0.15f, floor);
    }

    void Attack() 
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("attack");
        }
    }

    void Damage() 
    {
        Collider2D[] collidersDamaged = Physics2D.OverlapCircleAll(AttackPoint.position, radiusAttack, damageLayer);
        foreach (Collider2D item in collidersDamaged) 
        {
            LifeSystem lifeSystem = item.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamaged(1);
        }
    }

    public void GetDamaged() 
    {
        if (isAlive) 
        {
            anim.SetTrigger("hurt");
            rb.sharedMaterial = null;
            GetComponent<SpriteRenderer>().color = hitColor;
            Invoke(nameof(RestoreColor), 0.35f);
            UpdateLifes();
        }
    }

    private void UpdateLifes() 
    {
        float lifes = GetComponent<LifeSystem>().Lifes;

        if(lifes <= 0) 
        {
            isAlive = false;
            anim.SetBool("death", true);
            hearts[0].gameObject.SetActive(false);
            gameOverMenu.SetActive(true);
        }
        else 
        {
            for (int i = hearts.Length - 1; i + 1 > lifes; i--)
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    private void RestoreColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderStart") || collision.CompareTag("BorderEnd")) 
        {
            virtualCamera.Follow = null;
            if (collision.CompareTag("BorderEnd")) 
            {
                borderBoss.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderStart"))
        {
            virtualCamera.Follow = this.gameObject.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttackPoint.position, radiusAttack);
        Gizmos.DrawLine(feet.position, feet.position - new Vector3(0, 0.15f, 0f));
    }
}
