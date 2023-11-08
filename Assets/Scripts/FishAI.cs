using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class FishAI : MonoBehaviour
{

    public Transform pc;
    public NavMeshAgent nav_agent;
    public DecalProjector shadow;

    private enum AIState
    {
        Chase,
        FindStalkPath,
        Stalk,
        FindPC,
        Attack,
        Idle,
        GiveUp,
        RunAway,
        Finish
    } 

    private AIState state = AIState.Idle;
    private NavMeshPath current_path = null;

    private float stalk_timer = 0.0f;

    private UnityEngine.Vector3 ai_home_loc;

    public float STALK_RADIUS = 10.0f;
    public float AI_IDLE_RANGE = 100f;
    public float STALK_SPEED = 1.0f;
    public float ATTACK_SPEED = 2.0f;
    public float attack_chance = 0.25f;
    public float stalk_timeout = 5.0f;

    public float shadow_height = 3.724f;

    // Start is called before the first frame update
    void Start()
    {
        current_path = new NavMeshPath();
        ai_home_loc = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist_to_pc = (pc.position - transform.position).magnitude;
        //print(state.ToString());

        switch (state)
        {
            case AIState.Chase:
                swapShadowHard();
                if ((pc.position - nav_agent.destination).magnitude > 2)
                {
                    if (findPCPath())
                    {
                        nav_agent.SetPath(current_path);
                    } else
                    {
                        state = AIState.GiveUp; 
                        break;
                    }
                }
                
                doChase(Time.deltaTime);

                if (dist_to_pc > AI_IDLE_RANGE)
                {
                    state = AIState.Idle;
                    break;
                }
                break;

            case AIState.GiveUp:
                if (findRunAwayPath())
                {
                    nav_agent.SetPath(current_path);
                    state = AIState.RunAway; 
                    break;
                } else
                {
                    state = AIState.GiveUp;
                    break;
                }

            case AIState.RunAway:
                if (nav_agent.remainingDistance < 1.0f)
                {
                    state = AIState.Idle;
                    break;
                } else
                {
                    state = AIState.RunAway;
                    break;
                }

            case AIState.FindStalkPath:
                nav_agent.speed = STALK_SPEED;
                swapShadowSoft();
                if (dist_to_pc > AI_IDLE_RANGE)
                {
                    state = AIState.Idle;
                    break;
                }
                if (findStalkPath())
                {
                    nav_agent.SetPath(current_path);
                    state = AIState.Stalk;
                    stalk_timer = 0.0f;
                    break;
                } else
                {
                    state = AIState.Idle;
                    break;
                }

            case AIState.Stalk:
                swapShadowSoft();
                if (dist_to_pc > AI_IDLE_RANGE)
                {
                    state = AIState.Idle;
                    break;
                }
                doStalk(Time.deltaTime);

                if (stalk_timer >= stalk_timeout)
                {
                    float p = Random.Range(0.0f, 1.0f);
                    print(p);
                    if (p >= 1.0f - attack_chance)
                    {
                        state = AIState.FindPC; 
                        break;
                    }
                    else
                    {
                        state = AIState.FindStalkPath;
                        break;
                    }
                }
                break;

            case AIState.FindPC:
                nav_agent.speed = ATTACK_SPEED;
                swapShadowHard();
                if (dist_to_pc > AI_IDLE_RANGE)
                {
                    state = AIState.Idle;
                    break;
                }

                if (findPCPath())
                {
                    nav_agent.SetPath(current_path);
                    state = AIState.Chase;
                    break;
                } else
                {
                    state = AIState.Idle;
                    break;
                }

            case AIState.Attack:
                if (dist_to_pc > AI_IDLE_RANGE)
                {
                    state = AIState.Idle;
                    break;
                }

                

                doAttack(Time.deltaTime);
                break; 

            case AIState.Idle:
                nav_agent.speed = STALK_SPEED;
                swapShadowSoft();
                doIdle(Time.deltaTime);
                if (dist_to_pc <=  AI_IDLE_RANGE)
                {
                    state = AIState.FindStalkPath;
                } else
                {
                    state = AIState.Idle;
                }
                break;

            default:
                if (dist_to_pc <= AI_IDLE_RANGE)
                {
                    state = AIState.FindStalkPath;
                }
                else
                {
                    state = AIState.Idle;
                }
                break;
        }
        //Vector3 dir_to_pc = (pc.position - transform.position).normalized;
        //dir_to_pc.y = 0;
       
        //transform.position += 3.0f * Time.deltaTime * dir_to_pc;
    }

    private void doChase(float delta)
    {
        
    }

    private void doStalk(float delta)
    {
        //print(nav_agent.destination);
        nav_agent.Move(STALK_SPEED * delta * (nav_agent.nextPosition - transform.position).normalized);
        stalk_timer += delta;
    }

    private void doAttack(float delta)
    {
        nav_agent.Move(ATTACK_SPEED * delta * (nav_agent.nextPosition - transform.position).normalized);
    }

    private void doRunAway(float delta)
    {
        nav_agent.Move(STALK_SPEED * delta * (nav_agent.nextPosition - transform.position).normalized);
    }

    private void doIdle(float delta)
    {
    }

    private bool findStalkPath()
    {
        UnityEngine.Vector3 stalk_location = STALK_RADIUS * (UnityEngine.Quaternion.AngleAxis(Random.Range(-180, 180), UnityEngine.Vector3.up) * (pc.transform.forward)) + pc.transform.position;
        return nav_agent.CalculatePath(stalk_location, current_path);
    }

    private bool findPCPath()
    {
        UnityEngine.Vector3 stalk_location = pc.transform.position;
        return nav_agent.CalculatePath(stalk_location, current_path);
    }

    private bool findRunAwayPath()
    {
        UnityEngine.Vector3 location = new UnityEngine.Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
        location = ai_home_loc + AI_IDLE_RANGE * location;
        return nav_agent.CalculatePath(location, current_path);
    }


    private void swapShadowHard()
    {
        shadow.fadeFactor = Mathf.MoveTowards(shadow.fadeFactor, 1.0f, Time.deltaTime);
        shadow.size = UnityEngine.Vector3.MoveTowards(shadow.size, new UnityEngine.Vector3(8f, 8f, 15f), Time.deltaTime);
    }

    private void swapShadowSoft()
    {
        shadow.fadeFactor = Mathf.MoveTowards(shadow.fadeFactor, 0.5f, Time.deltaTime);
        shadow.size = UnityEngine.Vector3.MoveTowards(shadow.size, new UnityEngine.Vector3(10f, 10f, 15f), Time.deltaTime);
    }
}
