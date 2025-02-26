using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start_Button;
    private Button Settings_Button;
    private Button Exit_Button;
    //private PackedScene PlayerSelect;
    //private PackedScene OptionSelect;

    public override void _Ready()
    {
        Start_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Start_Button");
        Settings_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Settings_Button");
        Exit_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Exit_Button");

        Start_Button.Pressed += OnStartPressed;
        Settings_Button.Pressed += OnSettingsPressed;
        Exit_Button.Pressed += OnExitPressed;
    }

    private void OnStartPressed()
    {
        //SceneManager.Instance.PreviousScenePath = "res://Scenes/Main_Menu.tscn";
        //SceneManager.Instance.ChangeScene("res://Scenes/Settings_Menu.tscn");
    }

    private void OnSettingsPressed()
    {
        // Record that we're coming from the Main Menu.
        SceneManager.Instance.PreviousScenePath = "res://Scenes/Main_Menu.tscn";
        SceneManager.Instance.ChangeScene("res://Scenes/Settings_Menu.tscn");
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
