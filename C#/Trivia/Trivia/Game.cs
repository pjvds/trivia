using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    public class Game
    {
		const int MAX_NUM_PLAYERS = 6;
		const int BOARD_SIZE = 12;
		const int COINS_TO_WIN = 6;

		const string POP_CATEGORY = "Pop";
		const string SCIENCE_CATEGORY = " Science";
		const string SPORTS_CATEGORY = "Sports";
		const string ROCK_CATEGORY = "Rock";

        List<string> playerNames = new List<string>();

        internal int[] playerCurrentPlaces = new int[MAX_NUM_PLAYERS];
        internal int[] playerCoins = new int[MAX_NUM_PLAYERS];

        internal bool[] inPenaltyBox = new bool[MAX_NUM_PLAYERS];

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
   			playerNames.Add(playerName);
            playerCurrentPlaces[GetNumberOfPlayers()] = 0;
            playerCoins[GetNumberOfPlayers()] = 0;
            inPenaltyBox[GetNumberOfPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + playerNames.Count);
            return true;
        }

        public int GetNumberOfPlayers()
        {
            return playerNames.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(playerNames[currentPlayerIndex] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayerIndex])
            {
                if (roll % 2 != 0)
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
            if (inPenaltyBox[currentPlayerIndex])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    playerCoins[currentPlayerIndex]++;
                    Console.WriteLine(playerNames[currentPlayerIndex]
                            + " now has "
                            + playerCoins[currentPlayerIndex]
                            + " Player Coins.");

                    bool winner = didPlayerNotWin();
                    currentPlayerIndex++;
                    if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

                    return winner;
                }
                else
                {
                    currentPlayerIndex++;
                    if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
                    return true;
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

                bool winner = didPlayerNotWin();
                currentPlayerIndex++;
                if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

                return winner;
            }
        }

        public bool MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(playerNames[currentPlayerIndex] + " was sent to the penalty box");
            inPenaltyBox[currentPlayerIndex] = true;

            currentPlayerIndex++;
            if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
            return true;
        }

        private bool didPlayerNotWin()
        {
            return !(playerCoins[currentPlayerIndex] == COINS_TO_WIN);
        }
    }
}
