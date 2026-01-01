using ConsoleApp2.Squares;

namespace ConsoleApp2.View;

public class View
{
    public required Game g { get; set; }
    void DrawSquare(Square s)
    {
        Console.ForegroundColor = GetColor(s);
        Console.Write(s.ToString());
        Console.ResetColor();
    }
    public ConsoleColor GetColor(Square s)
    {
        return s switch
        {
            HareSquare => ConsoleColor.Magenta,
            CarrotSquare => ConsoleColor.DarkYellow,
            LettuceSquare => ConsoleColor.Green,
            TortoiseSquare => ConsoleColor.Cyan,
            FinalSquare => ConsoleColor.White,
            NumberSquare => ConsoleColor.Gray,
            _ => ConsoleColor.DarkGray
        };
    }
    public void Draw()
    { 
        for (int i = 2; i <= 8; i++)
            Console.Write(g.Board[i].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        for (int i = 58; i <= 62; i++)
            Console.Write(g.Board[i].ToString());
        Console.WriteLine();
        //SecondRow
        Console.Write(g.Board[1].ToString());
        for (int i = 0; i <= 24; i++)
            Console.Write(" ");
        Console.Write(g.Board[9].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        Console.WriteLine(g.Board[57].ToString());
        //Third Row
        
        Console.Write(g.Board[0].ToString());
        for (int i = 0; i <= 19; i++)
            Console.Write(" ");
        Console.Write(g.Board[11].ToString());
        Console.Write(g.Board[10].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        Console.Write(g.Board[56].ToString());
        Console.WriteLine(g.Board[55].ToString());
        //Fourth Row
        for (int i = 0; i <= 19; i++)
            Console.Write(" ");
        Console.Write(g.Board[13].ToString());
        Console.Write(g.Board[12].ToString());
        for (int _ = 0; _ <= 9; _++)
            Console.Write("     ");
        Console.Write(g.Board[54].ToString());
        Console.WriteLine(g.Board[53].ToString());
        //Fifth Row
        for (int i = 0; i <= 14; i++)
            Console.Write(" ");
        Console.Write(g.Board[15].ToString());
        Console.Write(g.Board[14].ToString());
        for (int _ = 0; _ <= 11; _++)
            Console.Write("     ");
        Console.Write(g.Board[52].ToString());
        Console.WriteLine(g.Board[51].ToString());

        //Sixth Row
        for (int i = 0; i <= 9; i++)
            Console.Write(" ");
        Console.Write(g.Board[17].ToString());
        Console.Write(g.Board[16].ToString());
        for (int _ = 0; _ <= 13; _++)
            Console.Write("     ");
        Console.Write(g.Board[49].ToString());
        Console.WriteLine(g.Board[48].ToString());
        //Seventh Row
        Console.Write(g.Board[20].ToString());
        Console.Write(g.Board[19].ToString());
        Console.Write(g.Board[18].ToString());
        for (int _ = 0; _ <= 15; _++)
            Console.Write("     ");
        Console.Write(g.Board[47].ToString());
        Console.Write(g.Board[46].ToString());
        Console.WriteLine(g.Board[45].ToString());
        
        //Eigth Row
        Console.Write(g.Board[21].ToString());
        for (int _ = 0; _ <= 19; _++)
            Console.Write("     ");
        Console.WriteLine(g.Board[44].ToString());
        
        //Ninth Row
        for (int i =22; i <= 43; i++)
            Console.Write(g.Board[i].ToString());
    }

    public void DrawPrefix()
    {
        Console.WriteLine($"Current Player: {g.Players[g.TurnIndex]} - Carrots: " +
                          $"{g.Players[g.TurnIndex].Carrots}" + 
                          $" - Lettuce: {g.Players[g.TurnIndex].Lettuce}");
    }

    public void DrawPostfix()
    {
        Console.WriteLine("---------------------------------------------------");
    }
    
    public static int AskHumanPlayers()
    {
        Console.WriteLine("How many human players? (1–4)");

        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int count) &&
                count >= 1 && count <= 4)
            {
                return count;
            }

            Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
        }
    }
}