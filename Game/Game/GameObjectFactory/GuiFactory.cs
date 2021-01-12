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

            CImageRender unclickedImage = button.GetComponents<CImageRender>()[0];
            unclickedImage.Canvas = canvas.GetComponent<CCanvas>();
            unclickedImage.LoadAndSetTexture("Content.UI.ui_grey_unclicked.png");
            unclickedImage.SetSize(size.X, size.Y);
            unclickedImage.Layer = 31;
            button.GetComponent<CButton>().UnClickedImage = unclickedImage;

            CImageRender clickedImage = button.GetComponents<CImageRender>()[1];
            clickedImage.Canvas = canvas.GetComponent<CCanvas>();
            clickedImage.LoadAndSetTexture("Content.UI.ui_grey_clicked.png");
            clickedImage.SetSize(size.X, size.Y);
            clickedImage.Visible = false;
            clickedImage.Layer = 31;
            button.GetComponent<CButton>().ClickedImage = clickedImage;

            CImageRender inactiveImage = button.GetComponents<CImageRender>()[1];
            inactiveImage.Canvas = canvas.GetComponent<CCanvas>();
            inactiveImage.LoadAndSetTexture("Content.UI.ui_grey_inactive.png");
            inactiveImage.SetSize(size.X, size.Y);
            inactiveImage.Visible = false;
            inactiveImage.Layer = 31;
            button.GetComponent<CButton>().InactiveImage = inactiveImage;

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

        public static GameObject BuildKeyUI(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject keyUI = new GameObject(scene, "KeyUI");
            keyUI.Transform.Position = position;

            keyUI.AddComponent<CImageRender>();
            CImageRender keyInactive = keyUI.GetComponent<CImageRender>();
            keyInactive.LoadAndSetTexture("Content.KeyInactive.png");
            keyInactive.SetSize(0.12f, 0.12f);
            keyInactive.Canvas = canvas.GetComponent<CCanvas>();

            keyUI.AddComponent<CImageRender>();
            CImageRender keyActive = keyUI.GetComponents<CImageRender>()[1];
            keyActive.LoadAndSetTexture("Content.Key.png");
            keyActive.SetSize(0.12f, 0.12f);
            keyActive.Canvas = canvas.GetComponent<CCanvas>();
            keyActive.Visible = false;

            keyUI.AddComponent<CKeyUIUpdater>();
            keyUI.GetComponent<CKeyUIUpdater>().KeyActive = keyActive;
            keyUI.GetComponent<CKeyUIUpdater>().KeyInactive = keyInactive;

            return keyUI;
        }

        public static GameObject BuildShopScreen(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject shopScreen = new GameObject(scene, "ShopScreen");
            shopScreen.Transform.Position = position;
            shopScreen.AddComponent<CShopScreen>();

            GameObject backgroundImage = BuildGuiImage(scene, canvas, position, "UI.ui_bg.png");
            backgroundImage.GetComponent<CImageRender>().SetSize(3f, 1.7f);
            backgroundImage.SetParent(shopScreen);

            GameObject titel = BuildTextField(scene, canvas, new Vector2(0f, 0.55f), "BOTZERS' STORE");
            titel.Transform.Rotation = -0.15f;
            titel.GetComponent<CGuiTextRender>().SetSize(0.09f);
            titel.GetComponent<CGuiTextRender>().Centered = true;
            titel.GetComponent<CGuiTextRender>().Layer = 33;
            titel.GetComponent<CGuiTextRender>().FontColor = new Color(209, 204, 199, 1);
            titel.SetParent(shopScreen);

            GameObject titelBg = BuildGuiImage(scene, canvas, new Vector2(0f, 0.05f), "UI.ui_wood_sign.png");
            titelBg.GetComponent<CImageRender>().SetSize(1.6f, 0.3f);
            titelBg.GetComponent<CImageRender>().Layer = 32;
            titelBg.SetParent(titel);

            GameObject closeButton = BuildButton(scene, canvas, new Vector2(0f, -0.7f), new Vector2(0.4f, 0.1f), "CONTINUE");
            closeButton.GetComponent<CButton>().ButtonClicked += shopScreen.GetComponent<CShopScreen>().OnContinue;
            closeButton.SetParent(shopScreen);

            GameObject textField = BuildTextField(scene, canvas, new Vector2(0.3f, 0.3f), "CURRENT DEBUFFS:");
            textField.GetComponent<CGuiTextRender>().SetSize(0.03f);
            textField.GetComponent<CGuiTextRender>().Layer = 33;
            textField.GetComponent<CGuiTextRender>().Centered = true;
            textField.SetParent(shopScreen);

            GameObject textField2 = BuildTextField(scene, canvas, new Vector2(-0.3f, 0.3f), "BUY FOR 15 COINS:");
            textField2.GetComponent<CGuiTextRender>().SetSize(0.03f);
            textField2.GetComponent<CGuiTextRender>().Layer = 33;
            textField2.GetComponent<CGuiTextRender>().Centered = true;
            textField2.SetParent(shopScreen);

            GameObject buyHealthButton = BuildButton(scene, canvas, new Vector2(-0.3f, 0.2f), new Vector2(0.4f, 0.1f), "HEAL 10 HP");
            buyHealthButton.GetComponent<CButton>().ButtonClicked += shopScreen.GetComponent<CShopScreen>().BuyHealth;
            buyHealthButton.SetParent(shopScreen);

            shopScreen.GetComponent<CShopScreen>().HealButton = buyHealthButton;

            return shopScreen;
        }
    }
}