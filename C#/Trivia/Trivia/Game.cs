﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    public class Game
    {
		public const int MAX_NUM_PLAYERS = 6;
		public const int BOARD_SIZE = 12;
		public const int COINS_TO_WIN = 6;

		const string POP_CATEGORY = "Pop";
		const string SCIENCE_CATEGORY = " Science";
		const string SPORTS_CATEGORY = "Sports";
		const string ROCK_CATEGORY = "Rock";

        List<string> playerNames = new List<string>();

        internal int[] playerCurrentPlaces = new int[MAX_NUM_PLAYERS];
        internal int[] playerCoins = new int[MAX_NUM_PLAYERS];

        private bool[] inPenaltyBox = new bool[MAX_NUM_PLAYERS];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        internal int currentPlayerIndex = 0;
        internal bool isGettingOutOfPenaltyBox;

        public Game()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast("Science Question " + i);
                sportsQuestions.AddLast("Sports Question " + i);
                rockQuestions.AddLast("Rock Question" + i);
            }
        }

        public bool IsPlayable()
        {
            return (GetNumberOfPlayers() >= 2);
        }

        public bool Add(String playerName)
        {
			if(GetNumberOfPlayers() == MAX_NUM_PLAYERS) throw new InvalidOperationException("The maximum number of players has been reached.");

			int newPlayerIndex = GetNumberOfPlayers();

   			playerNames.Add(playerName);
            playerCurrentPlaces[newPlayerIndex] = 0;
            playerCoins[newPlayerIndex] = 0;
            inPenaltyBox[newPlayerIndex] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + playerNames.Count);
            return true;
        }

        public int GetNumberOfPlayers()
        {
            return playerNames.Count;
        }

		public bool IsPreviousPlayerInPenaltyBox ()
		{
			int previousIndex = currentPlayerIndex-1;
			if(previousIndex < 0) previousIndex = GetNumberOfPlayers() -1;

			return inPenaltyBox[previousIndex];
		}

		public bool IsCurrentPlayerInPenaltyBox ()
		{
			return inPenaltyBox[currentPlayerIndex];
		}

		public bool RollGetsPlayerOutOfPenaltyBox (int roll)
		{
			return roll % 2 != 0;
		}

        public void Roll(int roll)
        {
            Console.WriteLine(playerNames[currentPlayerIndex] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (IsCurrentPlayerInPenaltyBox())
            {
                if (RollGetsPlayerOutOfPenaltyBox(roll))
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(playerNames[currentPlayerIndex] + " is getting out of the penalty box");
                    playerCurrentPlaces[currentPlayerIndex] = playerCurrentPlaces[currentPlayerIndex] + roll;
                    if (playerCurrentPlaces[currentPlayerIndex] > BOARD_SIZE - 1) playerCurrentPlaces[currentPlayerIndex] = playerCurrentPlaces[currentPlayerIndex] - BOARD_SIZE;

                    Console.WriteLine(playerNames[currentPlayerIndex]
                            + "'s new location is "
                            + playerCurrentPlaces[currentPlayerIndex]);
                    Console.WriteLine("The category is " + GetCurrentCategory());
                    printCurrentQuestionAndRemoveIt();
                }
                else
                {
                    Console.WriteLine(playerNames[currentPlayerIndex] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {
                playerCurrentPlaces[currentPlayerIndex] = playerCurrentPlaces[currentPlayerIndex] + roll;
                if (playerCurrentPlaces[currentPlayerIndex] > BOARD_SIZE - 1) playerCurrentPlaces[currentPlayerIndex] = playerCurrentPlaces[currentPlayerIndex] - BOARD_SIZE;

                Console.WriteLine(playerNames[currentPlayerIndex]
                        + "'s new location is "
                        + playerCurrentPlaces[currentPlayerIndex]);
                Console.WriteLine("The category is " + GetCurrentCategory());
                printCurrentQuestionAndRemoveIt();
            }

        }

        private void printCurrentQuestionAndRemoveIt()
        {
            if (GetCurrentCategory() == POP_CATEGORY)
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (GetCurrentCategory() == SCIENCE_CATEGORY)
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (GetCurrentCategory() == SPORTS_CATEGORY)
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (GetCurrentCategory() == ROCK_CATEGORY)
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        private String GetCurrentCategory()
        {
            if (playerCurrentPlaces[currentPlayerIndex] == 0) return POP_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 1) return SCIENCE_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 2) return SCIENCE_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 4) return POP_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 5) return SCIENCE_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 6) return SPORTS_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 8) return POP_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 9) return SCIENCE_CATEGORY;
            if (playerCurrentPlaces[currentPlayerIndex] == 10) return SPORTS_CATEGORY;
            return ROCK_CATEGORY;
        }

        public bool MarkCurrentAnswerAsCorrectAndMoveToNextPlayer()
        {
            Console.WriteLine("Answer was correct!!!!");
			
			if (IsCurrentPlayerInPenaltyBox())
            {
                if (isGettingOutOfPenaltyBox)
                {
					inPenaltyBox[currentPlayerIndex] = false;

                    playerCoins[currentPlayerIndex]++;
                    Console.WriteLine(playerNames[currentPlayerIndex]
                            + " now has "
                            + playerCoins[currentPlayerIndex]
                            + " Player Coins.");
				}
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                playerCoins[currentPlayerIndex]++;
                Console.WriteLine(playerNames[currentPlayerIndex]
                        + " now has "
                        + playerCoins[currentPlayerIndex]
                        + " Player Coins.");
            }

            return checkWinAndMoveToNextPlayer();
        }

		private bool checkWinAndMoveToNextPlayer ()
		{
			var didCurrentPlayerWin = didPlayerWin();

			currentPlayerIndex++;
            if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

			return didCurrentPlayerWin;
		}

        public bool MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(playerNames[currentPlayerIndex] + " was sent to the penalty box");
            inPenaltyBox[currentPlayerIndex] = true;

			return checkWinAndMoveToNextPlayer();
        }

        private bool didPlayerWin()
        {
            return (playerCoins[currentPlayerIndex] == COINS_TO_WIN);
        }
    }
}
