using Godot;
using GdUnit4;
using System;
using PropertyTycoon.Scripts;
using System.Threading.Tasks;

[TestSuite]
public class MainMenuTests
{
    [TestCase]
    public static void TestStartButton()
    {
        // Setup
        var runner = ISceneRunner.Load("res://Scenes/Main_Menu.tscn");
        var startButton = runner.FindChild("Start_Button") as Button;
        var mainMenu = runner.Scene().GetNode<MainMenu>(".");

        // Execution
        startButton.EmitSignal("pressed");

        // Assertion
        var sceneTree = runner.Scene().GetTree();
        Assertions.AssertThat(sceneTree.CurrentScene.SceneFilePath).IsEqual("res://Scenes/PlayerSelection.tscn");


    }
    /*
    [TestCase]
    public async Task TestOptionsButton()
    {
        // Setup
        var runner = ISceneRunner.Load("res://Scenes/Main_Menu.tscn");
        var optionsButton = runner.FindChild("Options_Button") as Button;
        var mainMenu = runner.Scene().GetNode<MainMenu>(".");

        // Execution
        optionsButton.EmitSignal("pressed");
        await GdUnit4Test.WaitForSignal(GetTree(), "scene_changed");

        // Assertion
        Assertions.AssertThat(GetTree().CurrentScene).IsInstanceOf<PackedScene>();
    }
    */
    /*
    [TestCase]
    public static void TestExitButton()
    {
        // Setup
        var runner = ISceneRunner.Load("res://Scenes/Main_Menu.tscn");
        var exitButton = runner.FindChild("Exit_Button") as Button;

        // Execution
        exitButton.EmitSignal("pressed");

        // Assertion
        Assertions.AssertBool(Godot.OS.IsQuitRequested()).IsTrue();
    }
    */
}
