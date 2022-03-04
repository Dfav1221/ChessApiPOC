using System.Diagnostics;
using System.Text;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using CliWrap;
using CliWrap.EventStream;
using Microsoft.Extensions.Options;

namespace ChessEngineInterfacePOC.Services;

public class EngineService : IEngineService
{
    private Process _engineProcess;

    private readonly IOptions<EngineConfig> _options;

    public EngineService(IOptions<EngineConfig> options)
    {
        _options = options;
        _engineProcess = new Process();
        _engineProcess.StartInfo.FileName = "C:\\Users\\JakubHirsz\\Desktop\\stockfish_14.1_win_x64_avx2\\ryba.exe";
        _engineProcess.StartInfo.RedirectStandardInput = true;
        _engineProcess.StartInfo.RedirectStandardOutput = true;
        _engineProcess.StartInfo.UseShellExecute = false;
        _engineProcess.StartInfo.CreateNoWindow = true;
    }

    public async Task<string> CalculatePosition(string position, int time, char color)
    {
        _engineProcess.Start();

        await using (var writer = _engineProcess.StandardInput)
        {
            await writer.WriteAsync($"position {position}\r\ngo wtime {time}");
        }

        var response = "";
        using (var reader = _engineProcess.StandardOutput)
        {
            var output = await reader.ReadToEndAsync();
            response = output
                .Split("bestmove ")[1]
                .Split('\r')[0];
        }


        return response;
    }
}