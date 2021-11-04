using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_UI_Trigger : MonoBehaviour
{
    [SerializeField] Image bossFillAmountImage;
    LayerMask playerReference;

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player").layer;
    }

    private void Update()
    {
        CheckUIEnable();
    }

    bool CheckForPlayer(LayerMask playerRef)
    {
        Collider[] playerDetectionArea = Physics.OverlapSphere(transform.position, 10, playerReference);
        if (playerDetectionArea!= null) return true;
        else return false;
    }

    void CheckUIEnable()
    {
        if (CheckForPlayer(playerReference)) bossFillAmountImage.enabled = true;
        else bossFillAmountImage.enabled = false;
    }
}
