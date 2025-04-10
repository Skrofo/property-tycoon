using Godot;
using System;

public partial class SettingsMenu : Control
{
    
    private Label headerLabel;
    private Button exitButton;

    public override void _Ready()
    {
        // Get node references and assign it to a variable

        //headerLabel = GetNode<Label>("header_label");
        exitButton = GetNode<Button>("MarginContainer/VBoxContainer/ExitButton");
        


        // Connect signals from each button to the corresponding callback.
        exitButton.Pressed += OnExitButtonPressed;
        
    }

    private void OnExitButtonPressed()
    {
        GD.Print("Exit Button Pressed!");
        // Return to Main Menu.
        //Scene handling handled in scenemanager.cs
        SceneManager.Instance.GoBack();
    }

    
}
