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

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bossPrefab.Waypoints = waypointsBoss;
        bossPrefab.HealthBar = healthBar;
    }

    void Update()
    {
        if (!player.IsAlive) 
        {
            gameOverMenu.SetActive(true);
            RestartOrExitLevel();
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

    public bool CheckBossAlive() 
    {
        if(!bossPrefab.IsDeath)
        {
            Debug.Log("Vivo");
            return true;
        }
        else 
        {
            Debug.Log("Muerto");
            return false;
        }
    }

    public void StartChangeLevel() 
    {
        StartCoroutine(ChangeLevel());
    }

    public IEnumerator ChangeLevel() 
    {
        //Sonido victoria
        bossHealthBar.SetActive(false);
        yield return new WaitForSeconds(3f);
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch
        {
            SceneManager.LoadScene(0);
        }
    }
}
