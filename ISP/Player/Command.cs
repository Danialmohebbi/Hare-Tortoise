namespace ConsoleApp2.Player;

public interface Command
{
    public GameState GameState { get; set; }
    public void Execute(PlayerBase player);
}