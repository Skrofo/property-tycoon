using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerSelection : Control
{
    // Struct to store the players selection
    public struct PlayerData
    {
        public int AvatarIndex;
        public string PlayerType;

        public PlayerData(int avatarIndex, string playerType)
        {
            AvatarIndex = avatarIndex;
            PlayerType = playerType;
        }
    }

    private Button _startButton;
    private Button _backButton;
    private Label _errorLabel;
    private List<PlayerSlot> _playerSlots = new List<PlayerSlot>();
    private PackedScene GameScene;
    private PackedScene MainMenu;

    public override void _Ready()
    {
        _startButton = GetNode<Button>("StartButton");
        _backButton = GetNode<Button>("BackButton");
        _errorLabel = GetNode<Label>("ErrorLabel");

        // Load all player slots
        for (int i = 1; i <= 6; i++)
        {
            var slot = GetNode<PlayerSlot>($"PlayerSlot{i}");
            _playerSlots.Add(slot);
        }

        GameScene = (PackedScene)GD.Load("res://Scenes/Game.tscn");
        MainMenu = (PackedScene)GD.Load("res://Scenes/Main_Menu.tscn");
        _startButton.Pressed += OnStartButtonPressed;
        _backButton.Pressed += OnBackButtonPressed;
        
    }

    // Once the strt button is selected checks if the players selected are valid, if they are then
    // Game data is stored into a singleton and into struct
    private void OnStartButtonPressed()
    {
        if (!ValidateSelections()) return; //Ensures valid selection

        List<PlayerData> activePlayers = new List<PlayerData>();

        foreach (var slot in _playerSlots)
        {
            string playerType = slot.SelectedTypeLabel.Text;

            if (playerType == "None")
                continue; // Skipping players marked as None

            activePlayers.Add(new PlayerData(slot.CurrentAvatarIndex, playerType));
        }

        // Converting list to array for easier storage
        PlayerData[] selectedPlayersArray = activePlayers.ToArray();

        // Stored data into singleton
        GameData gameData = GetNode<GameData>("/root/GameData");
        gameData.SaveSelection(selectedPlayersArray);

        // Move to the game scene
        GetTree().ChangeSceneToPacked(GameScene);
    }

    // Checks to see if the players are Unique and that there is at least 1 AI player
    private bool ValidateSelections()
    {
        HashSet<int> usedAvatars = new HashSet<int>();
        //bool hasAI = false;
        bool hasHuman = false;

        foreach (var slot in _playerSlots)
        {
            string playerType = slot.SelectedTypeLabel.Text;

            if (playerType == "None") continue;

            if (usedAvatars.Contains(slot.CurrentAvatarIndex))
            {
                _errorLabel.Text = "Error: Avatars must be unique!";
                return false;
            }

            usedAvatars.Add(slot.CurrentAvatarIndex);

            // Check at least one human or ai player
          //  if (playerType == "AI")
            //    hasAI = true;

            if (playerType == "Human")
                hasHuman = true;
        }

       // if (!hasAI)
        //{
          //  _errorLabel.Text = "Error: Must have at least one AI player!";
            //return false;
        //}

        if (!hasHuman)
        {
            _errorLabel.Text = "Error: Must have at least one Human player!";
            return false;
        }

        _errorLabel.Text = ""; // Clears error message
        return true;
    }

    // Go back to main menu
    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToPacked(MainMenu);
    }
}

