using Godot;
using System.Collections.Generic;

public partial class PlayerSlot : Control
{
    [Export] public TextureRect SelectedAvatar;
    [Export] public Label SelectedTypeLabel;
    [Export] public Button LeftArrow;
    [Export] public Button RightArrow;
    [Export] public Button PlayerTypeButton;

    private List<Texture2D> avatars = new List<Texture2D>();
    public int CurrentAvatarIndex = 0;
    public int CurrentTypeIndex = 0; // 0 = Human, 1 = AI, 2 = None
    private string[] playerTypes = { "Human", "AI", "None" };

    public override void _Ready()
    {
        // Load avatars into list
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/Boot.png"));
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/cat.png"));
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/hatstand.png"));
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/iron.png"));
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/ship.png"));
        avatars.Add((Texture2D)GD.Load("res://Assets/Avatars/smartphone.png"));

        // Make sure we have avatars
        if (avatars.Count > 0)
        {
            SelectedAvatar.Texture = avatars[CurrentAvatarIndex];
        }

        // Set defult player type to human
        SelectedTypeLabel.Text = playerTypes[CurrentTypeIndex];

        // Button signals
        LeftArrow.Pressed += () => ChangeAvatar(-1);
        RightArrow.Pressed += () => ChangeAvatar(1);
        PlayerTypeButton.Pressed += ChangePlayerType;
    }

    private void ChangeAvatar(int direction)
    {
        CurrentAvatarIndex = (CurrentAvatarIndex + direction + avatars.Count) % avatars.Count;
        SelectedAvatar.Texture = avatars[CurrentAvatarIndex];
    }

    private void ChangePlayerType()
    {
        CurrentTypeIndex = (CurrentTypeIndex + 1) % playerTypes.Length;
        SelectedTypeLabel.Text = playerTypes[CurrentTypeIndex];
    }
}