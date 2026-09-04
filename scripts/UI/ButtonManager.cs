using Godot;
using Godot.Collections;
using System;

public partial class ButtonManager : Control
{
	private Button _mainbutton;

	private int addamt;

	private Array<Button> _BuyButtons = new Array<Button>();

	private Timer B2time;

	public override void _Ready()
	{
		EventBus.Instance.ButtonUpgrade += OnButtonUpgrade;

		_mainbutton = GetNode<Button>("MainButton");
		_mainbutton.Pressed += OnMainPressed;
		addamt = 1;
		

		_BuyButtons.Add(GetNode<Button>("UpgradeMenu/Buy1"));
		_BuyButtons.Add(GetNode<Button>("UpgradeMenu/Buy2"));
		_BuyButtons[0].Pressed += OnB1Pressed;
		_BuyButtons[1].Pressed += OnB2Pressed;

		B2time = GetNode<Timer>("UpgradeMenu/Buy2/Timer");
		B2time.Timeout += OnB2Timeout;


	}

	private void OnMainPressed()
	{
		Stats.Instance.UpdateNumber(addamt);
		
	}
	private void OnB1Pressed()
	{
		addamt += 1;
	}

	private void OnB2Pressed(){ B2time.Start(); }

	private void OnB2Timeout()
	{
		Stats.Instance.UpdateNumber(1);
		
	}

	private void OnButtonUpgrade(int stage){
		//TODO: Change Value of add amt based on which upgrade
	}

	
}
