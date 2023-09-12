using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_AI_State
{
    FollowPlayer,
    Patrol

}
public class NPCBehaviourManager : MonoBehaviour
{
    public e_AI_State aiState;
    public NPCColorManager color;
    [SerializeField] private GameObject player;
    public NavMeshAgent agent;
    public Vector3 CurrentDestination;
    NavMeshTriangulation Triangulation;
    public float distanceToPlayer;
    public Vector3 distance;
    [SerializeField] private float playerFollowRange;
    public float speed = 2f;

    bool Follower;

    // Start is called before the first frame update
    public float followDuration;
    [SerializeField] private Material OriginalMaterial;
    void Start()
    {
        color = GetComponent<NPCColorManager>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        Triangulation = NavMesh.CalculateTriangulation();
        this.GetComponent<MeshRenderer>().material = OriginalMaterial;
        aiState = e_AI_State.Patrol;
    }

    

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        distance = player.transform.position - transform.position;
        switch (aiState)
        {
            case e_AI_State.FollowPlayer:
                agent.SetDestination(player.transform.position);
                agent.speed = distanceToPlayer;
                if (Follower == false)
                {
                    color.ChangeColor(player.GetComponent<MeshRenderer>().material);
                    Follower = true;
                }
                followDuration -= Time.deltaTime;
                break;
            case e_AI_State.Patrol:
                followDuration += Time.deltaTime;
                if (Follower == true)
                {
                    color.ChangeColor(OriginalMaterial);
                    Follower = false;
                }
                    if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        
                        MoveToRandomWaypoint();
                    }
                }
                break;
            default:
                break;
        }

        if (distanceToPlayer <= playerFollowRange)
        {
            aiState = e_AI_State.FollowPlayer;
            followDuration = Mathf.Max(0, followDuration - Time.deltaTime);
        }
       /* else
        {
            if (followDuration <= 0f)
            {
                aiState = e_AI_State.Patrol;
            }
            followDuration = Mathf.Min(followDuration + Time.deltaTime, playerFollowRange);
        }*/
    }

    public void MoveToRandomWaypoint()
    {

        int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

        NavMeshHit Hit;


        if (NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, -1))
        {
            if (CurrentDestination != Hit.position)
            {
                CurrentDestination = Hit.position;
                agent.SetDestination(CurrentDestination);
            }
            else
            {
                MoveToRandomWaypoint();
            }
        }
    }
}


