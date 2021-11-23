using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DapChuot_GameManager : MonoBehaviour
{
    public DapChuot_Bot[] bots;
    public static DapChuot_GameManager ins;

    public Text countdownStartGameText;
    public GameObject countdownStartGamePanel;
    [HideInInspector] public bool isCountdownStartGame = true;
    private float countdownStartGameTime = 5.0f;

    // countdown game
    public Text countdown30sText;
    private bool countdown30s;
    private float countdown30sTime = 31;
    public bool endGame;

    public GameObject endGamePanel;
    public Button restartButton;

    public Text rsText;
    private int rank = 1;



    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        restartButton.onClick.AddListener(Onclick_restartGame);
    }

    private void Update()
    {
        if(endGame) return;

        if(isCountdownStartGame)
        {
            countdownStartGameTime -= Time.deltaTime;
            int timee = (int) countdownStartGameTime;
            if(timee == 0)
            {
                countdownStartGamePanel.SetActive(false);
                isCountdownStartGame = false;
                countdown30s = true;
                countdown30sText.gameObject.SetActive(true);

                for(int i = 0; i < 4; i++)
                {
                    bots[i].SetUpTurn();
                }
                DapChuot_Player.ins.SetUpTurn();
            }
            else
            {
                countdownStartGameText.text = (timee - 1).ToString();
            }
        }

        if(countdown30s)
        {
            countdown30sTime -= Time.deltaTime;
            int tim = (int) countdown30sTime;
            if(tim == 0)
            {
                endGame = true;
                StartCoroutine(CheckEndGame());
            }
            else
            {
                countdown30sText.text = (tim - 1).ToString();
            }
        }
    }

    private IEnumerator CheckEndGame()
    {
        yield return new WaitForSeconds(1);

        var bots = FindObjectsOfType<DapChuot_Bot>();

        for (int i = 0; i < bots.Length; i++)
        {
            if(DapChuot_Player.ins.score < bots[i].score)
            {
                rank++;
            }
        }

        if(rank == 1)
        {
            rsText.text = "NO.1";
        }
        else if(rank == 2)
        {
            rsText.text = "NO.2";
        }
        else if(rank == 3)
        {
            rsText.text = "NO.3";
        }
        else if(rank == 4)
        {
            rsText.text = "NO.4";
        }
        else
        {
            rsText.text = "NO.5";
        }

        endGamePanel.SetActive(true);
        
    }

    private void Onclick_restartGame()
    {
        SceneManager.LoadScene(0);
    }
}
