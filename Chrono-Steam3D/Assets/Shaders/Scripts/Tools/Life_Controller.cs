#region usings
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#endregion
public class Life_Controller 
{
    private float maxLife;
    private float currentLife;
    public bool isDead = false;

    public UnityEvent Dead;
    public UnityEvent Damaged;

    public float CurrentLife { get => currentLife; set => currentLife = value; }

    public Life_Controller(float initialMaxLife) {
        maxLife = initialMaxLife;
        CurrentLife = maxLife;
        Dead = new UnityEvent();
        Damaged = new UnityEvent();
    }
    public void GetDamage(float damage) {
        CurrentLife -= damage;
        GameManager.Instance.EventQueue.Add(Damaged);
        if (CurrentLife <= 0)
        {
            Die();
        }
    }
    public void GetHeal(float heal) {
        CurrentLife += heal;

        if (CurrentLife > maxLife) {
            CurrentLife = maxLife;
        }
    }
    private void Die() {
        CurrentLife = 0;
        isDead = true;
        GameManager.Instance.EventQueue.Add(Dead);
    }
}
