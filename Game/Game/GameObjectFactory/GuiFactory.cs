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
            button.AddComponent<CImageRender>();
            button.AddComponent<CButton>();
            button.GetComponent<CButton>().Canvas = canvas.GetComponent<CCanvas>();
            button.GetComponent<CButton>().SetSize(size);

            CImageRender inactiveImage = button.GetComponents<CImageRender>()[0];
            inactiveImage.Canvas = canvas.GetComponent<CCanvas>();
            inactiveImage.LoadAndSetTexture("Content.UI.ui_grey_inactive.png");
            inactiveImage.SetSize(size.X, size.Y);
            button.GetComponent<CButton>().InactiveImage = inactiveImage;

            CImageRender activeImage = button.GetComponents<CImageRender>()[1];
            activeImage.Canvas = canvas.GetComponent<CCanvas>();
            activeImage.LoadAndSetTexture("Content.UI.ui_grey_active.png");
            activeImage.SetSize(size.X, size.Y);
            activeImage.Visible = false;
            button.GetComponent<CButton>().ActiveImage = activeImage;

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

        public static GameObject BuildCoinHUD(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject coinHUD = new GameObject(scene, "CoinHUD");
            GameObject textField = BuildTextField(scene, canvas, Vector2.Zero, "0");
            GameObject coinImage = BuildGuiImage(scene, canvas, new Vector2(-0.05f, -0f), "goldcoin1.png");

            textField.GetComponent<CGuiTextRender>().SetSize(0.05f);
            textField.SetParent(coinHUD);

            coinImage.GetComponent<CImageRender>().SetSize(0.09f, 0.09f);
            coinImage.SetParent(coinHUD);

            coinHUD.AddComponent<CCoinUIUpdater>();
            coinHUD.GetComponent<CCoinUIUpdater>().TextRender = textField.GetComponent<CGuiTextRender>();
            coinHUD.Transform.Position = position;

            return coinHUD;
        }

        public static GameObject BuildGameOverCoinUI(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject gameOverCoinUI = new GameObject(scene, "CoinUI");
            gameOverCoinUI.Transform.Position = position;
            gameOverCoinUI.AddComponent<CGameOverUI>();

            GameObject scoreText = BuildTextField(scene, canvas, new Vector2(0f, 0f), "You managed to collect     coins");
            scoreText.SetParent(gameOverCoinUI);
            scoreText.GetComponent<CGuiTextRender>().Centered = true;
            scoreText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            GameObject coinText = BuildTextField(scene, canvas, new Vector2(0.25f, 0f), "0");
            coinText.SetParent(gameOverCoinUI);
            coinText.GetComponent<CGuiTextRender>().Centered = true;
            coinText.GetComponent<CGuiTextRender>().FontColor = Color.DarkOrange;
            coinText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            gameOverCoinUI.GetComponent<CGameOverUI>().CoinsText = coinText.GetComponent<CGuiTextRender>();

            return gameOverCoinUI;
        }
    }
}