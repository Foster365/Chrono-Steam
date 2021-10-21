using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDamageArea : SphereDamageArea
{
    [SerializeField] private Vector3 AreaScale;
    public override Collider[] Area()
    {
        return Physics.OverlapBox(transform.position + transform.forward * distance, AreaScale, transform.rotation);
    }
    public override void Create(int Damage, Vector3 position)
    {
        damage = Damage;
        var _player = GameManager.Instance.PlayerInstance;
        Instantiate(gameObject, _player.transform.position, _player.transform.rotation);
    }
    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.forward * distance, AreaScale);
    }
}
