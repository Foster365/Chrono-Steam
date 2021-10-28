using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Image pjHealthUI;
    private float playerLife;
    private float maxLife;
    //public Image bossHealthUI;

    private void Start()
    {
        
       // bossHealthUI = GameObject.FindWithTag(UtilitiesTags.BOSS_HEALTH_UI_TAG).GetComponent<Image>();
    }
   
    private void Update()
    {
        playerLife = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().Life_Controller.CurrentLife;
        maxLife = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().PlayerStats.MaxLife;
        DisplayPlayerHealth(playerLife);
    }

    public void DisplayPlayerHealth(float value)
    {
        
        if(value<0f)
            value=0f;

        pjHealthUI.fillAmount = value/ maxLife;
    }

    //public void DisplayBossHealth(float value)
    //{
    //    value /= 300f;
    //    if (value < 0f)
    //        value = 0f;
    //    if (bossHealthUI.isActiveAndEnabled) { bossHealthUI.fillAmount = value; }
        
    //}
}
