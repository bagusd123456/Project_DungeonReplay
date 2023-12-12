using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject _loadingScreen;
    [SerializeField] GameObject _gameoverScreen;
    [SerializeField] GameObject _winScreen;
    [SerializeField] Slider _healthSlider;
    [SerializeField] Image currentGun;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthbar(int currentHealth)
    {
        _healthSlider.value = currentHealth;
    }

    public void SwitchGunSprite(Sprite gunSprite)
    {
        currentGun.sprite = gunSprite;
    }

    public void StartGame()
    {
        StartCoroutine(LoadingNextScene("LevelPath"));
    }

    public IEnumerator GameOver()
    {
        _gameoverScreen.SetActive(true);

        yield return new WaitForSeconds(5f);

        StartCoroutine(LoadingNextScene("MainMenu"));
    }

    public void WinTheGame()
    {
        StartCoroutine(WinGame());
    }

    public IEnumerator WinGame()
    {
        _winScreen.SetActive(true);
        yield return new WaitForSeconds(5f);

        StartCoroutine(LoadingNextScene("MainMenu"));
    }

    IEnumerator LoadingNextScene(string sceneName)
    {
        _loadingScreen.SetActive(true);

        yield return new WaitForSeconds(2f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
