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
    }

    //Gets the nth next position. E.g: The 3rd next position is the position 3  spaces  in front of this one.
    public Marker GetNthNextPos(int pos)
    {
        if (pos > 0)
        {
            return nextMarker.GetNthNextPos(pos-1);
        }
        else
        {
            return GetNode<Marker>(".");
        }
    }

    public void MoveTo(Player player) { MoveTo(player, false); }
    public void MoveTo(Player player, bool instant/*Is the movment instant/smooth*/)//Move the player to this place
	{
        Vector2I destination;//coords the player is movin towards

        player.node.ZIndex = players.Count + 1;
        if (players.Count<3)
        {
            if (GetNode<Node2D>(".").Rotation == 0 || GetNode<Node2D>(".").Rotation == 180)
            {
                destination = new Vector2I((int)GetNode<Node2D>(".").GlobalPosition.X, markerPos[players.Count].Y);
            }
            else
            {
                destination = new Vector2I(markerPos[players.Count].X, (int)GetNode<Node2D>(".").GlobalPosition.Y);
            }
        }
        else if (players.Count>3)
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
}
