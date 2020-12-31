using System;
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

        public static GameObject BuildGuiImage(Scene scene, Vector2 position, string texture)
        {
            GameObject sprite = new GameObject(scene);
            sprite.Transform.Position = position;

            sprite.AddComponent<CImageRender>();
            sprite.GetComponent<CImageRender>().LoadAndSetTexture($"Content.{texture}");

            return sprite;
        }

        public static GameObject BuildTextField(Scene scene, Vector2 position, string text)
        {
            GameObject textField = new GameObject(scene, "TextField");
            textField.Transform.Position = position;

            textField.AddComponent<CGuiTextRender>();
            textField.GetComponent<CGuiTextRender>().Text = text;

            return textField;
        }
    }
}