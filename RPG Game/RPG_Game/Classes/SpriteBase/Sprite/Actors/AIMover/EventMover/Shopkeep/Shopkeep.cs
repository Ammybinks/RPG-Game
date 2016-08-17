using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class Shopkeep : EventMover
    {
        internal Box shopBox = new Box();

        internal Box heroBox = new Box();

        internal Box confirmBox;

        internal List<Button> drawButtons = new List<Button>();

        internal List<Item> allItems = new List<Item>();
        internal List<Item> equivalentItems = new List<Item>();

        internal bool temp;

        internal int quantity = -1;
        internal int maxQuantity;

        public override void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (allBoxes[i] != null)
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        int skipIndex = 0;

                        temp = false;

                        if (allBoxes[i].GetButtons()[o].selectable == false)
                        {
                            skipIndex++;
                        }
                        else
                        {
                            if(previousIndex != -1)
                            {
                                if(o - skipIndex == previousIndex)
                                {
                                    temp = true;
                                }
                            }
                            else
                            {
                                if(o - skipIndex == index)
                                {
                                    temp = true;
                                }
                            }
                        }

                        allBoxes[i].GetButtons()[o].DrawParts(spriteBatch, spriteFont, temp);
                    }
                }
            }

            if (typingStrings != null)
            {
                spriteBatch.DrawString(spriteFont,
                                       typingStrings.line,
                                       new Vector2(580, 900),
                                       Color.Black);
            }

            if (drawButtons != null)
            {
                for (int i = 0; i < drawButtons.Count; i++)
                {
                    drawButtons[i].DrawParts(spriteBatch, spriteFont, false);
                }
            }
        }

        internal virtual void BuyMenu(NaviState naviState)
        {
            List<SpriteBase> tempList = new List<SpriteBase>();
            for (int i = 0; i < shopBox.buttons.Count; i++)
            {
                tempList.Add(shopBox.buttons[i]);
            }

            MenuUpdateReturn temp = naviState.MenuUpdate(tempList, index);
            index = temp.index;

            naviState.pointer.UpperLeft = new Vector2(shopBox.buttons[index].UpperLeft.X - naviState.pointer.GetWidth() - 15, shopBox.buttons[index].UpperLeft.Y + 5);
            naviState.pointer.isAlive = true;

            if (quantity != -1)
            {
                naviState.gold -= equivalentItems[index].buyingWorth * quantity;

                equivalentItems[index].heldCount += quantity;

                if (allItems[index].heldCount != -1)
                {
                    allItems[index].heldCount -= quantity;
                }

                quantity = -1;

                BuyRefresh(naviState);
            }

            if (temp.activate)
            {
                if (naviState.gold >= equivalentItems[index].buyingWorth &&
                    allItems[index].heldCount != 0 &&
                    equivalentItems[index].heldCount < equivalentItems[index].maxStack)
                {
                    SwitchState(4);

                    quantity = 1;

                    maxQuantity = equivalentItems[index].maxStack - equivalentItems[index].heldCount;
                    if(allItems[index].heldCount != -1)
                    {
                        if (allItems[index].heldCount < maxQuantity)
                        {
                            maxQuantity = allItems[index].heldCount;
                        }
                    }
                    if(naviState.gold / equivalentItems[index].buyingWorth < maxQuantity)
                    {
                        maxQuantity = naviState.gold / equivalentItems[index].buyingWorth;
                    }

                    ConfirmRefresh(naviState);
                }
            }
            else if (temp.menu)
            {
                SwitchState(0);

                naviState.pointer.isAlive = false;

                Box tempBox = allBoxes[0];
                allBoxes.Clear();
                allBoxes.Add(tempBox);

                drawButtons.Clear();
            }
        }


        internal virtual void SellMenu(NaviState naviState)
        {
            List<SpriteBase> tempList = new List<SpriteBase>();
            for (int i = 0; i < shopBox.buttons.Count; i++)
            {
                tempList.Add(shopBox.buttons[i]);
            }

            MenuUpdateReturn temp = naviState.MenuUpdate(tempList, index);
            index = temp.index;

            naviState.pointer.UpperLeft = new Vector2(shopBox.buttons[index].UpperLeft.X - naviState.pointer.GetWidth() - 15, shopBox.buttons[index].UpperLeft.Y + 5);
            naviState.pointer.isAlive = true;

            if (quantity != -1)
            {
                naviState.gold += naviState.heldItems[index].sellingWorth * quantity;
                naviState.heldItems[index].heldCount -= quantity;

                for (int i = 0; i < allItems.Count; i++)
                {
                    if (allItems[i].name.Equals(naviState.heldItems[index].name) && allItems[i].heldCount != -1)
                    {
                        allItems[i].heldCount += quantity;
                    }
                }
                
                quantity = -1;

                SellRefresh(naviState);
            }
            
            if (temp.activate)
            {
                if (naviState.heldItems[index].heldCount != -1)
                {
                    SwitchState(4);

                    quantity = 1;
                    maxQuantity = naviState.heldItems[index].heldCount;

                    ConfirmRefresh(naviState);
                }
            }
            else if (temp.menu)
            {
                SwitchState(0);

                naviState.pointer.isAlive = false;

                Box tempBox = allBoxes[0];
                allBoxes.Clear();
                allBoxes.Add(tempBox);

                drawButtons.Clear();
            }
        }


        internal virtual void ConfirmMenu(NaviState naviState, GameTime gameTime)
        {
            int temp;

            if (previousState == 2)
            {
                if (equivalentItems[previousIndex].moveInBulk)
                {
                    temp = ConfirmQuantityMenu(naviState);
                }
                else
                {
                    temp = ConfirmSingleMenu(naviState);
                }
            }
            else
            {
                if (naviState.heldItems[previousIndex].moveInBulk)
                {
                    temp = ConfirmQuantityMenu(naviState);
                }
                else
                {
                    temp = ConfirmSingleMenu(naviState);
                }
            }

            if (temp == 0)
            {
                naviState.pointer.isAlive = false;

                SwitchState(previousState);

                index = previousIndex;

                previousIndex = -1;
                
                allBoxes.Remove(confirmBox);
                confirmBox = null;
            }
            else if (temp == 1)
            {
                naviState.pointer.isAlive = false;

                quantity = -1;

                SwitchState(previousState);

                index = previousIndex;

                previousIndex = -1;

                allBoxes.Remove(confirmBox);
                confirmBox = null;
            }
        }

        internal virtual int ConfirmQuantityMenu(NaviState naviState)
        {
            if (naviState.upInput.inputState == Input.inputStates.pressed)
            {
                quantity++;

                if (quantity > maxQuantity)
                {
                    quantity = 1;
                }

                ConfirmRefresh(naviState);
            }
            if (naviState.downInput.inputState == Input.inputStates.pressed)
            {
                quantity--;

                if (quantity <= 0)
                {
                    quantity = maxQuantity;
                }

                ConfirmRefresh(naviState);
            }
            if (naviState.rightInput.inputState == Input.inputStates.pressed)
            {
                if (quantity == maxQuantity)
                {
                    quantity = 1;
                }
                else
                {
                    quantity += 10;
                }

                if (quantity > maxQuantity)
                {
                    quantity = maxQuantity;
                }

                ConfirmRefresh(naviState);
            }
            if (naviState.leftInput.inputState == Input.inputStates.pressed)
            {
                if (quantity == 1)
                {
                    quantity = maxQuantity;
                }
                else
                {
                    quantity -= 10;
                }

                if (quantity <= 0)
                {
                    quantity = 1;
                }

                ConfirmRefresh(naviState);
            }

            if(naviState.activateInput.inputState == Input.inputStates.pressed)
            {
                return 0;
            }
            else if(naviState.menuInput.inputState == Input.inputStates.pressed)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        internal virtual int ConfirmSingleMenu(NaviState naviState)
        {
            List<SpriteBase> tempList = new List<SpriteBase>();
            for (int i = 0; i < confirmBox.buttons.Count; i++)
            {
                tempList.Add(confirmBox.buttons[i]);
            }

            MenuUpdateReturn temp = naviState.MenuUpdate(tempList, index);
            index = temp.index;

            naviState.pointer.UpperLeft = new Vector2(confirmBox.buttons[index].UpperLeft.X - naviState.pointer.GetWidth() - 15, confirmBox.buttons[index].UpperLeft.Y + 5);
            naviState.pointer.isAlive = true;

            if (temp.activate)
            {
                if(index == 1)
                {
                    quantity = 0;
                }

                return 0;
            }
            else if (temp.menu)
            {
                for(int i = 0; i < drawButtons.Count; i++)
                {
                    if(drawButtons[i].display.Equals("Are you sure?"))
                    {
                        drawButtons.Remove(drawButtons[i]);
                    }
                }

                return 1;
            }
            else
            {
                return -1;
            }
        }


        internal virtual void BuyRefresh(NaviState naviState)
        {
            InitializeItems(naviState);

            MultiButton tempButton;

            shopBox.buttons.Clear();
            drawButtons.Clear();
            
            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();
            
            tempButton.UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + 10);
            tempButton.display = "Item";
            tempButton.frameWidth = shopBox.frameWidth - 320;
            tempButton.frameHeight = 50;
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.displayColour = Color.PaleGoldenrod;
            for (int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), shopBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Held";
            tempButton.extraButtons[0].frameWidth = 70;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth(),
                                                               shopBox.UpperLeft.Y + 10);
            tempButton.extraButtons[1].display = "Stock";
            tempButton.extraButtons[1].frameWidth = 85;
            tempButton.extraButtons[1].frameHeight = 50;
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[1].displayColour = Color.PaleGoldenrod;
            for (int i = 0; i < tempButton.extraButtons[1].parts.Count; i++)
            {
                tempButton.extraButtons[1].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[1].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[2].UpperLeft = new Vector2(tempButton.extraButtons[1].UpperLeft.X + tempButton.extraButtons[1].GetWidth(),
                                                               shopBox.UpperLeft.Y + 10);
            tempButton.extraButtons[2].display = "Cost";
            tempButton.extraButtons[2].frameWidth = 70;
            tempButton.extraButtons[2].frameHeight = 50;
            tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[2].displayColour = Color.PaleGoldenrod;
            for (int i = 0; i < tempButton.extraButtons[2].parts.Count; i++)
            {
                tempButton.extraButtons[2].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[2].icon = null;


            drawButtons.Add(tempButton);

            string tempString;

            for (int i = 0; i < allItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();
                
                tempButton.UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = allItems[i].name;
                tempButton.icon.SetTexture(naviState.iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)allItems[i].iconFrame.X, (int)allItems[i].iconFrame.Y);
                tempButton.frameWidth = shopBox.frameWidth - 320;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);
                
                if(equivalentItems[i] == null)
                {
                    tempString = "-----";
                }
                else
                {
                    tempString = equivalentItems[i].heldCount.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 70;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.extraButtons[0].icon = null;

                if (allItems[i].heldCount.ToString().Equals("-1"))
                {
                    tempString = "-------";
                }
                else
                {
                    tempString = allItems[i].heldCount.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth(),
                                                                   shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[1].display = tempString;
                tempButton.extraButtons[1].frameWidth = 85;
                tempButton.extraButtons[1].frameHeight = 50;
                tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.extraButtons[1].icon = null;
                
                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[2].UpperLeft = new Vector2(tempButton.extraButtons[1].UpperLeft.X + tempButton.extraButtons[1].GetWidth(),
                                                                   shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[2].display = allItems[i].buyingWorth.ToString();
                tempButton.extraButtons[2].frameWidth = 70;
                tempButton.extraButtons[2].frameHeight = 50;
                tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.extraButtons[2].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[3].UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[3].display = allItems[i].description;
                tempButton.extraButtons[3].frameWidth = shopBox.frameWidth - 90;
                tempButton.extraButtons[3].frameHeight = 100;
                tempButton.extraButtons[3].showOnSelected = true;
                tempButton.extraButtons[3].icon = null;

                if (allItems[i].heldCount == 0)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[1].displayColour = Color.OrangeRed;
                }
                if (naviState.gold < allItems[i].buyingWorth)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[2].displayColour = Color.OrangeRed;
                }
                if(equivalentItems[i] != null)
                {
                    if (equivalentItems[i].heldCount == equivalentItems[i].maxStack)
                    {
                        tempButton.displayColour = Color.OrangeRed;
                        tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                    }
                }

                shopBox.buttons.Add(tempButton);
            }

            heroBox.buttons[4].display = naviState.gold.ToString() + "G";

            shopBox.frameHeight = (int)shopBox.buttons[shopBox.buttons.Count - 1].UpperLeft.Y + 60;
            shopBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

            for (int i = 0; i < shopBox.buttons.Count; i++)
            {
                if (shopBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)shopBox.buttons[i];

                    tempButton.extraButtons[3].UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + shopBox.frameHeight);
                    tempButton.extraButtons[3].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                    shopBox.buttons[i] = tempButton;
                }
            }
        }


        internal virtual void SellRefresh(NaviState naviState)
        {
            InitializeItems(naviState);

            MultiButton tempButton;

            shopBox.buttons.Clear();
            drawButtons.Clear();
            
            if (naviState.heldItems.Count == 0)
            {
                Item item = new Item();
                item.name = "--Empty, really empty--";
                item.description = "Looks like you don't have any items on you,\nthis is literally the best time to stock back up.";
                item.iconFrame = new Vector2(8, 10);
                item.heldCount = -1;
                naviState.heldItems.Add(item);
            }

            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();

            tempButton.UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + 10);
            tempButton.display = "Item";
            tempButton.frameWidth = shopBox.frameWidth - 260;
            tempButton.frameHeight = 50;
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            for (int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), shopBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Held";
            tempButton.extraButtons[0].frameWidth = 70;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;
            
            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth(),
                                                               shopBox.UpperLeft.Y + 10);
            tempButton.extraButtons[1].display = "Worth";
            tempButton.extraButtons[1].frameWidth = 95;
            tempButton.extraButtons[1].frameHeight = 50;
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            for (int i = 0; i < tempButton.extraButtons[1].parts.Count; i++)
            {
                tempButton.extraButtons[1].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[1].icon = null;

            tempButton.displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[1].displayColour = Color.PaleGoldenrod;

            drawButtons.Add(tempButton);

            string tempString;

            for (int i = 0; i < naviState.heldItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = naviState.heldItems[i].name;
                tempButton.icon.SetTexture(naviState.iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)naviState.heldItems[i].iconFrame.X, (int)naviState.heldItems[i].iconFrame.Y);
                tempButton.frameWidth = shopBox.frameWidth - 260;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                if (naviState.heldItems[i].heldCount.ToString().Equals("-1"))
                {
                    tempString = "-----";
                }
                else
                {
                    tempString = naviState.heldItems[i].heldCount.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 70;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.extraButtons[0].icon = null;

                if (naviState.heldItems[i].sellingWorth.ToString().Equals("-1"))
                {
                    tempString = "--------";
                }
                else
                {
                    tempString = naviState.heldItems[i].sellingWorth.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth(),
                                                                   shopBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[1].display = tempString;
                tempButton.extraButtons[1].frameWidth = 95;
                tempButton.extraButtons[1].frameHeight = 50;
                tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.extraButtons[1].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[2].UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[2].display = naviState.heldItems[i].description;
                tempButton.extraButtons[2].frameWidth = shopBox.frameWidth - 90;
                tempButton.extraButtons[2].frameHeight = 100;
                tempButton.extraButtons[2].showOnSelected = true;
                tempButton.extraButtons[2].icon = null;

                if (naviState.heldItems[i].heldCount == -1)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                    tempButton.extraButtons[1].displayColour = Color.OrangeRed;
                }

                shopBox.buttons.Add(tempButton);
            }


            heroBox.buttons[4].display = naviState.gold + "G";

            shopBox.frameHeight = (int)shopBox.buttons[shopBox.buttons.Count - 1].UpperLeft.Y + 60;
            shopBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

            for (int i = 0; i < shopBox.buttons.Count; i++)
            {
                if (shopBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)shopBox.buttons[i];

                    tempButton.extraButtons[2].UpperLeft = new Vector2(shopBox.UpperLeft.X + 80, shopBox.UpperLeft.Y + shopBox.frameHeight);
                    tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                    shopBox.buttons[i] = tempButton;
                }
            }
        }


        internal virtual void ConfirmRefresh(NaviState naviState)
        {
            if (state == 4)
            {
                if (confirmBox == null)
                {
                    confirmBox = new Box();
                    confirmBox.buttons = new List<Button>();
                    confirmBox.UpperLeft = new Vector2(shopBox.buttons[0].UpperLeft.X, shopBox.UpperLeft.Y + shopBox.GetHeight() + 100);
                    confirmBox.frameWidth = 200;
                    confirmBox.frameHeight = 200;
                    confirmBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                    allBoxes.Add(confirmBox);

                    previousIndex = index;

                    index = 0;
                }

                if (previousState == 2)
                {
                    if (equivalentItems[previousIndex].moveInBulk)
                    {
                        ConfirmQuantityRefresh(naviState);
                    }
                    else
                    {
                        ConfirmSingleRefresh(naviState);
                    }
                }
                else
                {
                    if (naviState.heldItems[previousIndex].moveInBulk)
                    {
                        ConfirmQuantityRefresh(naviState);
                    }
                    else
                    {
                        ConfirmSingleRefresh(naviState);
                    }
                }
            }
        }

        internal virtual void ConfirmQuantityRefresh(NaviState naviState)
        {
            if (confirmBox.buttons.Count == 0)
            {
                Button tempButton;

                tempButton = new Button();

                tempButton.selectable = false;
                tempButton.UpperLeft = new Vector2(confirmBox.UpperLeft.X + 10, confirmBox.UpperLeft.Y + 10);
                tempButton.display = "How many?";
                tempButton.frameWidth = confirmBox.GetWidth() - 20;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.displayColour = Color.PaleGoldenrod;
                for (int i = 0; i < tempButton.parts.Count; i++)
                {
                    tempButton.parts[i].drawColour = Color.Navy;
                }
                tempButton.icon = null;

                confirmBox.buttons.Add(tempButton);

                tempButton = new Button();

                tempButton.selectable = false;
                tempButton.UpperLeft = new Vector2(confirmBox.buttons[0].UpperLeft.X, confirmBox.UpperLeft.Y + 70);
                tempButton.display = quantity.ToString();
                tempButton.frameWidth = 50;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon = null;

                confirmBox.buttons.Add(tempButton);

                tempButton = new Button();

                tempButton.selectable = false;
                tempButton.UpperLeft = new Vector2(confirmBox.buttons[1].UpperLeft.X + confirmBox.buttons[1].GetWidth() + 10, confirmBox.UpperLeft.Y + 70);
                tempButton.frameWidth = 95;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon = null;
                if (previousState == 2)
                {
                    tempButton.display = equivalentItems[previousIndex].buyingWorth.ToString() + "G";
                }
                else
                {
                    tempButton.display = naviState.heldItems[previousIndex].sellingWorth.ToString() + "G";
                }

                confirmBox.buttons.Add(tempButton);
            }
            else
            {
                confirmBox.buttons[1].display = quantity.ToString();

                if (previousState == 2)
                {
                    confirmBox.buttons[2].display = (equivalentItems[previousIndex].buyingWorth * quantity).ToString() + "G";
                }
                else
                {
                    confirmBox.buttons[2].display = (naviState.heldItems[previousIndex].sellingWorth * quantity).ToString() + "G";
                }
            }
        }

        internal virtual void ConfirmSingleRefresh(NaviState naviState)
        {
            if (confirmBox.buttons.Count == 0)
            {
                Button tempButton;

                tempButton = new Button();

                tempButton.selectable = false;
                tempButton.UpperLeft = new Vector2(confirmBox.UpperLeft.X + 10, confirmBox.UpperLeft.Y + 10);
                tempButton.display = "Are you sure?";
                tempButton.frameWidth = confirmBox.GetWidth() - 20;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.displayColour = Color.PaleGoldenrod;
                for (int i = 0; i < tempButton.parts.Count; i++)
                {
                    tempButton.parts[i].drawColour = Color.Navy;
                }
                tempButton.icon = null;

                drawButtons.Add(tempButton);

                tempButton = new Button();

                tempButton.UpperLeft = new Vector2(confirmBox.UpperLeft.X + 10, confirmBox.UpperLeft.Y + 70);
                tempButton.display = "Yes";
                tempButton.frameWidth = 65;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon = null;

                confirmBox.buttons.Add(tempButton);

                tempButton = new Button();

                tempButton.UpperLeft = new Vector2(confirmBox.buttons[0].UpperLeft.X, confirmBox.UpperLeft.Y + 130);
                tempButton.display = "No";
                tempButton.frameWidth = 65;
                tempButton.frameHeight = 50;
                tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
                tempButton.icon = null;

                confirmBox.buttons.Add(tempButton);

                index = 1;
            }
        }


        internal void InitializeBoxes(NaviState naviState)
        {
            Box tempBox = allBoxes[0];
            allBoxes.Clear();
            allBoxes.Add(tempBox);

            ////Hero Buttons Box
            heroBox = new Box();
            heroBox.frameWidth = 820;
            heroBox.frameHeight = 800;
            heroBox.UpperLeft = new Vector2(1920 - heroBox.GetWidth() - 5, 5);
            heroBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            heroBox.activatorState = 6;
            heroBox.buttons = new List<Button>();
            allBoxes.Add(heroBox);

            MultiButton tempButton;

            //Hero MultiButton
            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();
            tempButton.frameWidth = 150;
            tempButton.frameHeight = 150;
            tempButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((tempButton.GetHeight() + 25) * heroBox.buttons.Count));
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.icon.SetTexture(naviState.heroFace);
            tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 3, tempButton.UpperLeft.Y + 2);

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].frameWidth = 560;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y);
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[0].display = "The Adorable Manchild";
            tempButton.extraButtons[0].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].frameWidth = 45;
            tempButton.extraButtons[1].frameHeight = 42;
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[1].display = "HP:";
            tempButton.extraButtons[1].displaySize = 0.75f;
            tempButton.extraButtons[1].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[2].frameWidth = 115;
            tempButton.extraButtons[2].frameHeight = 42;
            tempButton.extraButtons[2].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[2].display = naviState.heroBattler.health.ToString() + "/" + naviState.heroBattler.maxHealth.ToString();
            tempButton.extraButtons[2].displaySize = 0.75f;
            tempButton.extraButtons[2].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[3].frameWidth = 45;
            tempButton.extraButtons[3].frameHeight = 42;
            tempButton.extraButtons[3].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[3].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[3].display = "MP:";
            tempButton.extraButtons[3].displaySize = 0.75f;
            tempButton.extraButtons[3].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[4].frameWidth = 115;
            tempButton.extraButtons[4].frameHeight = 42;
            tempButton.extraButtons[4].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[4].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[4].display = naviState.heroBattler.mana.ToString() + "/" + naviState.heroBattler.maxMana.ToString();
            tempButton.extraButtons[4].displaySize = 0.75f;
            tempButton.extraButtons[4].icon = null;

            heroBox.buttons.Add(tempButton);

            //Hiro MultiButton
            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();
            tempButton.frameWidth = 150;
            tempButton.frameHeight = 150;
            tempButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((tempButton.GetHeight() + 25) * heroBox.buttons.Count));
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.icon.SetTexture(naviState.hiroFace);
            tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 3, tempButton.UpperLeft.Y + 2);

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].frameWidth = 560;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y);
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[0].display = "The Absolutely-Not-Into-It Love Interest";
            tempButton.extraButtons[0].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].frameWidth = 45;
            tempButton.extraButtons[1].frameHeight = 42;
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[1].display = "HP:";
            tempButton.extraButtons[1].displaySize = 0.75f;
            tempButton.extraButtons[1].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[2].frameWidth = 115;
            tempButton.extraButtons[2].frameHeight = 42;
            tempButton.extraButtons[2].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[2].display = naviState.hiroBattler.health.ToString() + "/" + naviState.hiroBattler.maxHealth.ToString();
            tempButton.extraButtons[2].displaySize = 0.75f;
            tempButton.extraButtons[2].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[3].frameWidth = 45;
            tempButton.extraButtons[3].frameHeight = 42;
            tempButton.extraButtons[3].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[3].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[3].display = "MP:";
            tempButton.extraButtons[3].displaySize = 0.75f;
            tempButton.extraButtons[3].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[4].frameWidth = 115;
            tempButton.extraButtons[4].frameHeight = 42;
            tempButton.extraButtons[4].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[4].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[4].display = naviState.hiroBattler.mana.ToString() + "/" + naviState.hiroBattler.maxMana.ToString();
            tempButton.extraButtons[4].displaySize = 0.75f;
            tempButton.extraButtons[4].icon = null;

            heroBox.buttons.Add(tempButton);

            //Hearo MultiButton
            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();
            tempButton.frameWidth = 150;
            tempButton.frameHeight = 150;
            tempButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((tempButton.GetHeight() + 25) * heroBox.buttons.Count));
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.icon.SetTexture(naviState.hearoFace);
            tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 3, tempButton.UpperLeft.Y + 2);

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].frameWidth = 560;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y);
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[0].display = "The Endearing Father Figure";
            tempButton.extraButtons[0].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].frameWidth = 45;
            tempButton.extraButtons[1].frameHeight = 42;
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[1].display = "HP:";
            tempButton.extraButtons[1].displaySize = 0.75f;
            tempButton.extraButtons[1].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[2].frameWidth = 115;
            tempButton.extraButtons[2].frameHeight = 42;
            tempButton.extraButtons[2].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[2].display = naviState.hearoBattler.health.ToString() + "/" + naviState.hearoBattler.maxHealth.ToString();
            tempButton.extraButtons[2].displaySize = 0.75f;
            tempButton.extraButtons[2].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[3].frameWidth = 45;
            tempButton.extraButtons[3].frameHeight = 42;
            tempButton.extraButtons[3].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[3].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[3].display = "MP:";
            tempButton.extraButtons[3].displaySize = 0.75f;
            tempButton.extraButtons[3].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[4].frameWidth = 115;
            tempButton.extraButtons[4].frameHeight = 42;
            tempButton.extraButtons[4].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[4].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[4].display = naviState.hearoBattler.mana.ToString() + "/" + naviState.hearoBattler.maxMana.ToString();
            tempButton.extraButtons[4].displaySize = 0.75f;
            tempButton.extraButtons[4].icon = null;

            heroBox.buttons.Add(tempButton);

            //Hiero MultiButton
            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();
            tempButton.frameWidth = 150;
            tempButton.frameHeight = 150;
            tempButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((tempButton.GetHeight() + 25) * heroBox.buttons.Count));
            tempButton.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.icon.SetTexture(naviState.hieroFace);
            tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 3, tempButton.UpperLeft.Y + 2);

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].frameWidth = 560;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y);
            tempButton.extraButtons[0].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[0].display = "The Comic Relief";
            tempButton.extraButtons[0].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[1].frameWidth = 45;
            tempButton.extraButtons[1].frameHeight = 42;
            tempButton.extraButtons[1].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[1].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[1].display = "HP:";
            tempButton.extraButtons[1].displaySize = 0.75f;
            tempButton.extraButtons[1].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[2].frameWidth = 115;
            tempButton.extraButtons[2].frameHeight = 42;
            tempButton.extraButtons[2].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 50);
            tempButton.extraButtons[2].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[2].display = naviState.hieroBattler.health.ToString() + "/" + naviState.hieroBattler.maxHealth.ToString();
            tempButton.extraButtons[2].displaySize = 0.75f;
            tempButton.extraButtons[2].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[3].frameWidth = 45;
            tempButton.extraButtons[3].frameHeight = 42;
            tempButton.extraButtons[3].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth() + 5, tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[3].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[3].display = "MP:";
            tempButton.extraButtons[3].displaySize = 0.75f;
            tempButton.extraButtons[3].icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[4].frameWidth = 115;
            tempButton.extraButtons[4].frameHeight = 42;
            tempButton.extraButtons[4].UpperLeft = new Vector2((tempButton.extraButtons[0].UpperLeft.X + tempButton.extraButtons[0].GetWidth()) - tempButton.extraButtons[2].GetWidth(), tempButton.UpperLeft.Y + 90);
            tempButton.extraButtons[4].SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempButton.extraButtons[4].display = naviState.hieroBattler.mana.ToString() + "/" + naviState.hieroBattler.maxMana.ToString();
            tempButton.extraButtons[4].displaySize = 0.75f;
            tempButton.extraButtons[4].icon = null;

            heroBox.buttons.Add(tempButton);

            int temp = tempButton.GetHeight();

            Button tempSingle = new Button();

            //Money MultiButton
            tempSingle.frameWidth = 200;
            tempSingle.frameHeight = 50;
            tempSingle.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((temp + 25) * heroBox.buttons.Count));
            tempSingle.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            tempSingle.display = naviState.gold.ToString() + "G";
            tempSingle.icon.SetTexture(naviState.iconTexture, 16, 20);
            tempSingle.icon.setCurrentFrame(10, 19);
            tempSingle.icon.UpperLeft = new Vector2(tempSingle.UpperLeft.X + 10, tempSingle.UpperLeft.Y + 9);
            
            heroBox.buttons.Add(tempSingle);


            shopBox = new Box();

            shopBox.UpperLeft = new Vector2(5, 5);
            shopBox.frameWidth = 800;
            shopBox.buttons = new List<Button>();

            shopBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

            allBoxes.Add(shopBox);
        }

        internal void InitializeItems(NaviState naviState)
        {
            naviState.heldItems.Clear();
            equivalentItems.Clear();

            for (int i = 0; i < naviState.allItems.Count; i++)
            {
                if (naviState.allItems[i].heldCount != 0)
                {
                    naviState.heldItems.Add(naviState.allItems[i]);
                }
            }

            for (int i = 0; i < allItems.Count; i++)
            {
                for (int o = 0; o < naviState.allItems.Count; o++)
                {
                    if (allItems[i].name.Equals(naviState.allItems[o].name))
                    {
                        equivalentItems.Add(naviState.allItems[o]);

                        break;
                    }
                }
            }
        }
    }
}
