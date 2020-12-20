﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Components;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class ObjectFactory
    {
        public static GameObject BuildFloor(Scene scene)
        {
            Vector2 size = new Vector2(2f, 0.2f);

            GameObject floor = new GameObject(scene, "Floor");
            floor.Transform.Position = new Vector2(0f, 0f);
            floor.Transform.Scale = size;

            floor.AddComponent<CRender>();
            floor.AddComponent<CBoxCollider>();
            floor.GetComponent<CBoxCollider>().Geometry.Size = size;

            floor.AddComponent<CRigidBody>();
            CRigidBody rb = floor.GetComponent<CRigidBody>();
            rb.Static = true;

            return floor;
        }

        public static GameObject BuildBall(Scene scene)
        {
            GameObject ball = new GameObject(scene, "Ball");
            ball.Transform.Position = new Vector2(0.5f, 4f);

            ball.AddComponent<CRender>();
            ball.GetComponent<CRender>().SetSize(0.2f, 0.2f);
            ball.AddComponent<CCamera>();
            ball.AddComponent<CCircleCollider>();
            ball.AddComponent<CTriggerEventTest>();
            ball.AddComponent<CRigidBody>();

            ball.AddComponent<CBoxCollider>();
            CBoxCollider trigger = ball.GetComponent<CBoxCollider>();
            trigger.IsTrigger = true;
            trigger.Offset = new Vector2(0f, -0.2f);
            trigger.Geometry.Size = new Vector2(0.2f, 0.1f);

            ball.GetComponent<CCamera>().Scale = 2f;

            return ball;
        }

        public static GameObject BuildPlayer(Scene scene)
        {
            GameObject player = new GameObject(scene, "Player");
            player.AddComponent<CRender>();
            player.GetComponent<CRender>().SetSize(2.0f, 2.0f);
            player.GetComponent<CRender>().LoadAndSetTexture("KnightIdle.png");
            player.AddComponent<CRigidBody>();
            player.AddComponent<CBoxCollider>();
            player.GetComponent<CBoxCollider>().Geometry.Size = new Vector2(1.0f, 2.0f);
            player.AddComponent<CPlayerController>();

            // add all animations
            player.AddComponent<CAnmimationController>();
            CAnmimationController controll = player.GetComponent<CAnmimationController>();
            controll.AddAnimation(new Tools.Animation("Idle", 15, 0, true));

            return player;
        }
    }
}