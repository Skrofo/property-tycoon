using GdUnit4;
using Godot;
using System;
using PropertyTycoon.Scripts;
using System.Threading.Tasks;
using System.Threading;

[TestSuite]
public class GameLoopTests
{
    [TestCase]
    public static void RollDieOutput()
    {
        //Setup
        var runner = ISceneRunner.Load("res://Scenes/Game.tscn");//Loading the scene for testing
        Button rollDieButton = runner.FindChild("RollButton") as Button;
        Vector2 buttonPos = rollDieButton.GlobalPosition;
        Label[] diceResults = [runner.FindChild("TMPDiceResult1") as Label, runner.FindChild("TMPDiceResult2") as Label];
        string[] originalValues = [diceResults[0].Text, diceResults[1].Text];

        //Execution
        rollDieButton.EmitSignal("pressed");

        //Assertion
        bool diffOutputs = diceResults[0].Text != originalValues[0] && diceResults[1].Text != originalValues[1];
        Assertions.AssertBool(diffOutputs).IsTrue();
    }

    [TestCase]
    public void PlayerMovement()
    {

    }

    [TestCase(1,0)]
    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(0, 1)]
    public static void TestGame(int humanPlayers, int aiPlayers)
    {
        //Setup
        var runner = ISceneRunner.Load("res://Scenes/Game.tscn");//Loading the scene for testing
        var gameLoop = runner.Scene().GetNode<GameLoop>(".");
        int humanPlayerCount = 0;
        int aiPlayerCount = 0;

        //Execution
        gameLoop.TestGame(humanPlayers, aiPlayers);
        for (int i = 0; i < gameLoop.players.Length; i++)
        {
            if (gameLoop.players[i].cpu) aiPlayers++;
            else humanPlayers++;
        }

        //Assertion
        Assertions.AssertBool(aiPlayers == aiPlayerCount);
        Assertions.AssertBool(humanPlayers == humanPlayerCount);
    }
}
