﻿using Game.Entity;

namespace Game.Interfaces
{
    public interface IComponent
    {
        public GameObject MyGameObject { get; set; }
    }
}