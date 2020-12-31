﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Components.UI;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class GuiFactory
    {
        public static GameObject BuildCanvas(Scene scene)
        {
            GameObject canvas = new GameObject(scene, "Canvas");
            canvas.Transform.Position = new Vector2(0f, 0f);

            canvas.AddComponent<CCanvas>();
            return canvas;
        }

        public static GameObject BuildGuiImage(Scene scene, GameObject canvas, Vector2 position, string texture)
        {
            GameObject image = new GameObject(scene, "Image", canvas);
            image.Transform.Position = position;

            image.AddComponent<CImageRender>();
            image.GetComponent<CImageRender>().LoadAndSetTexture($"Content.{texture}");
            image.GetComponent<CImageRender>().Canvas = canvas.GetComponent<CCanvas>();

            return image;
        }

        public static GameObject BuildTextField(Scene scene, GameObject canvas, Vector2 position, string text)
        {
            GameObject textField = new GameObject(scene, "TextField", canvas);
            textField.Transform.Position = position;

            textField.AddComponent<CGuiTextRender>();
            textField.GetComponent<CGuiTextRender>().Text = text;
            textField.GetComponent<CGuiTextRender>().Canvas = canvas.GetComponent<CCanvas>();

            return textField;
        }
    }
}