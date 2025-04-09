using Godot;
using System;

// The SceneManager class is responsible for managing scene transitions,
// and it holds a reference to the file path of the previously active scene.
// This facilitates navigating back to a prior scene if needed.

public partial class SceneManager : Node
{
    // This property stores the file path of the previously active scene.
    // It should be set before initiating a scene change to allow the user to return to the current scene later.
    public string PreviousScenePath { get; set; } = "";

    // A singleton instance of the SceneManager is created for easy global access.
    // This pattern ensures that only one instance of the SceneManager exists throughout the game.
    public static SceneManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    // Changes the active scene to a new scene specified by the file path.
    public void ChangeScene(string newScenePath)
    {
        GetTree().ChangeSceneToFile(newScenePath);
    }


    // Transitions back to the previously recorded scene, if available.
  
    // The method first checks whether a valid previous scene file path has been recorded.
    // If a previous scene is present, it prints a message to the output and performs the transition.
    // If not, it prints that no previous scene was recorded.
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
