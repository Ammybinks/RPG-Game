using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RPG_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        //test
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyState;
        KeyboardState oldKeyState;

        MouseState currentMouseState;
        MouseState oldMouseState;

        Texture2D heroTexture;
        Character hero;

        Texture2D demonTexture;
        Character demon;

        Texture2D buttonTexture;
        Button button;

        Character victim = new Character();

        SpriteFont calibri;

        List<Character> heroes = new List<Character>(4);
        LinkedList<Character> enemies = new LinkedList<Character>();
        List<Character> priority = new List<Character>();

        LinkedList<Button> allButtons = new LinkedList<Button>();
        List<Button> activeButtons;
        List<Button> battleButtons = new List<Button>(2);

        bool menuActive = true;
        int buttonIndex = 0;
        int turn;
        float damageDealt;
        float damageLocation;
        double timer;

        //temp
        Texture2D pointerTexture;
        Sprite pointer = new Sprite();
        //temp

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

            pointerTexture = Content.Load<Texture2D>("Textures\\Pointer");

            hero = new Character();
            hero.SetTexture(heroTexture);
            hero.UpperLeft = new Vector2(400, 400);
            hero.battleOrigin = new Vector2(400, 400);
            hero.health = 1000;
            hero.PhAtk = 10;
            hero.PhDef = 100;
            hero.speed = 5;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.friendly = true;
            hero.attacking = false;
            heroes.Add(hero);

            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 350);
            demon.battleOrigin = new Vector2(800, 350);
            demon.health = 20;
            demon.PhAtk = 5;
            demon.PhDef = 0;
            demon.speed = 1;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.friendly = false;
            demon.attacking = true;
            enemies.AddFirst(demon);

            demon = new Character();
            demon.SetTexture(demonTexture);
            demon.UpperLeft = new Vector2(800, 500);
            demon.battleOrigin = new Vector2(800, 500);
            demon.health = 5;
            demon.PhAtk = 1;
            demon.PhDef = 50;
            demon.speed = 5;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.attacking = true;
            demon.friendly = false;
            //enemies.AddFirst(demon);


            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 60);
            button.action = "FIGHT";
            allButtons.AddFirst(button);
            battleButtons.Add(button);

            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 5);
            button.action = "QUIT";
            allButtons.AddFirst(button);
            battleButtons.Add(button);

            victim.IsAlive = false;

            activeButtons = battleButtons;

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


            if (turn >= priority.Count)
            {
                turn = 0;
            }

            if (priority[turn].friendly)
            {
                menuActive = true;

                if (priority[turn].attacking)
                {
                    priority[turn].Move();

                    if (priority[turn].UpperLeft.X >= priority[turn].battleOrigin.X + 50)
                    {
                        priority[turn].velocity = new Vector2(0, 0);

                        if (damageDealt == 0)
                        {
                            damageDealt = (priority[turn].PhAtk);
                            victim = enemies.First.Value;
                            victim.health -= damageDealt;
                            damageLocation = 30;

                            if (victim.health <= 0)
                            {
                                victim.IsAlive = false;
                                priority.Remove(victim);
                            }
                        }
                        if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.5)
                        {
                            priority[turn].velocity = new Vector2(-2, 0);
                        }
                    }
                    else if ((priority[turn].UpperLeft.X < priority[turn].battleOrigin.X) && (priority[turn].velocity == new Vector2(-2, 0)))
                    {
                        priority[turn].attacking = false;

                        turn++;

                        if (turn >= 1)
                        {
                            priority[turn - 1].velocity = new Vector2(0, 0);
                            priority[turn - 1].UpperLeft = priority[turn - 1].battleOrigin;
                            damageDealt = 0;
                            damageLocation = 0;
                            victim = new Character();
                            victim.IsAlive = false;
                        }
                    }
                    else if (priority[turn].velocity == new Vector2(-2, 0))
                    {
                    }
                    else
                    {
                        priority[turn].velocity = new Vector2(2, 0);
                        timer = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
            }
            else
            {
                menuActive = false;

                if (priority[turn].attacking)
                {
                    priority[turn].Move();

                    if (priority[turn].UpperLeft.X <= priority[turn].battleOrigin.X - 50)
                    {
                        priority[turn].velocity = new Vector2(0, 0);
                        if (damageDealt == 0)
                        {
                            damageDealt = (priority[turn].PhAtk);
                            victim = heroes[0];
                            victim.health -= damageDealt;
                            damageLocation = 30;

                            if (victim.health <= 0)
                            {
                                victim.IsAlive = false;
                                priority.Remove(victim);
                            }
                        }
                        if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.5)
                        {
                            priority[turn].velocity = new Vector2(2, 0);
                        }
                    }
                    else if ((priority[turn].UpperLeft.X > priority[turn].battleOrigin.X) && (priority[turn].velocity == new Vector2(2, 0)))
                    {
                        turn++;

                        if (turn >= 1)
                        {
                            priority[turn - 1].velocity = new Vector2(0, 0);
                            priority[turn - 1].UpperLeft = priority[turn - 1].battleOrigin;
                            damageDealt = 0;
                            damageLocation = 0;
                            victim = new Character();
                            victim.IsAlive = false;
                        }
                    }
                    else if (priority[turn].velocity == new Vector2(2, 0))
                    {
                    }
                    else
                    {
                        priority[turn].velocity = new Vector2(-2, 0);
                        timer = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
            }

            if(menuActive)
            {
                foreach (Button button in battleButtons)
                {
                    button.IsAlive = true;
                }

                if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
                {
                    buttonIndex -= 1;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = battleButtons.Capacity - 1;
                    }
                }
                if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                {
                    buttonIndex += 1;
                    if (buttonIndex > battleButtons.Capacity - 1)
                    {
                        buttonIndex = 0;
                    }
                }
                if ((currentKeyState.IsKeyDown(Keys.Z)) || (currentKeyState.IsKeyDown(Keys.Space)))
                {
                    if (activeButtons[buttonIndex].action == "FIGHT")
                    {
                        priority[turn].attacking = true;
                    }
                    if (activeButtons[buttonIndex].action == "QUIT")
                    {
                        Exit();
                    }
                }
            }
            else
            { 
                foreach (Button button in battleButtons)
                {
                    button.IsAlive = false;
                }
            }

            pointer.UpperLeft = new Vector2(battleButtons[buttonIndex].UpperLeft.X + 5, battleButtons[buttonIndex].UpperLeft.Y + 5);

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

            foreach (Button button in allButtons)
            {
                button.Draw(spriteBatch);

                spriteBatch.DrawString(calibri, button.action, new Vector2(button.UpperLeft.X + 70, button.UpperLeft.Y + 8), Color.Black, 0, new Vector2(0,0), 1.5f, SpriteEffects.None, 0);
            }

            foreach (Character enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            hero.Draw(spriteBatch);

            pointer.Draw(spriteBatch);

            if(victim.IsAlive)
            {
                spriteBatch.DrawString(calibri, damageDealt.ToString(), new Vector2(victim.UpperLeft.X, victim.UpperLeft.Y - damageLocation), Color.Black);
                damageLocation += damageLocation / 32;
            }

            spriteBatch.DrawString(calibri, enemies.First.Value.health.ToString(), new Vector2(enemies.First.Value.UpperLeft.X, enemies.First.Value.UpperLeft.Y - 100), Color.Black);
            spriteBatch.DrawString(calibri, hero.health.ToString(), new Vector2(hero.UpperLeft.X, hero.UpperLeft.Y - 100), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        void FightBegin()
        {

            LinkedList<Character> temp = new LinkedList<Character>();
            LinkedListNode<Character> tempNode;
            //Determine turn order and place contents in priority<>
            //Determine hero turn order
            for (int i = 0; i < heroes.Count; i++)
            {
                if (priority.Count == 0)
                {
                    temp.AddFirst(heroes[i]);
                }
                else foreach (Character character in priority)
                    {
                        tempNode = new LinkedListNode<Character>(character);

                        if (heroes[i].speed > character.speed)
                        {
                            temp.AddBefore(tempNode, heroes[i]);
                        }
                        else if (heroes[i].speed < character.speed)
                        {
                            temp.AddAfter(tempNode, heroes[i]);
                        }
                    }
            }
            //Determine enemy turn order
            foreach (Character enemy in enemies)
            {
                if (priority.Count == 0)
                {
                    temp.AddFirst(enemy);
                }
                foreach (Character character in priority)
                {
                    tempNode = new LinkedListNode<Character>(character);

                    if (enemy.speed > character.speed)
                    {
                        temp.AddBefore(tempNode, enemy);
                    }
                    else if (enemy.speed < character.speed)
                    {
                        temp.AddAfter(tempNode, enemy);
                    }
                }
            }
            //Clear and change size of priority to fit actors
            priority.Clear();
            priority.Capacity = (temp.Count);

            //Place all actors into main collection
            foreach (Character character in temp)
            {
                priority.Add(character);
            }

            turn = 0;
        }
    }
}
