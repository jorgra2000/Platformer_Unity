using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Hero player;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private int nextScene;
    [SerializeField] private AudioClip gameOverSound, victorySound;
    [SerializeField] private TextMeshProUGUI endLevelText;
    [Header("Boss Fight")]
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private GameObject borderBoss;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform[] waypointsBoss;
    [SerializeField] private AudioClip bossFightSong;
    [SerializeField] private Image circleTransition;
    [SerializeField] private bool isSpawned;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bossPrefab.Waypoints = waypointsBoss;
        bossPrefab.HealthBar = healthBar;

        if(SceneManager.GetActiveScene().buildIndex == 2) 
        {
            StartCoroutine(StartEscapeBoss());
        }
    }

    void Update()
    {
        try
        {
            if (!player.IsAlive)
            {
                audioSource.Stop();
                audioSource.clip = gameOverSound;
                audioSource.Play();
                gameOverMenu.SetActive(true);
                RestartOrExitLevel();
            }
        }
        catch 
        {
            Debug.Log("MainMenu");
        }
    }

    void RestartOrExitLevel()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator StartBossFight() 
    {
        borderBoss.SetActive(true);
        audioSource.Stop();
        yield return new WaitForSeconds(1.5f);
        audioSource.clip = bossFightSong;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        bossHealthBar.SetActive(true);
        if (!isSpawned) 
        {
            Instantiate(bossPrefab, bossPosition.position, Quaternion.identity);
        }
        else 
        {
            bossPrefab.StartFight = true;
        }
    }

    public IEnumerator StartEscapeBoss() 
    {
        yield return new WaitForSeconds(1.5f);
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab, bossPosition.position, bossPosition.rotation);
        GameObject.Find("RunText").SetActive(false);
    }

    public void StartChangeLevel() 
    {
        StartCoroutine(ChangeLevel());
    }

    public IEnumerator ChangeLevel() 
    {
        yield return new WaitForSeconds(0.5f);
        bossHealthBar.SetActive(false);
        audioSource.Stop();
        audioSource.clip = victorySound;
        audioSource.loop = false;
        audioSource.Play();
        endLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        circleTransition.GetComponent<Animator>().SetTrigger("endLevel");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }
}
