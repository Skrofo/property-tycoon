using Godot;
using PropertyTycoon.Scripts;
using System;

public partial class ClaimFreeParking : Control
{
    public const char PoundSymbol = (char)163;
    private string FreeParkingText;
    private Player player;
    private GameLoop gameLoop;
    private Card card;
    private int FreeParkingAmount;

    public override void _Ready()
    {
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
        FreeParkingAmount = gameLoop.GetParkingMoney();
        GetNode<Control>(".").Visible = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("DismissGameMenu") && GetNode<Control>(".").Visible == true)
        {
            GetNode<Control>(".").Visible = false;
            gameLoop.NextTurn(true);
        }
    }

    public void ShowMenu(Player Player)
    {
        FreeParkingAmount = gameLoop.GetParkingMoney();
        GetNode<Control>(".").Visible = true;
        if (FreeParkingAmount == 0)
        {
            GetNode<Label>("Panel/Label").Text = ($"Free Parking Amount is {PoundSymbol}0!\n Nothing to Claim :(");
            return;
        }
        else
        {
            FreeParkingText = GetNode<Label>("Panel/Label").Text = ($"You have claimed\n {PoundSymbol}{FreeParkingAmount}\n from free parking!");
            player = gameLoop.currentPlayer;
            int ParkingAmount = gameLoop.GetParkingMoney();
            player.AddMoney(ParkingAmount);
            gameLoop.AddParkingMoney(-ParkingAmount);
        }
    }
}