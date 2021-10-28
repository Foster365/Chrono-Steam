using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Image pjHealthUI;
    private float playerHealth;
    private float playerMaxHealth;

    //Boss Enemy

    [SerializeField] Image bossHealthUI;
    float bossHealth;
    float bossMaxHealth;

    [SerializeField] Enemy bossEnemy;

    public Image PjHealthUI { get => pjHealthUI; set => pjHealthUI = value; }
    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public float PlayerMaxHealth { get => playerMaxHealth; set => playerMaxHealth = value; }

    //public Image bossHealthUI;

    private void Start()
    {
        bossEnemy = GameManager.Instance.LvlManager.GetComponent<LevelManager>().BossInstance;
       // bossHealthUI = GameObject.FindWithTag(UtilitiesTags.BOSS_HEALTH_UI_TAG).GetComponent<Image>();
    }
   
    private void Update()
    {
        //Player Health
        playerHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().Life_Controller.CurrentLife;
        playerMaxHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().PlayerStats.MaxLife;

        //Boss Health

        bossHealth = bossEnemy.GetComponent<Enemy>().Stats.MaxHealth;
        bossMaxHealth = bossEnemy.GetComponent<Enemy>().Life_Controller.CurrentLife;

        //DisplayHealth(pjHealthUI, playerHealth, playerMaxHealth);
        //DisplayHealth(bossHealthUI, bossHealth, bossMaxHealth);
    }

    public void DisplayHealth(Image healthUI, float value, float maxValue)
    {
        
        if(value<0f)
            value=0f;

        healthUI.fillAmount = value/ maxValue;
    }

    //public void DisplayBossHealth(float value)
    //{
    //    value /= 300f;
    //    if (value < 0f)
    //        value = 0f;
    //    if (bossHealthUI.isActiveAndEnabled) { bossHealthUI.fillAmount = value; }
        
    //}
}
