using Godot;
using System;

public partial class GetOutOfJailFreeDisplay : Control
{
    private GameLoop gameLoop;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
        GetNode<Control>(".").Visible = false;
    }

    public void Yes()
    {
        gameLoop.currentPlayer.hasGetOutOfJail = false;
        gameLoop.currentPlayer.getOutOfJail();
        GetNode<Control>(".").Visible = false;
    }



    public void No()
    {
        GetNode<Control>(".").Visible = false;
        gameLoop.GetNode<Control>("UI/JailBail").Visible = true;
    }
}
