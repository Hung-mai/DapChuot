using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DapChuot_Player : MonoBehaviour
{
    private int turnIndex = 0;
    public static DapChuot_Player ins;

    // ************ object *************
    public Renderer[] bongDen;
    public Renderer conChuot;
    public TextMeshPro scoreText;
    public GameObject smokeEffect;
    public GameObject floatingText;

    // ********* component ***************
    private Animator animator;

    // ********** logic *****************
    public Material defaultMaterial;
    public Material lightMaterial;
    public Material defaultMouse;
    public Material dizzyMouse;

    [HideInInspector] public int score;
    //
    private float thoiGianNhapNhay;
    private float thoiGianCho;
    private float thoiGianDemNguoc;
    private int bongDenIndex = 0;
    private Vector3 chuotPos;
    private bool waitToDapChuot;

    private bool canFight;

    //
    private float timeToCalculateScore;
    private bool startCountingTime;
    private float timeAtFight;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        // SetUpTurn();
        chuotPos = conChuot.transform.position;
    }

    public void StartGame()
    {
        for(int i = 0; i < DapChuot_ReadFileLevel.ins.tableSize; i++)
        {
            float timeWait = DapChuot_ReadFileLevel.ins.levelList.levels[i].player_start;
            StartCoroutine(wait(timeWait));
        }
    }

    IEnumerator wait(float ti)
    {
        yield return new WaitForSeconds(ti);
        SetUpTurn();
    }

    private void Update()
    {
        if(DapChuot_GameManager.ins.endGame) return;

        if(DapChuot_GameManager.ins.isCountdownStartGame == true) return;

        if(Input.GetMouseButtonDown(0) && waitToDapChuot == false)
        {
           StartCoroutine(DapChuot());
        }

        // thoiGianDemNguoc += Time.deltaTime;
        // if(thoiGianDemNguoc >= thoiGianNhapNhay)
        // {
        //     for(int i = 0; i < 4; i++)
        //     {
        //         bongDen[i].material = defaultMaterial;
        //     }
        //     thoiGianDemNguoc = 0;
        //     if(bongDenIndex != 4)
        //     {
        //         bongDen[bongDenIndex].material = lightMaterial;
        //     }
        //     else
        //     {
        //         conChuot.transform.DOMove(new Vector3(chuotPos.x, 0.3f, chuotPos.z), 0.05f);
        //         canFight = true;
        //         startCountingTime = true;
        //         thoiGianNhapNhay = 10;

        //         StartCoroutine(EndTurn());
        //     }
        //     bongDenIndex = (bongDenIndex + 1) % 5;
        // }

        if(startCountingTime)
        {
            timeToCalculateScore += Time.deltaTime;
        }
    }

    private IEnumerator DapChuot()
    {
        
        waitToDapChuot = true;
        animator.Play("dapBua", -1, 0f);

        if(canFight == true)
        {
            var floatingT = Instantiate(floatingText, transform.position + Vector3.up * 6, Quaternion.identity);
            Destroy(floatingT, 0.8f);
            TextMeshPro tm = floatingT.transform.GetChild(0).GetComponent<TextMeshPro>();
            timeAtFight = timeToCalculateScore;
            // tính điểm
            if(timeAtFight < 0.25f)
            {
                score += 5;
                tm.text = "+5";
            }
            else if(timeAtFight < 0.5f)
            {
                score += 3;
                tm.text = "+3";
            }
            else if(timeAtFight < 0.75f)
            {
                score += 2;
                tm.text = "+2";
            }
            else
            {
                score += 1;
                tm.text = "+1";
            }
            yield return new WaitForSeconds(0.1f);
            var sm = Instantiate(smokeEffect, new Vector3(chuotPos.x, 0.5f, chuotPos.z), Quaternion.identity);
            Destroy(sm, 1);
            conChuot.material = dizzyMouse;
            conChuot.transform.GetChild(0).gameObject.SetActive(true);
            conChuot.transform.DOMove(new Vector3(chuotPos.x, -0.1f, chuotPos.z), 0.02f);

            // tính điểm
            scoreText.text = score.ToString();
        }

        yield return new WaitForSeconds(0.2f);
        waitToDapChuot = false;
    }

    private IEnumerator RunTurn()
    {
        for(int j = 0; j < 5; j++)
        {
            if(j != 0)
            {
                yield return new WaitForSeconds(thoiGianNhapNhay);
            }
            // for(int i = 0; i < 4; i++)
            // {
            //     bongDen[i].material = defaultMaterial;
            // }
            if(j != 4)
            {
                bongDen[j].material = lightMaterial;
                if(j - 1 >= 0)
                {
                    bongDen[j - 1].material = defaultMaterial;
                }
            }

            if(j == 3)
            {
                conChuot.transform.DOMove(chuotPos, 0.02f);
                canFight = false;
                conChuot.material = defaultMouse;
                conChuot.transform.GetChild(0).gameObject.SetActive(false);
            }

            if(j == 4)
            {
                conChuot.transform.DOMove(new Vector3(chuotPos.x, 0.3f, chuotPos.z), 0.02f);
                bongDen[j - 1].material = defaultMaterial;
                canFight = true;
                startCountingTime = true;

                StartCoroutine(EndTurn());
            }
        }
    }

    IEnumerator EndTurn()
    {
        // StartCoroutine(WaitToSetupturn());
        
        yield return new WaitForSeconds(1f);
        startCountingTime = false;
        timeToCalculateScore = 0;
        conChuot.transform.DOMove(chuotPos, 0.02f);
        conChuot.material = defaultMouse;
        conChuot.transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator WaitToSetupturn()
    {
        yield return new WaitForSeconds(thoiGianCho);
        if(DapChuot_GameManager.ins.endGame == false)
        {
            SetUpTurn();
        }
    }

    public void SetUpTurn()
    {
        thoiGianNhapNhay = (DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].player_end - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].player_start) / 4.0f;

        turnIndex ++;

        StartCoroutine(RunTurn());
    }
}
