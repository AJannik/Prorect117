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

        public static GameObject BuildButton(Scene scene, GameObject canvas, Vector2 position, string text)
        {
            GameObject button = new GameObject(scene, "Button", canvas);
            button.Transform.Position = position;
            Vector2 size = new Vector2(0.4f, 0.1f);

            button.AddComponent<CImageRender>();
            button.AddComponent<CButton>();
            button.GetComponent<CButton>().Canvas = canvas.GetComponent<CCanvas>();
            button.GetComponent<CButton>().SetSize(size);

            CImageRender imageRender = button.GetComponent<CImageRender>();
            imageRender.Canvas = canvas.GetComponent<CCanvas>();
            imageRender.SetSize(size.X, size.Y);

            GameObject textField = BuildTextField(scene, canvas, new Vector2(0f, -size.Y / 5f), text);
            textField.SetParent(button);
            textField.GetComponent<CGuiTextRender>().Centered = true;
            textField.GetComponent<CGuiTextRender>().Layer = 31;
            textField.GetComponent<CGuiTextRender>().SetSize(0.03f);

            return button;
        }

        public static GameObject BuildMainMenuManager(Scene scene)
        {
            GameObject mainMenuManager = new GameObject(scene, "MainMenuManager");
            mainMenuManager.AddComponent<CMainMenuManager>();

            return mainMenuManager;
        }
    }
}