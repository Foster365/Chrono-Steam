using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaBall : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] float speed;
    [SerializeField] float damage;
    Player_Controler player;
    [SerializeField] ParticleSystem[] particles;

    Vector3 playerPos;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Controler>();
    }

    // Update is called once per frame
    void Update()
    {

        PlayParticles();

        playerPos = player.transform.position;

        Vector3 deltaVector = (playerPos - transform.position).normalized;
        deltaVector.y = 0;

        direction = deltaVector;

        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotSpeed);

        transform.position += transform.forward * speed * Time.deltaTime;

        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Life_Controller.GetDamage(damage);
            Destroy(gameObject);
        }
    }

    void PlayParticles()
    {
        foreach (ParticleSystem ps in particles)
        {
            Debug.Log("Tesla Ball Rendered");
            ps.Play();
        }
    }
}
