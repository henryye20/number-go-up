using Godot;
using System;

public partial class Apple0 : TextureRect
{
	private int amt = 1;
	
    public void data(int _amt){
        amt = _amt;
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

    

	
}
