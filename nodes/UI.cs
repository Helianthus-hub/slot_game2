using Godot;
using System;

public partial class UI : CanvasLayer {
	[Export] public Button SpinButton;
    [Export] public SlotMachineEngine Engine;
	public override void _Ready(){
		SpinButton.Pressed += OnSpinButtonPressed;
	}
	private void OnSpinButtonPressed(){
		Engine.Spin();
	}
}
