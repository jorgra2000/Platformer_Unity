using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Hero player;
    [SerializeField] private GameObject gameOverMenu;
    [Header("Boss Fight")]
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private GameObject borderBoss;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform[] waypointsBoss;
    [SerializeField] private AudioClip bossFightSong;
    [SerializeField] private Image circleTransition;

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
        yield return new WaitForSeconds(2f);
        audioSource.clip = bossFightSong;
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab, bossPosition.position, Quaternion.identity);
    }

    public IEnumerator StartEscapeBoss() 
    {
        yield return new WaitForSeconds(2f);
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab, bossPosition.position, bossPosition.rotation);
    }

    public void StartChangeLevel() 
    {
        StartCoroutine(ChangeLevel());
    }

    public IEnumerator ChangeLevel() 
    {
        yield return new WaitForSeconds(0.5f);
        bossHealthBar.SetActive(false);
        //Sonido victoria
        yield return new WaitForSeconds(2f);
        circleTransition.GetComponent<Animator>().SetTrigger("endLevel");
        yield return new WaitForSeconds(1f);
        int maxScene = SceneManager.sceneCount;
        if((SceneManager.GetActiveScene().buildIndex + 1) >= maxScene) 
        {
            SceneManager.LoadScene(0);
        }
        else 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
