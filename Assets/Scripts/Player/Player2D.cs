using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.Experimental;

public class Player2D : Character2D//, IDamagable, IHealable, IScoreable
{
    [SerializeField] IntVariable score;
    [SerializeField] FloatVariable healthVar;
    //[SerializeField] Weapon2D weapon;

    [SerializeField, Range(0, 100)] float jump = 12;

    private void Update()
    {
        if (characterController.onGround && Input.GetButtonDown("Jump"))
        {
            movement.y = jump;
        }
        animator.SetBool("OnGround", characterController.onGround);

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    weapon.Use(animator);
        //}
    }

    protected override void FixedUpdate()
    {
        // horizontal movement
        movement.x = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(movement.x));
        if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;
        base.FixedUpdate();
    }

    public void Attack()
    {
       // Weapon2D.eDirection direction = (facing == eFace.Right) ? Weapon2D.eDirection.Right : Weapon2D.eDirection.Left;
       // weapon.Attack(direction);
    }

    public override void ApplyDamage(float damage)
    {
        healthVar.value -= damage;
        print("player hit: " + damage);
    }

    public void Heal(float health)
    {
        healthVar.value += health;
        print("player heal: " + health);
    }

    public void AddScore(int score)
    {
        this.score.value += score;
    }
}
