﻿using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;

public class EntityController : MonoBehaviour, IEntityNotify
{
    public Animator animator;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public Vector3 position;
    public Vector3 direction;
    Quaternion rotation;

    public Vector3 lastPosition;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    private void Start()
    {
        if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            UpdateTransform();
        }
        if (!isPlayer)
        {
            rb.useGravity = false;
        }

    }

    void UpdateTransform()
    {
        position = GameObjectTool.LogicToWorld(entity.position);
        direction = GameObjectTool.LogicToWorld(entity.direction);

        rb.MovePosition(position);
        transform.forward = direction;
        lastPosition = position;
        lastRotation = rotation;
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            Debug.Log($"{name} OnDestroy :ID:{entity.entityId} POS:{entity.position} DIR:{entity.direction} SPEED:{entity.speed}");
        }

        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(transform);
        }
    }

    private void FixedUpdate()
    {
        if (entity == null)
        {
            return;
        }

        entity.OnUpdate(Time.fixedDeltaTime);
        if (!isPlayer)
        {
            UpdateTransform();
        }
    }

    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                animator.SetBool("Move", false);
                animator.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                animator.SetBool("Move", true);
                break;

            case EntityEvent.MoveBack:
                animator.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                animator.SetTrigger("Jump");
                break;
        }
    }

    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        Destroy(this.gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.Log($"OnEntityChanged :ID:{entity.entityId} POS:{entity.EntityData.Position} DIR:{entity.EntityData.Direction} SPEED:{entity.EntityData.Speed}");
    }
    #region 原始代码
    /*    public Animator anim;
        public Rigidbody rb;
        private AnimatorStateInfo currentBaseState;

        public Entity entity;

        public UnityEngine.Vector3 position;
        public UnityEngine.Vector3 direction;
        Quaternion rotation;

        public UnityEngine.Vector3 lastPosition;
        Quaternion lastRotation;

        public float speed;
        public float animSpeed = 1.5f;
        public float jumpPower = 3.0f;

        public bool isPlayer = false;*/

    // Use this for initialization
    /*    void Start () {
            if (entity != null)
            {
                this.UpdateTransform();
            }

            if (!this.isPlayer)
                rb.useGravity = false;
        }*/

    /*    void UpdateTransform()
        {
            this.position = GameObjectTool.LogicToWorld(entity.position);
            this.direction = GameObjectTool.LogicToWorld(entity.direction);

            this.rb.MovePosition(this.position);
            this.transform.forward = this.direction;
            this.lastPosition = this.position;
            this.lastRotation = this.rotation;
        }*/

    /*    void OnDestroy()
        {
            if (entity != null)
                Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);

            if(UIWorldElementManager.Instance!=null)
            {
                UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
            }
        }*/

    // Update is called once per frame
    /*    void FixedUpdate()
        {
            if (this.entity == null)
                return;

            this.entity.OnUpdate(Time.fixedDeltaTime);

            if (!this.isPlayer)
            {
                this.UpdateTransform();
            }
        }*/

    /*    public void OnEntityEvent(EntityEvent entityEvent)
        {
            switch (entityEvent)
            {
                case EntityEvent.Idle:
                    anim.SetBool("Move", false);
                    anim.SetTrigger("Idle");
                    break;
                case EntityEvent.MoveFwd:
                    anim.SetBool("Move", true);
                    break;
                case EntityEvent.MoveBack:
                    anim.SetBool("Move", true);
                    break;
                case EntityEvent.Jump:
                    anim.SetTrigger("Jump");
                    break;
            }
        }
    */
    #endregion
}
