﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DapChuot_Bot : MonoBehaviour
{

    public int id;
    private int turnIndex = 0;

    // ************ object *************
    public Renderer[] bongDen;
    public Renderer conChuot;
    public TextMeshPro scoreText;
    public GameObject smokeEffect;

    public GameObject floatingText;

    // ********* component ***************
    private Animator botAnimator;

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
    private float ranTime;

    private Vector3 chuotPos;

    private void Start()
    {
        botAnimator = GetComponent<Animator>();


        chuotPos = conChuot.transform.position;
    }

    // private void Update()
    // {
    //     if(DapChuot_GameManager.ins.endGame) return;
    //     if(DapChuot_GameManager.ins.isCountdownStartGame == true) return;

    //     thoiGianDemNguoc += Time.deltaTime;
    //     if(thoiGianDemNguoc >= thoiGianNhapNhay)
    //     {
    //         for(int i = 0; i < 4; i++)
    //         {
    //             bongDen[i].material = defaultMaterial;
    //         }
    //         thoiGianDemNguoc = 0;
    //         if(bongDenIndex != 4)
    //         {
    //             bongDen[bongDenIndex].material = lightMaterial;
    //         }
    //         else
    //         {
    //             conChuot.transform.DOMove(new Vector3(chuotPos.x, 0.3f, chuotPos.z), 0.05f);
    //             thoiGianNhapNhay = 10;

    //             StartCoroutine(EndTurn());
    //         }
    //         bongDenIndex = (bongDenIndex + 1) % 5;
    //     }
    // }

    private IEnumerator RunTurn()
    {
        for(int j = 0; j < 5; j++)
        {
            if(j != 0)
            {
                yield return new WaitForSeconds(thoiGianNhapNhay);
            }
            for(int i = 0; i < 4; i++)
            {
                bongDen[i].material = defaultMaterial;
            }
            if(j != 4)
            {
                bongDen[j].material = lightMaterial;
            }
            else
            {
                conChuot.transform.DOMove(new Vector3(chuotPos.x, 0.3f, chuotPos.z), 0.05f);
                thoiGianNhapNhay = 10;

                StartCoroutine(EndTurn());
            }
        }
    }

    IEnumerator EndTurn()
    {
        StartCoroutine(WaitToSetupTurn());

        ranTime = Random.Range(0.1f, 0.9f);
        yield return new WaitForSeconds(ranTime);
        StartCoroutine(DapChuot());
        yield return new WaitForSeconds(1 - ranTime);
        conChuot.transform.DOMove(chuotPos, 0.1f);
        conChuot.material = defaultMouse;
        conChuot.transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator WaitToSetupTurn()
    {
        yield return new WaitForSeconds(thoiGianCho);
        if(DapChuot_GameManager.ins.endGame == false)
        {
            SetUpTurn();
        }
    }

    private IEnumerator DapChuot()
    {
        var floatingT = Instantiate(floatingText, transform.position + Vector3.up * 6, Quaternion.identity);
        Destroy(floatingT, 0.8f);
        TextMeshPro tm = floatingT.transform.GetChild(0).GetComponent<TextMeshPro>();
        // tính điểm
        if(ranTime < 0.25f)
        {
            score += 5;
            tm.text = "+5";
        }
        else if(ranTime < 0.5f)
        {
            score += 3;
            tm.text = "+3";
        }
        else if(ranTime < 0.75f)
        {
            score += 2;
            tm.text = "+2";
        }
        else
        {
            score += 1;
            tm.text = "+1";
        }

        tm.color = scoreText.color;

        botAnimator.Play("dapBua", -1, 0f);
        yield return new WaitForSeconds(0.1f);
        
        

        var sm = Instantiate(smokeEffect, new Vector3(chuotPos.x, 0.5f, chuotPos.z), Quaternion.identity);
        Destroy(sm, 1);
        scoreText.text = score.ToString();
        conChuot.transform.DOMove(new Vector3(chuotPos.x, -0.1f, chuotPos.z), 0.05f);
        conChuot.material = dizzyMouse;
        conChuot.transform.GetChild(0).gameObject.SetActive(true);

        
    }

    public void SetUpTurn()
    {
        for(int i = 0; i < 4; i++)
        {
            bongDen[i].material = defaultMaterial;
        }

        switch (id)
        {
            case 1:
                thoiGianNhapNhay = (DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot1_end - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot1_start) / 4.0f;
                if(turnIndex + 1 == DapChuot_ReadFileLevel.ins.tableSize)
                {
                    thoiGianCho = 2;
                }
                else
                {
                    thoiGianCho = DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex + 1].bot1_start - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot1_end;
                }
                
            break;

            case 2:
                thoiGianNhapNhay = (DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot2_end - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot2_start) / 4.0f;
                if(turnIndex + 1 == DapChuot_ReadFileLevel.ins.tableSize)
                {
                    thoiGianCho = 2;
                }
                else
                {
                    thoiGianCho = DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex + 1].bot2_start - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot2_end;
                }
                
            break;

            case 4:
                thoiGianNhapNhay = (DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot4_end - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot4_start) / 4.0f;
                if(turnIndex + 1 == DapChuot_ReadFileLevel.ins.tableSize)
                {
                    thoiGianCho = 2;
                }
                else
                {
                    thoiGianCho = DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex + 1].bot4_start - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot4_end;
                }
                
            break;

            case 5:
                thoiGianNhapNhay = (DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot5_end - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot5_start) / 4.0f;
                if(turnIndex + 1 == DapChuot_ReadFileLevel.ins.tableSize)
                {
                    thoiGianCho = 2;
                }
                else
                {
                    thoiGianCho = DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex + 1].bot5_start - DapChuot_ReadFileLevel.ins.levelList.levels[turnIndex].bot5_end;
                }
                
            break;
            
            default:
                Debug.LogError("lỗi set id cho bot");
            break;
        }

        turnIndex ++;
        
        thoiGianDemNguoc = 10;
        
        StartCoroutine(RunTurn());
    }

}
