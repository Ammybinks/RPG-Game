using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RPG_Game
{
    class ArenaManager : BattleMover
    {
        public override void Move(GameTime gameTime, Tile[,] map)
        {
            map[(int)gridPosition.X, (int)gridPosition.Y].occupied = true;
            map[(int)gridPosition.X, (int)gridPosition.Y].occupier = this;
        }
        
        public override void InitializeTroops(NaviState naviState, Main main)
        {
            Enemy tempEnemy;

            Troop troop;

            potentialTroops.Clear();

            troop = new Troop();

            tempEnemy = new Wolfy(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Webber(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Woody(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Waxwell(main, naviState);
            troop.enemies.Add(tempEnemy);

            troop.proportion = 50;
            potentialTroops.Add(troop);


            troop = new Troop();

            tempEnemy = new Webber(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Webber(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Waxwell(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Waxwell(main, naviState);
            troop.enemies.Add(tempEnemy);

            troop.proportion = 25;
            potentialTroops.Add(troop);


            troop = new Troop();

            tempEnemy = new Woody(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Woody(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Woody(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Woody(main, naviState);
            troop.enemies.Add(tempEnemy);

            troop.proportion = 15;
            potentialTroops.Add(troop);


            troop = new Troop();

            tempEnemy = new Wolfy(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Webber(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Wolfy(main, naviState);
            troop.enemies.Add(tempEnemy);

            tempEnemy = new Webber(main, naviState);
            troop.enemies.Add(tempEnemy);

            troop.proportion = 9;
            potentialTroops.Add(troop);


            troop = new Troop();

            tempEnemy = new Windfury(main, naviState);
            troop.enemies.Add(tempEnemy);

            troop.proportion = 1;
            potentialTroops.Add(troop);
        }

        public override void Call(GameTime gameTime, NaviState naviState)
        {
            if (typingStrings == null)
            {
                Initialize(naviState);
                
                typingStrings.lines.Add("You wanna foight mate?\n");
                typingStrings.lines.Add("\nI'll focken shank ya fam.");
                typingStrings.lines.Add("Or at least these arseholes will,\n\nI don't have hands.");

                previousState = 0;
                state = 0;

                index = 0;
            }

            if (naviState.Type(typingStrings, gameTime, 0.01))
            {
                naviState.pointer.Scale = new Vector2(0.8f, 0.8f);

                typingStrings = null;

                typingStrings = new TypingStrings();
                typingStrings.lines = new List<string>();
                typingStrings.lines.Add("Still no hands.");
                typingStrings.line = "";
                typingStrings.previousLines = "";

                naviState.BattleBegin(potentialTroops);
            }
        }
    }
}
