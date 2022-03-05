using System.Diagnostics;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using Microsoft.Extensions.Options;

namespace ChessEngineInterfacePOC.Services;

public class StockfishService : IStockfishService
{
    private readonly Process _process;
    private readonly IOptions<EngineConfig> _options;
    private readonly StreamWriter _writer;
    private readonly StreamReader _reader;

    public StockfishService(IOptions<EngineConfig> options)
    {
        _options = options;
        _process = CreateProcess();
        _writer = _process.StandardInput;
        _reader = _process.StandardOutput;
    }

    private Process CreateProcess()
    {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            FileName = _options.Value.Path,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;


        return process;
    }

    private async Task<string[]?> ReadLineAsList() => (await _reader.ReadLineAsync())?.Split(' ');

    public bool Start() => _process.Start();

    public async Task<bool> SetPosition(string fen)
    {
        try
        {
            await _writer.WriteLineAsync($"position {fen}");
            await _writer.FlushAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Go(int time, string color)
    {
        try
        {
            await _writer.WriteLineAsync($"go {color}time {time}");
            await _writer.FlushAsync();
            _process.WaitForExit(time);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> BestMove()
    {
        while (true)
        {
            var line = await ReadLineAsList();
            if (line?[0] == "bestmove")
                return line[1];
        }
    }
}