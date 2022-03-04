using System.Diagnostics;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using Microsoft.Extensions.Options;

namespace ChessEngineInterfacePOC.Services;

public class EngineService : IEngineService
{
    private Process _engineProcess;

    private readonly IOptions<EngineConfig> _options;
    public StreamWriter Writer;
    public StreamReader Reader;

    public EngineService(IOptions<EngineConfig> options)
    {
        _options = options;
        _engineProcess = new Process();
        _engineProcess.StartInfo.FileName = "C:\\Users\\JakubHirsz\\Desktop\\stockfish_14.1_win_x64_avx2\\ryba.exe";
        _engineProcess.StartInfo.RedirectStandardInput = true;
        _engineProcess.StartInfo.RedirectStandardOutput = true;
        _engineProcess.StartInfo.UseShellExecute = false;
        _engineProcess.StartInfo.CreateNoWindow = true;
        _engineProcess.Start();
        Reader = _engineProcess.StandardOutput;
        Writer = _engineProcess.StandardInput;
    }

    public async Task<string> CalculatePosition(string position, int time, char color)
    {
        await Writer.WriteLineAsync($"position {position}");
        await Writer.WriteLineAsync($"go wtime {time}");
        await Writer.FlushAsync();
        _engineProcess.WaitForExit(100);
        while (true)
        {
            var lineAsList = (await Reader.ReadLineAsync()).Split(' ');
            if (lineAsList[0] != "bestmove") continue;
            return lineAsList[1];
        }
    }
}