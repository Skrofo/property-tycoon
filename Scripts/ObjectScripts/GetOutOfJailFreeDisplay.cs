using Godot;
using PropertyTycoon.Scripts;
using System;

public partial class GetOutOfJailFreeDisplay : Control
{
    private GameLoop gameLoop;
    private Player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
        GetNode<Control>(".").Visible = false;
    }

    public void ShowMenu(Player bailedPlayer)
    {
        player = bailedPlayer;
        GetNode<Control>(".").Visible = true;
    }

    public void Yes()
    {
        player.hasGetOutOfJail = false;
        player.getOutOfJail();
        GetNode<Control>(".").Visible = false;
        gameLoop.NextTurn(true);
    }

    public void No()
    {
        GetNode<Control>(".").Visible = false;
        gameLoop.GetNode<JailBail>("UI/JailBail").ShowMenu(player);
    }
}
