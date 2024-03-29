using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Character2D : MonoBehaviour, IDamagable
{
    public enum eFace
    {
        Left,
        Right
    }

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;

    [SerializeField, Range(0, 100)] protected float speed = 5;
    [SerializeField, Range(0, 1000)] protected float gravity = 60;
    [SerializeField] protected float health = 100;
    [SerializeField] protected eFace spriteFacing = eFace.Right;
    [SerializeField] protected bool CanFly;

    protected CharacterController2D characterController;

    protected Vector2 movement = Vector2.zero;
    protected eFace facing = eFace.Right;


    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController2D>();
    }


    protected virtual void FixedUpdate()
    {
        // vertical movement (gravity)
        movement.y -= gravity * Time.fixedDeltaTime;
        if (!CanFly)
        {
            movement.y = Mathf.Max(movement.y, -gravity * Time.fixedDeltaTime * 3);
        }

        characterController.Move(movement * Time.fixedDeltaTime);
        UpdateFacing();
    }

    protected void UpdateFacing()
    {
        if (facing == eFace.Left)
        {
            spriteRenderer.flipX = (spriteFacing == eFace.Right);
        }
        else
        {
            spriteRenderer.flipX = !(spriteFacing == eFace.Right);
        }
    }

	public virtual void ApplyDamage(float damage)
	{
		health -= damage;
        Debug.Log("generic hit");
	}
}
