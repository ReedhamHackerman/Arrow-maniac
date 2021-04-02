using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Rewired.Controller;

public class RoundSystemUI : MonoBehaviour
{
    private GameObject[] player1trophies;
    private GameObject[] player2trophies;
    [SerializeField] private GameObject player1Parent;
    [SerializeField] private GameObject player2Parent;
    private GameObject scoreTrophy; 
    private int trophySpawnDistance = 80;


    [SerializeField] private GameObject roundUI;
    private int winScore = 3;
    //private int maxRounds = 5;


    private void Awake()
    {
        player1trophies = new GameObject[winScore];
        player2trophies = new GameObject[winScore];

        scoreTrophy = Resources.Load<GameObject>("Prefabs/HUD/Trophy");

        

    }


   


    void Start()
    {
        LoadTrophiesinArray();
        MakeAllTrophyDeactive();
        roundUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StopTrophyUI()
    {
        roundUI.SetActive(false);
    }

    public void StartTrophyUI()
    {
        roundUI.SetActive(true);

    }

    public  void LoadTrophiesinArray()
    {
        for (int i = 0; i < winScore; i++)
        {
            player1trophies[i] = Instantiate(scoreTrophy,new Vector3(player1Parent.transform.position.x + trophySpawnDistance, player1Parent.transform.position.y,player1Parent.transform.position.z), Quaternion.identity, player1Parent.transform);
            player2trophies[i] = Instantiate(scoreTrophy,new Vector3(player2Parent.transform.position.x + trophySpawnDistance, player2Parent.transform.position.y, player2Parent.transform.position.z), Quaternion.identity, player2Parent.transform);

            trophySpawnDistance += 80;
        }
    }


    public void MakeAllTrophyDeactive()
    {
        for (int i = 0; i < winScore; i++)
        {
            player1trophies[i].SetActive(false);
            player2trophies[i].SetActive(false);
        }
    }

    public void IncrementScore()
    {
              
        if (PlayerManager.Instance.UnitDictionary.Count == 1)
        {
            foreach (int key in PlayerManager.Instance.UnitDictionary.Keys)
            {
                PlayerManager.Instance.ScoreDict[key] = PlayerManager.Instance.ScoreDict[key] + 1;
            }
            //WinScreenUI();
            //StartTrophyUI();
           // SceneManager.LoadScene("MainScene");
        }
    }



    public void WinScreenUI()
    {
        foreach (KeyValuePair<int, int> dict in PlayerManager.Instance.ScoreDict)
        {
            if (dict.Value == winScore)
            {
                Debug.Log("Player won :" + dict.Key);

            }

        }
    }

    public void ReloadScne()
    {
        SceneManager.LoadScene("MainScene");
    }





    public void IncrementTrophyInUI()
    {

        foreach (KeyValuePair<int, int> dict in PlayerManager.Instance.ScoreDict)
        {
            if (dict.Key == 0)
            {
                for (int i = 0; i < PlayerManager.Instance.ScoreDict[dict.Key]; i++)
                {
                    player1trophies[i].SetActive(true);

                }
            }

            if (dict.Key == 1)
            {
                for (int i = 0; i < PlayerManager.Instance.ScoreDict[dict.Key]; i++)
                {
                    player2trophies[i].SetActive(true);
                }
            }
        }
    }

}
