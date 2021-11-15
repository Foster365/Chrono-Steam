using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Damage_Frame_UI : MonoBehaviour
{
    Image playerDamageFrameImage;

    float playerHealth;
    float playerMaxHealth;

    private void Awake()
    {

        playerDamageFrameImage = GameObject.FindGameObjectWithTag("Health_Frame_UI").gameObject.GetComponent<Image>();

        if (playerDamageFrameImage == null) Debug.Log("Player Health Frame UI is null in Awake function");


    }

    void Update()
    {

        playerHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controller>().Life_Controller.CurrentLife;
        playerMaxHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controller>().PlayerStats.MaxLife;

        var tempColor = playerDamageFrameImage.color;
        tempColor.a = -(playerHealth/playerMaxHealth)+1;
        playerDamageFrameImage.color = tempColor;

        //Debug.Log("Fill Amount is: " + tempColor.a);


    }
}
