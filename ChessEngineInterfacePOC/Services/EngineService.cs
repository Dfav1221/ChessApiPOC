using System.Diagnostics;
using System.Text;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using CliWrap;
using CliWrap.EventStream;
using Microsoft.Extensions.Options;
using Stockfish = Stockfish.NET.Stockfish;


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

    public string CalculatePosition(string position, int time, char color)
    {
        Writer.Write($"position {position}\r\ngo wtime {time}");
        Writer.Flush();
        _engineProcess.WaitForExit(100);
        var output = "";
        while (true)
        {
            var lineAsList = Reader.ReadLine().Split(' ');
            if (lineAsList[0] != "bestmove") continue;
            return lineAsList[1];
        }
    }
}