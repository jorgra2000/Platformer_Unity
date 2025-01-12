using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject borderBoss;
    [SerializeField] private Image[] hearts;

    [Header("Combat")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float radiusAttack;
    [SerializeField] private LayerMask damageLayer;

    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Jump();
        Attack();
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
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
        anim.SetTrigger("hurt");
        UpdateLifesGUI();
    }

    private void UpdateLifesGUI() 
    {
        float lifes = GetComponent<LifeSystem>().Lifes;

        for (int i = hearts.Length - 1; i + 1 > lifes; i--) 
        {
            hearts[i].gameObject.SetActive(false);
        }
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
    }
}
