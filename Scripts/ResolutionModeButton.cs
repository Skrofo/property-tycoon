using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

public partial class ResolutionModeButton : Node
{

    private OptionButton optionButton;

    // Store resolution data as Vector2.
    private readonly Dictionary<int, (string text, Vector2 resolution)> RESOLUTION_DICTIONARY =
        new Dictionary<int, (string, Vector2)>
    {
        { 0, ("1024 x 768", new Vector2(1024, 768)) },
        { 1, ("1280 x 720", new Vector2(1280, 720)) },
        { 2, ("1600 x 900", new Vector2(1600, 900)) },
        { 3, ("1920 x 1080", new Vector2(1920, 1080)) },
        { 4, ("1920 x 1200", new Vector2(1920, 1200)) }
    };

    public override void _Ready()
    {
        // Adjust the node path as needed.
        optionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");

        // Add items to the OptionButton.
        foreach (var kvp in RESOLUTION_DICTIONARY)
        {
            optionButton.AddItem(kvp.Value.text);
        }

        // Connect the signal.
        optionButton.ItemSelected += OnResolutionSelected;
    }

    private void OnResolutionSelected(long index)
    {
        /* // Get the resolution as Vector2.
         Vector2 res = RESOLUTION_DICTIONARY[(int)index].resolution;

         // Convert Vector2 to Vector2i since DisplayServer.WindowSetSize expects integers.
         Vector2i windowSize = new Vector2i((int)res.x, (int)res.y);
         DisplayServer.WindowSetSize(windowSize);

         GD.Print($"Changed resolution to: {res}");*/
        GD.Print("Resolution Changed");

    }
}