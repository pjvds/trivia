using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {
        private static bool notAWinner;

        public static void Main(String[] args)
        {
            Game aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            Random rand = new Random();

            do
            {

                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    notAWinner = aGame.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer();
                }
                else
                {
                    notAWinner = aGame.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer();
                }



            } while (notAWinner);
	    }
    }
}