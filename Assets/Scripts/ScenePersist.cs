using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour {

    int startingSceneIndex;

    private void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        startingSceneIndex = FindObjectOfType<LevelLoader>().GetSceneIndex();
	}
	
	// Update is called once per frame
	void Update ()
    {
        int currentSceneIndex = FindObjectOfType<LevelLoader>().GetSceneIndex();

        if (currentSceneIndex != startingSceneIndex)
            Destroy(gameObject);

    }
}
