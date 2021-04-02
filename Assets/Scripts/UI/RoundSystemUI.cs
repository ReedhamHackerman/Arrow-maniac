using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Rewired.Controller;

public class RoundSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject[] player1trophies ;
    [SerializeField] private GameObject[] player2trophies;
    [SerializeField] private GameObject roundUI;
    private int winScore = 3;



    private void Awake()
    {
        
        for (int i = 0; i < 5; i++)
        {
            player1trophies[i].SetActive(false);
            player2trophies[i].SetActive(false);
        }
      
    }


    void Start()
    {
       
        //roundUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StopTrophyUI()
    {
        //roundUI.SetActive(false);
    }

    public void StartTrophyUI()
    {
        //roundUI.SetActive(true);

    }

    public void IncrementScore()
    {
              
        if (PlayerManager.Instance.UnitDictionary.Count == 1)
        {
            foreach (int key in PlayerManager.Instance.UnitDictionary.Keys)
            {
                PlayerManager.Instance.ScoreDict[key] = PlayerManager.Instance.ScoreDict[key] + 1;
            }
            WinScreenUI();
            StartTrophyUI();
            SceneManager.LoadScene("MainScene");
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
