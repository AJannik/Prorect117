﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Components.Renderer;
using Game.Interfaces;

namespace Game.Components
{
    public class CCombat : IComponent, IUpdateable, IOnStart
    {
        private float maxHP = 100;

        public CCombat()
        {
        }

        public float MaxHealth
        {
            get
            {
                return maxHP;
            }

            set
            {
                maxHP = value;
                if (CurrentHealth > maxHP)
                {
                    CurrentHealth = maxHP;
                }
            }
        }

        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; } = null;

        public float CurrentHealth { get; set; } = 100f;

        public float Armor { get; set; } = 10f;

        public float AttackSpeed { get; set; } = 1.8f;

        public float NextAttackTime { get; set; } = 0f;

        public float AttackDamage { get; set; } = 10f;

        public string HurtAnimationName { get; set; } = "Hurt";

        public void Start()
        {
            if (MyGameObject.Name == "Player")
            {
                CurrentHealth = MyGameObject.Scene.GameManager.PlayerHealth;
            }
        }

        public void Update(float deltaTime)
        {
            if (NextAttackTime > 0f)
            {
                NextAttackTime -= deltaTime;
            }

            if (CurrentHealth <= 0f)
            {
                AnimationSystem.PlayAnimation("death");
                MyGameObject.Scene.RemoveGameObject(MyGameObject);
                if (MyGameObject.Name == "Player")
                {
                    MyGameObject.Scene.GameManager.GameOver();
                }
            }

            if (MyGameObject.Name == "Player")
            {
                MyGameObject.Scene.GameManager.PlayerHealth = CurrentHealth;
            }
        }

        /// <summary>
        /// Attack in given hitbox. Returns false if attack if still on cooldown.
        /// </summary>
        /// <param name="hitbox">Attack hitbox.</param>
        /// <param name="dmgMultiplier">Multiplies the base damage by this value.</param>
        /// <param name="ignoreArmor">Whether to ignore Armor or not.</param>
        /// <returns>Returns False if attack still on cooldown.</returns>
        public bool Attack(ITrigger hitbox, float dmgMultiplier, bool ignoreArmor)
        {
            if (NextAttackTime > 0f)
            {
                return false;
            }

            if (AttackSpeed >= 0)
            {
                NextAttackTime = 1f / AttackSpeed;
            }
            else
            {
                NextAttackTime = 1f;
            }

            foreach (IComponent hit in hitbox.GetTriggerHits())
            {
                if ((hit.MyGameObject.Name == "Player" || hit.MyGameObject.Name == "Enemy") &&
                    MyGameObject.Name != hit.MyGameObject.Name && hit.MyGameObject.GetComponent<CCombat>() != null)
                {
                    hit.MyGameObject.GetComponent<CCombat>().TakeDamage(dmgMultiplier * AttackDamage, ignoreArmor,
                        hit.MyGameObject.GetComponent<CCombat>().HurtAnimationName);
                }
            }

            return true;
        }

        public void TakeDamage(float dmgAmount, bool ignoreArmor, string dmgAnimationName)
        {
            if (ignoreArmor)
            {
                CurrentHealth -= dmgAmount;
            }
            else if (Armor >= 0f)
            {
                CurrentHealth -= dmgAmount * (100 / (100 + Armor));
            }
            else
            {
                CurrentHealth -= dmgAmount * (2 - (100 / (100 - Armor)));
            }

            if (AnimationSystem != null)
            {
                AnimationSystem.PlayAnimation(dmgAnimationName, true);
            }
        }
    }
}