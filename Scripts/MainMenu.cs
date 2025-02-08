using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start_Button;
    private Button Options_Button;
    private Button Exit_Button;
    private PackedScene PlayerSelect;
    private PackedScene OptionSelect;

    public override void _Ready()
    {
        Start_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Start_Button");
        Options_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Options_Button");
        Exit_Button = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Exit_Button");
        PlayerSelect = (PackedScene)GD.Load(""); //add player select link after
        OptionSelect = (PackedScene)GD.Load(""); //add options select link after

        Start_Button.Pressed += OnStartPressed;
        Options_Button.Pressed += OnOptionsPressed;
        Exit_Button.Pressed += OnExitPressed;
    }

    private void OnStartPressed()
    {
        GetTree().ChangeSceneToPacked(PlayerSelect);
    }

    private void OnOptionsPressed()
    {
        GetTree().ChangeSceneToPacked(OptionSelect);
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
