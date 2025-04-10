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
        GameLoop gameLoop = runner.Scene().GetNode<GameLoop>(".");
        Button rollDieButton = runner.FindChild("RollButton") as Button;
        TextureRect[] die = new TextureRect[2];
        die[0] = runner.Scene().GetNode<TextureRect>("UI/DiceFace1");
        die[1] = runner.Scene().GetNode<TextureRect>("UI/DiceFace2");
        Texture2D[] dieTextures = GameLoop.LoadDiceTextures();
        bool correctDieFaces = true;

        //Execution
        rollDieButton.EmitSignal("pressed");
        for (int i = 0; i < 2; i++) //Loops once for each die
        {
            GD.Print($"die result 1: {gameLoop.diceResult[0]} \ndie result 2: {gameLoop.diceResult[1]}");
            if (die[i].Texture != dieTextures[gameLoop.diceResult[i]-1])
            {
                correctDieFaces = false;
            }
        }


            //Assertion
            Assertions.AssertBool(correctDieFaces).IsTrue();
    }

    [TestCase]
    public static void GetPlayerPieceTextures()
    {
        List<Texture2D> textures = PlayerSlot.GetTextures();
        Assertions.AssertArray(textures).IsNotEmpty();
    }

    [TestCase(1,0, TestName = "Test Game(1 human, 0 ai)")]
    [TestCase(0, 0, TestName = "Test Game(0 human, 0 ai)")]
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
