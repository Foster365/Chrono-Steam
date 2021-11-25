using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_UI_Trigger : MonoBehaviour
{
    [SerializeField] GameObject BossUI;
    [SerializeField] LayerMask playerReference;

    private void Start()
    {
        //playerReference = GameObject.FindGameObjectWithTag("Player");
        //if (playerReference == null) Debug.Log("Player Reference is null");

        //bossFillAmountImage = GameObject.FindGameObjectWithTag("Boss_Health_UI").GetComponent<Image>();
        //bossHealthUIImage = GameObject.FindGameObjectWithTag("Boss_Health_UI").GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        Collider[] playerDetectionArea = Physics.OverlapSphere(transform.position, 30, playerReference);
        Debug.Log("PlayerDetectionArea" + playerDetectionArea.Length);
        Debug.Log("Player ref layer name" + playerReference);
        if (playerDetectionArea.Length != 0)
        {
            Debug.Log("Player detected, enabling UI");
            BossUI.SetActive(true);
            //return true;
        }
        else
        {
            Debug.Log("Player not detected, UI not enabled");
            BossUI.SetActive(false);
            //return false;
        }
    }

    void CheckUIEnable()
    {

        //if (CheckForPlayer(playerReference.layer))
        //{
        //    Debug.Log("Entered in UI enabler");
        //    BossUI.SetActive(true);
        //}

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 30);
    }
}
