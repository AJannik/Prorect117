using Game.Components.Actor;
using Game.Components.Combat;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActor : IComponent, IUpdateable, IOnStart
    {
        public ActorState State { get; set; }

        public bool FacingRight { get; set; }

        public ITrigger LeftTrigger { get; set; }

        public ITrigger RightTrigger { get; set; }

        public int LeftOnGround { get; set; }

        public int RightOnGround { get; set; }

        public IActorMovementBehavior ActorMovementBehavior { get; }

        public IActorCombatBehavior ActorCombatBehavior { get; }

        public IActorStateBehavior ActorStateBehavior { get; }

        public CCombat Combat { get; set; }

        public void SetupLeftTrigger(ITrigger trigger);

        public void SetupRightTrigger(ITrigger trigger);
    }
}