using Godot;
using System;

public partial class WindowModeButton : Control
{

    private OptionButton optionButton;

    // The array of window mode strings.
    // cannot be changed.
    private readonly string[] WINDOW_MODE_ARRAY =
    {
        "Full-Screen",
        "Window Mode",
        "Borderless Window",
        "Borderless Full-Screen"
    };

    public override void _Ready()
    {
        // Get a reference to the OptionButton
        optionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");

        // Populate the OptionButton with items.
        foreach (string modeName in WINDOW_MODE_ARRAY)
        {
            optionButton.AddItem(modeName);
            GD.Print("In ready. adding item");
        }

        // Subscribe to the ItemSelected signal/event.
        optionButton.ItemSelected += OnWindowModeSelected;
    }

    private void OnWindowModeSelected(long index)
    {
        // Switch on the selected index to set window mode & flags.
        switch (index)
        {
            case 0:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                GD.Print("Full Screen");
                break;

            case 1:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                GD.Print("Windowed");
                break;

            case 2:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                GD.Print("Borderless Window");
                break;

            case 3:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                GD.Print("Borderless Full-Screen");
                break;
        }
    }
}
