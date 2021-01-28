﻿using System.Diagnostics.CodeAnalysis;
using Game.Components.Player;
using Game.Components.UI;
using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;

namespace Game.GameObjectFactory
{
    [ExcludeFromCodeCoverage]
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
            button.AddComponent<CImageRender>();
            button.AddComponent<CButton>();
            button.GetComponent<CButton>().Canvas = canvas.GetComponent<CCanvas>();
            button.GetComponent<CButton>().SetSize(size);

            CImageRender unclickedImage = button.GetComponents<CImageRender>()[0];
            unclickedImage.Canvas = canvas.GetComponent<CCanvas>();
            unclickedImage.LoadAndSetTexture("Content.UI.ui_grey_unclicked.png");
            unclickedImage.SetSize(size.X, size.Y);
            unclickedImage.Visible = true;
            unclickedImage.Layer = 31;
            button.GetComponent<CButton>().UnClickedImage = unclickedImage;

            CImageRender clickedImage = button.GetComponents<CImageRender>()[1];
            clickedImage.Canvas = canvas.GetComponent<CCanvas>();
            clickedImage.LoadAndSetTexture("Content.UI.ui_grey_clicked.png");
            clickedImage.SetSize(size.X, size.Y);
            clickedImage.Visible = false;
            clickedImage.Layer = 31;
            button.GetComponent<CButton>().ClickedImage = clickedImage;

            CImageRender inactiveImage = button.GetComponents<CImageRender>()[2];
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

        public static GameObject BuildGameOverCoinUi(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject gameOverCoinUi = new GameObject(scene, "CoinUI");
            gameOverCoinUi.Transform.Position = position;

            GameObject scoreText = BuildTextField(scene, canvas, new Vector2(0f, 0f), "YOU MANAGED TO COLLECT     COINS");
            scoreText.SetParent(gameOverCoinUi);
            scoreText.GetComponent<CGuiTextRender>().Centered = true;
            scoreText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            GameObject coinText = BuildTextField(scene, canvas, new Vector2(0.25f, 0f), " 0");
            coinText.SetParent(gameOverCoinUi);
            coinText.GetComponent<CGuiTextRender>().Centered = true;
            coinText.GetComponent<CGuiTextRender>().FontColor = Color.DarkOrange;
            coinText.GetComponent<CGuiTextRender>().SetSize(0.06f);

            return gameOverCoinUi;
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

            GameObject textField2 = BuildTextField(scene, canvas, new Vector2(-0.3f, 0.3f), "BUY FOR 10 COINS:");
            textField2.GetComponent<CGuiTextRender>().SetSize(0.03f);
            textField2.GetComponent<CGuiTextRender>().Layer = 33;
            textField2.GetComponent<CGuiTextRender>().Centered = true;
            textField2.SetParent(shopScreen);

            GameObject buyHealthButton = BuildButton(scene, canvas, new Vector2(-0.3f, 0.2f), new Vector2(0.4f, 0.1f), "HEAL 10 HP");
            buyHealthButton.GetComponent<CButton>().ButtonClicked += shopScreen.GetComponent<CShopScreen>().BuyHealth;
            buyHealthButton.SetParent(shopScreen);

            for (int i = 0; i < 4; i++)
            {
                string text = $"x {(EffectType)i}";
                Vector2 pos = new Vector2(0.05f, 0.2f - (0.15f * i));
                shopScreen.GetComponent<CShopScreen>().PowerDownDisplays.Add(BuildShopPowerDown(scene, canvas, shopScreen, pos, text, i));
            }

            shopScreen.GetComponent<CShopScreen>().HealButton = buyHealthButton;

            return shopScreen;
        }

