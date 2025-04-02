using Godot;
using System;

public partial class VoiceSliderSettings : Control
{
    // This property lets you pick which bus to control (e.g. "Master", "Music", "SFX", "Voice")
    [Export]
    public string BusName { get; set; } = "Voice";

    // (Optional) This is the text for the left Label (caption). Adjust as you wish or remove if unnecessary.
    [Export]
    public string LabelText { get; set; } = "Voice Volume";

    private HSlider slider;
    private Label audioNameLabel;    // Left Label (bus name or caption)
    private Label audioNumLabel;      // Right Label (numeric value)
    private int busIndex;

    public override void _Ready()
    {
        // Get references to your child nodes inside the HBoxContainer
        audioNameLabel = GetNode<Label>("HBoxContainer/Audio_Name_Lbl");           // The first Label
        slider = GetNode<HSlider>("HBoxContainer/HSlider");       // The slider
        audioNumLabel = GetNode<Label>("HBoxContainer/Audio_Num_Lbl");          // The second Label (rename in the scene if needed)

        // Set the caption text on the left label
        audioNameLabel.Text = LabelText;

        // Get the bus index from the bus name
        busIndex = AudioServer.GetBusIndex(BusName);

        // Initialize the slider from the bus's current volume (in decibels, convert to linear)
        float currentDb = AudioServer.GetBusVolumeDb(busIndex);
        slider.Value = DbToLinear(currentDb);

        // Update the right label text (show a percentage, for example)
        UpdateValueLabel();

        // Connect the slider’s ValueChanged signal
        slider.ValueChanged += OnSliderValueChanged;
    }

    private void OnSliderValueChanged(double newValue)
    {
        // Convert slider’s 0..1 range to decibels and set bus volume
        float db = LinearToDb((float)newValue);
        AudioServer.SetBusVolumeDb(busIndex, db);

        // Update the numeric label
        UpdateValueLabel();
    }

    private void UpdateValueLabel()
    {
        // Display slider value as a percentage (0..100)
        float percent = (float)slider.Value * 100f;
        audioNumLabel.Text = Mathf.Round(percent).ToString();
    }

    /// <summary>
    /// Converts decibels to a linear 0..1 value for the slider.
    /// </summary>
    private float DbToLinear(float db)
    {
        // If volume is extremely low, clamp to 0
        if (db <= -80f)
            return 0f;

        return Mathf.Pow(10f, db / 20f);
    }

    /// <summary>
    /// Converts a linear 0..1 slider value to decibels.
    /// </summary>
    private float LinearToDb(float linear)
    {
        if (linear <= 0f)
            return -80f; // a typical “silent” floor in dB

        return 20f * Mathf.Log(linear);
        //an issue above log10 doesnt work ******************************
    }
}
