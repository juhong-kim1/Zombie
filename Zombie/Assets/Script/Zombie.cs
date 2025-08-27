using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    private Status currentStatus;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set { var prevStatus = currentStatus;
            currentStatus = value;

            switch (CurrentStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    animator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    break;
            }
        }
    }

    private NavMeshAgent agent;
    private Animator animator;
    private Collider collider;

    private Transform target;

    private AudioSource audioSource;
    public AudioClip deathClip;
    public AudioClip hitClip;

    public ParticleSystem BloodSprayEffect;

    public float traceDistance;
    public float attackDistance;

    public float damage = 10f;
    public float lastAttackTime;
    public float attackInterval = 0.5f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        collider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdleStatus();
                break;
            case Status.Trace:
                UpdateTraceStatus();
                break;
            case Status.Attack:
                UpdateAttackStatus();
                break;
            case Status.Die:
                //UpdateDieStatus();
                break;

        }

    }

    private void UpdateAttackStatus()
    {
        if (target == null)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        if (target != null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (lastAttackTime + attackInterval < Time.time)
        {
            lastAttackTime = Time.time;

            var damagable = target.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.OnDamage(damage, transform.position, -transform.forward);
            }
        }
    }

    private void UpdateTraceStatus()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateIdleStatus()
    {
        if (target != null &&
            Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
        }

        target = FindTarget(traceDistance);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        audioSource.PlayOneShot(hitClip);

        BloodSprayEffect.transform.position = hitPoint;
        BloodSprayEffect.transform.forward = hitNormal;
        BloodSprayEffect.Play();
    }

    protected override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;

        collider.enabled = false;
        audioSource.PlayOneShot(deathClip);
    }

    public LayerMask targetLayer;

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if (colliders.Length == 0)
        {
            return null;
        }

        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

        return target.transform;
    }
}
