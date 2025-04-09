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
    static int counter = 0;

    public override void _Ready()
    {

        SelectedAvatar = GetNode<TextureRect>("SelectedAvatar");
        LeftArrow = GetNode<Button>("LeftArrow");
        RightArrow = GetNode<Button>("RightArrow");
        PlayerTypeButton = GetNode<Button>("PlayerTypeButton");
        SelectedTypeLabel = GetNode<Label>("SelectedTypeLabel");

        // Load avatars into list
        avatars = GetTextures();

        // Make sure we have avatars
        if (avatars.Count > 0)
        {
            CurrentAvatarIndex = GetUniqueAvatarIndex();
            SelectedAvatar.Texture = avatars[CurrentAvatarIndex];
        }

        // Set defult player type to human
        SelectedTypeLabel.Text = playerTypes[CurrentTypeIndex];

        // Button signals
        LeftArrow.Pressed += () => ChangeAvatar(-1);
        RightArrow.Pressed += () => ChangeAvatar(1);
        PlayerTypeButton.Pressed += ChangePlayerType;
    }

    //Returns the avatars textures
    public static List<Texture2D> GetTextures()
    {
        List<Texture2D> textures = new();
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/Boot.png"));
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/cat.png"));
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/hatstand.png"));
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/iron.png"));
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/ship.png"));
        textures.Add((Texture2D)GD.Load("res://Assets/Avatars/smartphone.png"));

        return textures;
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

        bool isNone = (SelectedTypeLabel.Text == "None");

        LeftArrow.Visible = !isNone;
        RightArrow.Visible = !isNone;
        SelectedAvatar.Visible = !isNone;
    }
    private int GetUniqueAvatarIndex()
    {
        int index = counter % avatars.Count;
        counter++;
        return index;
    }
}