using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public enum EffectType { Slow, Brittle, NoBlock}

    public class CEffectSystem : IComponent
    {
        public CEffectSystem()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {
        }

        public void AddEffect(EffectType type)
        {
            AddEffect(type, 10, 1);
        }

        public void AddEffect(EffectType type, float duration, float strength)
        {

        }
    }
}
