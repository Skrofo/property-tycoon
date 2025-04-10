using Godot;
using PropertyTycoon.Scripts;
using System;

public partial class CardDisplay : Control
{
	public Card card;

	private string[] OpportunityCards;
	private string[] potCards;
    private GameLoop gameLoop;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<Control>(".").Visible = false;
        OpportunityCards = Card.GetCards(Card.CardType.OpportunityKnocks);
        potCards = Card.GetCards(Card.CardType.PotLuck);
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("DismissGameMenu") && GetNode<Control>(".").Visible == true)
        {
            GetNode<Control>(".").Visible = false;
            card.ExecuteAction(gameLoop);
            gameLoop.NextTurn(true);
        }
    }

    public void Show(string cardName) { ShowCard(new Card(cardName)); }
    public void Show(Card.CardType type) { ShowCard(new Card(type)); }
    private void ShowCard(Card newCard)
    {
        card = newCard;
        if (card.type == Card.CardType.OpportunityKnocks)
        {
            GetNode<TextureRect>("TextureRect").Texture = GD.Load<Texture2D>(Card.opportunityDirPath + card.name);
        }
        else if (card.type == Card.CardType.PotLuck)
        {
            GetNode<TextureRect>("TextureRect").Texture = GD.Load<Texture2D>(Card.potDirPath + card.name);
        }
        GetNode<Control>(".").Visible = true;
    }
}
