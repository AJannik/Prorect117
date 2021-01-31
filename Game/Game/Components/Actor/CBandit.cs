using System;
using Game.Components.Collision;
using Game.Components.Combat;
using Game.Entity;
using Game.Interfaces;

namespace Game.Components.Actor
{
    public enum EnemyState
    {
        Idle,
        Running,
        Attacking,
        Dying,
        Dead,
    }

    public class CBandit : IComponent, IUpdateable, IOnStart
    {
        public GameObject MyGameObject { get; set; }

        public EnemyState State { get; private set; } = EnemyState.Idle;

        public bool FacingRight { get; set; } = false;

        public CBoxTrigger LeftTrigger { get; set; }

        public CBoxTrigger RightTrigger { get; set; }

        public int LeftOnGround { get; set; } = 0;

        public int RightOnGround { get; set; } = 0;

        public BanditStateHandler BanditStateHandler { get; } = new BanditStateHandler();

        public BanditMovementBehavior BanditMovementBehavior { get; } = new BanditMovementBehavior();

        public BanditCombatBehavior BanditCombatBehavior { get; } = new BanditCombatBehavior();

        public CCombat Combat { get; set; }

        public void Start()
        {
            BanditCombatBehavior.Bandit = this;
            BanditMovementBehavior.Bandit = this;
            BanditStateHandler.Bandit = this;
            BanditStateHandler.SetupPickupDisplay(MyGameObject);
        }

        public void Update(float deltaTime)
        {
            State = BanditCombatBehavior.UpdateCombatBehavior(State, deltaTime);
            State = BanditMovementBehavior.UpdateMovementBehavior(State, deltaTime);

            switch (State)
            {
                case EnemyState.Idle:
                    BanditStateHandler.Idle();
                    break;
                case EnemyState.Running:
                    BanditStateHandler.Running(BanditMovementBehavior.MoveSpeed);
                    break;
                case EnemyState.Attacking:
                    BanditStateHandler.Attacking(RightTrigger, LeftTrigger);
                    break;
                case EnemyState.Dying:
                    BanditStateHandler.Dying();
                    break;
                case EnemyState.Dead:
                    BanditStateHandler.Dead();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetupLeftTrigger(CBoxTrigger trigger)
        {
            LeftTrigger = trigger;
            LeftTrigger.TriggerExited += LeftFootExited;
            LeftTrigger.TriggerEntered += LeftEntered;
        }

        public void SetupRightTrigger(CBoxTrigger trigger)
        {
            RightTrigger = trigger;
            RightTrigger.TriggerExited += RightFootExited;
            RightTrigger.TriggerEntered += RightEntered;
        }

        private void LeftEntered(object sender, IComponent e)
        {
            switch (e.MyGameObject.Name)
            {
                case "Player":
                    FacingRight = false;
                    BanditCombatBehavior.UpdateCombatBehavior(State, 0f);
                    break;
                case "Wall":
                case "Enemy" when e.MyGameObject != MyGameObject:
                    FacingRight = true;
                    break;
                case "Floor":
                    LeftOnGround++;
                    break;
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
            switch (e.MyGameObject.Name)
            {
                case "Player":
                    FacingRight = true;
                    BanditCombatBehavior.UpdateCombatBehavior(State, 0f);
                    break;
                case "Wall":
                case "Enemy" when e.MyGameObject != MyGameObject:
                    FacingRight = false;
                    break;
                case "Floor":
                    RightOnGround++;
                    break;
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