using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RPG_Game
{
    public class BattleState : StateManager
    {
        Texture2D background;
        Sprite backgroundSprite = new Sprite();
        
        Texture2D heroTexture;
        Battler hero;

        Texture2D hiroTexture;
        Battler hiro;

        Texture2D hearoTexture;
        Battler hearo;

        Texture2D hieroTexture;
        Battler hiero;

        Texture2D werewolfTexture;
        Battler werewolf;

        List<Battler> heroes = new List<Battler>(4);
        List<Battler> enemies = new List<Battler>(4);

        List<Battler> battlers = new List<Battler>();
        
        Texture2D meterTexture;

        Texture2D shadowTexture;
        LinkedList<Sprite> shadows = new LinkedList<Sprite>();

        Battler target = new Battler();
        List<Battler> targets = new List<Battler>(0);

        Battler actor = new Battler();
        
        Random rand = new Random();

        Ability ability;
        Action<GameTime> currentAction;

        int targetIndex = 0;
        int buttonIndex = 0;
        float damageDealt;
        float damageLocation;
        bool actionComplete;

        Action<GameTime>[] stateMethods = new Action<GameTime>[8];
        bool[] state = new bool[8];
        int currentState;

        public override void LoadContent(Main main)
        {
            calibri = main.Content.Load<SpriteFont>("Fonts\\Calibri");

            background = main.Content.Load<Texture2D>("World\\Battle Backs\\Translucent");

            pointerTexture = main.Content.Load<Texture2D>("Misc\\Pointer");
            iconTexture = main.Content.Load<Texture2D>("Misc\\IconSet");

            meterTexture = main.Content.Load<Texture2D>("Misc\\ActionBar");
            shadowTexture = main.Content.Load<Texture2D>("Misc\\Shadow");

            cornerTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Corner");
            wallTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Wall Expandable");
            backTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Back");

            heroTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Battle\\The Adorable Manchild");
            hiroTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Battle\\The Absolutely-Not-Into-It Love Interest");
            hearoTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Battle\\The Endearing Father Figure");
            hieroTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Battle\\The Comic Relief");

            werewolfTexture = main.Content.Load<Texture2D>("Characters\\Enemies\\Werewolf");
            
            //Miscellaneous Initialization Begins//
            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);

            backgroundSprite.SetTexture(background);
            backgroundSprite.UpperLeft = new Vector2(0, (main.graphics.PreferredBackBufferHeight - backgroundSprite.GetHeight()) * -1);
            backgroundSprite.Scale = new Vector2(2f, 2f);

            //Heroes Initialization Begins//
            //Hero Initialization
            hero = new Battler();
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
            hiro = new Battler();
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
            hearo = new Battler();
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
            hiero = new Battler();
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
            werewolf = new Battler();
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
            enemies.Add(werewolf);
            ////enemies.Add(werewolf);

            werewolf = new Battler();
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
            enemies.Add(werewolf);

            werewolf = new Battler();
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
            enemies.Add(werewolf);
            ////enemies.Add(werewolf);

            werewolf = new Battler();
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
            enemies.Add(werewolf);

            //Boxes Initialization Begins//
            //Battle Menu Box
            box = new Box();
            box.frameWidth = 230;
            box.frameHeight = 190;
            box.UpperLeft = new Vector2(5, main.graphics.PreferredBackBufferHeight - box.GetHeight() - 5);
            box.SetParts(cornerTexture, wallTexture, backTexture);
            box.activatorState = 3;
            box.activatorTrack = 1;
            box.buttons = new List<Button>(3);
            allBoxes.Add(box);

            //Button that advances into targeting state
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, main.graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((box.buttons.Capacity - box.buttons.Count) - 1)));
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
            button.UpperLeft = new Vector2(75, main.graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((box.buttons.Capacity - box.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "SKILL";
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(15, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            box.buttons.Add(button);

            //skillsMenu Box
            box = new Box();
            box.frameWidth = 800;
            box.frameHeight = 800;
            box.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth / 2 - box.frameWidth / 2, 5);
            box.SetParts(cornerTexture, wallTexture, backTexture);
            box.activatorState = 4;
            box.activatorTrack = 1;
            box.buttons = new List<Button>(0);
            allBoxes.Add(box);

            stateMethods[0] = Idle;
            stateMethods[1] = StepForwards;
            stateMethods[2] = Animate;
            stateMethods[3] = Menu;
            stateMethods[4] = SkillsMenu;
            stateMethods[5] = Idle;
            stateMethods[6] = TargetMenu;
            stateMethods[7] = StateSwitch;

            targetState = 1;

            FightBegin();
        }
        
        public override void Update(GameTime gameTime)
        {
            targetState = 1;

            //Execute main update method
            if (currentState == 0)
            {
                Idle(gameTime);
            }
            else if (currentState == 1)
            {
                StepForwards(gameTime);
            }
            else if (currentState == 2)
            {
                Animate(gameTime);
            }
            else if (currentState == 3)
            {
                Menu(gameTime);
            }
            else if (currentState == 4)
            {
                SkillsMenu(gameTime);
            }
            else if (currentState == 6)
            {
                TargetMenu(gameTime);
            }
            else if (currentState == 7)
            {
                StateSwitch(gameTime);
            }

            //Update characters
            //Heroes
            for (int i = 0; i < heroes.Count; i++)
            {
                if (!heroes[i].IsAnimating())
                {
                    if (heroes[i].getTotalFrame() == 0 || heroes[i].getTotalFrame() == 2)
                    {
                        heroes[i].AnimationInterval = rand.Next(100, 500);

                        if (!heroes[i].reverseAnimating)
                        {
                            heroes[i].reverseAnimating = true;
                            heroes[i].StartAnimationShort(gameTime, 0, 2, 0);
                        }
                        else
                        {
                            heroes[i].reverseAnimating = false;
                            heroes[i].StartAnimationShort(gameTime, 0, 2, 0);
                        }
                    }
                }

                heroes[i].Animate(gameTime);

                heroes[i].shadow.UpperLeft = new Vector2(heroes[i].UpperLeft.X - (heroes[i].shadow.GetWidth() / 2 - heroes[i].GetWidth() / 2),
                                                         heroes[i].UpperLeft.Y + (heroes[i].GetHeight() - (heroes[i].shadow.GetHeight() / 2)));
            }
            //Enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].shadow.UpperLeft = new Vector2(enemies[i].UpperLeft.X - (enemies[i].shadow.GetWidth() / 2 - enemies[i].GetWidth() / 2),
                                                          enemies[i].UpperLeft.Y + (enemies[i].GetHeight() - (enemies[i].shadow.GetHeight() / 2)));
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Main main)
        {
            backgroundSprite.Draw(spriteBatch);

            //Enemy Draw Cycle
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);

                enemies[i].meterSprite.Draw(spriteBatch);

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

            //Hero Draw Cycle
            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].shadow.Draw(spriteBatch);

                heroes[i].Draw(spriteBatch);

                heroes[i].meterSprite.Draw(spriteBatch);

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

            //Damage Balloon Draw
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

            //Boxes \ Buttons Draw Cycle
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        allBoxes[i].buttons[o].DrawParts(spriteBatch);

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
        }

        //Ticks up all the battler's meters based on their speed values, and allows them to act if theirs is full
        //If two or more characters are able to act in the same tick, randomly choose between them
        private void Idle(GameTime gameTime)
        {
            List<Battler> potentialActions = new List<Battler>();

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

                //Switch to step.Invoke Forwards State
                ActivateState(1);

                timer = gameTime.TotalGameTime.TotalSeconds;

            }
        }

        private void StepForwards(GameTime gameTime)
        {
            if (step.Invoke(gameTime, timer, 2, 0.25, actor, new Vector2(IFF(actor), 0)))
            {
                //If the actor is playable
                if (actor.friendly)
                {
                    //actor.setCurrentFrame(0, 1);
                    //actor.animationShortStarted = false;

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

        private void Animate(GameTime gameTime)
        {
            currentAction(gameTime);

            if (actionComplete)
            {
                if (enemies.Count == 0)
                {
                    //Switch to Track Switch State
                    ActivateState(7);
                }
                else
                {
                    //Switch to Idle State
                    ActivateState(0);
                }
            }
        }

        private void Menu(GameTime gameTime)
        {
            pointer.isAlive = true;

            Rectangle buttonRect = new Rectangle((int)activeButtons[buttonIndex].UpperLeft.X,
                                                 (int)activeButtons[buttonIndex].UpperLeft.Y,
                                                 activeButtons[buttonIndex].frameWidth,
                                                 activeButtons[buttonIndex].frameHeight);

            if (mouseMoving)
            {
                for (int i = 0; i < activeButtons.Count; i++)
                {
                    buttonRect = new Rectangle((int)activeButtons[i].UpperLeft.X,
                                               (int)activeButtons[i].UpperLeft.Y,
                                               activeButtons[i].frameWidth,
                                               activeButtons[i].frameHeight);

                    if (buttonRect.Contains(mousePosition))
                    {
                        buttonIndex = i;
                    }
                }
            }

            if (upInput.inputState == Input.inputStates.pressed)
            {
                buttonIndex -= 1;
                if (buttonIndex < 0)
                {
                    buttonIndex = activeButtons.Count - 1;
                }
            }
            if (downInput.inputState == Input.inputStates.pressed)
            {
                buttonIndex += 1;
                if (buttonIndex > activeButtons.Count - 1)
                {
                    buttonIndex = 0;
                }
            }
            if (activateInput.inputState == Input.inputStates.pressed)
            {
                if (activateInput.inputType != Input.inputTypes.mouse | buttonRect.Contains(mousePosition))
                {
                    pointer.isAlive = false;

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
                        SwitchState(4);

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
                }
            }

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (actor.getTotalFrame() != 0 && actor.getTotalFrame() != 2 && !actor.IsAnimating())
            {
                actor.setCurrentFrame(0, 0);
            }
        }

        private void SkillsMenu(GameTime gameTime)
        {
            pointer.isAlive = true;

            Rectangle buttonRect = new Rectangle((int)activeButtons[buttonIndex].UpperLeft.X,
                                                 (int)activeButtons[buttonIndex].UpperLeft.Y,
                                                 activeButtons[buttonIndex].frameWidth,
                                                 activeButtons[buttonIndex].frameHeight);

            if (mouseMoving)
            {
                for (int i = 0; i < activeButtons.Count; i++)
                {
                    buttonRect = new Rectangle((int)activeButtons[i].UpperLeft.X,
                                               (int)activeButtons[i].UpperLeft.Y,
                                               activeButtons[i].frameWidth,
                                               activeButtons[i].frameHeight);

                    if (buttonRect.Contains(mousePosition))
                    {
                        buttonIndex = i;
                    }
                }
            }

            if (upInput.inputState == Input.inputStates.pressed)
            {
                buttonIndex -= 1;
                if (buttonIndex < 0)
                {
                    buttonIndex = activeButtons.Count - 1;
                }
            }
            if (downInput.inputState == Input.inputStates.pressed)
            {
                buttonIndex += 1;
                if (buttonIndex > activeButtons.Count - 1)
                {
                    buttonIndex = 0;
                }
            }
            if (activateInput.inputState == Input.inputStates.pressed)
            {
                if (activateInput.inputType != Input.inputTypes.mouse | buttonRect.Contains(mousePosition))
                {
                    pointer.isAlive = false;

                    ActivateState(6);

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        targets.Capacity += 1;
                        targets.Add(enemies[i]);
                    }

                    currentAction = actor.abilities[buttonIndex].action;

                    timer = gameTime.TotalGameTime.TotalSeconds + 1;
                }
            }

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);
        }

        private void TargetMenu(GameTime gameTime)
        {
            pointer.isAlive = true;

            if (targetIndex >= targets.Count)
            {
                targetIndex = 0;
            }

            Rectangle targetRect = new Rectangle((int)targets[targetIndex].UpperLeft.X,
                                                 (int)targets[targetIndex].UpperLeft.Y,
                                                 targets[targetIndex].GetWidth(),
                                                 targets[targetIndex].GetHeight());

            if (mouseMoving)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    targetRect = new Rectangle((int)targets[i].UpperLeft.X,
                                               (int)targets[i].UpperLeft.Y,
                                               targets[i].GetWidth(),
                                               targets[i].GetHeight());

                    if (targetRect.Contains(mousePosition))
                    {
                        targetIndex = i;
                    }
                }
            }

            if (targets.Count <= 1)
            {
                //Switch to Animating State
                ActivateState(2);

                target = targets[targetIndex];
                targets.Clear();
                targets.Capacity = 0;
                timer = gameTime.TotalGameTime.TotalSeconds + 1;
            }
            else
            {
                pointer.UpperLeft = new Vector2(targets[targetIndex].UpperLeft.X - pointer.GetWidth() - 5,
                                                targets[targetIndex].UpperLeft.Y + targets[targetIndex].GetHeight() - 5);

                if (upInput.inputState == Input.inputStates.pressed)
                {
                    targetIndex -= 1;
                    if (targetIndex < 0)
                    {
                        targetIndex = targets.Count - 1;
                    }
                }
                if (downInput.inputState == Input.inputStates.pressed)
                {
                    targetIndex += 1;
                    if (targetIndex > targets.Count - 1)
                    {
                        targetIndex = 0;
                    }
                }
                if (activateInput.inputState == Input.inputStates.pressed)
                {
                    if (activateInput.inputType != Input.inputTypes.mouse | targetRect.Contains(mousePosition))
                    {
                        pointer.isAlive = false;

                        //Switch to Animating State
                        ActivateState(2);

                        target = targets[targetIndex];
                        targets.Clear();
                        targets.Capacity = 0;
                        timer = gameTime.TotalGameTime.TotalSeconds + 1;
                    }
                }
            }
        }

        private void StateSwitch(GameTime gameTime)
        {
            targetState = 0;

            ActivateState(0);
        }

        //Activates the target state, setting all states to false, while keeping the target state true
        private void ActivateState(int targetState)
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
        private void SwitchState(int targetState)
        {
            if (state[targetState])
            {
                state[targetState] = false;

                //Find the highest value state still active, and set currentState to it
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

        //Animates a basic attack, animating both the actor and target, and removing health from the target.
        //Keep in mind that the actor has already been moved forwards by the time this is called, 
        //and that it is this function's job to return all its actors to their original positions
        private void Attack(GameTime gameTime)
        {
            actionComplete = false;

            if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.5)
            {
                //Reset all values ready for continuing the fight
                damageDealt = 0;
                damageLocation = 0;
                timer = 0;

                //Make sure our actor and targets aren't moving or animating and are in the correct position
                actor.UpperLeft = actor.battleOrigin;
                actor.setCurrentFrame(0, 0);
                actor.animationShortStarted = false;

                target.UpperLeft = target.battleOrigin;
                target.setCurrentFrame(0, 0);
                target.animationShortStarted = false;

                actionComplete = true;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.25)
            {
                actor.animationShortStarted = false;
                actor.setCurrentFrame(0, 1);

                //If the target is dead
                if (target.health <= 0)
                {
                    battlers.Remove(target);
                    enemies.Remove(target);
                    heroes.Remove(target);
                }

                //Reset target to a neutral frame
                target.setCurrentFrame(0, 0);

                //Stepping characters back to starting position
                step.Invoke(gameTime, timer, -2, 1.5, actor, new Vector2(IFF(actor), 0));
                step.Invoke(gameTime, timer, 2, 1.5, target, new Vector2(IFF(target), 0));
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.75)
            {
                //step.Invoke the target backwards
                step.Invoke(gameTime, timer, -2, 1, target, new Vector2(IFF(target), 0));

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
                    target.StartAnimationShort(gameTime, 36, 38, 38);
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= timer)
            {
                //Start animating the actor's attack
                actor.reverseAnimating = false;
                actor.AnimationInterval = 250;
                actor.StartAnimationShort(gameTime, 2, 5, 5);

                //Reset the timer (Should only run once)
                timer = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        //Jokey test copy of Attack(), deals 1000x the damage Attack() does, to be removed later on.
        private void Murder(GameTime gameTime)
        {
            actionComplete = false;

            if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.5)
            {
                //Switch to Idle State
                ActivateState(0);

                //Reset all values ready for continuing the fight
                damageDealt = 0;
                damageLocation = 0;
                timer = 0;

                //Make sure our actor and targets aren't moving or animating and are in the correct position
                actor.UpperLeft = actor.battleOrigin;
                actor.setCurrentFrame(0, 0);
                actor.animationShortStarted = false;

                target.UpperLeft = target.battleOrigin;
                target.setCurrentFrame(0, 0);
                target.animationShortStarted = false;

                actionComplete = true;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 1.25)
            {
                actor.animationShortStarted = false;
                actor.setCurrentFrame(0, 1);

                //If the target is dead
                if (target.health <= 0)
                {
                    battlers.Remove(target);
                    enemies.Remove(target);
                    heroes.Remove(target);
                }

                //Reset target to a neutral frame
                target.setCurrentFrame(0, 0);

                //Stepping characters back to starting position
                step.Invoke(gameTime, timer, -2, 1.5, actor, new Vector2(IFF(actor), 0));
                step.Invoke(gameTime, timer, 2, 1.5, target, new Vector2(IFF(target), 0));
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= timer + 0.75)
            {
                //step.Invoke the target backwards
                step.Invoke(gameTime, timer, -2, 1, target, new Vector2(IFF(target), 0));

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
                    target.StartAnimationShort(gameTime, 36, 38, 38);
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= timer)
            {
                //Start animating the actor's attack
                actor.reverseAnimating = false;
                actor.AnimationInterval = 250;
                actor.StartAnimationShort(gameTime, 2, 5, 5);

                //Reset the timer (Should only run once)
                timer = gameTime.TotalGameTime.TotalSeconds;
            }
        }
        
        private int IFF(Battler character)
        {
            if(character.friendly)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private void FightBegin()
        {

            List<Battler> temp = new List<Battler>();

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
    }
}
