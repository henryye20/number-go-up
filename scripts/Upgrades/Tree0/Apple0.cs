using Godot;
using System;

public partial class Apple0 : TextureRect
{
	private int amt = 1;
	private float expire_loss_ratio = .4f;
	private Timer expire_timer;
	
	public PackedScene NumberPlusScene { get; set; }
	public void data(int _amt){
		amt = _amt;
	}
	public override void _Ready()
	{
		expire_timer = GetNode<Timer>("ExpireTimer");
		expire_timer.Timeout += OnTimeout;
		NumberPlusScene = GD.Load<PackedScene>("res://scenes/UI/NumberPlus.tscn");
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("select"))
		{
			
			CreateNumber();
			
			Stats.Instance.UpdateNumber(amt);
			AcceptEvent();
			QueueFree();
		}
	}
	public void OnTimeout()
	{
		int expire_amt = (int)(amt*expire_loss_ratio);

		CreateNumber();
		Stats.Instance.UpdateNumber(expire_amt);

		QueueFree();
	}
	
	private void CreateNumber()
	{
		var _NumberPlus = NumberPlusScene.Instantiate<NumberPlus>();
		
		GetTree().CurrentScene.AddChild(_NumberPlus);
		_NumberPlus.data(amt);
		_NumberPlus.GlobalPosition = GlobalPosition + (Size / 2) - (_NumberPlus.Size / 2);;
		_NumberPlus.Run();
	}
	
}