        public static GameObject BuildHudElements(Scene scene, GameObject canvas, CPlayerCombatController playerCombatController)
        {
            GameObject hud = new GameObject(scene, "Hud");

            BuildPlayerHpHud(scene, canvas, new Vector2(0.8f, -0.735f)).SetParent(hud);
            BuildCoinHud(scene, canvas, new Vector2(0.85f, 0.89f)).SetParent(hud);
            BuildKeyHud(scene, canvas, new Vector2(0.70f, 0.89f)).SetParent(hud);

            hud.AddComponent<CPlayerStatsHud>();
            CPlayerStatsHud playerStatsHud = hud.GetComponent<CPlayerStatsHud>();
            playerStatsHud.Combat = playerCombatController.Combat;
            playerStatsHud.PlayerController = playerCombatController.MyGameObject.GetComponent<CPlayerController>();
            playerStatsHud.HpText = hud.GetChild(0).GetChild(0).GetComponent<CGuiTextRender>();
            playerStatsHud.AttackText = hud.GetChild(0).GetChild(1).GetComponent<CGuiTextRender>();
            playerStatsHud.ArmorText = hud.GetChild(0).GetChild(2).GetComponent<CGuiTextRender>();
            playerStatsHud.SpeedText = hud.GetChild(0).GetChild(3).GetComponent<CGuiTextRender>();

            return hud;
        }

        public static GameObject BuildControls(Scene scene, GameObject canvas)
        {
            GameObject controls = new GameObject(scene, "Controls");
            controls.AddComponent<CControlsWindow>();

            GameObject bgImage = BuildGuiImage(scene, canvas, Vector2.Zero, "UI.ui_controls_bg.png");
            bgImage.GetComponent<CImageRender>().SetSize(1.5f, 1f);
            bgImage.GetComponent<CImageRender>().Layer = 29;
            bgImage.SetParent(controls);

            GameObject titel = BuildTextField(scene, canvas, new Vector2(0f, 0.35f), "CONTROLS");
            titel.GetComponent<CGuiTextRender>().SetSize(0.08f);
            titel.GetComponent<CGuiTextRender>().Centered = true;
            titel.SetParent(controls);

            GameObject movement = BuildTextField(scene, canvas, new Vector2(-0.3f, 0.23f), "A / D  - WALK LEFT-RIGHT");
            movement.GetComponent<CGuiTextRender>().SetSize(0.04f);
            movement.SetParent(controls);

            GameObject jump = BuildTextField(scene, canvas, new Vector2(-0.3f, 0.11f), "SPACE  - JUMP");
            jump.GetComponent<CGuiTextRender>().SetSize(0.04f);
            jump.SetParent(controls);

            GameObject roll = BuildTextField(scene, canvas, new Vector2(-0.3f, -0.01f), "LSHIFT - ROLL");
            roll.GetComponent<CGuiTextRender>().SetSize(0.04f);
            roll.SetParent(controls);

            GameObject attack = BuildTextField(scene, canvas, new Vector2(-0.3f, -0.13f), "LMOUSE - ATTACK");
            attack.GetComponent<CGuiTextRender>().SetSize(0.04f);
            attack.SetParent(controls);

            GameObject btnClose = BuildButton(scene, canvas, new Vector2(0f, -0.33f), new Vector2(0.5f, 0.1f), "GOT IT!");
            btnClose.GetComponent<CButton>().ButtonClicked += controls.GetComponent<CControlsWindow>().OnBtnClick;
            btnClose.SetParent(controls);

            return controls;
        }

        private static GameObject BuildShopPowerDown(Scene scene, GameObject canvas, GameObject parent, Vector2 position, string text, int num)
        {
            GameObject gameObject = new GameObject(scene, "ShopPowerDown", parent);
            gameObject.Transform.Position = position;

            gameObject.AddComponent<CGuiTextRender>();
            CGuiTextRender textRender = gameObject.GetComponent<CGuiTextRender>();
            textRender.Text = text;
            textRender.Canvas = canvas.GetComponent<CCanvas>();
            textRender.SetSize(0.04f);
            textRender.Layer = 31;

            GameObject button = BuildButton(scene, canvas, new Vector2(0.5f, 0f), new Vector2(0.8f, 0.1f), "REMOVE ONE FOR 15 COINS");
            button.GetComponent<CButton>().Number = num;
            button.GetComponent<CButton>().ButtonClicked += parent.GetComponent<CShopScreen>().RemovePowerDown;
            button.SetParent(gameObject);

            return gameObject;
        }

