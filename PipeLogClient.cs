using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppOrgXml
{
    public class PipeLogEntry
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public bool IsRaw { get; set; }
        public string RawLine { get; set; }
        public string ParseError { get; set; }
    }

    public class PipeLogClient : IDisposable
    {
        private const string PipeName = "OrgXmlLogPipe";
        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private CancellationTokenSource _cts;
        private Task _loopTask;

        public event Action<PipeLogEntry> LogReceived;

        public void Start()
        {
            if (_loopTask != null && !_loopTask.IsCompleted) return;
            _cts = new CancellationTokenSource();
            _loopTask = Task.Run(() => LoopAsync(_cts.Token));
        }

        private async Task LoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using (var client = new NamedPipeClientStream(".", PipeName, PipeDirection.In, PipeOptions.Asynchronous))
                    {
                        try
                        {
                            // Connect (bloqueante) com timeout
                            client.Connect(10000);
                        }
                        catch (TimeoutException)
                        {
                            EmitRawInfo("Timeout ao conectar. Re-tentando em 1s.", "INFO");
                            await Task.Delay(1000, ct);
                            continue;
                        }
                        catch (Exception exConn)
                        {
                            EmitRawInfo("Falha ao conectar: " + exConn.GetType().FullName + " HR=0x" +
                                        (exConn.HResult & 0xFFFFFFFF).ToString("X8") + " Msg=" + exConn.Message, "ERROR");
                            await Task.Delay(2000, ct);
                            continue;
                        }

                        EmitRawInfo("Conectado ao pipe.", "INFO");

                        using (var reader = new StreamReader(client, Encoding.UTF8, false, 4096, true))
                        {
                            while (!ct.IsCancellationRequested && client.IsConnected)
                            {
                                string line = null;
                                try
                                {
                                    line = await reader.ReadLineAsync();
                                }
                                catch (IOException ioEx)
                                {
                                    // Diferenciar erros conhecidos
                                    int code = ioEx.HResult & 0xFFFF;
                                    if (code == 109) // ERROR_BROKEN_PIPE
                                    {
                                        EmitRawInfo("Pipe quebrado (server fechou).", "WARN");
                                    }
                                    else
                                    {
                                        EmitRawInfo("IOException lendo pipe (code " + code + "): " + ioEx.Message, "ERROR");
                                    }
                                    break;
                                }
                                catch (ObjectDisposedException)
                                {
                                    // Cancel / dispose simultâneo – tratar como saída limpa
                                    break;
                                }
                                catch (Exception exRead)
                                {
                                    EmitRawInfo("Erro inesperado lendo: " + exRead.GetType().FullName + " " + exRead.Message, "ERROR");
                                    break;
                                }

                                if (line == null) // EOF
                                    break;

                                try
                                {
                                    PipeLogEntry entry = ParseLine(line);
                                    LogReceived?.Invoke(entry);
                                }
                                catch (Exception exParse)
                                {
                                    EmitRawInfo("Falha inesperada no Parse (não JSON parse normal): " + exParse.Message, "ERROR");
                                }
                            }
                        }

                        EmitRawInfo("Desconectado. tentando novamente em 2s...", "WARN");
                    }

                    await Task.Delay(2000, ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // Log detalhado
                    EmitRawInfo("Erro no consumo do pipe: " + ex.GetType().FullName +
                                " HR=0x" + (ex.HResult & 0xFFFFFFFF).ToString("X8") +
                                " Msg=" + ex.Message, "ERROR");
                    try
                    {
                        await Task.Delay(3000, ct);
                    }
                    catch (OperationCanceledException) { break; }
                }
            }
        }

        private PipeLogEntry ParseLine(string line)
        {
            try
            {
                using (var doc = JsonDocument.Parse(line))
                {
                    var root = doc.RootElement;
                    var level = root.TryGetProperty("level", out var lvlEl) ? lvlEl.GetString() : "UNK";
                    DateTimeOffset ts = DateTimeOffset.Now;
                    if (root.TryGetProperty("timestamp", out var tsEl))
                    {
                        if (tsEl.ValueKind == JsonValueKind.String &&
                            DateTimeOffset.TryParse(tsEl.GetString(), out var parsed))
                            ts = parsed;
                        else if (tsEl.ValueKind == JsonValueKind.Number && tsEl.TryGetInt64(out var unix))
                            ts = DateTimeOffset.FromUnixTimeMilliseconds(unix);
                    }

                    var msg = root.TryGetProperty("message", out var msgEl) ? msgEl.GetString() : line;
                    return new PipeLogEntry
                    {
                        Level = level,
                        Message = msg,
                        Timestamp = ts,
                        IsRaw = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new PipeLogEntry
                {
                    IsRaw = true,
                    RawLine = line,
                    ParseError = ex.Message,
                    Level = "RAW",
                    Timestamp = DateTimeOffset.Now,
                    Message = line
                };
            }
        }

        private void EmitRawInfo(string msg, string level)
        {
            LogReceived?.Invoke(new PipeLogEntry
            {
                IsRaw = true,
                RawLine = msg,
                Level = level,
                Message = msg,
                Timestamp = DateTimeOffset.Now
            });
        }

        public void Stop()
        {
            try
            {
                _cts?.Cancel();
                _loopTask?.Wait(2000);
            }
            catch { }
        }

        public void Dispose()
        {
            Stop();
            _cts?.Dispose();
        }
    }
}