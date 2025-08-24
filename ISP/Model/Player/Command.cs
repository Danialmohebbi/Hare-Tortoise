namespace ConsoleApp2.Player;

public interface Command
{
    public Game State { get; set; }
    public void Execute(PlayerBase player);
}