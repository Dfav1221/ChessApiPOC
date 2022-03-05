using System.Diagnostics;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using Microsoft.Extensions.Options;

namespace ChessEngineInterfacePOC.Services;

public class EngineService : IEngineService
{
    private readonly IStockfishService _stockfish;


    public EngineService(IStockfishService stockfish)
    {
        _stockfish = stockfish;
        _stockfish.Start();
    }

    public async Task<string> CalculatePosition(string position, int time, string color)
    {
        await _stockfish.SetPosition(position);
        await _stockfish.Go(time,color);
        return await _stockfish.BestMove();
    }
}