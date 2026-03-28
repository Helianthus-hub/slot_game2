using Godot;
using System;

public partial class UI : CanvasLayer {
	[Export] public Button SpinButton;
    [Export] public SlotMachineEngine Engine;
	[Export] public VBoxContainer HUD;
	private Label payoutLabel;
	private Label chipsLabel;
	private Label multiLabel;
	private Label handLabel;
	
	public override void _Ready(){
		payoutLabel = HUD.GetNode<Label>("PayoutLabel");
		chipsLabel = HUD.GetNode<Label>("ChipsLabel");
		multiLabel = HUD.GetNode<Label>("MultiLabel");
		handLabel   = HUD.GetNode<Label>("HandLabel");
		SpinButton.Pressed += OnSpinButtonPressed;
		Engine.SpinCompleted += OnSpinCompleted;
		Engine.HandResolved += OnHandResolved;
	}
	private void OnSpinButtonPressed(){
		Engine.Spin();
	}
	private void OnSpinCompleted(int payout, int chips, float multi){
		payoutLabel.Text = $"Payout: ${payout}";
		multiLabel.Text = $"{multi}";
		chipsLabel.Text = $"{chips}";
	}

	private void OnHandResolved(int handType, int payout, int chips, float multi) {
    handLabel.Text = $"{(HandType)handType}";
    chipsLabel.Text = $"Chips: {chips}";
    multiLabel.Text = $"Multi: {multi:F2}x";
    payoutLabel.Text = $"Payout: {payout}";
	}
}
