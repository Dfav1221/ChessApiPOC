namespace ChessEngineInterfacePOC.Interfaces;

public interface IEngineService
{
    public Task<string> CalculatePosition(string position, int time, char color);
}