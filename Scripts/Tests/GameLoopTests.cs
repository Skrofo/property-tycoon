using GdUnit4;
using Godot;
using System;
using PropertyTycoon.Scripts;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

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
    public static void GetPlayerPieceTextures()
    {
        List<Texture2D> textures = PlayerSlot.GetTextures();
        Assertions.AssertArray(textures).IsNotEmpty();
    }

    [TestCase(1,0, TestName = "Test Game(1 human, 0 ai)")]
    [TestCase(1, 1, TestName = "Test Game(1 human, 1 ai)")]
    [TestCase(0, 1, TestName = "Test Game(0 human, 1 ai)")]
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
