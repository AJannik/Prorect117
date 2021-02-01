using System;
using Game.Components.Actor.Bandit;
using Game.Components.Combat;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor
{
    public class CBandit : IActor
    {
        public GameObject MyGameObject { get; set; }

        public ActorState State { get; set; } = ActorState.Idle;

        public bool FacingRight { get; set; } = false;

        public ITrigger LeftTrigger { get; set; }

        public ITrigger RightTrigger { get; set; }

        public int LeftOnGround { get; set; } = 0;

        public int RightOnGround { get; set; } = 0;

        public IActorStateBehavior ActorStateBehavior { get; } = new BanditStateBehavior();

        public IActorMovementBehavior ActorMovementBehavior { get; } = new BanditMovementBehavior();

        public IActorCombatBehavior ActorCombatBehavior { get; } = new BanditCombatBehavior();

        public IActorStats ActorStats { get; } = new BanditStats();

        public CCombat Combat { get; set; }

        public void Start()
        {
            ActorCombatBehavior.Actor = this;
            ActorMovementBehavior.Actor = this;
            ActorStateBehavior.Actor = this;
            ActorStateBehavior.SetupPickupDisplay(MyGameObject);
        }

        public void Update(float deltaTime)
        {
            State = ActorCombatBehavior.UpdateCombatBehavior(State, deltaTime);
            State = ActorMovementBehavior.UpdateMovementBehavior(State, deltaTime);

            switch (State)
            {
                case ActorState.Idle:
                    ActorStateBehavior.Idle();
                    break;
                case ActorState.Running:
                    ActorStateBehavior.Running(ActorStats.MoveSpeed);
                    break;
                case ActorState.Attacking:
                    ActorStateBehavior.Attacking(FacingRight ? RightTrigger : LeftTrigger);
                    break;
                case ActorState.Dying:
                    ActorStateBehavior.Dying();
                    break;
                case ActorState.Dead:
                    ActorStateBehavior.Dead();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetupLeftTrigger(ITrigger trigger)
        {
            LeftTrigger = trigger;
            LeftTrigger.TriggerExited += LeftFootExited;
            LeftTrigger.TriggerEntered += LeftEntered;
        }

        public void SetupRightTrigger(ITrigger trigger)
        {
            RightTrigger = trigger;
            RightTrigger.TriggerExited += RightFootExited;
            RightTrigger.TriggerEntered += RightEntered;
        }

        private void LeftEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                FacingRight = false;
            }

            if (e.MyGameObject.Name == "Wall" || (e.MyGameObject.Name == "Enemy" && e.MyGameObject != MyGameObject))
            {
                FacingRight = true;
            }

            if (e.MyGameObject.Name == "Floor")
            {
                LeftOnGround++;
            }
        }

        private void LeftFootExited(object sender, IComponent e)
        {
            if ((e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground") && LeftOnGround > 0)
            {
                LeftOnGround--;
            }
        }

        private void RightEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                FacingRight = true;
            }

            if (e.MyGameObject.Name == "Wall" || (e.MyGameObject.Name == "Enemy" && e.MyGameObject != MyGameObject))
            {
                FacingRight = false;
            }

            if (e.MyGameObject.Name == "Floor")
            {
                RightOnGround++;
            }
        }

        private void RightFootExited(object sender, IComponent e)
        {
            if ((e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground") && RightOnGround > 0)
            {
                RightOnGround--;
            }
        }
    }
}