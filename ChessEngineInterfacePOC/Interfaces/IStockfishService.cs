namespace ChessEngineInterfacePOC.Interfaces;

public interface IStockfishService
{
    public Task<bool> SetPosition(string fen);
    public Task<bool> Go(int time, string color);
    public Task<string> BestMove();
    public bool Start();
}