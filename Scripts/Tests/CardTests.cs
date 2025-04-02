using GdUnit4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyTycoon.Scripts;

namespace PropertyTycoon.Scripts.Tests
{
    [TestSuite]
    class CardTests
    {
        [TestCase(Card.CardType.OpportunityKnocks)]
        [TestCase(Card.CardType.PotLuck)]
        public static void TestGetCards(Card.CardType type)//Can the game find the cards
        {
            //Setup
            string[] cards = Card.GetCards(type);

            //Assertion
            bool hasCards = cards.Length > 0;
            Assertions.AssertBool(hasCards).IsTrue();
        }

        [TestCase(Card.CardType.OpportunityKnocks)]
        [TestCase(Card.CardType.PotLuck)]
        public static void TestCardFunctionality(Card.CardType type)//Fails if every card from the given card type does not have it's functionality coded in
        {
            //Setup
            string[] cards = Card.GetCards(type);
            var runner = ISceneRunner.Load("res://Scenes/Game.tscn");//Loading the scene for testing
            var gameLoop = runner.Scene().GetNode<GameLoop>(".");
            string failedCards = "";

            //Execution
            foreach (var cardName in cards)
            {
                Card card = new(cardName);
                if (!card.ExecuteAction(gameLoop))
                {
                    failedCards += cardName + " ";
                }
            }

            //Assertion
            var assertion = Assertions.AssertString(failedCards);
            assertion.OverrideFailureMessage($"Cards that failed functionality test: [{failedCards}]");
            assertion.IsEmpty();
        }
    }
}
