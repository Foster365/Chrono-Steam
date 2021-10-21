using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float LifeTime;
    float Damage;
    float Distance;
    void Update()
    {
        var realSpeed = speed * Time.deltaTime;
        transform.Translate(0,0, realSpeed);
        Destroy(gameObject, LifeTime);
    }
    public void Create(int damage, float distance)
    {
        //Debug.Log("NapalmInstantie");
        Damage = damage;
        Distance = distance;
        var _player = GameManager.Instance.PlayerInstance;
        Instantiate(gameObject, _player.transform.position , _player.transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            float weapondamage = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>()
                                    .PlayerStats.Weapon.GetComponent<Weapon>().WeaponStats.EspDamage;
            other.gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(weapondamage);
        }
    }
}
