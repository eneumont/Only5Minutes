using UnityEngine;

public class Enemy2D : Character2D//, IDamagable
{
    enum eState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Death
    }

    [SerializeField] AIPerception2D ProjectilePerception;
    [SerializeField] AIPerception2D MeleePerception;
    [SerializeField] AIPath2D path2D;

    [SerializeField] float attackRange = 2;
    [SerializeField] float maxHealth = 2;
    [SerializeField] Weapon2D weapon2D;

    [SerializeField] ProjectileWeapon2D ProjectilePrefab;
    [SerializeField] GameObject Target;
    [SerializeField] Transform LaunchOffset;
    [SerializeField] float ShootCooldown = 1;


    private eState state;
    private float timer;
    private float ShootTimer;

    private GameObject enemy = null;


    protected override void Start()
    {
        base.Start();

        health = maxHealth;
        state = eState.Idle;
        timer = 0;
        ShootTimer = ShootCooldown;
        ProjectilePrefab.Target = Target;
    }

    void Update()
    {
        var ProjectileSensed = ProjectilePerception.GetSensedGameObjects();
        enemy = (ProjectileSensed.Length > 0) ? ProjectileSensed[0] : null;
        ShootTimer -= Time.deltaTime;

        var MeleeSensed = MeleePerception.GetSensedGameObjects();
        enemy = (MeleeSensed.Length > 0) ? MeleeSensed[0] : enemy;        

        switch (state)
        {
            case eState.Idle:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = eState.Patrol;
                }
                break;
            case eState.Patrol:
                if (enemy != null)
                {
                    state = eState.Chase;
                }
                break;
            case eState.Chase:
                if (enemy == null)
                {
                    timer = 0;
                    state = eState.Idle;
                }
                break;
            case eState.Attack:
                timer = 1;
                state = eState.Idle;
                break;
            case eState.Death:
                animator.SetBool("Death", true);
                movement.x = 0;
                break;
        }

        animator.SetFloat("speed", speed);
    }

    protected override void FixedUpdate()
    {
        
        if (state == eState.Patrol)
        {
            movement.x = (transform.position.x < path2D.targetPosition.x) ? speed : -speed;
            movement.y = (transform.position.y < path2D.targetPosition.y) ? speed : -speed;
        }
        if (state == eState.Chase)
        {
            movement.x = (transform.position.x < enemy.transform.position.x) ? speed : -speed;
            movement.y = (transform.position.y < enemy.transform.position.y) ? speed : -speed;
            movement.x += Random.value > 0.5 ? Random.value * 20 : -(Random.value * 20);
            movement.y += Random.value > 0.5 ? Random.value * 20 : -(Random.value * 20);
            if ((Mathf.Abs(transform.position.x - enemy.transform.position.x) < attackRange) && (Mathf.Abs(transform.position.y - enemy.transform.position.y) < attackRange))
            {
                state = eState.Attack;
                weapon2D.Use(animator);
                animator.SetTrigger("Attack");
            }
            else if (ProjectilePerception.GetSensedGameObjects() != null && ShootTimer <= 0)
            {
                ShootTimer = ShootCooldown;
                Shoot();
            }

        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
        if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;

        base.FixedUpdate();
    }

    public void AttackDone()
    {
        if (state != eState.Death)
        {
            //state = eState.Chase;
        }
    }

    public void Attack()
    {
        Weapon2D.eDirection direction = (facing == eFace.Right) ? Weapon2D.eDirection.Right : Weapon2D.eDirection.Left;
        weapon2D.Attack(direction);
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            state = eState.Death;
        }
    }

    public void Shoot()
    {
        
        var clone = Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);

        clone.GetComponent<Rigidbody2D>().velocity = (GameObject.Find("Player").transform.position - transform.position).normalized * speed * 1.3f;
    }
}
