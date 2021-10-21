using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class ObstacleAvoidance : MonoBehaviour
{
    public GameObject waypointsContainer;
    private Transform[] allWaypoints;
    [SerializeField] private List<Transform> myWaypoints;
    [SerializeField] private LayerMask layerObstacle;
    [SerializeField] private float radius;
    private int currentIndexNode;
    private const float threshold = 1f;
    private Enemy enemy;
  

    private Transform closerObstacle;
    private Vector3 direction;
    public bool move = false;

    private void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
    }

    private void Start()
    {
        //waypointsContainer = GameObject.FindGameObjectWithTag("WaipointContainer");
        fillWaypoints();
    }


    private void Update()
    {
        if(waypointsContainer != null)
        {
            CheckWaypoint();
            Avoidance();
            if (move)
            {
                Movement();
            }
        }

    }
    public void Movement()
    {
        Vector3 aux1 = (myWaypoints[currentIndexNode].transform.position - transform.position).normalized;
        aux1.y = 0; //Le saco la Y

        direction = aux1;
        if (closerObstacle != null)
        {
            Vector3 aux2 = closerObstacle.transform.position - transform.position;
            aux2.y = 0; //Le saco la Y

            //Le sumo a mi dirección el opuesto para evadir el obstaculo
            direction -= aux2;
        }

        transform.forward = Vector3.Lerp(transform.forward, //Vector dirección inicial - Punto A
                                          direction,         //Vector direcciín destino - Punto B
                                          Time.deltaTime * enemy.Stats.RotationSpeed); //Valor entre 0 y 1 que representa el porcentaje  entre el punto A y el B
                                                                           //Siendo 0 el punto A, 1 el punto B y los valores en el medio un porcentaje del recorrido. 0,5 sería el punto medio entre A y B

        //transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotationSpeed); //Lo mismo que el Lerp pero en vez de linear es esferico

        transform.position += transform.forward * enemy.Stats.RotationSpeed * Time.deltaTime;
    }

    void Avoidance()
    {
        closerObstacle = null;

        // Conseguimos todos nuestros obstaculos
        Collider[] overlapSphere = Physics.OverlapSphere(transform.position, radius, layerObstacle, QueryTriggerInteraction.Ignore);
        if (overlapSphere.Length > 0)
        {
            //Tomamos el primero como nuestro posible obstaculo más cercano.
            closerObstacle = overlapSphere[0].transform;
            //Conseguimos su distancia a nuestro personaje.
            float closerDistance = Vector3.Distance(overlapSphere[0].transform.position, transform.position);

            //Revisamos el resto de los objetos para buscar cual es el más cercano
            for (int i = 1; i < overlapSphere.Length; i++)
            {
                var posibleCloserObstacle = Vector3.Distance(overlapSphere[i].transform.position, transform.position);
                // Si encontramos un más cercano
                if (posibleCloserObstacle < closerDistance)
                {
                    //Lo reemplazamos
                    closerObstacle = overlapSphere[i].transform;
                }
            }
        }
    }

    void CheckWaypoint()
    {
        var distance = Vector3.Distance(transform.position, myWaypoints[currentIndexNode].transform.position);
        //var distance = (transform.position - allWaypoints[currentIndexNode].position).magnitude; //Esta es otra manera de conseguir la distancia entre el punto A y B

        // Si estamos "muy cerca" de nuestro destino, consideramos que llegamos
        if (distance <= threshold)
        {
            //Pasamos al siguiente indice
            currentIndexNode++;

            //Nos aseguramos de no pasarnos del limite del array
            if (currentIndexNode >= myWaypoints.Count)
            {
                currentIndexNode = 0;
            }
        }
    }

    void fillWaypoints()
    {
        if (waypointsContainer != null)
        {
            allWaypoints = waypointsContainer.GetComponentsInChildren<Transform>();

            for (int i = 0; i < allWaypoints.Length; i++)
            {
                if (allWaypoints[i] != waypointsContainer.transform)
                {
                    myWaypoints.Add(allWaypoints[i]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
