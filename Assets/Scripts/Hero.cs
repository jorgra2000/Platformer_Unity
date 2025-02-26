using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private Sprite[] wolfIconsPU;

    [Header("Combat")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float radiusAttack;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private Transform powerUpPoint;
    [SerializeField] private AudioClip attackSound;

    private bool isAlive = true;
    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;
    private Color32 hitColor = new Color32(255, 117, 117, 255);
    private bool canPowerUpWolf;
    private bool canDoubleJump;
    private bool isJumping;
    private AudioSource audioSource;
    private bool canUseWolf = true;

    public bool IsAlive { get => isAlive; set => isAlive = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex == 3) 
        {
            powerUpIcon.sprite = wolfIconsPU[0];
        }

        if (SceneManager.GetActiveScene().buildIndex >= 2) 
        {
            canDoubleJump = true;
        }
        if(SceneManager.GetActiveScene().buildIndex >= 2) 
        {
            canPowerUpWolf = true;
        }
    }

    void Update()
    {
        if (isAlive) 
        {
            Movement();
            if (canDoubleJump) 
            {
                DoubleJump();
            }
            else 
            {
                Jump();
            }
            Attack();

            if (canPowerUpWolf) 
            {
                PowerUpWolf();
            }
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
        if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
            anim.SetTrigger("jump");
            isJumping = true;
        }
    }

    void DoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            isJumping = true;
        }
        else if (Input.GetKeyDown(KeyCode.X) && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * (jumpForce - 5), ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            isJumping = false;
        }
    }

    void PowerUpWolf() 
    {
        if (Input.GetKeyDown(KeyCode.Q) && IsGrounded() && canUseWolf)
        {
            anim.SetTrigger("wolf");
            Instantiate(wolfPrefab, powerUpPoint.position, transform.rotation);
            StartCoroutine(CooldownWolf());
        }

    }

    IEnumerator CooldownWolf() 
    {
        canUseWolf = false;
        powerUpIcon.sprite = wolfIconsPU[1];
        yield return new WaitForSeconds(20);
        canUseWolf = true;
        powerUpIcon.sprite = wolfIconsPU[0];
    }

    bool IsGrounded()
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
            audioSource.clip = attackSound;
            audioSource.Play();
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
            foreach(Image heart in hearts) 
            {
                heart.gameObject.SetActive(false);
            }
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
                StartCoroutine(gameManager.StartBossFight());
                Destroy(collision.gameObject);
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
