using System;
using NUnit.Framework;
using UglyTrivia;
using NUnit.Framework.SyntaxHelpers;

namespace Trivia
{
	public class GameTests
	{
		[Test]
		public void shouldAddPlayers() 
		{
			Game g = createGameWithTwoPlayers();
			Assert.That (g.GetNumberOfPlayers(), Is.EqualTo(2));
		}
	
		[Test]
		public void needsTwoPlayersToPlay() {
			Game g = new Game();
			g.Add("x");
			Assert.IsFalse(g.IsPlayable());
			g.Add("y");
			Assert.IsTrue(g.IsPlayable());
		}
	
		[Test]
		public void advancesPlayerAfterAnswer ()
		{
			Game g = createGameWithTwoPlayers ();
			Assert.That (g.currentPlayerIndex, Is.EqualTo(0));
			g.Roll (5);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer();
			Assert.That (g.currentPlayerIndex, Is.EqualTo(1));
			g.Roll (3);
			g.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer();
			Assert.That (g.currentPlayerIndex, Is.EqualTo(0));
		}

		private Game createGameWithTwoPlayers ()
		{
			Game g = new Game ();
			g.Add ("x");
			g.Add ("y");
			return g;
		}
	
		[Test]
		public void toPenaltyBoxAfterWrongAnswer ()
		{
			Game game = createGameWithTwoPlayers ();
			game.Roll (4);
			game.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer ();
			Assert.That (game.IsCurrentPlayerInPenaltyBox(), Is.True);
		}
	
		[Test]
		public void notToPenaltyBoxAfterCorrectAnswer ()
		{
			Game g = createGameWithTwoPlayers ();
			g.Roll (2);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer();
			Assert.IsFalse (g.IsCurrentPlayerInPenaltyBox());
		}
	
		[Test]
		public void outOfPenaltyBoxAfterOddRoll ()
		{
			Game g = createGameWithTwoPlayers ();
			g.Roll (2);
			g.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer ();
			Assert.IsTrue (g.IsCurrentPlayerInPenaltyBox());
			g.Roll (3);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
			g.Roll (3);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
			Assert.IsFalse (g.IsCurrentPlayerInPenaltyBox());
		}

		[Test]
		public void notOutOfPenaltyBoxAfterEvenRoll ()
		{
			Game g = createGameWithTwoPlayers ();
			g.Roll (2);
			g.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer ();
			Assert.IsTrue (g.IsCurrentPlayerInPenaltyBox());
			g.Roll (3);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
			g.Roll (2);
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
			Assert.IsTrue (g.IsCurrentPlayerInPenaltyBox());
		}
	
		[Test]
		public void getsCoinAfterCorrectAnswer ()
		{
			Game g = createGameWithTwoPlayers ();
			g.Roll (5);
			Assert.That (g.playerCoins [0], Is.EqualTo (0));
			g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
			Assert.That (g.playerCoins [0], Is.EqualTo (1));
		}

		[Test]
		public void doesNotGetCoinAfterWrongAnswer ()
		{
			Game g = createGameWithTwoPlayers ();
			g.Roll (5);
			Assert.AreEqual (0, g.playerCoins [0]);
			g.MarkCurrentAnswerAsIncorrectAndMoveToNextPlayer ();
			Assert.AreEqual (0, g.playerCoins [0]);
		}
	
		[Test]
		public void winAfterSixCorrectAnswers ()
		{
			Game g = createGameWithTwoPlayers ();
			for (int i = 0; i < 10; i++) {
				Assert.IsFalse (rollAndGiveCorrectAnswer (g));
			}
			Assert.IsTrue (rollAndGiveCorrectAnswer (g));
		}
	
		private bool rollAndGiveCorrectAnswer (Game g)
		{
			g.Roll (4);
			return g.MarkCurrentAnswerAsCorrectAndMoveToNextPlayer ();
		}
	}
}