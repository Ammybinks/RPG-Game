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

        public List<Battler> heroes = new List<Battler>(4);
        public List<Battler> enemies = new List<Battler>(4);

        public List<Battler> battlers = new List<Battler>();
        
        Texture2D meterTexture;

        Texture2D shadowTexture;
        LinkedList<Sprite> shadows = new LinkedList<Sprite>();

        public Battler target = new Battler();
        public List<SpriteBase> targets = new List<SpriteBase>(0);

        public Battler actor = new Battler();
        
        Random rand = new Random();

        Box battleBox;
        Box skillsBox;
        Box itemsBox;

        Attack attack = new Attack();

        int targetIndex = 0;
        public float damageDealt;
        public float damageLocation;
        public bool actionComplete;

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

            heldItems = main.heldItems;
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
            hiero.PhAtk = 5;
            hiero.PhDef = -100;
            hiero.speed = 25;
            hiero.Acc = 100;
            hiero.Eva = 0;
            hiero.friendly = true;
            hiero.meterSprite.SetTexture(meterTexture);
            hiero.meterSprite.UpperLeft = new Vector2(hiero.UpperLeft.X,
                                                      hiero.UpperLeft.Y + hiero.meterSprite.GetHeight() + (hiero.GetHeight()) + 20);
            hiero.shadow.SetTexture(shadowTexture);
            heroes.Add(hiero);

            ////Hiero Ability Initialization
            //Heal Ability
            Bountiful_Light bountiful_Light = new Bountiful_Light();
            hiero.abilities.Add(bountiful_Light);
            
            //Heroes Initialization Ends//

            //Enemies Initialization Begins//
            werewolf = new Battler();
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
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            enemies.Add(werewolf);
            ////enemies.Add(werewolf);

            werewolf = new Battler();
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
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            enemies.Add(werewolf);

            werewolf = new Battler();
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
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
            enemies.Add(werewolf);

            werewolf = new Battler();
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
                                                      werewolf.UpperLeft.Y + werewolf.meterSprite.GetHeight() + (werewolf.GetHeight()) + 20);
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
            stateMethods[7] = StateSwitch;

            switchStateMethods[0] = MenuSwitch;
            switchStateMethods[1] = MenuSwitch;
            switchStateMethods[2] = MenuSwitch;
            switchStateMethods[3] = MenuSwitch;
            switchStateMethods[4] = SkillsSwitch;
            switchStateMethods[5] = ItemSwitch;
            switchStateMethods[6] = TargetSwitch;
            switchStateMethods[7] = MenuSwitch;

            targetState = 1;

            FightBegin();
        }
        
        public override void Update(GameTime gameTime)
        {
            targetState = 1;

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
                if (state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        if(o == buttonIndex)
                        {
                            temp = true;
                        }
                        else
                        {
                            temp = false;
                        }

                        allBoxes[i].buttons[o].DrawParts(spriteBatch, calibri, temp);
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
                if(actor.abilities[buttonIndex].battleUsable)
                {
                    pointer.isAlive = false;

                    ActivateState(6);

                    targets = actor.abilities[buttonIndex].GetTargets(this);

                    currentAction = actor.abilities[buttonIndex];
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
                if(heldItems[buttonIndex].battleUsable)
                {
                    pointer.isAlive = false;

                    ActivateState(6);

                    targets = heldItems[buttonIndex].GetTargets(this);

                    currentAction = heldItems[buttonIndex];
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
            if (targets.Count == 1)
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
                MenuUpdateReturn temp = MenuUpdate(targets, targetIndex);
                targetIndex = temp.index;

                pointer.UpperLeft = new Vector2(targets[targetIndex].UpperLeft.X - pointer.GetWidth() - 5,
                                                targets[targetIndex].UpperLeft.Y + targets[targetIndex].GetHeight() - 5);


                if (temp.activate)
                {
                    pointer.isAlive = false;

                    //Switch to Animating State
                    ActivateState(2);

                    target = (Battler)targets[targetIndex];
                    targets.Clear();
                    targets.Capacity = 0;
                    timer = gameTime.TotalGameTime.TotalSeconds + 1;
                }
                else if (temp.menu)
                {
                    SwitchState(6, gameTime);
                }
            }
        }

        private void StateSwitch(GameTime gameTime)
        {
            targetState = 0;

            ActivateState(0);
        }

        private void MenuSwitch(GameTime gameTime)
        {
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

        private void SkillsSwitch(GameTime gameTime)
        {
            //Switch to Skill Menu State
            SwitchState(4, gameTime);

            MultiButton tempButton;

            skillsBox.buttons.Clear();
            skillsBox.buttons.Capacity = 0;

            if (actor.abilities.Count == 0)
            {
                Ability ability = new Ability();
                ability.name = "--Overwhelming Ineptitude--";
                ability.description = actor.name + " doesn't seem very clever.\nThey don't know a single ability,\nand might not be much use in a fight.";
                ability.iconFrame = new Vector2(5, 0);
                ability.cost = 137;
                actor.abilities.Add(ability);
            }

            for (int i = 0; i < actor.abilities.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.display = actor.abilities[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)actor.abilities[i].iconFrame.X, (int)actor.abilities[i].iconFrame.Y);
                tempButton.frameWidth = skillsBox.frameWidth - 160;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), skillsBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.extraButtons[0].display = actor.abilities[i].cost.ToString();
                tempButton.extraButtons[0].frameWidth = 70;
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


                if (!actor.abilities[i].battleUsable)
                {
                    tempButton.displayColour = Color.Gray;
                    tempButton.extraButtons[0].displayColour = Color.Gray;
                }
                skillsBox.buttons.Add(tempButton);
            }

            skillsBox.frameHeight = (int)skillsBox.buttons[skillsBox.buttons.Count - 1].UpperLeft.Y + 60;
            skillsBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons = skillsBox.buttons;

            buttonIndex = 0;
        }

        private void ItemSwitch(GameTime gameTime)
        {
            SwitchState(5, gameTime);

            MultiButton tempButton;

            itemsBox.buttons.Clear();
            itemsBox.buttons.Capacity = 0;

            if (heldItems.Count == 0)
            {
                Item item = new Item();
                item.name = "--Empty, really empty--";
                item.description = "Looks like you don't have any items on you,\nmaybe this is a good time to stock up?";
                item.iconFrame = new Vector2(7, 10);
                item.heldCount = 13;
                heldItems.Add(item);
            }

            for (int i = 0; i < heldItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, itemsBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.display = heldItems[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)heldItems[i].iconFrame.X, (int)heldItems[i].iconFrame.Y);
                tempButton.frameWidth = skillsBox.frameWidth - 140;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), itemsBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.extraButtons[0].display = heldItems[i].heldCount.ToString();
                tempButton.extraButtons[0].frameWidth = 50;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(itemsBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[1].display = heldItems[i].description;
                tempButton.extraButtons[1].frameWidth = skillsBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;

                if (!heldItems[i].battleUsable)
                {
                    tempButton.displayColour = Color.Gray;
                    tempButton.extraButtons[0].displayColour = Color.Gray;
                }
                itemsBox.buttons.Add(tempButton);
            }

            itemsBox.frameHeight = (int)itemsBox.buttons[itemsBox.buttons.Count - 1].UpperLeft.Y + 60;
            itemsBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons = itemsBox.buttons;

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

            currentAction = attack;
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
