using System.Diagnostics;
using System.Text;
using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using Microsoft.Extensions.Options;

namespace ChessEngineInterfacePOC.Services;

public class EngineService : IEngineService
{
    private Process _engineProcess;

    private readonly IOptions<EngineConfig> _options;

    public EngineService(IOptions<EngineConfig> options)
    {
        _options = options;
    }

    public async Task<string> CalculatePosition(string position, int time, char color)
    {
        _engineProcess = new Process();
        _engineProcess.StartInfo.UseShellExecute = false;
        _engineProcess.StartInfo.RedirectStandardInput = true;
        _engineProcess.StartInfo.RedirectStandardOutput = true;
        _engineProcess.StartInfo.FileName = _options.Value.Path;
        _engineProcess.StartInfo.CreateNoWindow = false;
        _engineProcess.Start();
        await using (var writer = _engineProcess.StandardInput)
        {
            await writer.WriteAsync("uci");
            await writer.WriteLineAsync($"position {position}");
            await writer.WriteLineAsync($"go wtime {time}");
        }
        
        var response = "";
        using (var reader = _engineProcess.StandardOutput)
        {
            var output = await reader.ReadToEndAsync();
            response = output.Split("bestmove ")[1];
        }

        await _engineProcess.WaitForExitAsync();
        
        return response;
    }
}