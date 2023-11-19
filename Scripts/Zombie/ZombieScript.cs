using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    public enum ZombieType
    {
        shuffle,
        dizzy,
        alert
    }

    public enum ZombieState
    {
        Idle,
        Walking,
        Eating
    }

    public ZombieType typeOfZombie; 
    public ZombieState stateOfZombie;
    public bool isAngry = false;
    public bool startInHouse = false;

    [SerializeField] private float hiddenRange;
    [SerializeField] private float alertSpeed;
    [SerializeField] private float gunAlertRange;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float randomTime = 5f;
    [SerializeField] private bool randomZombieState = false;
    [SerializeField] private float yAdjustment = 0.0f;
    [SerializeField] private float zombieAlertRange;
    private Animator animator;
    private NavMeshAgent agent;
    private AudioSource chaseMusicPlayer;
    private AudioSource zombieSound;
    private AnimatorStateInfo animatorInfo;
    private int newState = 0;
    private int currentTarget;
    private int currentState;
    private float distanceToTarget;
    private float distanceToPlayer;
    private float zombieAlertRangeBase;
    private bool awareOfPlayer = false;
    private bool adding = true;
    private GameObject[] target;
    private GameObject player;
    private float[] walkSpeed = { 0.15f, 1.0f, 0.75f };

    private void Start()
    {
        chaseMusicPlayer = GameObject.Find("ChaseMusic").GetComponent<AudioSource>();
        zombieSound = GetComponent<AudioSource>();
        player = GameObject.Find("FPSController");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectsWithTag("Target");
        animator.SetLayerWeight(((int)typeOfZombie + 1), 1);
        currentState = (int)stateOfZombie;

        zombieAlertRangeBase = zombieAlertRange;

        if(typeOfZombie == ZombieType.shuffle)
            transform.position = new Vector3(transform.position.x, transform.position.y + yAdjustment, transform.position.z);

        animator.SetTrigger(stateOfZombie.ToString());

        if (randomZombieState)
            InvokeRepeating("SetAnimationState", randomTime, randomTime);

        agent.destination = target[Random.Range(0, target.Length)].transform.position;
        agent.speed = walkSpeed[(int)typeOfZombie];

    }

    private void Update()
    {
        if (animator.GetBool("isDead") == false)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (SaveScripts.bottlePos == Vector3.zero)
                isAngry = false;

            if (SaveScripts.bottlePos != Vector3.zero && distanceToPlayer > attackDistance && isAngry == false)     //Distraction by bottle smashed!
            {
                agent.destination = SaveScripts.bottlePos;
                animator.SetBool("attacking", false);
                stateOfZombie = ZombieState.Walking;
            }
            else
            {

                if (distanceToPlayer <= attackDistance)
                {
                    agent.isStopped = true;
                    animator.SetBool("attacking", true);
                    animator.speed = 1.0f;

                    Vector3 pos = (player.transform.position - transform.position).normalized;
                    Quaternion posRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, posRotation, rotateSpeed * Time.deltaTime);
                }
                else
                {
                    animator.SetBool("attacking", false);
                    if (SaveScripts.zombiesChasing.Count > 0)
                    {
                        if (chaseMusicPlayer.volume <= 0.3f)
                        {
                            if (!chaseMusicPlayer.isPlaying)
                                chaseMusicPlayer.Play();

                            chaseMusicPlayer.volume += 0.1f * Time.deltaTime;
                        }
                    }
                    else if (SaveScripts.zombiesChasing.Count == 0)
                    {
                        if (chaseMusicPlayer.volume > 0.0f)
                        {
                            chaseMusicPlayer.volume -= 0.05f * Time.deltaTime;
                        }
                        else if (chaseMusicPlayer.volume <= 0.0f)
                            chaseMusicPlayer.Stop();
                    }

                    distanceToTarget = Vector3.Distance(transform.position, target[currentTarget].transform.position);
                    animatorInfo = animator.GetCurrentAnimatorStateInfo((int)typeOfZombie);

                    if (distanceToPlayer <= zombieAlertRange)        //NEEED FIX TO IF DETECT PLAYER FOLLOW HIM
                    {
                        stateOfZombie = ZombieState.Walking;
                        agent.destination = player.transform.position;
                        awareOfPlayer = true;
                        animator.speed = alertSpeed;
                        if (adding)
                        {
                            if (SaveScripts.zombiesChasing.Contains(this.gameObject))
                            {
                                adding = false;
                                return;
                            }
                            else
                            {
                                SaveScripts.zombiesChasing.Add(this.gameObject);
                                adding = false;
                            }
                        }
                    }
                    else if (distanceToPlayer > zombieAlertRange)
                    {
                        awareOfPlayer = false;
                        animator.speed = 1.0f;
                        if (SaveScripts.zombiesChasing.Contains(this.gameObject))
                        {
                            SaveScripts.zombiesChasing.Remove(this.gameObject);
                            adding = true;
                        }
                    }

                    if (distanceToPlayer < 10 && startInHouse)
                    {
                        agent.destination = player.transform.position;
                        awareOfPlayer = true;
                        stateOfZombie = ZombieState.Walking;
                        animator.SetTrigger("Walking");
                        SetChaseMusicForInHouseZombie();
                        if (adding)
                        {
                            if (SaveScripts.zombiesChasing.Contains(this.gameObject))
                            {
                                adding = false;
                                return;
                            }
                            else
                            {
                                SaveScripts.zombiesChasing.Add(this.gameObject);
                                adding = false;
                            }
                        }
                    }

                    if (distanceToPlayer > 200)
                    {
                        awareOfPlayer = false;
                        if (SaveScripts.zombiesChasing.Contains(this.gameObject))
                        {
                            SaveScripts.zombiesChasing.Remove(this.gameObject);
                            adding = true;
                        }
                        Destroy(gameObject);
                    }

                    if (animatorInfo.IsTag("motion"))
                    {
                        if (animator.IsInTransition((int)typeOfZombie))
                        {
                            agent.isStopped = true;
                        }
                    }

                    if (stateOfZombie == ZombieState.Walking && !awareOfPlayer)
                    {
                        if (distanceToTarget < 1.5f)
                        {
                            if (currentTarget < target.Length - 1)
                            {
                                currentTarget = Random.Range(0, target.Length);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (SaveScripts.zombiesChasing.Contains(this.gameObject))
            {
                SaveScripts.zombiesChasing.Remove(this.gameObject);
                adding = true;
            }
            
            if (SaveScripts.zombiesChasing.Count == 0)
            {
                if (chaseMusicPlayer.volume > 0.0f)
                {
                    chaseMusicPlayer.volume -= 0.05f * Time.deltaTime;
                }
                else if (chaseMusicPlayer.volume <= 0.0f)
                    chaseMusicPlayer.Stop();
            }

            CancelInvoke();
            Destroy(gameObject, 8);
        }

        if (SaveScripts.gunUsed)
        {
            zombieAlertRange = gunAlertRange;
            StartCoroutine(ResetAlertRange());
        }
        else if (SaveScripts.isHidden)
        {
            zombieAlertRange = hiddenRange;
        }
        else if (!SaveScripts.gunUsed)
        {
            zombieAlertRange = zombieAlertRangeBase;
        }

    
    }

    IEnumerator ResetAlertRange()
    {
        yield return new WaitForSeconds(15);
        SaveScripts.gunUsed = false;
    }

    private void OnDestroy()
    {
        SaveScripts.zombiesInGameAmount--;
    }

    private void SetAnimationState()
    {
        newState = Random.Range(0, 3);
        if(newState != currentState)
        {
            stateOfZombie = (ZombieState)newState;
            currentState = (int)stateOfZombie;
            animator.SetTrigger(stateOfZombie.ToString());
        }

        if (awareOfPlayer == true)
        {
            stateOfZombie = ZombieState.Walking;
        }

        zombieSound.Play();
    }

    private void SetChaseMusicForInHouseZombie()
    {
        if (!chaseMusicPlayer.isPlaying)
            chaseMusicPlayer.Play();
        chaseMusicPlayer.volume = 0.4f;
    }

    public void WalkOn()            //Use in animation event
    {
        agent.isStopped = false;
        agent.destination = target[currentTarget].transform.position;
    }

    public void WalkOff()           //Use in animation event
    {
        agent.isStopped = true;
    }

}
