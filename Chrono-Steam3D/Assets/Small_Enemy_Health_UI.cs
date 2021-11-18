using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Small_Enemy_Health_UI : MonoBehaviour
{

    [SerializeField] Enemy enemy;

    [SerializeField] TMP_Text enemyHealth;

    float enemyMaxHealth;
    float enemyCurrentHealth;

    // Update is called once per frame
    void Update()
    {
        enemyMaxHealth = enemy.Stats.MaxHealth;
        enemyCurrentHealth = enemy.Life_Controller.CurrentLife;

        UpdateHealth();
    }

    void UpdateHealth()
    {
        enemyHealth.text = enemyCurrentHealth.ToString();
    }

}
