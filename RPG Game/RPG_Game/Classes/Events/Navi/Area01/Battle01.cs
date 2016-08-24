using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Battle01 : BattleEvent
    {
        public override void InitializePotentials(NaviState naviState, Main main)
        {
            PotentialEnemy tempEnemy;

            potentialEnemies.Clear();

            //Enemies Initialization Begins//
            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Wolfy(main, naviState);
            tempEnemy.proportion = 5;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Wolfy(main, naviState);
            tempEnemy.proportion = 5;
            potentialEnemies.Add(tempEnemy);


            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Webber(main, naviState);
            tempEnemy.proportion = 5;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Webber(main, naviState);
            tempEnemy.proportion = 5;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Webber(main, naviState);
            tempEnemy.proportion = 5;
            potentialEnemies.Add(tempEnemy);


            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Woody(main, naviState);
            tempEnemy.proportion = 15;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Woody(main, naviState);
            tempEnemy.proportion = 15;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Woody(main, naviState);
            tempEnemy.proportion = 15;
            potentialEnemies.Add(tempEnemy);

            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Woody(main, naviState);
            tempEnemy.proportion = 15;
            potentialEnemies.Add(tempEnemy);


            tempEnemy = new PotentialEnemy();
            tempEnemy.enemy = new Waxwell(main, naviState);
            tempEnemy.proportion = 15;
            potentialEnemies.Add(tempEnemy);

        }

        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            Random rand = new Random();

            if(rand.Next(1, 101) <= 10 + naviState.encounterRate)
            {
                naviState.BattleBegin(potentialEnemies);

                naviState.encounterRate = 0;
            }
            else
            {
                naviState.encounterRate += 4;

                naviState.Movement();
            }

            Complete(naviState);

            return true;
        }
    }
}
