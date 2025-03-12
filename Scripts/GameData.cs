using Godot;

public partial class GameData : Node
{
    public PlayerSelection.PlayerData[] SelectedPlayers { get; private set; } = new PlayerSelection.PlayerData[0];

    public void SaveSelection(PlayerSelection.PlayerData[] data)
    {
        SelectedPlayers = data;
    }
}
