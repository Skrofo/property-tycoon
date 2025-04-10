using Godot;
using System;

public partial class JailBail : Control
{
	[Export] public int fineAmount;//How much jail bail is
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<Control>(".").Visible = false;
    }

	//runs if they press the pay button
	public void Yes()
    {
        GameLoop gameLoop = FindParent("Game").GetNode<GameLoop>(".");
		if (gameLoop.currentPlayer.PayBail())
        {
            GetNode<Control>(".").Visible = false;
        }
        else
        {
            //TODO: Add visual effect when player tries to pay bail and can't
        }
    }

	//runs if they press no
	public void No()
	{
        GetNode<Control>(".").Visible = false;
    }
}
