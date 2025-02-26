using Godot;
using System;

public partial class BackButton : Button
{
    public override void _Ready()
    {
        Pressed += OnBackPressed;
    }

    private void OnBackPressed()
    {
        SceneManager.Instance.GoBack();
    }
}
