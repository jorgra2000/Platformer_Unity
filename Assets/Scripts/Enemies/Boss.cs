using UnityEngine;
using UnityEngine.UI;

public class Boss : Ghost
{
    [SerializeField] private Image healthBar;

    private LifeSystem lifeSystem;
    private float startLifes;

    public Image HealthBar { get => healthBar; set => healthBar = value; }

    void Awake()
    {
        lifeSystem = GetComponent<LifeSystem>();
        startLifes = lifeSystem.Lifes;
    }

    void Update()
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