        private static GameObject BuildPlayerHpHud(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject playerHp = new GameObject(scene, "PlayerHpHUD");
            playerHp.Transform.Position = position;

            GameObject hpText = BuildTextField(scene, canvas, Vector2.Zero, "PlayerHP");
            hpText.GetComponent<CGuiTextRender>().SetSize(0.075f);
            hpText.GetComponent<CGuiTextRender>().Centered = true;
            hpText.GetComponent<CGuiTextRender>().Layer = 29;
            hpText.SetParent(playerHp);

            GameObject attackText = BuildTextField(scene, canvas, new Vector2(0f, -0.07f), "AttackText");
            attackText.GetComponent<CGuiTextRender>().SetSize(0.03f);
            attackText.GetComponent<CGuiTextRender>().Centered = true;
            attackText.GetComponent<CGuiTextRender>().Layer = 29;
            attackText.SetParent(playerHp);

            GameObject armorText = BuildTextField(scene, canvas, new Vector2(0f, -0.14f), "ArmorText");
            armorText.GetComponent<CGuiTextRender>().SetSize(0.03f);
            armorText.GetComponent<CGuiTextRender>().Centered = true;
            armorText.GetComponent<CGuiTextRender>().Layer = 29;
            armorText.SetParent(playerHp);

            GameObject speedText = BuildTextField(scene, canvas, new Vector2(0f, -0.21f), "SpeedText");
            speedText.GetComponent<CGuiTextRender>().SetSize(0.03f);
            speedText.GetComponent<CGuiTextRender>().Centered = true;
            speedText.GetComponent<CGuiTextRender>().Layer = 29;
            speedText.SetParent(playerHp);

            const float sizeX = 0.7f;
            const float sizeY = 0.4f;
            GameObject bgImage = BuildGuiImage(scene, canvas, new Vector2(position.X, -0.793f), "UI.hud_bg2.png");
            bgImage.GetComponent<CImageRender>().SetSize(sizeX, sizeY);
            bgImage.GetComponent<CImageRender>().Layer = 28;

            return playerHp;
        }

        private static GameObject BuildKeyHud(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject keyUi = new GameObject(scene, "KeyUI");
            keyUi.Transform.Position = position;

            keyUi.AddComponent<CImageRender>();
            keyUi.GetComponent<CImageRender>().Layer = 29;
            CImageRender keyInactive = keyUi.GetComponent<CImageRender>();
            keyInactive.LoadAndSetTexture("Content.KeyInactive.png");
            keyInactive.SetSize(0.12f, 0.12f);
            keyInactive.Canvas = canvas.GetComponent<CCanvas>();

            keyUi.AddComponent<CImageRender>();
            keyUi.GetComponents<CImageRender>()[1].Layer = 29;
            CImageRender keyActive = keyUi.GetComponents<CImageRender>()[1];
            keyActive.LoadAndSetTexture("Content.Key.png");
            keyActive.SetSize(0.12f, 0.12f);
            keyActive.Canvas = canvas.GetComponent<CCanvas>();
            keyActive.Visible = false;

            keyUi.AddComponent<CKeyUIUpdater>();
            keyUi.GetComponent<CKeyUIUpdater>().KeyActive = keyActive;
            keyUi.GetComponent<CKeyUIUpdater>().KeyInactive = keyInactive;

            return keyUi;
        }

        private static GameObject BuildCoinHud(Scene scene, GameObject canvas, Vector2 position)
        {
            GameObject coinHud = new GameObject(scene, "CoinHUD");
            GameObject textField = BuildTextField(scene, canvas, Vector2.Zero, "0");
            GameObject coinImage = BuildGuiImage(scene, canvas, new Vector2(-0.05f, -0f), "goldcoin1.png");

            textField.GetComponent<CGuiTextRender>().SetSize(0.05f);
            textField.GetComponent<CGuiTextRender>().Layer = 29;
            textField.SetParent(coinHud);

            coinImage.GetComponent<CImageRender>().SetSize(0.09f, 0.09f);
            coinImage.GetComponent<CImageRender>().Layer = 29;
            coinImage.SetParent(coinHud);

            coinHud.AddComponent<CCoinUIUpdater>();
            coinHud.GetComponent<CCoinUIUpdater>().TextRender = textField.GetComponent<CGuiTextRender>();
            coinHud.Transform.Position = position;

            GameObject bgImage = BuildGuiImage(scene, canvas, new Vector2(0.8f, 0.893f), "UI.hud_bg.png");
            bgImage.GetComponent<CImageRender>().SetSize(0.70f, 0.2f);
            bgImage.GetComponent<CImageRender>().Layer = 28;

            return coinHud;
        }
    }
}