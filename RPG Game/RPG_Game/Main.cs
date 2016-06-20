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

        Character target = new Character();
        List<Character> targets = new List<Character>(0);

        SpriteFont calibri;

        List<Character> heroes = new List<Character>(4);
        LinkedList<Character> enemies = new LinkedList<Character>();
        List<Character> priority = new List<Character>();

        LinkedList<Button> allButtons = new LinkedList<Button>();
        List<Button> fightButtons = new List<Button>(2);

        bool menuActive = true;
        int targetIndex = 0;
        int buttonIndex = 0;
        int turn;
        float damageDealt;
        float damageLocation;
        double timer;

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
            demon.speed = 10;
            demon.Acc = 100;
            demon.Eva = 0;
            demon.attacking = true;
            demon.friendly = false;
            enemies.AddFirst(demon);


            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 60);
            button.action = "FIGHT";
            allButtons.AddFirst(button);
            fightButtons.Add(button);

            button = new Button();
            button.SetTexture(buttonTexture);
            button.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - button.GetHeight() - 5);
            button.action = "QUIT";
            allButtons.AddFirst(button);
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

            if (turn >= priority.Count)
            {
                turn = 0;
            }
            if (targetIndex >= targets.Count)
            {
                targetIndex = 0;
            }

            if (battleButtons == BattleButtons.idle)
            {
                if(priority[turn].friendly)
                {
                    battleButtons = BattleButtons.battleMenu;
                }
                else
                {
                    battleButtons = BattleButtons.animating;
                    target = heroes[0];
                    timer = gameTime.TotalGameTime.TotalSeconds;
                }
            }

            if(battleButtons == BattleButtons.targetMenu)
            {
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
                    pointer.UpperLeft = new Vector2(targets[targetIndex].UpperLeft.X + 5, targets[targetIndex].UpperLeft.Y + 5);
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

                        foreach(Character enemy in enemies)
                        {
                            targets.Capacity += 1;
                            targets.Add(enemy);
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
                foreach (Button button in allButtons)
                {
                    button.Draw(spriteBatch);

                    spriteBatch.DrawString(calibri, button.action, new Vector2(button.UpperLeft.X + 70, button.UpperLeft.Y + 8), Color.Black, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                }
            }

            foreach (Character enemy in enemies)
            {
                enemy.Draw(spriteBatch);
                spriteBatch.DrawString(calibri, enemy.health.ToString(), new Vector2(enemy.UpperLeft.X, enemy.UpperLeft.Y + enemy.GetHeight() + 5), Color.Black);
            }

            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].Draw(spriteBatch);
                spriteBatch.DrawString(calibri, heroes[i].health.ToString(), new Vector2(heroes[i].UpperLeft.X, heroes[i].UpperLeft.Y + heroes[i].GetHeight() + 5), Color.Black);
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

        void FightBegin()
        {

            LinkedList<Character> temp = new LinkedList<Character>();

            Character dummy = new Character();
            dummy.speed = 0;
            temp.AddLast(dummy);

            //Determine turn order and place contents in priority<>
            //Determine hero turn order
            for (int i = 0; i < heroes.Count; i++)
            {
                if (temp.Count == 0)
                {
                    temp.AddFirst(heroes[i]);
                }
                else for (LinkedListNode<Character> tempNode = temp.First; tempNode != null; tempNode = tempNode.Next)
                    {

                        if (heroes[i].speed > tempNode.Value.speed)
                        {
                            temp.AddBefore(tempNode, heroes[i]);
                            break;
                        }
                    }
            }
            //Determine enemy turn order
            foreach (Character enemy in enemies)
            {
                if (temp.Count == 0)
                {
                    temp.AddFirst(enemy);
                }
                else for (LinkedListNode<Character> tempNode = temp.First; tempNode != null; tempNode = tempNode.Next)
                    {
                        if (enemy.speed > tempNode.Value.speed)
                        {
                            temp.AddBefore(tempNode, enemy);
                            break;
                        }
                    }
            }
            temp.RemoveLast();

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

        void Attack(GameTime gameTime)
        {
            priority[turn].Move();

            Character attacker = priority[turn];
            int modifier;

            if (priority[turn].friendly)
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

                turn++;

                damageDealt = 0;
                damageLocation = 0;
                timer = 0;

                if (turn >= 1)
                {
                    priority[turn - 1].velocity = new Vector2(0, 0);
                    priority[turn - 1].UpperLeft = priority[turn - 1].battleOrigin;
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.5)
            {
                priority[turn].velocity = new Vector2(-2 * modifier, 0);
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1)
            {
                priority[turn].velocity = new Vector2(0, 0);

                if (damageDealt == 0)
                {
                    damageDealt = (priority[turn].PhAtk);
                    target.health -= damageDealt;
                    damageLocation = 30;

                    if (target.health <= 0)
                    {
                        target.IsAlive = false;

                        priority.Remove(target);
                        enemies.Remove(target);
                        heroes.Remove(target);

                        turn = priority.IndexOf(attacker);
                    }
                }
            }
            else
            {
                priority[turn].velocity = new Vector2(2 * modifier, 0);
            }

        }
    }
}
