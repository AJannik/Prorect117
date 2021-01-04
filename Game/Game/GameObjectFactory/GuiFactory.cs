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

        public static GameObject BuildGuiImage(Scene scene, GameObject canvas, Vector2 position, string textureFile)
        {
            GameObject image = new GameObject(scene, "Image", canvas);
            image.Transform.Position = position;

            image.AddComponent<CImageRender>();
            image.GetComponent<CImageRender>().LoadAndSetTexture($"Content.{textureFile}");
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

        public static GameObject BuildButton(Scene scene, GameObject canvas, Vector2 position, Vector2 size, string text)
        {
            GameObject button = new GameObject(scene, "Button", canvas);
            button.Transform.Position = position;

            button.AddComponent<CImageRender>();
            button.AddComponent<CImageRender>();
            button.AddComponent<CButton>();
            button.GetComponent<CButton>().Canvas = canvas.GetComponent<CCanvas>();
            button.GetComponent<CButton>().SetSize(size);

            CImageRender inactiveImage = button.GetComponents<CImageRender>()[0];
            inactiveImage.Canvas = canvas.GetComponent<CCanvas>();
            inactiveImage.LoadAndSetTexture("Content.UI.ui_grey_inactive.png");
            inactiveImage.SetSize(size.X, size.Y);
            inactiveImage.Layer = 31;
            button.GetComponent<CButton>().InactiveImage = inactiveImage;

            CImageRender activeImage = button.GetComponents<CImageRender>()[1];
            activeImage.Canvas = canvas.GetComponent<CCanvas>();
            activeImage.LoadAndSetTexture("Content.UI.ui_grey_active.png");
            activeImage.SetSize(size.X, size.Y);
            activeImage.Visible = false;
            activeImage.Layer = 31;
            button.GetComponent<CButton>().ActiveImage = activeImage;

            GameObject textField = BuildTextField(scene, canvas, new Vector2(0f, -size.Y / 5f), text);
            textField.SetParent(button);
            textField.GetComponent<CGuiTextRender>().Centered = true;
            textField.GetComponent<CGuiTextRender>().Layer = 32;
            textField.GetComponent<CGuiTextRender>().SetSize(size.Y * 0.3f);

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

            GameObject scoreText = BuildTextField(scene, canvas, new Vector2(0f, 0f), "YOU MANAGED TO COLLECT     COINS");
            scoreText.SetParent(gameOverCoinUI);
            scoreText.GetComponent<CGuiTextRender>().Centered = true;
            scoreText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            GameObject coinText = BuildTextField(scene, canvas, new Vector2(0.25f, 0f), " 0");
            coinText.SetParent(gameOverCoinUI);
            coinText.GetComponent<CGuiTextRender>().Centered = true;
            coinText.GetComponent<CGuiTextRender>().FontColor = Color.DarkOrange;
            coinText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            gameOverCoinUI.GetComponent<CGameOverUI>().CoinsText = coinText.GetComponent<CGuiTextRender>();

            return gameOverCoinUI;
        }

        public static GameObject BuildShopScreen(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject shopScreen = new GameObject(scene, "ShopScreen");
            shopScreen.Transform.Position = position;
            shopScreen.AddComponent<CShopScreen>();

            GameObject backgroundImage = BuildGuiImage(scene, canvas, position, "default.png");
            backgroundImage.GetComponent<CImageRender>().SetSize(3f, 1.5f);
            backgroundImage.SetParent(shopScreen);

            GameObject titel = BuildTextField(scene, canvas, new Vector2(0f, 0.55f), "SHOP");
            titel.Transform.Rotation = 0.3f;
            titel.GetComponent<CGuiTextRender>().SetSize(0.1f);
            titel.GetComponent<CGuiTextRender>().Centered = true;
            titel.GetComponent<CGuiTextRender>().Layer = 33;
            titel.SetParent(shopScreen);

            GameObject closeButton = BuildButton(scene, canvas, new Vector2(0f, -0.6f), new Vector2(0.4f, 0.1f), "CONTINUE");
            closeButton.GetComponent<CButton>().ButtonClicked += shopScreen.GetComponent<CShopScreen>().OnContinue;
            closeButton.SetParent(shopScreen);

            GameObject textField = BuildTextField(scene, canvas, new Vector2(0.3f, 0.35f), "CURRENT DEBUFFS:");
            textField.GetComponent<CGuiTextRender>().SetSize(0.03f);
            textField.GetComponent<CGuiTextRender>().Layer = 33;
            textField.SetParent(shopScreen);

            GameObject textField2 = BuildTextField(scene, canvas, new Vector2(-0.3f, 0.35f), "BUY FOR 15 COINS:");
            textField2.GetComponent<CGuiTextRender>().SetSize(0.03f);
            textField2.GetComponent<CGuiTextRender>().Layer = 33;
            textField2.GetComponent<CGuiTextRender>().Centered = true;
            textField2.SetParent(shopScreen);

            GameObject buyHealthButton = BuildButton(scene, canvas, new Vector2(-0.3f, 0.25f), new Vector2(0.4f, 0.1f), "BUY 10 HP");
            buyHealthButton.GetComponent<CButton>().ButtonClicked += shopScreen.GetComponent<CShopScreen>().BuyHealth;
            buyHealthButton.SetParent(shopScreen);

            return shopScreen;
        }
    }
}