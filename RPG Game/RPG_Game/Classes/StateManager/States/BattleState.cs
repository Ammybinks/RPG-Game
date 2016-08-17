﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_Game
{
    public class BattleState : StateManager
    {
        Texture2D background;
        Sprite backgroundSprite = new Sprite();
        
        Texture2D heroTexture;
        Hero hero;

        Texture2D hiroTexture;
        Hero hiro;

        Texture2D hearoTexture;
        Hero hearo;

        Texture2D hieroTexture;
        Hero hiero;

        Texture2D werewolfTexture;
        Enemy werewolf;

        public List<Hero> heroes = new List<Hero>(4);
        public List<Enemy> enemies = new List<Enemy>(4);

        public List<Battler> battlers = new List<Battler>();
        
        Texture2D meterTexture;

        Texture2D shadowTexture;
        LinkedList<Sprite> shadows = new LinkedList<Sprite>();

        public Battler target = new Battler();
        public List<SpriteBase> targets = new List<SpriteBase>(0);

        public Battler actor = new Battler();
        
        Random rand = new Random();

        Box battleBox;
        MultiBox statusBox;
        Box skillsBox;
        Box itemsBox;

        Attack attack = new Attack();

        int targetIndex = 0;
        public float damageDealt;
        public float damageLocation;
        public bool actionComplete;

        public override void LoadContent(Main main)
        {
            gold = main.gold;

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
            backgroundSprite.SetTexture(background);
            backgroundSprite.UpperLeft = new Vector2(0, (main.graphics.PreferredBackBufferHeight - backgroundSprite.GetHeight()) * -1);
            backgroundSprite.Scale = new Vector2(2f, 2f);

            allItems = main.heldItems;

            activeButtons = new List<Button>();
            drawButtons = new List<Button>();

            pointer = new Sprite();

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            pointer.Origin = pointer.GetCenter();

            extraPointer = new Sprite();

            extraPointer.SetTexture(pointerTexture);
            extraPointer.drawColour = Color.White;
            extraPointer.Scale = new Vector2(0.8f, 0.8f);
            extraPointer.Origin = extraPointer.GetCenter();
            extraPointer.isAlive = false;
            //Miscellaneous Initialization Ends//


            //Heroes Initialization Begins//
            //Hero Initialization
            hero = main.hero;
            hero.name = "The Adorable Manchild";
            hero.SetTexture(heroTexture, 9, 6);
            hero.AnimationInterval = 250;
            hero.reverseAnimating = true;
            hero.ContinuousAnimation = false;
            hero.UpperLeft = new Vector2(500, 200);
            hero.battleOrigin = hero.UpperLeft;
            hero.maxHealth = 1000;
            hero.health = 1000;
            hero.maxMana = 50;
            hero.mana = 50;
            hero.PhAtk = 10;
            hero.PhDef = 75;
            hero.speed = 5;
            hero.Acc = 100;
            hero.Eva = 0;
            hero.XP = 0;
            hero.XPToLevel = 50;
            hero.Level = 1;
            hero.friendly = true;
            hero.meterSprite.SetTexture(meterTexture);
            hero.meterSprite.UpperLeft = new Vector2(hero.UpperLeft.X,
                                                     hero.UpperLeft.Y + hero.meterSprite.GetHeight() + (hero.GetHeight()));
            hero.shadow.SetTexture(shadowTexture);
            heroes.Add(hero);

            ////Hero Ability Initialization
            //Attack Ability
            attack = new Attack();
            hero.abilities.Add(attack);


            //Hiro Initialization
            hiro = main.hiro;
            hiro.name = "The Aboslutely-Not-Into-It Love Interest";
            hiro.SetTexture(hiroTexture, 9, 6);
            hiro.AnimationInterval = 250;
            hiro.reverseAnimating = true;
            hiro.ContinuousAnimation = false;
            hiro.UpperLeft = new Vector2(475, 350);
            hiro.battleOrigin = hiro.UpperLeft;
            hiro.maxHealth = 100;
            hiro.health = 100;
            hiro.maxMana = 0;
            hiro.mana = 0;
            hiro.PhAtk = 5;
            hiro.PhDef = 50;
            hiro.speed = 20;
            hiro.Acc = 100;
            hiro.Eva = 0;
            hiro.XP = 0;
            hiro.XPToLevel = 50;
            hiro.Level = 1;
            hiro.friendly = true;
            hiro.meterSprite.SetTexture(meterTexture);
            hiro.meterSprite.UpperLeft = new Vector2(hiro.UpperLeft.X,
                                                     hiro.UpperLeft.Y + hiro.meterSprite.GetHeight() + (hiro.GetHeight()));
            hiro.shadow.SetTexture(shadowTexture);
            heroes.Add(hiro);

            //Hearo Initialization
            hearo = main.hearo;
            hearo.name = "The Endearing Father Figure";
            hearo.SetTexture(hearoTexture, 9, 6);
            hearo.AnimationInterval = 250;
            hearo.reverseAnimating = true;
            hearo.ContinuousAnimation = false;
            hearo.UpperLeft = new Vector2(450, 500);
            hearo.battleOrigin = hearo.UpperLeft;
            hearo.maxHealth = 5;
            hearo.health = 5;
            hearo.maxMana = 125;
            hearo.mana = 125;
            hearo.PhAtk = 2;
            hearo.PhDef = 0;
            hearo.speed = 50;
            hearo.Acc = 100;
            hearo.Eva = 50;
            hearo.XP = 0;
            hearo.XPToLevel = 50;
            hearo.Level = 1;
            hearo.friendly = true;
            hearo.meterSprite.SetTexture(meterTexture);
            hearo.meterSprite.UpperLeft = new Vector2(hearo.UpperLeft.X,
                                                      hearo.UpperLeft.Y + hearo.meterSprite.GetHeight() + (hearo.GetHeight()));
            hearo.shadow.SetTexture(shadowTexture);

            ////Hearo Ability Initialization
            //Attack Ability
            attack = new Attack();
            hearo.abilities.Add(attack);

            //Murder Ability
            Murder murder = new Murder();
            hearo.abilities.Add(murder);

            heroes.Add(hearo);

            //Hiero Initialization
            hiero = main.hiero;
            hiero.name = "The Comic Relief";
            hiero.SetTexture(hieroTexture, 9, 6);
            hiero.AnimationInterval = 250;
            hiero.reverseAnimating = true;
            hiero.ContinuousAnimation = false;
            hiero.UpperLeft = new Vector2(425, 650);
            hiero.battleOrigin = hiero.UpperLeft;
            hiero.maxHealth = 50;
            hiero.health = 50;
            hiero.maxMana = 300;
            hiero.mana = 300;
            hiero.PhAtk = 5;
            hiero.PhDef = -100;
            hiero.speed = 25;
            hiero.Acc = 100;
            hiero.Eva = 0;
            hiero.XP = 0;
            hiero.XPToLevel = 50;
            hiero.Level = 1;
            hiero.friendly = true;
            hiero.meterSprite.SetTexture(meterTexture);
            hiero.meterSprite.UpperLeft = new Vector2(hiero.UpperLeft.X,
                                                      hiero.UpperLeft.Y + hiero.meterSprite.GetHeight() + (hiero.GetHeight()));
            hiero.shadow.SetTexture(shadowTexture);
            heroes.Add(hiero);

            ////Hiero Ability Initialization
            //Heal Ability
            Bountiful_Light bountiful_Light = new Bountiful_Light();
            hiero.abilities.Add(bountiful_Light);
            
            //Heroes Initialization Ends//

            //Enemies Initialization Begins//
            werewolf = new Enemy();
            werewolf.name = "Wolfy";
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1200, 200);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.maxHealth = 20;
            werewolf.health = 20;
            werewolf.PhAtk = 99999;
            werewolf.PhDef = 0;
            werewolf.speed = 1;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                         werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()));
            werewolf.goldYield = 50;
            werewolf.XPYield = 15;
            enemies.Add(werewolf);
            ////enemies.Add(werewolf);

            werewolf = new Enemy();
            werewolf.name = "Webber";
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1000, 300);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.maxHealth = 35;
            werewolf.health = 35;
            werewolf.PhAtk = 100;
            werewolf.PhDef = 50;
            werewolf.speed = 10;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                         werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()));
            werewolf.goldYield = 50;
            werewolf.XPYield = 15;
            enemies.Add(werewolf);

            werewolf = new Enemy();
            werewolf.name = "Woody";
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1200, 400);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.maxHealth = 5;
            werewolf.health = 5;
            werewolf.PhAtk = 2;
            werewolf.PhDef = 0;
            werewolf.speed = 49;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                         werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()));
            werewolf.goldYield = 50;
            werewolf.XPYield = 15;
            enemies.Add(werewolf);

            werewolf = new Enemy();
            werewolf.name = "Waxwell";
            werewolf.SetTexture(werewolfTexture);
            werewolf.Scale = new Vector2(0.5f, 0.5f);
            werewolf.UpperLeft = new Vector2(1000, 500);
            werewolf.battleOrigin = werewolf.UpperLeft;
            werewolf.maxHealth = 50;
            werewolf.health = 50;
            werewolf.PhAtk = 10;
            werewolf.PhDef = -50;
            werewolf.speed = 5;
            werewolf.Acc = 100;
            werewolf.Eva = 0;
            werewolf.friendly = false;
            werewolf.meterSprite.SetTexture(meterTexture);
            werewolf.meterSprite.UpperLeft = new Vector2(werewolf.UpperLeft.X,
                                                         werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()));
            werewolf.goldYield = 50;
            werewolf.XPYield = 15;
            enemies.Add(werewolf);

            //Boxes Initialization Begins//
            //Battle Menu Box
            battleBox = new Box();
            battleBox.frameWidth = 230;
            battleBox.frameHeight = 190;
            battleBox.UpperLeft = new Vector2(5, main.graphics.PreferredBackBufferHeight - battleBox.GetHeight() - 5);
            battleBox.SetParts(cornerTexture, wallTexture, backTexture);
            battleBox.activatorState = 3;
            battleBox.buttons = new List<Button>(3);
            allBoxes.Add(battleBox);

            //Button that advances into targeting state
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, main.graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((battleBox.buttons.Capacity - battleBox.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "FIGHT";
            button.action = TargetSwitch;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(12, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            battleBox.buttons.Add(button);

            //Button that advances into skillsMenu Box
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, main.graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((battleBox.buttons.Capacity - battleBox.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "SKILL";
            button.action = SkillsSwitch;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(15, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            battleBox.buttons.Add(button);

            //Button that advances into itemsMenu Box
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(75, main.graphics.PreferredBackBufferHeight - button.GetHeight() - 15 - (60 * ((battleBox.buttons.Capacity - battleBox.buttons.Count) - 1)));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "ITEM";
            button.action = ItemSwitch;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(1, 13);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            battleBox.buttons.Add(button);

            statusBox = new MultiBox();
            statusBox.frameWidth = 970;
            statusBox.frameHeight = 250;
            statusBox.UpperLeft = new Vector2(5 + battleBox.GetWidth(), main.graphics.PreferredBackBufferHeight - statusBox.GetHeight() - 5);
            statusBox.SetParts(cornerTexture, wallTexture, backTexture);
            statusBox.activatorState = -1;
            statusBox.multiButtons = new List<MultiButton>();
            allBoxes.Add(statusBox);

            targets = heroes.Cast<SpriteBase>().ToList();

            StatusRefresh();

            //skillsMenu Box
            skillsBox = new Box();
            skillsBox.frameWidth = 800;
            skillsBox.frameHeight = 800;
            skillsBox.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth / 2 - skillsBox.frameWidth / 2, 5);
            skillsBox.SetParts(cornerTexture, wallTexture, backTexture);
            skillsBox.activatorState = 4;
            skillsBox.buttons = new List<Button>();
            allBoxes.Add(skillsBox);

            //itemsMenu Box
            itemsBox = new Box();
            itemsBox.frameWidth = 800;
            itemsBox.frameHeight = 800;
            itemsBox.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth / 2 - itemsBox.frameWidth / 2, 5);
            itemsBox.SetParts(cornerTexture, wallTexture, backTexture);
            itemsBox.activatorState = 5;
            itemsBox.buttons = new List<Button>();
            allBoxes.Add(itemsBox);

            stateMethods[0] = Idle;
            stateMethods[1] = StepForwards;
            stateMethods[2] = Animate;
            stateMethods[3] = Menu;
            stateMethods[4] = SkillsMenu;
            stateMethods[5] = ItemsMenu;
            stateMethods[6] = TargetMenu;
            stateMethods[7] = BattleEnd;

            switchStateMethods[0] = MenuSwitch;
            switchStateMethods[1] = MenuSwitch;
            switchStateMethods[2] = MenuSwitch;
            switchStateMethods[3] = MenuSwitch;
            switchStateMethods[4] = SkillsSwitch;
            switchStateMethods[5] = ItemSwitch;
            switchStateMethods[6] = TargetSwitch;
            switchStateMethods[7] = MenuSwitch;

            targetState = 0;

            FightBegin();
        }
        
        public override void Update(GameTime gameTime)
        {
            //Execute main update method
            stateMethods[currentState].Invoke(gameTime);

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
        }

        public override void Draw(SpriteBatch spriteBatch, Main main)
        {
            backgroundSprite.Draw(spriteBatch);

            //Enemy Draw Cycle
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);

                enemies[i].meterSprite.Draw(spriteBatch);
            }

            //Hero Draw Cycle
            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].shadow.Draw(spriteBatch);

                heroes[i].Draw(spriteBatch);

                heroes[i].meterSprite.Draw(spriteBatch);
            }

            //Damage Balloon Draw
            if (damageDealt != 0)
            {
                Color tempColor = new Color();

                float tempDamage = damageDealt;

                if (tempDamage < 0)
                {
                    tempDamage *= -1;

                    tempColor = Color.Red;
                }
                else
                {
                    tempColor = Color.Green;
                }

                spriteBatch.DrawString(calibri,
                                       tempDamage.ToString(),
                                       new Vector2(target.UpperLeft.X,
                                       target.UpperLeft.Y - damageLocation),
                                       tempColor,
                                       0,
                                       new Vector2(0, 0),
                                       1f,
                                       SpriteEffects.None, 0);

                damageLocation += damageLocation / 32;
            }

            bool temp = false;

            //Boxes \ Buttons Draw Cycle
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (allBoxes[i].activatorState == -1 || state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    int skipIndex = 0;

                    for (int o = 0; o < allBoxes[i].GetButtons().Count; o++)
                    {
                        if (allBoxes[i].GetButtons()[o].selectable == false)
                        {
                            temp = false;

                            skipIndex++;
                        }
                        else if (allBoxes[i].activatorState == currentState && o - skipIndex == buttonIndex)
                        {
                            temp = true;
                        }
                        else
                        {
                            temp = false;
                        }

                        allBoxes[i].GetButtons()[o].DrawParts(spriteBatch, calibri, temp);
                    }
                }
            }


            pointer.Draw(spriteBatch);
            extraPointer.Draw(spriteBatch);
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
                    MenuSwitch(gameTime);
                }
                else
                {
                    //Select the target randomly from all heroes
                    target = heroes[rand.Next(0, 3)];

                    currentAction = attack;

                    //Switch to Animating State
                    ActivateState(2);

                    timer = gameTime.TotalGameTime.TotalSeconds;
                }
                timer = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        private void Animate(GameTime gameTime)
        {
            if (currentAction.Call(gameTime, this))
            {
                List<Battler> tempEnemies = new List<Battler>();

                for(int i = 0; i < enemies.Count; i++)
                {
                    if(enemies[i].isAlive)
                    {
                        tempEnemies.Add(enemies[i]);
                    }
                }

                if (tempEnemies.Count == 0)
                {
                    //Switch to Track Switch State
                    ActivateState(7);
                }
                else
                {
                    //Switch to Idle State
                    ActivateState(0);

                    targets.Clear();

                    for(int i = 0; i < heroes.Count; i++)
                    {
                        targets.Add(heroes[i]);
                    }

                    StatusRefresh();
                }
            }
        }

        private void Menu(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.isAlive = true;

            if (temp.activate)
            {
                pointer.isAlive = false;

                activeButtons[buttonIndex].action.Invoke(gameTime);
            }

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (actor.getTotalFrame() != 0 && actor.getTotalFrame() != 2 && !actor.IsAnimating())
            {
                actor.setCurrentFrame(0, 0);
            }
        }

        private void SkillsMenu(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            if (temp.activate)
            {
                if (actor.mana >= actor.abilities[buttonIndex].cost)
                {
                    if (actor.abilities[buttonIndex].battleUsable)
                    {
                        targets = actor.abilities[buttonIndex].GetTargets(this);

                        TargetSwitch();

                        extraPointer.isAlive = true;

                        currentAction = actor.abilities[buttonIndex];
                    }
                }
            }
            else if (temp.menu)
            {
                SwitchState(4, gameTime);
            }

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);
        }

        private void ItemsMenu(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            if (temp.activate)
            {
                if(allItems[buttonIndex].battleUsable)
                {
                    targets = allItems[buttonIndex].GetTargets(this);

                    TargetSwitch();

                    currentAction = allItems[buttonIndex];
                }
            }
            else if (temp.menu)
            {
                SwitchState(5, gameTime);
            }


            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);
        }

        private void TargetMenu(GameTime gameTime)
        {
            List<SpriteBase> availableTargets = new List<SpriteBase>();

            for(int i = 0; i < targets.Count; i++)
            {
                if(targets[i].isAlive)
                {
                    availableTargets.Add(targets[i]);
                }
            }

            if (availableTargets.Count == 1)
            {
                //Switch to Animating State
                ActivateState(2);

                target = (Battler)targets[0];
                targets.Clear();
                targets.Capacity = 0;
                timer = gameTime.TotalGameTime.TotalSeconds + 1;
            }
            else
            {
                MenuUpdateReturn temp = MenuUpdate(targets, targetIndex, statusBox.multiButtons.Cast<SpriteBase>().ToList());
                targetIndex = temp.index;

                pointer.UpperLeft = new Vector2(statusBox.multiButtons[targetIndex].UpperLeft.X - pointer.GetWidth() - 5,
                                                statusBox.multiButtons[targetIndex].UpperLeft.Y + 5);

                if (targets[targetIndex].isAlive)
                {
                    extraPointer.UpperLeft = new Vector2(targets[targetIndex].UpperLeft.X - pointer.GetWidth() - 5,
                                                         targets[targetIndex].UpperLeft.Y + targets[targetIndex].GetHeight() - 5);
                }
                else
                {
                    extraPointer.isAlive = false;
                }

                if (temp.activate)
                {
                    if(targets[targetIndex].isAlive)
                    {
                        pointer.isAlive = false;
                        extraPointer.isAlive = false;

                        //Switch to Animating State
                        ActivateState(2);

                        target = (Battler)targets[targetIndex];
                        timer = gameTime.TotalGameTime.TotalSeconds + 1;
                    }
                }
                else if (temp.menu)
                {
                    SwitchState(6, gameTime);

                    targets.Clear();

                    for(int i = 0; i < heroes.Count; i++)
                    {
                        targets.Add(heroes[i]);
                    }

                    StatusRefresh();
                }
            }
        }


        private void MenuSwitch(GameTime gameTime)
        {
            //Switch to Battle Menu State
            ActivateState(3);

            activeButtons.Clear();

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            pointer.Origin = pointer.GetCenter();

            for (int i = 0; i < battleBox.buttons.Count; i++)
            {
                activeButtons.Add(battleBox.buttons[i]);
            }
        }

        private void SkillsSwitch(GameTime gameTime)
        {
            //Switch to Skill Menu State
            SwitchState(4, gameTime);

            MultiButton tempButton;

            skillsBox.buttons.Clear();
            drawButtons.Clear();

            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();

            tempButton.selectable = false;
            tempButton.UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + 10);
            tempButton.display = "Ability";
            tempButton.frameWidth = skillsBox.frameWidth - 165;
            tempButton.frameHeight = 50;
            tempButton.SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), skillsBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Cost";
            tempButton.extraButtons[0].frameWidth = 75;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;

            tempButton.displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;

            skillsBox.buttons.Add(tempButton);

            for (int i = 0; i < actor.abilities.Count; i++)
            {
                if (actor.abilities[i].name.Equals("--Overwhelming Ineptitude--"))
                {
                    actor.abilities.RemoveAt(i);
                }
            }
            if (actor.abilities.Count == 0)
            {
                Ability ability = new Ability();
                ability.name = "--Overwhelming Ineptitude--";
                ability.description = actor.name + " doesn't seem very clever.\nThey don't know a single ability,\nand might not be much use in a fight.";
                ability.iconFrame = new Vector2(5, 0);
                actor.abilities.Add(ability);
            }

            string tempString;

            for (int i = 0; i < actor.abilities.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = actor.abilities[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)actor.abilities[i].iconFrame.X, (int)actor.abilities[i].iconFrame.Y);
                tempButton.frameWidth = skillsBox.frameWidth - 165;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                if (actor.abilities[i].cost.ToString().Equals("-1"))
                {
                    tempString = "------";
                }
                else
                {
                    tempString = actor.abilities[i].cost.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), skillsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 75;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[1].display = actor.abilities[i].description;
                tempButton.extraButtons[1].frameWidth = skillsBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;

                if (actor.mana < actor.abilities[i].cost)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }
                if (!actor.abilities[i].battleUsable)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }

                skillsBox.buttons.Add(tempButton);
            }

            skillsBox.frameHeight = (int)skillsBox.buttons[skillsBox.buttons.Count - 1].UpperLeft.Y + 60;
            skillsBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons.Clear();
            drawButtons.Clear();

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            pointer.Origin = pointer.GetCenter();

            for (int i = 0; i < skillsBox.buttons.Count; i++)
            {
                if (skillsBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)skillsBox.buttons[i];

                    tempButton.extraButtons[1].UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + skillsBox.frameHeight);
                    tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);

                    activeButtons.Add(tempButton);
                }
                else
                {
                    drawButtons.Add(skillsBox.buttons[i]);
                }
            }

            buttonIndex = 0;
        }

        private void ItemSwitch(GameTime gameTime)
        {
            SwitchState(5, gameTime);

            MultiButton tempButton;

            itemsBox.buttons.Clear();
            drawButtons.Clear();

            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();

            tempButton.selectable = false;
            tempButton.UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, itemsBox.UpperLeft.Y + 10);
            tempButton.display = "Item";
            tempButton.frameWidth = itemsBox.frameWidth - 165;
            tempButton.frameHeight = 50;
            tempButton.SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), itemsBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Qty.";
            tempButton.extraButtons[0].frameWidth = 75;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;

            tempButton.displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;

            itemsBox.buttons.Add(tempButton);

            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].name.Equals("--Empty, really empty--"))
                {
                    allItems.RemoveAt(i);
                }
            }
            if (allItems.Count == 0)
            {
                Item item = new Item();
                item.name = "--Empty, really empty--";
                item.description = "Looks like you don't have any items on you,\nmaybe this is a good time to stock up?";
                item.iconFrame = new Vector2(8, 10);
                allItems.Add(item);
            }

            string tempString;

            for (int i = 0; i < allItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, itemsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = allItems[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)allItems[i].iconFrame.X, (int)allItems[i].iconFrame.Y);
                tempButton.frameWidth = skillsBox.frameWidth - 165;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                if (allItems[i].heldCount.ToString().Equals("-1"))
                {
                    tempString = "------";
                }
                else
                {
                    tempString = allItems[i].heldCount.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), itemsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 75;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[1].display = allItems[i].description;
                tempButton.extraButtons[1].frameWidth = skillsBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;

                if (!allItems[i].battleUsable)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }
                itemsBox.buttons.Add(tempButton);
            }

            itemsBox.frameHeight = (int)itemsBox.buttons[itemsBox.buttons.Count - 1].UpperLeft.Y + 60;
            itemsBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons.Clear();
            drawButtons.Clear();
            
            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            pointer.Origin = pointer.GetCenter();

            for (int i = 0; i < itemsBox.buttons.Count; i++)
            {
                if (itemsBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)itemsBox.buttons[i];

                    tempButton.extraButtons[1].UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, itemsBox.UpperLeft.Y + itemsBox.frameHeight);
                    tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);

                    activeButtons.Add(tempButton);
                }
                else
                {
                    drawButtons.Add(itemsBox.buttons[i]);
                }
            }

            buttonIndex = 0;
        }

        private void TargetSwitch(GameTime gameTime)
        {
            //Switch to Target Menu State
            ActivateState(6);

            targets.Clear();

            for(int i = 0; i < enemies.Count; i++)
            {
                targets.Add(enemies[i]);
            }

            StatusRefresh();

            extraPointer.isAlive = true;

            currentAction = attack;
        }

        private void TargetSwitch()
        {
            //Switch to Target Menu State
            ActivateState(6);

            StatusRefresh();
        }

        private void BattleEnd(GameTime gameTime)
        {
            double goldIncrease = 0;

            for(int i = 0; i < enemies.Count; i++)
            {
                double temp;

                temp = rand.Next(0, 101);
                temp += 50;
                temp /= 100;
                temp *= enemies[i].goldYield;

                goldIncrease += temp;
                
                for(int o = 0; o < heroes.Count; o++)
                {
                    temp = rand.Next(0, 101);
                    temp += 50;
                    temp /= 100;
                    temp *= enemies[i].XPYield;

                    heroes[i].XP += (int)Math.Round(temp, 0, MidpointRounding.AwayFromZero);

                    while(heroes[i].XP > heroes[i].XPToLevel)
                    {
                        heroes[i].Level++;

                        heroes[i].XP -= heroes[i].XPToLevel;

                        heroes[i].XPToLevel = (int)Math.Round(heroes[i].XPToLevel * 1.5, 0, MidpointRounding.AwayFromZero);
                    }
                }
                //gold += enemies[i].goldYield * ((rand.Next(0, 101) + 50) / 100);
            }

            gold += (int)Math.Round(goldIncrease, 0, MidpointRounding.AwayFromZero);

            ActivateState(0);
            
            finished = true;
        }


        public void StatusRefresh()
        {
            statusBox.multiButtons.Clear();

            for (int i = 0; i < targets.Count; i++)
            {
                statusBox.multiButtons.Add(new MultiButton());

                statusBox.multiButtons[i].frameWidth = statusBox.GetWidth() - 480;
                statusBox.multiButtons[i].frameHeight = 50;
                statusBox.multiButtons[i].UpperLeft = new Vector2(statusBox.UpperLeft.X + 70, statusBox.UpperLeft.Y + 10 + (60 * i));
                statusBox.multiButtons[i].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].display = (targets[i] as Battler).name;
                statusBox.multiButtons[i].icon = null;
                statusBox.multiButtons[i].extraButtons = new List<Button>();
                if(!targets[i].isAlive)
                {
                    statusBox.multiButtons[i].displayColour = Color.OrangeRed;
                }

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[0].displayColour = Color.DarkGreen;
                statusBox.multiButtons[i].extraButtons[0].frameWidth = 75;
                statusBox.multiButtons[i].extraButtons[0].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[0].UpperLeft = new Vector2(statusBox.multiButtons[i].UpperLeft.X + statusBox.multiButtons[i].GetWidth(),
                                                                                  statusBox.multiButtons[i].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[0].display = (targets[i] as Battler).health.ToString();
                statusBox.multiButtons[i].extraButtons[0].icon = null;

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[1].displayColour = Color.DarkGreen;
                statusBox.multiButtons[i].extraButtons[1].frameWidth = 50;
                statusBox.multiButtons[i].extraButtons[1].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[1].UpperLeft = new Vector2(statusBox.multiButtons[i].extraButtons[0].UpperLeft.X + statusBox.multiButtons[i].extraButtons[0].GetWidth(),
                                                                                  statusBox.multiButtons[i].extraButtons[0].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[1].display = " /";
                statusBox.multiButtons[i].extraButtons[1].icon = null;

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[2].displayColour = Color.DarkGreen;
                statusBox.multiButtons[i].extraButtons[2].frameWidth = 75;
                statusBox.multiButtons[i].extraButtons[2].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[2].UpperLeft = new Vector2(statusBox.multiButtons[i].extraButtons[1].UpperLeft.X + statusBox.multiButtons[i].extraButtons[1].GetWidth(),
                                                                                  statusBox.multiButtons[i].extraButtons[1].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[2].display = (targets[i] as Battler).maxHealth.ToString();
                statusBox.multiButtons[i].extraButtons[2].icon = null;

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[3].displayColour = Color.DodgerBlue;
                statusBox.multiButtons[i].extraButtons[3].frameWidth = 75;
                statusBox.multiButtons[i].extraButtons[3].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[3].UpperLeft = new Vector2(statusBox.multiButtons[i].extraButtons[2].UpperLeft.X + statusBox.multiButtons[i].extraButtons[2].GetWidth(),
                                                                                  statusBox.multiButtons[i].extraButtons[2].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[3].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[3].display = (targets[i] as Battler).mana.ToString();
                statusBox.multiButtons[i].extraButtons[3].icon = null;

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[4].displayColour = Color.DodgerBlue;
                statusBox.multiButtons[i].extraButtons[4].frameWidth = 50;
                statusBox.multiButtons[i].extraButtons[4].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[4].UpperLeft = new Vector2(statusBox.multiButtons[i].extraButtons[3].UpperLeft.X + statusBox.multiButtons[i].extraButtons[3].GetWidth(),
                                                                                  statusBox.multiButtons[i].extraButtons[3].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[4].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[4].display = " /";
                statusBox.multiButtons[i].extraButtons[4].icon = null;

                statusBox.multiButtons[i].extraButtons.Add(new Button());
                statusBox.multiButtons[i].extraButtons[5].displayColour = Color.DodgerBlue;
                statusBox.multiButtons[i].extraButtons[5].frameWidth = 75;
                statusBox.multiButtons[i].extraButtons[5].frameHeight = 50;
                statusBox.multiButtons[i].extraButtons[5].UpperLeft = new Vector2(statusBox.multiButtons[i].extraButtons[4].UpperLeft.X + statusBox.multiButtons[i].extraButtons[4].GetWidth(),
                                                                                  statusBox.multiButtons[i].extraButtons[4].UpperLeft.Y);
                statusBox.multiButtons[i].extraButtons[5].SetParts(cornerTexture, wallTexture, backTexture);
                statusBox.multiButtons[i].extraButtons[5].display = (targets[i] as Battler).maxMana.ToString();
                statusBox.multiButtons[i].extraButtons[5].icon = null;
            }
        }


        public int IFF(Battler character)
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
