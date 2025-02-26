using Godot;
using System;

public partial class Settings_Menu : Control
{
    
    private Label headerLabel;
    private Button audioSettingsButton;
    private Button accessibilitySettingsButton;
    private Button defaultSettingsButton;
    private Button videoSettingsButton;

    public override void _Ready()
    {
        // Get node references
        //headerLabel = GetNode<Label>("header_label");
        audioSettingsButton = GetNode<Button>("audiotsettings_button");
        //accessibilitySettingsButton = GetNode<Button>("accessibilitysettings_button");
        defaultSettingsButton = GetNode<Button>("defaultsettings_button");
        //videoSettingsButton = GetNode<Button>("videosettings_button");


        // Connect signals from each button to the corresponding callback.
        audioSettingsButton.Pressed += OnAudioSettingsPressed;
        //accessibilitySettingsButton.Pressed += OnAccessibilitySettingsPressed;
        defaultSettingsButton.Pressed += OnDefaultSettingsPressed;
        //videoSettingsButton.Pressed += OnVideoSettingsPressed;
    }

    private void OnAudioSettingsPressed()
    {
        GD.Print("Audio Settings Pressed!");
        // Record that we're coming from the Audio Menu.
        SceneManager.Instance.PreviousScenePath = "res://Scenes/Settings_Menu.tscn";
        SceneManager.Instance.ChangeScene("res://Scenes/Audio_Settings.tscn");
    }

    private void OnAccessibilitySettingsPressed()
    {
        GD.Print("Accessibility Settings Pressed!");
        //Switch to an Accessibility Settings screen
    }

    private void OnDefaultSettingsPressed()
    {
        GD.Print("Reset All To Defaults Pressed!");
        //Implement logic to reset settings to default values or call all reset functions
    }

    // If you have a dedicated Video Settings button, include a callback:
    private void OnVideoSettingsPressed()
    {
        GD.Print("Video Settings Pressed!");
        //Switch to a Video Settings screen
    }
}
