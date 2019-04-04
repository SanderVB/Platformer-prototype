using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] AudioClip[] levelMusic;
    [SerializeField] AudioClip winSound, loseSound;
    [SerializeField] float soundVolume = .5f;

    private AudioSource musicPlayer;
    private AudioSource soundPlayer;

    private void Awake()
    {
        int musicPlayerCount = FindObjectsOfType<MusicPlayer>().Length;
        if (musicPlayerCount > 1)
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
        musicPlayer = GetComponents<AudioSource>()[0];
        soundPlayer = GetComponents<AudioSource>()[1];

        MusicChanger(FindObjectOfType<LevelLoader>().GetSceneIndex());
    }

    public void ResultSound(bool hasWon)
    {
        Debug.Log("speelt result geluidje: " + hasWon);
        if (hasWon)
            soundPlayer.clip = winSound;
        else
            soundPlayer.clip = loseSound;
        soundPlayer.PlayOneShot(soundPlayer.clip);
    }

    public void MusicChanger(int levelNumber) //changes music based on scene# being loaded
    {
        try
        {
            musicPlayer.clip = levelMusic[levelNumber];
            musicPlayer.Play();
        }
        catch { Debug.LogError("Add more level songs in the music player object"); }
        /*switch (levelNumber)
        {
            case 0:
                myAudioSource.clip = menuMusic;
                break;
            case 1:
                myAudioSource.clip = levelMusic;
                break;
            case 2:
                myAudioSource.clip = bossMusic;
                break;
            default:
                myAudioSource.clip = levelMusic;
                break;
        }*/
    }
}
