using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_UI_Trigger : MonoBehaviour
{
    [SerializeField] Image bossFillAmountImage;
    [SerializeField] Image bossHealthUIImage;
    LayerMask playerReference;

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player").layer;
        //bossFillAmountImage = GameObject.FindGameObjectWithTag("Boss_Health_UI").GetComponent<Image>();
        //bossHealthUIImage = GameObject.FindGameObjectWithTag("Boss_Health_UI").GetComponent<Image>();
    }

    private void Update()
    {
        //CheckUIEnable();
    }

    bool CheckForPlayer(LayerMask playerRef)
    {
        Collider[] playerDetectionArea = Physics.OverlapSphere(transform.position, 1.5f, playerReference);
        if (playerDetectionArea!= null) return true;
        else return false;
    }

    void CheckUIEnable()
    {

        if (CheckForPlayer(playerReference))
        {
            bossFillAmountImage.enabled = true;
            bossHealthUIImage.enabled = true;
        }

    }
}
