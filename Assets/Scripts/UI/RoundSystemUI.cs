using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Rewired.Controller;

public class RoundSystemUI : MonoBehaviour
{
    private GameObject[] player1trophies;
    private GameObject[] player2trophies;
    [SerializeField] private GameObject player1Parent;
    [SerializeField] private GameObject player2Parent;
    private GameObject scoreTrophy; 
    private int trophySpawnDistance = 50;
    private int charImageSpawnDistance = 0;
    
    

    [SerializeField] private GameObject roundUI;
    private int winScore = 5;
    //private int maxRounds = 5;


    private void Awake()
    {
        player1trophies = new GameObject[winScore];
        player2trophies = new GameObject[winScore];

        scoreTrophy = Resources.Load<GameObject>("Prefabs/HUD/Trophy");
        LoadCharImageInUI();



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

    private void LoadCharImageInUI()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;


        for (int i = 0; i < connectedPlayerCount; i++)
        {
            int charId = CharacterSelection.playerWithSelectedCharacter[i];
            GameObject playerchar =  GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/HUD/CharImage"),new Vector3 (player1Parent.transform.position.x,player1Parent.transform.position.y - charImageSpawnDistance,player1Parent.transform.position.z) ,Quaternion.identity,roundUI.transform);
            RawImage playerImage = playerchar.GetComponent<RawImage>();
            playerImage.texture = Resources.Load<Texture2D>("Prefabs/Characters/" + charId);
            charImageSpawnDistance += 100;
        }




       


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
                PlayerUnit playerUnit = PlayerManager.Instance.UnitDictionary[key];
                playerUnit.IsMovementStop = true;
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
