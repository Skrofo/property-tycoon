using Godot;
using System;

public partial class SceneManager : Node
{
    // This variable stores the file path of the previous scene.
    public string PreviousScenePath { get; set; } = "";

    // (Optional) Make this singleton easily accessible.
    public static SceneManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// Changes to a new scene.
    /// Before switching, you should set PreviousScenePath to the current scene's file path.
    /// </summary>
    public void ChangeScene(string newScenePath)
    {
        GetTree().ChangeSceneToFile(newScenePath);
    }

    /// <summary>
    /// Changes back to the previously recorded scene.
    /// </summary>
    public void GoBack()
    {
        if (!string.IsNullOrEmpty(PreviousScenePath))
        {
            GD.Print($"Going back to: {PreviousScenePath}");
            GetTree().ChangeSceneToFile(PreviousScenePath);
        }
        else
        {
            GD.Print("No previous scene recorded.");
        }
    }
}
