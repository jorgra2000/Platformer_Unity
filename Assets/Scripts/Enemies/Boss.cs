using UnityEngine;
using UnityEngine.UI;

public class Boss : Ghost
{
    [SerializeField] private Image healthBar;

    private LifeSystem lifeSystem;
    private float startLifes;
    private bool startFight = false;

    public Image HealthBar { get => healthBar; set => healthBar = value; }
    public bool StartFight { get => startFight; set => startFight = value; }

    void Awake()
    {
        lifeSystem = GetComponent<LifeSystem>();
        startLifes = lifeSystem.Lifes;
    }

    protected virtual void Update()
    {
        LookAtPoint();
        UpdateHealthBar();
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

    void UpdateHealthBar() 
    {
        healthBar.fillAmount = lifeSystem.Lifes / startLifes;
    }
}
