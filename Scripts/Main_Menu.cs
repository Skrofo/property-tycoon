using Godot;
using System;

public partial class Main_Menu : Control
{
    private Button Start_Button;
    private Button Options_Button;
    private Button Exit_Button;
    //private PackedScene PlayerSelect;
    //private PackedScene OptionSelect;

    public override void _Ready()
    {
        Start_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Start_Button");
        Options_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Settings_Button");
        Exit_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Exit_Button");

        PlayerSelect = (PackedScene)GD.Load("res://Scenes/PlayerSelection.tscn"); //add player select link after
        //OptionSelect = (PackedScene)GD.Load(""); //add options select link after
        //PlayerSelect = (PackedScene)GD.Load("es://Scenes/PlayerSelection.tscn"); 
        //OptionSelect = (PackedScene)GD.Load("es://Scenes/Settings_Menu.tcsn"); 
        
        Start_Button.Pressed += OnStartPressed;
        Options_Button.Pressed += OnOptionsPressed;
        Exit_Button.Pressed += OnExitPressed;
    }

    private void OnStartPressed()
    {
        // Record that we're coming from the Main Menu.
        SceneManager.Instance.PreviousScenePath = "res://Scenes/Main_Menu.tscn";
        SceneManager.Instance.ChangeScene("res://Scenes/PlayerSelection.tscn");

        //GetTree().ChangeSceneToPacked(PlayerSelect);
    }

    private void OnOptionsPressed()
    {
        // Record that we're coming from the Main Menu.
        SceneManager.Instance.PreviousScenePath = "res://Scenes/Main_Menu.tscn";
        SceneManager.Instance.ChangeScene("res://Scenes/Settings_Menu.tscn");
        //GetTree().ChangeSceneToPacked(OptionSelect);
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
