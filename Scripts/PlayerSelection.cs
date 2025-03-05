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
    private Label _errorLabel;
    private List<PlayerSlot> _playerSlots = new List<PlayerSlot>();

    public override void _Ready()
    {
        _startButton = GetNode<Button>("StartButton");
        _errorLabel = GetNode<Label>("ErrorLabel");

        // Load all player slots
        for (int i = 1; i <= 7; i++)
        {
            var slot = GetNode<PlayerSlot>($"PlayerSlot{i}");
            _playerSlots.Add(slot);
        }

        _startButton.Pressed += OnStartButtonPressed;
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

        // Stored data into singleton ENTER THE LINK FOR WHERE THE SINGLETON IS
        GameData gameData = GetNode<GameData>("");
        gameData.SaveSelection(selectedPlayersArray);

        // Move to the game scene
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    // Checks to see if the players are Unique and that there is at least 1 AI player
    private bool ValidateSelections()
    {
        HashSet<int> usedAvatars = new HashSet<int>();
        bool hasAI = false;

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

            if (playerType == "AI")
                hasAI = true;
        }

        if (!hasAI)
        {
            _errorLabel.Text = "Error: Must have at least one AI player!";
            return false;
        }

        _errorLabel.Text = ""; // Clears error message
        return true;
    }
}

