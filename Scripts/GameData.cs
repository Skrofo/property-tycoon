using Godot;

public partial class GameData : Node
{
    public PlayerSelection.PlayerData[] SelectedPlayers { get; private set; } = new PlayerSelection.PlayerData[0];

    public void SaveSelection(PlayerSelection.PlayerData[] data)
    {
        SelectedPlayers = data;


        // Quick test to console output to test what is saved
        GD.Print("GameData: Player Selection saved");
        GD.Print($"Total players: {SelectedPlayers.Length}");

        for (int i = 0; i < SelectedPlayers.Length; i++)
        {
            GD.Print($"Player {i + 1} -> Avatar: {SelectedPlayers[i].AvatarIndex}, Type: {SelectedPlayers[i].PlayerType}");
        }
    }
}
