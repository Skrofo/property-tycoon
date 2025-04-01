using Godot;
using PropertyTycoon.Scripts;
using System;
using System.Collections.Generic;

public partial class Marker : Node2D
{
    public enum SpaceType// The different types of spaces
    {
        Go,
        Property,
        PotLuck,
        OpportunityKnocks,
        FreeParking,
        VisitingJail,
        Jail,
        GoToJail
    }

    [Export] public Marker nextMarker;//The place on the board after this one
    [Export] public SpaceType type = SpaceType.Property;

    public Marker prevMarker;//The place on the board before this one

    private List<Player> players;//The players that are currently on this place
    private Vector2I[] markerPos;
    private GameLoop game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Assignin varaibles at startup
        players = new List<Player>();
        markerPos = new Vector2I[GameLoop.MaxPlayerCount];
        game = GetNode<GameLoop>("/root/Game");
        for (int i = 0; i < markerPos.Length; i++)
        {
            markerPos[i] = (Vector2I)GetNode<Node2D>("Marker"+(i+1)).GlobalPosition;
        }


        // Check if this marker has a next marker (the next space on the board)
        if (nextMarker != null)
        {
            // Set the previous marker of the next space to be this marker
            nextMarker.prevMarker = this;
        }
    }

    //Gets the nth next position. E.g: The 3rd next position is the position 3  spaces  in front of this one.
    //If a players is given and one of the passed position is go, the players will "pass go"
    public Marker GetNthNextPos(int pos, Player player = null)
    {
        if (pos > 0)
        {
            if (nextMarker.type == SpaceType.Go)
            {
                if (player != null)
                {
                    player.passedGo = true;
                }
            }
            return nextMarker.GetNthNextPos(pos-1, player);
        }
        else
        {
            return GetNode<Marker>(".");
        }
    }

    public void MoveTo(Player player) { MoveTo(player, false); }
    public void MoveTo(Player player, bool instant/*Whether the movment is instant/smooth*/)//Move the player to this place
	{
        Vector2I destination;//coords the player is movin towards

        player.node.ZIndex = players.Count + 1;//Making sure the player renders over other players that are supposed to be behind it
        if (players.Count<3 && type!= SpaceType.VisitingJail)
        {
            if (GetNode<Node2D>(".").Rotation == 0 || GetNode<Node2D>(".").Rotation == 270)
            {
                destination = new Vector2I((int)GetNode<Node2D>(".").GlobalPosition.X, markerPos[players.Count].Y);
            }
            else
            {
                destination = new Vector2I(markerPos[players.Count].X, (int)GetNode<Node2D>(".").GlobalPosition.Y);
            }
        }
        else if (players.Count>3 || type == SpaceType.VisitingJail)
        {
            destination = markerPos[players.Count];
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                game.movingPlayers.Add(players[i], markerPos[i]);
            }
            destination = markerPos[players.Count];
        }
        if (instant)
        {
            player.node.GlobalPosition = destination;
        }
        else
        {
            game.movingPlayers.Add(player, destination);
        }
        players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
        if (players.Count == 3)
        {
            for (int i = 0; i < players.Count; i++)
            {
                game.movingPlayers.Add(players[i], new Vector2I((int)GetNode<Node2D>(".").GlobalPosition.X, markerPos[players.Count].Y));
            }
        }
    }

    //Gets the nth previous position. E.g: The 3rd previous position is the position 3  spaces  behind this one.
    //If a players is given and one of the passed position is go, the players will "pass go"
    public Marker GetNthPreviousPos(int pos, Player player = null)
    {
        if (pos > 0 && prevMarker != null)
        {
            if (prevMarker.type == SpaceType.Go)
            {
                if (player != null)
                {
                    player.passedGo = true;
                }
            }
            return prevMarker.GetNthPreviousPos(pos - 1, player);
        }
        else
        {
            return GetNode<Marker>(".");
        }
    }
}
