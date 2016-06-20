using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace RPG_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyState;
        KeyboardState oldKeyState;

        MouseState currentMouseState;
        MouseState oldMouseState;

        Texture2D pointerTexture;
        Sprite pointer = new Sprite();

        Texture2D heroTexture;
        Character hero;

        Texture2D demonTexture;
        Character demon;

        Texture2D buttonTexture;
        Button button;

        Texture2D meterTexture;
        Texture2D meterBlotTexture;

        Character target = new Character();
        List<Character> targets = new List<Character>(0);

        Character actor = new Character();

        SpriteFont calibri;

        List<Character> heroes = new List<Character>(4);
        List<Character> enemies = new List<Character>(4);
        List<Character> battlers = new List<Character>();

        List<Button> allButtons = new List<Button>(2);
        List<Button> fightButtons = new List<Button>(2);
        
        int targetIndex = 0;
        int buttonIndex = 0;
        float damageDealt;
        float damageLocation;
        double timer;

        Random rand = new Random();

        enum BattleButtons
        {
            idle,
            targetMenu,
            animating,
            battleMenu,
            skillsMenu,
            itemsMenu,
        }
        BattleButtons battleButtons = BattleButtons.idle;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            calibri = Content.Load<SpriteFont>("Fonts\\Calibri");
            heroTexture = Content.Load<Texture2D>("Textures\\Just a Box");
            demonTexture = Content.Load<Texture2D>("Textures\\Enemy1");
            buttonTexture = Content.Load<Texture2D>("Textures\\Button");
            meterTexture = Content.Load<Texture2D>("Textures\\ActionBar");
            meterBlotTexture = Content.Load<Texture2D>("Textures\\BarBlot");

            pointerTexture = Content.Load<Texture2D>("Textures\\Pointer");

            hero = new Character();
            hero.SetTexture(heroTexture);
            hero.UpperLeft = new Vector2(400, 400);
            hero.battleOrigin = new Vector2(400, 400);
            hero.health = 1000;
            hero.PhAtk = 10;
            hero.PhDef = 75;
            hero.speed = 5;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.friendly = true;
            hero.meterBlot.SetTexture(meterBlotTexture);
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterBlot.UpperLeft = new Vector2(hero.UpperLeft.X - hero.meterBlot.GetWidth(),
                                                    hero.UpperLeft.Y + hero.meterBlot.GetHeight() + (hero.GetHeight()) + 20);
            hero.meterSprite.UpperLeft = hero.meterBlot.UpperLeft;
            heroes.Add(hero);

            hero = new Character();
            hero.SetTexture(heroTexture);
            hero.UpperLeft = new Vector2(400, 550);
            hero.battleOrigin = new Vector2(400, 550);
            hero.health = 100;
            hero.PhAtk = 5;
            hero.PhDef = 50;
            hero.speed = 20;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.friendly = true;
            hero.meterBlot.SetTexture(meterBlotTexture);
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterBlot.UpperLeft = new Vector2(hero.UpperLeft.X - hero.meterBlot.GetWidth(),
                                                    hero.UpperLeft.Y + hero.meterBlot.GetHeight() + (hero.GetHeight()) + 20);
            hero.meterSprite.UpperLeft = hero.meterBlot.UpperLeft;
            heroes.Add(hero);

            hero = new Character();
            hero.SetTexture(heroTexture);
            hero.UpperLeft = new Vector2(400, 700);
            hero.battleOrigin = new Vector2(400, 700);
            hero.health = 5;
            hero.PhAtk = 2;
            hero.PhDef = 0;
            hero.speed = 50;
            hero.Acc = 100;
            hero.Eva = 50;
            hero.friendly = true;
            hero.meterBlot.SetTexture(meterBlotTexture);
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterBlot.UpperLeft = new Vector2(hero.UpperLeft.X - hero.meterBlot.GetWidth(),
                                                    hero.UpperLeft.Y + hero.meterBlot.GetHeight() + (hero.GetHeight()) + 20);
            hero.meterSprite.UpperLeft = hero.meterBlot.UpperLeft;
            heroes.Add(hero);

            hero = new Character();
            hero.SetTexture(heroTexture);
            hero.UpperLeft = new Vector2(400, 850);
            hero.battleOrigin = new Vector2(400, 850);
            hero.health = 50;
            hero.PhAtk = 5;
            hero.PhDef = 70;
            hero.speed = 10;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.friendly = true;
            hero.meterBlot.SetTexture(meterBlotTexture);
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterBlot.UpperLeft = new Vector2(hero.UpperLeft.X - hero.meterBlot.GetWidth(),
                                                    hero.UpperLeft.Y + hero.meterBlot.GetHeight() + (hero.GetHeight()) + 20);
            hero.meterSprite.UpperLeft = hero.meterBlot.UpperLeft;
            heroes.Add(hero);

            //Enemy initialisation
            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 300);
            demon.battleOrigin = new Vector2(800, 300);
            demon.health = 20;
            demon.PhAtk = 99999;
            demon.PhDef = 0;
            demon.speed = 1;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.friendly = false;
            demon.meterBlot.SetTexture(meterBlotTexture);
            demon.meterSprite.SetTexture(meterTexture);
            demon.meterBlot.UpperLeft = new Vector2(demon.UpperLeft.X - demon.meterBlot.GetWidth(),
                                                    demon.UpperLeft.Y + demon.meterBlot.GetHeight() + (demon.GetHeight()) + 20);
            demon.meterSprite.UpperLeft = demon.meterBlot.UpperLeft;
            enemies.Add(demon);

            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 500);
            demon.battleOrigin = new Vector2(800, 500);
            demon.health = 35;
            demon.PhAtk = 100;
            demon.PhDef = 50;
            demon.speed = 10;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.friendly = false;
            demon.meterBlot.SetTexture(meterBlotTexture);
            demon.meterSprite.SetTexture(meterTexture);
            demon.meterBlot.UpperLeft = new Vector2(demon.UpperLeft.X - demon.meterBlot.GetWidth(),
                                                    demon.UpperLeft.Y + demon.meterBlot.GetHeight() + (demon.GetHeight()) + 20);
            demon.meterSprite.UpperLeft = demon.meterBlot.UpperLeft;
            enemies.Add(demon);

            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 700);
            demon.battleOrigin = new Vector2(800, 700);
            demon.health = 5;
            demon.PhAtk = 2;
            demon.PhDef = 0;
            demon.speed = 49;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.friendly = false;
            demon.meterBlot.SetTexture(meterBlotTexture);
            demon.meterSprite.SetTexture(meterTexture);
            demon.meterBlot.UpperLeft = new Vector2(demon.UpperLeft.X - demon.meterBlot.GetWidth(),
                                                    demon.UpperLeft.Y + demon.meterBlot.GetHeight() + (demon.GetHeight()) + 20);
            demon.meterSprite.UpperLeft = demon.meterBlot.UpperLeft;
            enemies.Add(demon);

            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 900);
            demon.battleOrigin = new Vector2(800, 900);
            demon.health = 50;
            demon.PhAtk = 10;
            demon.PhDef = -50;
            demon.speed = 5;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.friendly = false;
            demon.meterBlot.SetTexture(meterBlotTexture);
            demon.meterSprite.SetTexture(meterTexture);
            demon.meterBlot.UpperLeft = new Vector2(demon.UpperLeft.X - demon.meterBlot.GetWidth(),
                                                    demon.UpperLeft.Y + demon.meterBlot.GetHeight() + (demon.GetHeight()) + 20);
            demon.meterSprite.UpperLeft = demon.meterBlot.UpperLeft;
            enemies.Add(demon);


            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 60);
            button.action = "FIGHT";
            allButtons.Add(button);
            fightButtons.Add(button);

            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 5);
            button.action = "QUIT";
            allButtons.Add(button);
            fightButtons.Add(button);

            target.IsAlive = false;
            
            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);

            FightBegin();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if (oldKeyState == null)
            {
                oldKeyState = currentKeyState;
            }
            if (oldMouseState == null)
            {
                oldMouseState = currentMouseState;
            }

            pointer.IsAlive = false;
            
            if (battleButtons == BattleButtons.idle)
            {
                for(int i = 0; i < battlers.Count; i++)
                {
                    battlers[i].meter += battlers[i].speed / 100;

                    if (battlers[i].meter >= 100)
                    {
                        battlers[i].meter = 0;
                        actor = battlers[i];

                        if (actor.friendly)
                        {
                            battleButtons = BattleButtons.battleMenu;
                        }
                        else
                        {
                            battleButtons = BattleButtons.animating;
                            target = heroes[rand.Next(0, 3)];
                            timer = gameTime.TotalGameTime.TotalSeconds;
                        }
                    }

                    battlers[i].meterSprite.UpperLeft = new Vector2(battlers[i].meterBlot.UpperLeft.X + battlers[i].meter, battlers[i].meterBlot.UpperLeft.Y);
                }
            }

            if(battleButtons == BattleButtons.targetMenu)
            {
                if (targetIndex >= targets.Count)
                {
                    targetIndex = 0;
                }

                if (targets.Count <= 1)
                {
                    battleButtons = BattleButtons.animating;
                    target = targets[targetIndex];
                    targets.Clear();
                    targets.Capacity = 0;
                    timer = gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    pointer.UpperLeft = new Vector2(targets[targetIndex].UpperLeft.X - pointer.GetWidth() - 5, targets[targetIndex].UpperLeft.Y + targets[targetIndex].GetHeight() - 5);
                    pointer.IsAlive = true;

                    if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
                    {
                        targetIndex -= 1;
                        if (targetIndex < 0)
                        {
                            targetIndex = targets.Capacity - 1;
                        }
                    }
                    if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                    {
                        targetIndex += 1;
                        if (targetIndex > targets.Capacity - 1)
                        {
                            targetIndex = 0;
                        }
                    }
                    if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) || ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
                    {
                        battleButtons = BattleButtons.animating;
                        target = targets[targetIndex];
                        targets.Clear();
                        targets.Capacity = 0;
                        timer = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
            }

            if (battleButtons == BattleButtons.animating)
            {
                Attack(gameTime);
            }
            
            if(battleButtons == BattleButtons.battleMenu)
            {
                if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
                {
                    buttonIndex -= 1;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = fightButtons.Capacity - 1;
                    }
                }
                if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                {
                    buttonIndex += 1;
                    if (buttonIndex > fightButtons.Capacity - 1)
                    {
                        buttonIndex = 0;
                    }
                }
                if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) || ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
                {
                    if (fightButtons[buttonIndex].action == "FIGHT")
                    {
                        battleButtons = BattleButtons.targetMenu;

                        for(int i = 0; i < enemies.Count; i++)
                        {
                            targets.Capacity += 1;
                            targets.Add(enemies[i]);
                        }
                    }
                    if (fightButtons[buttonIndex].action == "QUIT")
                    {
                        Exit();
                    }
                }

                pointer.UpperLeft = new Vector2(fightButtons[buttonIndex].UpperLeft.X + 5, fightButtons[buttonIndex].UpperLeft.Y + 5);
                pointer.IsAlive = true;
            }


            oldKeyState = currentKeyState;
            oldMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            if (battleButtons == BattleButtons.battleMenu)
            {
                for (int i = 0; i < allButtons.Count; i++)
                {
                    button.Draw(spriteBatch);

                    spriteBatch.DrawString(calibri, allButtons[i].action, new Vector2(allButtons[i].UpperLeft.X + 70, allButtons[i].UpperLeft.Y + 8), Color.Black, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                }
            }

            for (int i = enemies.Count - 1; i > -1; i--)
            {
                enemies[i].Draw(spriteBatch);
                spriteBatch.DrawString(calibri, enemies[i].health.ToString(), new Vector2(enemies[i].UpperLeft.X, enemies[i].UpperLeft.Y + enemies[i].GetHeight() + 5), Color.Black);
                enemies[i].meterSprite.Draw(spriteBatch);
                enemies[i].meterBlot.Draw(spriteBatch);
            }

            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].Draw(spriteBatch);
                spriteBatch.DrawString(calibri, heroes[i].health.ToString(), new Vector2(heroes[i].UpperLeft.X, heroes[i].UpperLeft.Y + heroes[i].GetHeight() + 5), Color.Black);
                heroes[i].meterSprite.Draw(spriteBatch);
                heroes[i].meterBlot.Draw(spriteBatch);
            }

            pointer.Draw(spriteBatch);

            if(damageDealt > 0)
            {
                spriteBatch.DrawString(calibri, damageDealt.ToString(), new Vector2(target.UpperLeft.X, target.UpperLeft.Y - damageLocation), Color.Black);
                damageLocation += damageLocation / 32;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Initialises the beginning of a fight, including generating enemies and adding all fighters to the battlers<> list for processing
        void FightBegin()
        {

            List<Character> temp = new List<Character>();

            for (int i = 0; i < heroes.Count; i++)
            {
                temp.Capacity += 1;
                temp.Add(heroes[i]);
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                temp.Capacity += 1;
                temp.Add(enemies[i]);
            }
            battlers.Clear();
            battlers.Capacity = (temp.Count);

            for (int i = 0; i < temp.Count; i++)
            {
                battlers.Add(temp[i]);
            }
        }

        //Animates a basic attack, moving the object stored in actor, and removing health from the object stored in target
        void Attack(GameTime gameTime)
        {
            actor.Move();
            
            int modifier;

            if (actor.friendly)
            {
                modifier = 1;
            }
            else
            {
                modifier = -1;
            }

            if (gameTime.TotalGameTime.TotalSeconds >= timer + 2.5)
            {
                battleButtons = BattleButtons.idle;

                damageDealt = 0;
                damageLocation = 0;
                timer = 0;
                actor.velocity = new Vector2(0, 0);
                actor.UpperLeft = actor.battleOrigin;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.5)
            {
                actor.velocity = new Vector2(-2 * modifier, 0);
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1)
            {
                actor.velocity = new Vector2(0, 0);

                if (damageDealt == 0)
                {
                    damageDealt = (actor.PhAtk * ((100 - target.PhDef) / 100));
                    damageDealt = (float)Math.Round(damageDealt, 0, MidpointRounding.AwayFromZero);
                    target.health -= (int)damageDealt;
                    damageLocation = 30;

                    if (target.health <= 0)
                    {
                        target.IsAlive = false;

                        battlers.Remove(target);
                        enemies.Remove(target);
                        heroes.Remove(target);
                    }
                }
            }
            else
            {
                actor.velocity = new Vector2(2 * modifier, 0);
            }

        }
    }
}
