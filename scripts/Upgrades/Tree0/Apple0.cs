using Godot;
using System;

public partial class Apple0 : TextureRect
{
	private int amt = 1;
    private float expire_loss_ratio = .4f;
	private Timer expire_timer;
    public void data(int _amt){
        amt = _amt;
    }
    public override void _Ready()
    {
        expire_timer = GetNode<Timer>("ExpireTimer");
        expire_timer.Timeout += OnTimeout;
    }
	public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            
            Stats.Instance.UpdateNumber(amt);
            AcceptEvent();
			QueueFree();
        }
	}
    public void OnTimeout()
    {
        int expire_amt = (int)(amt*expire_loss_ratio);
        Stats.Instance.UpdateNumber(expire_amt);
        QueueFree();
    }
    

	
}
