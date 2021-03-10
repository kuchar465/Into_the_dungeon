using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelEntryController : MonoBehaviour
{
    public Object nextLevel;
    private GameObject Player;
    private GameObject PlayerUi;
    private GameObject teleport;
    private GameObject deathScreen;
    [SerializeField] private string startLocName;

    void OnTriggerEnter2D(Collider2D exit) {
        StartCoroutine(LoadYourAsyncScene());
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerUi = GameObject.Find("PlayerUi");
        deathScreen = GameObject.Find("DeathScreen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadYourAsyncScene()
    {
        
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel.name, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.MoveGameObjectToScene(Player, SceneManager.GetSceneByName(nextLevel.name));
        SceneManager.MoveGameObjectToScene(PlayerUi, SceneManager.GetSceneByName(nextLevel.name));
        SceneManager.MoveGameObjectToScene(deathScreen, SceneManager.GetSceneByName(nextLevel.name));
        SceneManager.UnloadSceneAsync(currentScene);
        teleport = GameObject.Find(startLocName);
        Player.transform.position = teleport.transform.position;
    }
}
