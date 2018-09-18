using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineGameManager : MonoBehaviour {

    [SerializeField] private float startWait;
    [SerializeField] private float endWait;   
    public TMP_Text messageDisplay;
    [SerializeField] private Transform[] pickupSpawnPoints;  //Potential spawns
    [SerializeField] private int numberOfPickups;    // How many things are we spawning in?
    [SerializeField] private GameObject pickupPrefab; //What are we spawning in?
    [SerializeField] private FadeInUI transition;
    public EnemyAI[] ai_units;


    bool gameInProgress = true;

    GameObject gameWinner;

    public static OfflineGameManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {

        yield return StartCoroutine(RoundStarting());     
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        //Handle logic when we're done

    }

    private IEnumerator RoundStarting()
    {
        //Round starting Logic
        for (int i = 0; i < ai_units.Length; i++)
        {
            //ai_units[i].Init();
        }
        SpawnObjects();
        //messageDisplay.text = "Find the data files...";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
        gameInProgress = true;
        
        yield return new WaitForSeconds(startWait);

    }
    private IEnumerator RoundPlaying()
    {      
        yield return new WaitForSeconds(.5f);

        //messageDisplay.text = " ";
        gameInProgress = true;
        while (gameInProgress)
        {
             
            //for (int i = 0; i < ai_units.Length; i++)
            //{   
            //    ai_units[i].Tick();
            //}
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {

        //DISPLAY Game over screen
        //Play ending animation

        Debug.Log(gameWinner.name + " won");
        if (messageDisplay != null) {
            if (gameWinner.GetComponent<EnemyAI>() != null)
                messageDisplay.text = "Failure!";
            else if (gameWinner.GetComponent<PlayerPower>() != null)
                messageDisplay.text = "Success!";

        }
        yield return new WaitForSeconds(endWait);

        //transition.FadeOutCall(true);
        SceneManager.LoadScene(0);
        //End of round logic
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;


    }

    //Let's hold our spawned objects in a list for potential use later
    private List<GameObject> objectsSpawned = new List<GameObject>();

    private void SpawnObjects()
    {

        //Create a list that'll hold numbers. 1 number for each potential spawn point
        List<int> numbers = new List<int>(pickupSpawnPoints.Length);
        for (int i = 0; i < pickupSpawnPoints.Length; i++)
        {
            numbers.Add(i);
        }

        //An array of 4 random numbers picked from the list of numbers. 
        //Once a number is chosen, it is removed from the list so it cannot be chosen again
        var randNumbers = new int[numberOfPickups];
        for (int i = 0; i < randNumbers.Length; i++)
        {
            int thisNumber = Random.Range(0, numbers.Count);
            randNumbers[i] = numbers[thisNumber];
            numbers.RemoveAt(thisNumber);

        }

        for (int i = 0; i < numberOfPickups; i++)
        {
            var m_object = Instantiate(pickupPrefab ,pickupSpawnPoints[randNumbers[i]].position, pickupSpawnPoints[randNumbers[i]].localRotation) as GameObject;
            objectsSpawned.Add(m_object.gameObject);
        }
    }

    public void AssignWinner(GameObject winner)
    {
        gameWinner = winner;
        gameInProgress = false;
    }

}
