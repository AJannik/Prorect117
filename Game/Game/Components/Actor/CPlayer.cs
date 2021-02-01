using System;
using Game.Components.Actor.Bandit;
using Game.Components.Actor.Player;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor
{
    public class CPlayer : IActor
    {
        public GameObject MyGameObject { get; set; }

        public ActorState State { get; set; }

        public bool FacingRight { get; set; }

        public ITrigger LeftTrigger { get; set; }

        public ITrigger RightTrigger { get; set; }

        public int LeftOnGround { get; set; }

        public int RightOnGround { get; set; }

        public IActorMovementBehavior ActorMovementBehavior { get; } = new PlayerMovementBehavior();

        public IActorCombatBehavior ActorCombatBehavior { get; } = new PlayerCombatBehavior();

        public IActorStateBehavior ActorStateBehavior { get; } = new PlayerStateBehavior();

        public IActorStats ActorStats { get; } = new PlayerStats();

        public CombatController CombatController { get; } = new CombatController();

        public void Start()
        {
            ActorCombatBehavior.Actor = this;
            ActorMovementBehavior.Actor = this;
            ActorStateBehavior.Actor = this;
            CombatController.Actor = this;
            ActorStateBehavior.SetupPickupDisplay(MyGameObject);
        }

        public void Update(float deltaTime)
        {
            if (IsDead(State, deltaTime))
            {
                State = ActorState.Dead;
                ActorStateBehavior.Dead();
                return;
            }

            if (IsDying())
            {
                State = ActorState.Dying;
                ActorStateBehavior.Dying();
                return;
            }

            ActorMovementBehavior.UpdateMovementBehavior(ActorState.Idle, deltaTime);
            ActorCombatBehavior.UpdateCombatBehavior(ActorState.Idle, deltaTime);
        }

        public void SetUpGroundTrigger(ITrigger trigger)
        {
            ((PlayerMovementBehavior)ActorMovementBehavior).GroundTrigger = trigger;
            ((PlayerMovementBehavior)ActorMovementBehavior).GroundTrigger.TriggerEntered += ((PlayerMovementBehavior)ActorMovementBehavior).OnGroundTriggerEntered;
            ((PlayerMovementBehavior)ActorMovementBehavior).GroundTrigger.TriggerExited += ((PlayerMovementBehavior)ActorMovementBehavior).OnGroundTriggerExited;
        }

        public void SetupLeftTrigger(ITrigger trigger)
        {
            LeftTrigger = trigger;
        }

        public void SetupRightTrigger(ITrigger trigger)
        {
            RightTrigger = trigger;
        }

        private bool IsDying()
        {
            return ActorStats.CurrentHealth <= 0f;
        }

        private bool IsDead(ActorState currentState, float deltaTime)
        {
            if (currentState == ActorState.Dying)
            {
                if (ActorStats.DyingTime <= 0f)
                {
                    return true;
                }

                ActorStats.DyingTime -= deltaTime;
            }

            return false;
        }
    }
}