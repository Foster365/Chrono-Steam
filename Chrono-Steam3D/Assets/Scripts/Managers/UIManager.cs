using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Dictionary<string, Image> UIElements;
    [SerializeField] HealthUI healthUI;
    [SerializeField] WeaponsUI durabilityUI;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] UIelem = GameObject.FindGameObjectsWithTag(UtilitiesTags.UI_ICON_TAG);
    }

    // Update is called once per frame
    void Update()
    {
        //healthUI.DisplayHealth(healthUI.PjHealthUI, healthUI.PlayerHealth, healthUI.PlayerMaxHealth);
        durabilityUI.DisplayDurability(durabilityUI.WeaponDurabilityImage, durabilityUI.WeaponDurability, durabilityUI.WeaponMaxDurability);
        //durabilityUI.DisplayDurability(currentWPUI, wpDurability, wpMaxDurability);
        //If trigger boss Health UI
        //Enable Boss UI
        //Display Boss UI
    }

    void TestSetIcons()
    {
        //if(playerController.IsWeaponSlotNull) UIElements.ContainsKey()
    }

    void EnableUI(string UIName)
    {
        if (UIElements.ContainsKey(UIName))
            UIElements[UIName].enabled = true;
    }

    void DisableUI(string UIName)
    {
        if (UIElements.ContainsKey(UIName))
            UIElements[UIName].enabled = false;
    }

    //private void Update()
    //{
    //    //Player Health
    //    playerHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().Life_Controller.CurrentLife;
    //    playerMaxHealth = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().PlayerStats.MaxLife;

    //    //Boss Health

    //    bossHealth = bossEnemy.GetComponent<Enemy>().Stats.MaxHealth;
    //    bossMaxHealth = bossEnemy.GetComponent<Enemy>().Life_Controller.CurrentLife;

    //    DisplayHealth(pjHealthUI, playerHealth, playerMaxHealth);
    //    //DisplayHealth(bossHealthUI, bossHealth, bossMaxHealth);
    //}

    //public void DisplayHealth(Image healthUI, float value, float maxValue)
    //{

    //    if (value < 0f)
    //        value = 0f;

    //    healthUI.fillAmount = value / maxValue;
    //}

}
