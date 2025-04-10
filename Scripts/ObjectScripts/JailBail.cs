using Godot;
using PropertyTycoon.Scripts;
using System;

public partial class JailBail : Control
{
	[Export] public int fineAmount;//How much jail bail is

    private Player player;
    private GameLoop gameLoop;
    private string originalText;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
        GetNode<Control>(".").Visible = false;
        originalText = GetNode<Label>("Panel/Label").Text;
    }

    public void ShowMenu(Player bailedPlayer)
    {
        player = bailedPlayer;
        GetNode<Control>(".").Visible = true;
    }

	//runs if they press the pay button
	public void Yes()
    {
		if (player.PayBail())
        {
            GetNode<Control>(".").Visible = false;
            gameLoop.NextTurn(true);
        }
        else
        {
            // Add visual effect when player tries to pay bail and can't
            GetNode<Label>("Panel/Label").Text = "You don't have enough\n money to pay bail!";
            
        }
    }

	//runs if they press no
	public void No()
	{
        GetNode<Control>(".").Visible = false;
        player.jailTurns = 3;
        gameLoop.NextTurn(true);
        GetNode<Label>("Panel/Label").Text = originalText;
    }
}
