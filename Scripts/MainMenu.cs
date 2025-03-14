using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start_Button;
    private Button Settings_Button;
    private Button Exit_Button;
    private PackedScene PlayerSelect;
    private PackedScene OptionSelect;

    public override void _Ready()
    {
        Start_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Start_Button");
        Settings_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Settings_Button");
        Exit_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Exit_Button");

        PlayerSelect = (PackedScene)GD.Load("res://Scripts/PlayerSelection.cs"); //add player select link after
        OptionSelect = (PackedScene)GD.Load(""); //add options select link after
        
        Start_Button.Pressed += OnStartPressed;
        Settings_Button.Pressed += OnOptionsPressed;
        Exit_Button.Pressed += OnExitPressed;
    }

    private void OnStartPressed()
    {
        //SceneManager.Instance.PreviousScenePath = "es://Scenes/Main_Menu.tscn";
        //SceneManager.Instance.ChangeScene("es://Scenes/Settings_Menu.tscn");

        GetTree().ChangeSceneToPacked(PlayerSelect);
    }

    private void OnOptionsPressed()
    {
        // Record that we're coming from the Main Menu.
        //SceneManager.Instance.PreviousScenePath = "es://Scenes/Main_Menu.tscn";
        //SceneManager.Instance.ChangeScene("es://Scenes/Settings_Menu.tscn");
        GetTree().ChangeSceneToPacked(OptionSelect);
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
