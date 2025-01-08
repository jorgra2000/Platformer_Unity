using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    
    void Start()
    {
        StartCoroutine(hideText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator hideText() 
    {
        while (true) 
        {
            if (startText.gameObject.activeSelf)
            {
                startText.gameObject.SetActive(false);
            }
            else 
            {
                startText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(1);
        }
    }
}
