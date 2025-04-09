using GdUnit4;
using Godot;
using System;
using PropertyTycoon.Scripts;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

namespace PropertyTycoon.Scripts.Tests
{
    [TestSuite]
    public class SelectionScreenTest
    {
        [TestCase]
        public void TestTypeButton()
        {
            // setup
            var runner = ISceneRunner.Load("res://Scenes/PlayerSelection.tscn");
            Button PlayerTypebutton = runner.Scene().GetNode<Button>("PlayerSlot1/PlayerTypeButton");
            Label PlayerTypeLabel = runner.Scene().GetNode<Label>("PlayerSlot1/SelectedTypeLabel");

            // execution
            PlayerTypebutton.EmitSignal("pressed");

            // assertion
            string expectedText = "AI";
            Assertions.AssertString(PlayerTypeLabel.Text).IsEqual(expectedText);
        }

        [TestCase]
        public void RightButtonPressed()
        {
            // setup
            var runner = ISceneRunner.Load("res://Scenes/PlayerSelection.tscn");
            Button rightButton = runner.Scene().GetNode<Button>("PlayerSlot1/RightArrow");
            TextureRect PlayerAvatar = runner.Scene().GetNode<TextureRect>("PlayerSlot1/SelectedAvatar");
            List<Texture2D> textures = PlayerSlot.GetTextures();

            var initialAvatar = PlayerAvatar.Texture;
            int initialAvatarIndex = textures.IndexOf((Texture2D)initialAvatar);

            // execution
            rightButton.EmitSignal("pressed");

            var newAvatar = PlayerAvatar.Texture;
            int newAvatarIndex = textures.IndexOf((Texture2D)newAvatar);

            // assertion
            Assertions.AssertInt(newAvatarIndex).IsEqual(initialAvatarIndex + 1);
        }

        [TestCase]
        public void LeftButtonPressed()
        {
            var runner = ISceneRunner.Load("res://Scenes/PlayerSelection.tscn");
            Button leftButton = runner.Scene().GetNode<Button>("PlayerSlot1/LeftArrow");
            TextureRect PlayerAvatar = runner.Scene().GetNode<TextureRect>("PlayerSlot1/SelectedAvatar");
            List<Texture2D> textures = PlayerSlot.GetTextures();

            var initialAvatar = PlayerAvatar.Texture;
            int initialAvatarIndex = textures.IndexOf((Texture2D)initialAvatar);

            // execution
            leftButton.EmitSignal("pressed");

            var newAvatar = PlayerAvatar.Texture;
            int newAvatarIndex = textures.IndexOf((Texture2D)newAvatar);

            // assertion
            Assertions.AssertInt(newAvatarIndex).IsEqual(initialAvatarIndex + 5);
        }

    }
}
