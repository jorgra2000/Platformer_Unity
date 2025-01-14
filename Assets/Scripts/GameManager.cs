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
    [SerializeField] private HellGato bossPrefab;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform[] waypointsBoss;

    void Start()
    {
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
        yield return new WaitForSeconds(1f);
        //Musica
        yield return new WaitForSeconds(3f);
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab, bossPosition.position, Quaternion.identity);
    }
}
