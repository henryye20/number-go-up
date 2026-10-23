using Godot;
using Godot.Collections;
using System;

public partial class ButtonManager : Control
{
	private Button _mainbutton;

	private int addamt;

	private Timer B2time;


	
	public override void _Ready()
	{

		_mainbutton = GetNode<Button>("MainButton");
		_mainbutton.Pressed += OnMainPressed;
		addamt = 500;

	}

	private void OnMainPressed()
	{
		Stats.Instance.UpdateNumber(addamt);
		
	}
	
	

	


	
}
