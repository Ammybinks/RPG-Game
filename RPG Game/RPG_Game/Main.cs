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

        Texture2D background;
        Sprite backgroundSprite = new Sprite();

        Texture2D pointerTexture;
        Sprite pointer = new Sprite();
        
        Texture2D heroTexture;
        Character hero;

        Texture2D hiroTexture;
        Character hiro;

        Texture2D hearoTexture;
        Character hearo;

        Texture2D hieroTexture;
        Character hiero;

        List<Character> heroes = new List<Character>(4);

        Texture2D werewolfTexture;
        Character werewolf;

        List<Character> enemies = new List<Character>(4);

        List<Character> battlers = new List<Character>();

        Box box;

        List<Box> allBoxes = new List<Box>(1);

        List<Button> activeButtons;

        Texture2D iconTexture;
        Button button;

        Ability ability;
        Action<GameTime> currentAction;
        
        Texture2D cornerTexture;
        Texture2D wallTexture;
        Texture2D backTexture;

        Texture2D meterTexture;

        Texture2D shadowTexture;
        LinkedList<Sprite> shadows = new LinkedList<Sprite>();

        Character target = new Character();
        List<Character> targets = new List<Character>(0);

        Character actor = new Character();

        SpriteFont calibri;

        int targetIndex = 0;
        int buttonIndex = 0;
        float damageDealt;
        float damageLocation;
        double timer;

        Random rand = new Random();

        bool[] state = new bool[7];
        int currentState;

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

            background = Content.Load<Texture2D>("Backgrounds\\Translucent");

            pointerTexture = Content.Load<Texture2D>("Misc\\Pointer");
            meterTexture = Content.Load<Texture2D>("Misc\\ActionBar");
            shadowTexture = Content.Load<Texture2D>("Misc\\Shadow");
            iconTexture = Content.Load<Texture2D>("Misc\\IconSet");

            cornerTexture = Content.Load<Texture2D>("Misc\\Interface\\Interface Corner");
            wallTexture = Content.Load<Texture2D>("Misc\\Interface\\Interface Wall Expandable");
            backTexture = Content.Load<Texture2D>("Misc\\Interface\\Interface Back");

            heroTexture = Content.Load<Texture2D>("Characters\\Heroes\\The Adorable Manchild");
            hiroTexture = Content.Load<Texture2D>("Characters\\Heroes\\The Absolutely-Not-Into-It Love Interest");
            hearoTexture = Content.Load<Texture2D>("Characters\\Heroes\\The Endearing Father Figure");
            hieroTexture = Content.Load<Texture2D>("Characters\\Heroes\\The Comic Relief");
            
            werewolfTexture = Content.Load<Texture2D>("Characters\\Enemies\\Werewolf");

            //Miscellaneous Initialization Begins//
            backgroundSprite.SetTexture(background);
            backgroundSprite.UpperLeft = new Vector2(0, (graphics.PreferredBackBufferHeight - backgroundSprite.GetHeight()) * -1);
            backgroundSprite.Scale = new Vector2(2f, 2f);

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            
            target.IsAlive = false;
            //Miscellaneous Initialization Ends//

            //Heroes Initialization Begins//
            //Hero Initialization
            hero = new Character();
            hero.SetTexture(heroTexture, 9, 6);
            hero.AnimationInterval = 250;
            hero.reverseAnimating = true;
            hero.ContinuousAnimation = false;
            hero.UpperLeft = new Vector2(500, 200);
            hero.battleOrigin = hero.UpperLeft;
            hero.health = 1000;
            hero.PhAtk = 10;
            hero.PhDef = 75;
            hero.speed = 5;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.friendly = true;
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterSprite.UpperLeft = new Vector2(hero.UpperLeft.X,
                                                     hero.UpperLeft.Y + hero.meterSprite.GetHeight() + (hero.GetHeight()) + 20);
            hero.shadow.SetTexture(shadowTexture);
            heroes.Add(hero);

            //Hiro Initialization
            hiro = new Character();
            hiro.SetTexture(hiroTexture, 9, 6);
            hiro.AnimationInterval = 250;
            hiro.reverseAnimating = true;
            hiro.ContinuousAnimation = false;
            hiro.UpperLeft = new Vector2(475, 350);
            hiro.battleOrigin = hiro.UpperLeft;
            hiro.health = 100;
            hiro.PhAtk = 5;
            hiro.PhDef = 50;
            hiro.speed = 20;
            hiro.Acc = 100;
            hiro.Eva = 0;
            hiro.friendly = true;
            hiro.meterSprite.SetTexture(meterTexture);
            hiro.meterSprite.UpperLeft = new Vector2(hiro.UpperLeft.X,
                                                     hiro.UpperLeft.Y + hiro.meterSprite.GetHeight() + (hiro.GetHeight()) + 20);
            hiro.shadow.SetTexture(shadowTexture);
            heroes.Add(hiro);

            //Hearo Initialization
            hearo = new Character();
            hearo.SetTexture(hearoTexture, 9, 6);
            hearo.AnimationInterval = 250;
            hearo.reverseAnimating = true;
            hearo.ContinuousAnimation = false;
            hearo.UpperLeft = new Vector2(450, 500);
            hearo.battleOrigin = hearo.UpperLeft;
            hearo.health = 5;
            hearo.PhAtk = 2;
            hearo.PhDef = 0;
            hearo.speed = 50;
            hearo.Acc = 100;
            hearo.Eva = 50;
            hearo.friendly = true;
            hearo.meterSprite.SetTexture(meterTexture);
            hearo.meterSprite.UpperLeft = new Vector2(hearo.UpperLeft.X,
                                                      hearo.UpperLeft.Y + hearo.meterSprite.GetHeight() + (hearo.GetHeight()) + 20);
            hearo.shadow.SetTexture(shadowTexture);

            ////Hearo Ability Initialization
            //Attack Ability
            ability = new Ability();
            ability.action = Attack;
            ability.display = "ATTACK - Generic attack bullshit";

            ability.frameHeight = 50;
            ability.UpperLeft = new Vector2(10, 10);
            ability.SetParts(cornerTexture, wallTexture, backTexture);
            ability.icon.SetTexture(iconTexture, 16, 20);
            ability.icon.setCurrentFrame(3, 4);
            ability.icon.UpperLeft = new Vector2(ability.UpperLeft.X + 10, ability.UpperLeft.Y + 9);
            hearo.abilities.Add(ability);

            //Murder Ability
            ability = new Ability();
            ability.action = Murder;
            ability.display = "KILL THEM - Like, seriously. Kill them already";
            
            ability.frameHeight = 50;
            ability.UpperLeft = new Vector2(10, 10);
            ability.SetParts(cornerTexture, wallTexture, backTexture);
            ability.icon.SetTexture(iconTexture, 16, 20);
            ability.icon.setCurrentFrame(10, 0);
            ability.icon.UpperLeft = new Vector2(ability.UpperLeft.X + 10, ability.UpperLeft.Y + 9);
            hearo.abilities.Add(ability);

            heroes.Add(hearo);

            //Hiero Initialization
            hiero = new Character();
            hiero.SetTexture(hieroTexture, 9, 6);
            hiero.AnimationInterval = 250;
            hiero.reverseAnimating = true;
            hiero.ContinuousAnimation = false;
            hiero.UpperLeft = new Vector2(425, 650);
            hiero.battleOrigin = hiero.UpperLeft;
            hiero.health = 50;
            hiero.PhAtk = 5;
            hiero.PhDef = 70;
            hiero.speed = 10;
            hiero.Acc = 100;
            hiero.Eva = 0;
            hiero.friendly = true;
            hiero.meterSprite.SetTexture(meterTexture);
            hiero.meterSprite.UpperLeft = new Vector2(hiero.UpperLeft.X, 
                                                      hiero.UpperLeft.Y + hiero.meterSprite.GetHeight() + (hiero.GetHeight()) + 20);
            hiero.shadow.SetTexture(shadowTexture);
            heroes.Add(hiero);
            //Heroes Initialization Ends//

            //Enemies Initialization Begins//
            werewolf = new Character();
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1200, 200);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.health = 20;
            werewolf.PhAtk = 99999;
            werewolf.PhDef = 0;
            werewolf.speed = 1;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            werewolf.shadow.SetTexture(shadowTexture);
            werewolf.shadow.IsAlive = false;
            enemies.Add(werewolf);

            werewolf = new Character();
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1000, 300);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.health = 35;
            werewolf.PhAtk = 100;
            werewolf.PhDef = 50;
            werewolf.speed = 10;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            werewolf.shadow.SetTexture(shadowTexture);
            werewolf.shadow.IsAlive = false;
            enemies.Add(werewolf);

            werewolf = new Character();
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1200, 400);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.health = 5;
            werewolf.PhAtk = 2;
            werewolf.PhDef = 0;
            werewolf.speed = 49;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            werewolf.shadow.SetTexture(shadowTexture);
            werewolf.shadow.IsAlive = false;
            enemies.Add(werewolf);

            werewolf = new Character();
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1000, 500);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.health = 50;
            werewolf.PhAtk = 10;
            werewolf.PhDef = -50;
            werewolf.speed = 5;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            werewolf.shadow.SetTexture(shadowTexture);
            werewolf.shadow.IsAlive = false;
            enemies.Add(werewolf);
            //Enemies Initialization Ends//

            //Boxes Initialization Begins//
            //Battle Menu Box
            box = new Box();
            box.frameWidth = 230;
            box.frameHeight = 190;
            box.UpperLeft = new Vector2(5, graphics.PreferredBackBufferHeight - box.GetHeight() - 5);
            box.SetParts(cornerTexture, wallTexture, backTexture);
            box.activatorState = 3;
            box.buttons = new List<Button>(3);
            allBoxes.Add(box);
            
            //Button that advances into targeting state
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((box.buttons.Capacity - box.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "FIGHT";
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(12, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            box.buttons.Add(button);

            //Button that advances into skillsMenu Box
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((box.buttons.Capacity - box.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "SKILL";
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(15, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            box.buttons.Add(button);

            //Button that exits the game
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((box.buttons.Capacity - box.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "QUIT";
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(2, 5);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            box.buttons.Add(button);
            
            //skillsMenu Box
            box = new Box();
            box.frameWidth = 800;
            box.frameHeight = 800;
            box.UpperLeft = new Vector2(graphics.PreferredBackBufferWidth / 2 - box.frameWidth / 2, 5);
            box.SetParts(cornerTexture, wallTexture, backTexture);
            box.activatorState = 4;
            box.buttons = new List<Button>(0);
            allBoxes.Add(box);
            //Boxes Initialization Ends//

            //State Inititalization Begins//
            state[0] = true; //Idle State
            state[1] = false; //Step Forwards State
            state[2] = false; //Animating State
            state[3] = false; //Battle Menu State
            state[4] = false; //Skills Menu State
            state[5] = false; //Items Menu State
            state[6] = false; //Targeting Menu State
            //State Initialization Ends//

            currentState = 0;

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
        /// <param display="gameTime">Provides a snapshot of timing values.</param>
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

            //Idle State//
            //Ticks up all the battler's meters based on their speed values, and allows them to act if theirs is full
            //If two or more characters are able to act in the same tick, randomly choose between them
            if (currentState == 0)
            {
                List<Character> potentialActions = new List<Character>();

                for (int i = 0; i < battlers.Count; i++)
                {
                    battlers[i].meter += battlers[i].speed / 100;

                    //If the meter is full for this character
                    if (battlers[i].meter >= 100)
                    {
                        //Add it to the list of potential actors this turn
                        potentialActions.Capacity++;
                        potentialActions.Add(battlers[i]);
                    }

                    //Scale the display bar in accordance with the actor's current meter
                    battlers[i].meterSprite.Scale = new Vector2(battlers[i].meter / 100, 1);
                }

                //If we have at least one character acting this turn
                if (potentialActions.Count != 0)
                {
                    //Create an index that randomly chooses between all of them
                    //(Will always return 0 if there's only one actor in the list)
                    int index = rand.Next(0, potentialActions.Count);

                    //Reset the acting character's meter and bar
                    potentialActions[index].meter = 0;
                    potentialActions[index].meterSprite.Scale = new Vector2(potentialActions[index].meter / 100, 1);

                    //Set actor to the acting character
                    actor = potentialActions[index];

                    //Switch to Step Forwards State
                    ActivateState(1);

                    timer = gameTime.TotalGameTime.TotalSeconds;

                }
            }
            else if (currentState == 1) //Step Forwards State//
            {
                if (Step(gameTime, timer, new Vector2(2, 0), 0.25, actor))
                {
                    //If the actor is playable
                    if (actor.friendly)
                    {
                        actor.setCurrentFrame(0, 1);
                        actor.animationShortStarted = false;

                        //Switch to Battle Menu State
                        ActivateState(3);

                        for (int i = 0; i < allBoxes.Count; i++)
                        {
                            if (allBoxes[i].activatorState == currentState)
                            {
                                activeButtons = allBoxes[i].buttons;
                            }
                        }
                    }
                    else
                    {
                        //Select the target randomly from all heroes
                        target = heroes[rand.Next(0, 3)];

                        //Switch to Animating State
                        ActivateState(2);

                        timer = gameTime.TotalGameTime.TotalSeconds;
                    }
                    timer = gameTime.TotalGameTime.TotalSeconds;
                }
            }
            else if (currentState == 2) //Animating State//
            {
                currentAction(gameTime);
            }
            else if (currentState == 3) //Battle Menu State//
            {
                pointer.IsAlive = true;
                pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

                if (actor.getTotalFrame() != 0 && actor.getTotalFrame() != 2 && !actor.IsAnimating())
                {
                    actor.setCurrentFrame(0, 0);
                }

                if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
                {
                    buttonIndex -= 1;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = activeButtons.Count - 1;
                    }
                }
                if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                {
                    buttonIndex += 1;
                    if (buttonIndex > activeButtons.Count - 1)
                    {
                        buttonIndex = 0;
                    }
                }
                if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) || ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
                {
                    if (activeButtons[buttonIndex].display == "FIGHT")
                    {
                        //Switch to Target Menu State
                        ActivateState(6);

                        currentAction = Attack;

                        for (int i = 0; i < enemies.Count; i++)
                        {
                            targets.Capacity += 1;
                            targets.Add(enemies[i]);
                        }
                    }
                    else if (activeButtons[buttonIndex].display == "SKILL")
                    {
                        //Switch to Skill Menu State
                        ActivateState(4);

                        for (int i = 0; i < allBoxes.Count; i++)
                        {
                            if (allBoxes[i].activatorState == currentState)
                            {
                                allBoxes[i].buttons.Clear();
                                allBoxes[i].buttons.Capacity = 0;

                                for (int o = 0; o < actor.abilities.Count; o++)
                                {
                                    actor.abilities[o].UpperLeft = new Vector2(allBoxes[i].UpperLeft.X + 80, allBoxes[i].UpperLeft.Y + 10 + (60 * o));
                                    actor.abilities[o].frameWidth = allBoxes[i].frameWidth - 90;
                                    actor.abilities[o].SetParts(cornerTexture, wallTexture, backTexture);
                                    actor.abilities[o].icon.UpperLeft = new Vector2(actor.abilities[o].UpperLeft.X + 10, actor.abilities[o].UpperLeft.Y + 9);

                                    allBoxes[i].buttons.Add(actor.abilities[o]);
                                }

                                allBoxes[i].frameHeight = (int)allBoxes[i].buttons[allBoxes[i].buttons.Count - 1].UpperLeft.Y + 60;
                                allBoxes[i].SetParts(cornerTexture, wallTexture, backTexture);

                                activeButtons = allBoxes[i].buttons;

                                buttonIndex = 0;
                            }
                        }
                    }
                    else if (activeButtons[buttonIndex].display == "QUIT")
                    {
                        Exit();
                    }
                }
            }
            else if (currentState == 4) //Skills Menu State//
            {
                pointer.IsAlive = true;
                pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

                if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
                {
                    buttonIndex -= 1;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = activeButtons.Count - 1;
                    }
                }
                if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                {
                    buttonIndex += 1;
                    if (buttonIndex > activeButtons.Count - 1)
                    {
                        buttonIndex = 0;
                    }
                }
                if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) || ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
                {
                    ActivateState(6);

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        targets.Capacity += 1;
                        targets.Add(enemies[i]);
                    }
                    currentAction = actor.abilities[buttonIndex].action;
                }
            }
            else if (currentState == 6) //Targeting State//
            {
                if (targetIndex >= targets.Count)
                {
                    targetIndex = 0;
                }

                if (targets.Count <= 1)
                {
                    //Switch to Animating State
                    ActivateState(2);

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
                            targetIndex = targets.Count - 1;
                        }
                    }
                    if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
                    {
                        targetIndex += 1;
                        if (targetIndex > targets.Count - 1)
                        {
                            targetIndex = 0;
                        }
                    }
                    if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) || ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
                    {
                        //Switch to Animating State
                        ActivateState(2);
                        
                        target = targets[targetIndex];
                        targets.Clear();
                        targets.Capacity = 0;
                        timer = gameTime.TotalGameTime.TotalSeconds + 1;
                    }
                }
            }


            oldKeyState = currentKeyState;
            oldMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param display="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            backgroundSprite.Draw(spriteBatch);
            
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].shadow.UpperLeft = new Vector2(enemies[i].UpperLeft.X - (enemies[i].shadow.GetWidth() / 2 - enemies[i].GetWidth() / 2),
                                                          enemies[i].UpperLeft.Y + (enemies[i].GetHeight() - (enemies[i].shadow.GetHeight() / 2)));
                enemies[i].shadow.Draw(spriteBatch);

                enemies[i].Draw(spriteBatch);

                enemies[i].meterSprite.Draw(spriteBatch);

                if (enemies[i].health > 0)
                {
                    spriteBatch.DrawString(calibri,
                                           enemies[i].health.ToString(),
                                           new Vector2(enemies[i].UpperLeft.X,
                                           enemies[i].UpperLeft.Y + enemies[i].GetHeight() + 5),
                                           Color.Black,
                                           0,
                                           new Vector2(0, 0),
                                           0.75f,
                                           SpriteEffects.None, 0);
                }

                enemies[i].Move();
                enemies[i].Animate(gameTime);
            }

            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].shadow.UpperLeft = new Vector2(heroes[i].UpperLeft.X - (heroes[i].shadow.GetWidth() / 2 - heroes[i].GetWidth() / 2),
                                                         heroes[i].UpperLeft.Y + (heroes[i].GetHeight() - (heroes[i].shadow.GetHeight() / 2)));
                heroes[i].shadow.Draw(spriteBatch);

                heroes[i].Draw(spriteBatch);

                heroes[i].meterSprite.Draw(spriteBatch);

                if(heroes[i].health > 0)
                {
                    spriteBatch.DrawString(calibri,
                                           heroes[i].health.ToString(),
                                           new Vector2(heroes[i].UpperLeft.X,
                                           heroes[i].UpperLeft.Y + heroes[i].GetHeight() + 5),
                                           Color.Black,
                                           0,
                                           new Vector2(0, 0),
                                           0.75f,
                                           SpriteEffects.None, 0);
                }

                if (!heroes[i].IsAnimating())
                {
                    if (heroes[i].getTotalFrame() == 0 || heroes[i].getTotalFrame() == 2)
                    {
                        heroes[i].AnimationInterval = rand.Next(100, 500);

                        if (!heroes[i].reverseAnimating)
                        {
                            heroes[i].reverseAnimating = true;
                            heroes[i].StartAnimationShort(0, 2, 2);
                        }
                        else
                        {
                            heroes[i].reverseAnimating = false;
                            heroes[i].StartAnimationShort(0, 2, 0);
                        }
                    }
                }

                heroes[i].Move();
                heroes[i].Animate(gameTime);
            }

            if (damageDealt > 0)
            {
                spriteBatch.DrawString(calibri,
                                       damageDealt.ToString(),
                                       new Vector2(target.UpperLeft.X,
                                       target.UpperLeft.Y - damageLocation),
                                       Color.Black,
                                       0,
                                       new Vector2(0, 0),
                                       1f,
                                       SpriteEffects.None, 0);

                damageLocation += damageLocation / 32;
            }

            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (state[allBoxes[i].activatorState])
                {
                    for (int o = 0; o < allBoxes[i].parts.Count; o++)
                    {
                        allBoxes[i].parts[o].Draw(spriteBatch);
                    }

                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        for (int p = 0; p < allBoxes[i].buttons[o].parts.Count; p++)
                        {
                            allBoxes[i].buttons[o].parts[p].Draw(spriteBatch);
                        }

                        if (allBoxes[i].buttons[o].icon != null)
                        {
                            allBoxes[i].buttons[o].icon.Draw(spriteBatch);
                        }

                        spriteBatch.DrawString(calibri,
                                               allBoxes[i].buttons[o].display,
                                               new Vector2(allBoxes[i].buttons[o].UpperLeft.X + 70, allBoxes[i].buttons[o].UpperLeft.Y + 8),
                                               Color.Black,
                                               0,
                                               new Vector2(0, 0),
                                               1f,
                                               SpriteEffects.None, 0);
                    }
                }
            }

            pointer.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Activates the target state, setting all states to false, while keeping the target state true
        void ActivateState(int targetState)
        {
            //Set all states to false
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = false;
            }

            currentState = targetState;
            //Set the target state to true
            state[targetState] = true;
        }

        //Switches the target state, setting it to true if it was false, and false otherwise
        //If target state was false, currentState will be set to the last state still active
        void SwitchState(int targetState)
        {
            if (state[targetState])
            {
                state[targetState] = false;
                
                //Find the last state still active, and set currentSignals to it
                for (int i = state.Length; i >= 0; i--)
                {
                    if (state[i])
                    {
                        currentState = i;
                    }
                }
            }
            else
            {
                state[targetState] = true;

                currentState = targetState;
            }
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

        //Animates a basic attack, animating both the actor and target, and removing health from the target.
        //Keep in mind that the actor has already been moved forwards by the time this is called, 
        //and that it is this function's job to return all it's actors to their original positions
        void Attack(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.6)
            {
                //Switch to Idle State
                ActivateState(0);

                //Reset all values ready for continuing the fight
                damageDealt = 0;
                damageLocation = 0;
                timer = 0;

                //Make sure our actor and targets aren't moving or animating and are in the correct position
                actor.velocity = new Vector2(0, 0);
                actor.UpperLeft = actor.battleOrigin;
                actor.setCurrentFrame(0, 0);
                actor.animationShortStarted = false;

                target.velocity = new Vector2(0, 0);
                target.UpperLeft = target.battleOrigin;
                target.setCurrentFrame(0, 0);
                target.animationShortStarted = false;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.25)
            {
                actor.animationShortStarted = false;
                actor.setCurrentFrame(0, 1);

                //If the target is dead
                if (target.health <= 0)
                {
                    target.IsAlive = false;

                    battlers.Remove(target);
                    enemies.Remove(target);
                    heroes.Remove(target);
                }

                //Reset target to a neutral frame
                target.setCurrentFrame(0, 0);

                //Stepping characters back to starting position
                Step(gameTime, timer, new Vector2(-2, 0), 1.5, actor);
                Step(gameTime, timer, new Vector2(2, 0), 1.5, target);
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.75)
            {
                //Step the target backwards
                Step(gameTime, timer, new Vector2(-2, 0), 1, target);

                //If we haven't dealt damage yet (Single run conditional)
                if (damageDealt == 0)
                {
                    //Deal damage according to physical attack, reduced by physical defence
                    damageDealt = (actor.PhAtk * ((100 - target.PhDef) / 100));
                    damageDealt = (float)Math.Round(damageDealt, 0, MidpointRounding.AwayFromZero);
                    target.health -= (int)damageDealt;

                    //Reset the damage indicator
                    damageLocation = 30;

                    //Start animation on target, making sure they're not reversing or stepping through too fast or slow
                    target.reverseAnimating = false;
                    target.AnimationInterval = 50;
                    target.StartAnimationShort(36, 38, 38);
                }
            }
            else if(gameTime.TotalGameTime.TotalSeconds <= timer)
            {
                //Start animating the actor's attack
                actor.reverseAnimating = false;
                actor.AnimationInterval = 250;
                actor.StartAnimationShort(2, 5, 5);

                //Reset the timer (Should only run once)
                timer = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        //Jokey test copy of Attack(), deals 1000x the damage Attack() does, to be removed later on.
        void Murder(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.6)
            {
                //Switch to Idle State
                ActivateState(0);

                //Reset all values ready for continuing the fight
                damageDealt = 0;
                damageLocation = 0;
                timer = 0;

                //Make sure our actor and targets aren't moving or animating and are in the correct position
                actor.velocity = new Vector2(0, 0);
                actor.UpperLeft = actor.battleOrigin;
                actor.setCurrentFrame(0, 0);
                actor.animationShortStarted = false;

                target.velocity = new Vector2(0, 0);
                target.UpperLeft = target.battleOrigin;
                target.setCurrentFrame(0, 0);
                target.animationShortStarted = false;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.25)
            {
                actor.animationShortStarted = false;
                actor.setCurrentFrame(0, 1);

                //If the target is dead
                if (target.health <= 0)
                {
                    target.IsAlive = false;

                    battlers.Remove(target);
                    enemies.Remove(target);
                    heroes.Remove(target);
                }

                //Reset target to a neutral frame
                target.setCurrentFrame(0, 0);

                //Stepping characters back to starting position
                Step(gameTime, timer, new Vector2(-2, 0), 1.5, actor);
                Step(gameTime, timer, new Vector2(2, 0), 1.5, target);
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.75)
            {
                //Step the target backwards
                Step(gameTime, timer, new Vector2(-2, 0), 1, target);

                //If we haven't dealt damage yet (Single run conditional)
                if (damageDealt == 0)
                {
                    //Deal damage according to physical attack, reduced by physical defence
                    damageDealt = (actor.PhAtk * ((100 - target.PhDef) / 100)) * 1000;
                    damageDealt = (float)Math.Round(damageDealt, 0, MidpointRounding.AwayFromZero);
                    target.health -= (int)damageDealt;

                    //Reset the damage indicator
                    damageLocation = 30;

                    //Start animation on target, making sure they're not reversing or stepping through too fast or slow
                    target.reverseAnimating = false;
                    target.AnimationInterval = 50;
                    target.StartAnimationShort(36, 38, 38);
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= timer)
            {
                //Start animating the actor's attack
                actor.reverseAnimating = false;
                actor.AnimationInterval = 250;
                actor.StartAnimationShort(2, 5, 5);

                //Reset the timer (Should only run once)
                timer = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        //Steps a character forwards in a direction for a specified time, returning true if the movement is finished, false if not
        bool Step(GameTime gameTime, double timer, Vector2 speed, double duration, Character character)
        {
            //Initialise a modifier value for determining which direction to step the character
            int modifier = 1;
            if(!character.friendly)
            {
                modifier = -1;
            }

            return Step(gameTime, timer, speed, duration, character, modifier);
        }
        bool Step(GameTime gameTime, double timer, Vector2 speed, double duration, Character character, int modifier)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= timer + duration)
            {
                character.velocity = new Vector2(0, 0);

                return true;
            }
            else
            {
                //Starts moving the character in the specified direction, inverting if the character is on the right side of the field
                character.velocity = new Vector2(speed.X * modifier, speed.Y);

                return false;
            }
        }
        
        void test()
        {

        }
    }
}