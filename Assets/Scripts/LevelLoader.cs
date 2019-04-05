using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] float loadDelay = 3f;
    [SerializeField] float respawnDelay = 1f;
    int currentSceneIndex;
    bool sceneUpdated, isLoading = false;

    private void Awake()
    {
        int levelManagerCount = FindObjectsOfType<LevelLoader>().Length;
        if (levelManagerCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        SetSceneIndex();
    }

    private void Update()
    {
        if (!sceneUpdated)
            SetSceneIndex();
        if (currentSceneIndex == 0 && !isLoading) //for splash screen
        {
            isLoading = true;
            StartCoroutine(WaitAndLoadGameLevel(1, loadDelay));
        }

    }

    IEnumerator WaitAndLoadGameLevel(int loadIndex, float newLoadDelay)
    {
        yield return new WaitForSecondsRealtime(newLoadDelay);
        SceneManager.LoadScene(loadIndex);
        FindObjectOfType<MusicPlayer>().MusicChanger(loadIndex);
        sceneUpdated = false;
        Time.timeScale = 1f;
        isLoading = false;
    }

    public void BackToMainMenu()
    {
        StartCoroutine(WaitAndLoadGameLevel(1, loadDelay));
    }

    public void GameOver()
    {
        StartCoroutine(WaitAndLoadGameLevel(0, loadDelay * 3));
    }

    public void LoadNextlevel()
    {
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1) //prevents 'loading outside of build-index' error & goes back to splash atm
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex + 1, respawnDelay));
        else
            WaitAndLoadGameLevel(0, loadDelay);
    }

    public void LoadPreviousLevel()
    {
        if (currentSceneIndex < 1)
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex, loadDelay));
        else
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex - 1, loadDelay));
    }

    public void RestartLevel()
    {
        StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex, respawnDelay));
    }

    public int GetSceneIndex()
    {
        SetSceneIndex();
        return currentSceneIndex;
    }

    private void SetSceneIndex()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneUpdated = true;
    }
}
