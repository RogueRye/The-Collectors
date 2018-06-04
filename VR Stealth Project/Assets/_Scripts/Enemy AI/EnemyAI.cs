using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public Transform target;

    [Header("Stats")]
    [Range(1.0f, 50.0f)]
    public float range = 10f;
    [Range(.1f, 10.0f)]
    public float innerRange = 2f;
    [Range(0.0f, 360.0f)]
    public float fieldOfView = 35;
    public float walkSpeed = 3;
    public float runSpeed;
    public float stunTime = 1;
    public float distanceTimeout = 7f;
    [Header("Info")]
    public Transform eyes;
    public WayPoints[] wayPoints;


    int curWp = 0;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public bool isInView;
    bool isInSight;
    [HideInInspector]
    public AIStates curState;
    [HideInInspector]
    public Animator anim;
    KillBox mKillBox;
    bool arrived = false;

 
    Vector3 lastKnownPosition;
    float wpTimer = 0;
    float distanceTimer = 0;


    public enum AIStates
    {
        idle, patrol, chase, stunned
    }

    private bool isFrozen;

    public bool IsFrozen
    {
        get
        {
            return isFrozen;
        }

        set
        {
            isFrozen = value;
           
            if (value == true)
            {
                //Being stunned loses track of the player
                StartCoroutine(StunTimer());
            }
           
        }
    }


    // Use this for initialization
    public void Start () {

        distanceTimer = distanceTimeout;
        agent = GetComponent<NavMeshAgent>();
        curState = AIStates.idle;
        anim = GetComponentInChildren<Animator>();
        mKillBox = GetComponentInChildren<KillBox>();
        if(mKillBox != null)
            mKillBox.Init(this);
	}
	
	// Update is called once per frame
	public void Update () {

     
        if (IsFrozen)
        {
            ChangeState(AIStates.stunned);
            if(agent.enabled)
                agent.enabled = false;
        }

        if (!IsFrozen)
        {
            if (!agent.enabled)
                agent.enabled = true;

            LocateEnemy();

            if (isInView)
            {
                ChangeState(AIStates.chase);
            }
            else
            {
                ChangeState(AIStates.patrol);
            }
        }

     
        DetermineAction(curState);
        HandleAnimations(agent.desiredVelocity);

        //// If is tracking player but cannot see them, countdown until they lose track
        //if (!isInSight && isInView)
        //{
        //    distanceTimer = Mathf.Clamp(distanceTimer - Time.deltaTime, 0, distanceTimeout);
        //    if (distanceTimer <= 0)
        //    {
        //        isInView = false;
        //        distanceTimer = distanceTimeout;
        //    }
        //}
        ////When the android can see them, reset the timer
        //if (isInSight)
        //    distanceTimer = distanceTimeout;

    }


    public void LocateEnemy()
    {
        //isInView = false;
        isInSight = false;
        Debug.DrawRay(eyes.position, eyes.forward * range, Color.red);
        if (target != null)
        {
            var distance = Vector3.Distance(transform.position, target.position);
            //If the player is really close, AI knows they are there
            if(distance < innerRange)
            {
                isInView = true;
                var dir = target.position - transform.position;
                if (isInView)
                    HandleRotations(dir);
                
                return;
            }            
            else if (distance < range)
            {

                var dir = target.position - transform.position;
                dir.y = 0;
                if (dir == Vector3.zero)
                    dir = transform.forward;

                var angle = Vector3.Angle(eyes.forward, dir) /2;
                //if they're in range, check angle to see if they're in view and not behind an obstacle
                if (angle < fieldOfView / 2)
                {
                    var ray = new Ray(eyes.position, dir);
                    var hit = new RaycastHit();                   
                    Debug.DrawRay(eyes.position, dir * range, Color.red);
                    if (Physics.Raycast(ray, out hit, range))
                    {
                        if (hit.transform == target)
                        {       
                            isInView = true;
                            isInSight = true;
                        }
                    }
                    if(isInView)
                        HandleRotations(dir);
                }

            }

        }


    }

    public void HandleRotations(Vector3 dir)
    {
        var targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5);
    }
    
    // Editor feedback
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.grey;   
        Gizmos.DrawWireSphere(transform.position, innerRange);

        Gizmos.color = Color.blue;

        var viewAngleA = DirFromAngle(-fieldOfView / 2, false);
        var viewAngleB = DirFromAngle(fieldOfView / 2, false);
        Gizmos.DrawLine(eyes.position, eyes.position + eyes.forward* range);
        Gizmos.DrawLine(eyes.position, eyes.position + viewAngleA * range);
        Gizmos.DrawLine(eyes.position , eyes.position + viewAngleB * range);

    }

    public Vector3 DirFromAngle(float angleInDefrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDefrees += ( eyes.eulerAngles.y);
        }
        return new Vector3(Mathf.Sin(angleInDefrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDefrees * Mathf.Deg2Rad));
    }

    void Patrol()
    {

        if (wayPoints.Length == 0)
        {
            ChangeState(AIStates.idle);
            return;
        }
        agent.speed = walkSpeed;

        agent.stoppingDistance = .3f;
        agent.SetDestination(wayPoints[curWp].point.position);          
        if(agent.remainingDistance > .4f && arrived)
        {
            arrived = false;
        }
        if (agent.remainingDistance <= 0.3f)
        {
          
            if (!arrived)
            {
             
                if (!string.IsNullOrEmpty(wayPoints[curWp].idleAnim))
                {
                    anim.CrossFade(wayPoints[curWp].idleAnim, .24f);
                }
                arrived = true;
            }
            if(wpTimer < wayPoints[curWp].waitTime)
                wpTimer += Time.deltaTime;
            if(wpTimer >= wayPoints[curWp].waitTime)
            {               
                curWp = (curWp + 1) % (wayPoints.Length);        
                wpTimer = 0;
           
            }
        }

    }

    void Chase(Vector3 destination)
    {
        agent.stoppingDistance = 1f;
        agent.speed = runSpeed;
        agent.SetDestination(destination);
    }

    void DetermineAction(AIStates state)
    {
        switch(state){
            case (AIStates.idle):                
                break;
            case (AIStates.patrol):
                Patrol();
                break;
            case (AIStates.chase):
                lastKnownPosition = target.position;
                Chase(lastKnownPosition);
                break;
            case (AIStates.stunned):
                anim.SetBool("stunned", true);
                isInView = false;
                break;
        }
    }

    void ChangeState(AIStates newState)
    {
        if (newState == curState)
            return;

        curState = newState;
    }

    void HandleAnimations(Vector3 desiredVelocity)
    {

        if (IsFrozen)
        {
            anim.SetFloat("vertical", 0);
            return;
        }

        var relative = transform.InverseTransformDirection(desiredVelocity);

        relative.Normalize();
        float z = relative.z;
        if (curState == AIStates.patrol)
            z = Mathf.Clamp(z, 0, 0.5f);

        anim.SetFloat("vertical", z);
    }

    IEnumerator StunTimer()
    {
        // Enable UI to display that you're frozen
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("stunned", false);
        isFrozen = false;
        arrived = true;
        isInView = false;

    }

}

[System.Serializable]
public class WayPoints
{
    public Transform point;
    public string idleAnim = " ";
    public float waitTime = 1.4f;

}
