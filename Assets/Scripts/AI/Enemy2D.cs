using System.Collections;
using System.Collections.Generic;
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

	[SerializeField] AIPerception2D perception;
	[SerializeField] AIPath2D path2D;

	[SerializeField] float attackRange = 2;
	[SerializeField] float maxHealth = 2;
	[SerializeField] Weapon2D weapon2D;

	private eState state;
	private float timer;

	private GameObject enemy = null;


	protected override void Start()
	{
		base.Start();

		health = maxHealth;
		state = eState.Idle;
		timer = 2;
	}
		
	void Update()
	{
		var sensed = perception.GetSensedGameObjects();
		enemy = (sensed.Length > 0) ? sensed[0] : null;

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
					timer = 2;
					state = eState.Idle;
				}
				break;
			case eState.Attack:
				
				break;
			case eState.Death:
				animator.SetBool("Death", true);
				movement.x = 0;
				break;
		}

		//animator.SetFloat("speed", speed);
	}

	protected override void FixedUpdate()
	{
		// horizontal movement
		if (state == eState.Patrol)
		{
			movement.x = (transform.position.x < path2D.targetPosition.x) ? speed : -speed;
		}
		if (state == eState.Chase)
		{
			movement.x = (transform.position.x < enemy.transform.position.x) ? speed : -speed;
			if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < attackRange)
			{
				state = eState.Attack;
				weapon2D.Use(animator);
				animator.SetTrigger("Attack");
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
			state = eState.Chase;
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
		if(health <= 0)
		{
			state = eState.Death;
		}
    }
}
