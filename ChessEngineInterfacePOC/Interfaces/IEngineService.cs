namespace ChessEngineInterfacePOC.Interfaces;

public interface IEngineService
{
    public string CalculatePosition(string position, int time, char color);
}