﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HitCounter : MonoBehaviour
{
   // private TextMeshPro textMeshPro;
    //private CanvasRenderer canvasRenderer;
    [SerializeField] private Text myText;
    [SerializeField] private GameObject player;
    
    private int hitCount;
    bool hit = false;
    float timer = 0f;

    public int HitCount { get => hitCount; set => hitCount = value; }


    private void Start()
    {
       // textMeshPro = GetComponent<TextMeshPro>();
        //meshRenderer = GetComponent<MeshRenderer>();
        myText = GetComponent<Text>();
        HideHitCounter();
    }

    private void Update()
    {
        //textMeshPro.transform.position = player.transform.position + new Vector3(2.5f, 1, 1);

        /*if (Input.GetKey(KeyCode.H))
        {
            SetHitCounter();
            HitCount += 5;
        }*/ //para probar el counter

        if (hit)
        {
            timer += 1 * Time.deltaTime;

            if(timer >= 8f)
            {
                HideHitCounter();
                hit = false;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public void SetHitCounter()
    {
        /*textMeshPro.SetText(HitCount.ToString());
        meshRenderer.enabled = true;*/
        myText.enabled = true;
        myText.text = hitCount.ToString();

        Color textColor = Color.white;

        if (HitCount >= 10) textColor = Color.blue;
        if (HitCount >= 20) textColor = Color.green;
        if (HitCount >= 30) textColor = Color.yellow;
        if (HitCount >= 40) textColor = Color.red;
        if (HitCount >= 50) textColor = Color.magenta;

        //textMeshPro.color = textColor;
        myText.color = textColor;
        float fontSize = 50f;
        float perHitFontSize = 1f;

        if (HitCount >= 50f)
        {
            //textMeshPro.fontSize = 2f;
            myText.fontSize = 100;
        }
        else
        {
            //textMeshPro.fontSize = fontSize + perHitFontSize * HitCount;
            myText.fontSize = ((int)fontSize) + ((int)perHitFontSize) * HitCount;
        }
        
    }

    public void AddHitCounter()
    {
        HitCount++;
        SetHitCounter();
        hit = true;
        timer = 0;
    }

    public void HideHitCounter()
    {
        // meshRenderer.enabled = false;
        HitCount = 0;
        myText.enabled = false;
    }
}
