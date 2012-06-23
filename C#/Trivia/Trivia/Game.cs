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

        int[] places = new int[MAX_NUM_PLAYERS];
        int[] playerCoins = new int[MAX_NUM_PLAYERS];

        bool[] inPenaltyBox = new bool[MAX_NUM_PLAYERS];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

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

        public bool add(String playerName)
        {
   			playerNames.Add(playerName);
            places[GetNumberOfPlayers()] = 0;
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

        public void roll(int roll)
        {
            Console.WriteLine(playerNames[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(playerNames[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > BOARD_SIZE - 1) places[currentPlayer] = places[currentPlayer] - BOARD_SIZE;

                    Console.WriteLine(playerNames[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + GetCurrentCategory());
                    askQuestion();
                }
                else
                {
                    Console.WriteLine(playerNames[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > BOARD_SIZE - 1) places[currentPlayer] = places[currentPlayer] - BOARD_SIZE;

                Console.WriteLine(playerNames[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + GetCurrentCategory());
                askQuestion();
            }

        }

        private void askQuestion()
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
            if (places[currentPlayer] == 0) return POP_CATEGORY;
            if (places[currentPlayer] == 4) return POP_CATEGORY;
            if (places[currentPlayer] == 8) return POP_CATEGORY;
            if (places[currentPlayer] == 1) return SCIENCE_CATEGORY;
            if (places[currentPlayer] == 5) return SCIENCE_CATEGORY;
            if (places[currentPlayer] == 9) return SCIENCE_CATEGORY;
            if (places[currentPlayer] == 2) return SCIENCE_CATEGORY;
            if (places[currentPlayer] == 6) return SPORTS_CATEGORY;
            if (places[currentPlayer] == 10) return SPORTS_CATEGORY;
            return ROCK_CATEGORY;
        }

        public bool MarkCurrentAnswerAsCorrectAndMoveToNextPlayer()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    playerCoins[currentPlayer]++;
                    Console.WriteLine(playerNames[currentPlayer]
                            + " now has "
                            + playerCoins[currentPlayer]
                            + " Player Coins.");

                    bool winner = didPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == playerNames.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == playerNames.Count) currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                playerCoins[currentPlayer]++;
                Console.WriteLine(playerNames[currentPlayer]
                        + " now has "
                        + playerCoins[currentPlayer]
                        + " Player Coins.");

                bool winner = didPlayerWin();
                currentPlayer++;
                if (currentPlayer == playerNames.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(playerNames[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == playerNames.Count) currentPlayer = 0;
            return true;
        }

        private bool didPlayerWin()
        {
            return !(playerCoins[currentPlayer] == COINS_TO_WIN);
        }
    }
}
