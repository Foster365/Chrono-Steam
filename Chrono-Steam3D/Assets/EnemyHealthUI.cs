using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class EnemyHealthUI : MonoBehaviour
{

    [SerializeField] Enemy enemy;

    [SerializeField] Image enemyHealth;

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
        enemyHealth.fillAmount = enemyCurrentHealth/enemyMaxHealth;
    }

}
