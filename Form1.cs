using System;
using System.ServiceProcess;
using System.Windows.Forms;
using System.IO;

namespace AppOrgXml
{
    public partial class Form1 : Form
    {
        private Timer timer;

        private DateTime _ultimaAtualizacaoLog = DateTime.MinValue;
        private long _tamanhoUltimoLog = 0;

        private readonly string caminhoLog = @"C:\ORGXML\OrgXmlService\logs\log.txt";

        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 1000; // 1 segundo
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var sc = new ServiceController("OrgXmlService"))
                {
                    sc.Refresh();
                    lblStatus.Text = $"Status do Serviço: {sc.Status}";
                }
            }
            catch
            {
                lblStatus.Text = "Serviço não instalado.";
            }
        }

        private void btn_iniciar_click(object sender, EventArgs e)
        {
            try
            {
                using (var serviceController = new ServiceController("OrgXmlService"))
                {
                    serviceController.Refresh();

                    if (serviceController.Status == ServiceControllerStatus.Stopped ||
                        serviceController.Status == ServiceControllerStatus.Paused)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                        MessageBox.Show("Serviço iniciado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("O serviço já está em execução ou não pode ser iniciado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (System.TimeoutException)
            {
                MessageBox.Show("Tempo esgotado ao aguardar o serviço iniciar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o serviço: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_parar_click(object sender, EventArgs e)
        {
            try
            {
                using (var serviceController = new ServiceController("OrgXmlService"))
                {
                    serviceController.Refresh();

                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                        MessageBox.Show("Serviço parado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("O serviço não está em execução ou não pode ser parado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (System.TimeoutException)
            {
                MessageBox.Show("Tempo esgotado ao aguardar o serviço parar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao parar o serviço: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_reiniciar_click(object sender, EventArgs e)
        {
            try
            {
                using (var serviceController = new ServiceController("OrgXmlService"))
                {
                    serviceController.Refresh();

                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                    }

                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                    MessageBox.Show("Serviço iniciado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.TimeoutException)
            {
                MessageBox.Show("Tempo esgotado ao reiniciar o serviço.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao reiniciar o serviço: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void logBox_Load(object sender, EventArgs e)
        {
            try
            {
                MonitoraLog(caminhoLog);
                CarregarLog(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, Path.GetDirectoryName(caminhoLog), Path.GetFileName(caminhoLog)));


                using (var serviceController = new ServiceController("OrgXmlService"))
                {
                    serviceController.Refresh();
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        timer.Interval = 500;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o log: {ex}");
            }
        }

        private static FileSystemWatcher logWatcher;

        public void MonitoraLog(string caminhoLog = @"C:\ORGXML\OrgXmlService\logs\log.txt")
        {
            if (logWatcher != null)
            {
                logWatcher.EnableRaisingEvents = false;
                logWatcher.Changed -= CarregarLog;
                logWatcher.Created -= CarregarLog;
                logWatcher.Dispose();
                logWatcher = null;
            }

            logWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(caminhoLog),
                Filter = Path.GetFileName(caminhoLog),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                SynchronizingObject = this
            };

            logWatcher.Changed += CarregarLog;
            logWatcher.Created += CarregarLog;
            logWatcher.EnableRaisingEvents = true;
        }

        private void CarregarLog(object source, FileSystemEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new FileSystemEventHandler(CarregarLog), source, e);
                    return;
                }

                if (!File.Exists(caminhoLog))
                {
                    logBox.Text = "Arquivo de log não encontrado.";
                    _ultimaAtualizacaoLog = DateTime.MinValue;
                    _tamanhoUltimoLog = -1;
                    return;
                }

                var infoArquivo = new FileInfo(caminhoLog);

                if (infoArquivo.LastWriteTime == _ultimaAtualizacaoLog && infoArquivo.Length == _tamanhoUltimoLog)
                    return;

                using (var fs = new FileStream(caminhoLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    string conteudo = sr.ReadToEnd();
                    logBox.Text = conteudo;
                    logBox.SelectionStart = logBox.Text.Length;
                    logBox.ScrollBars = ScrollBars.Vertical;
                    logBox.ScrollToCaret();
                }

                _ultimaAtualizacaoLog = infoArquivo.LastWriteTime;
                _tamanhoUltimoLog = infoArquivo.Length;

            }
            catch (Exception ex)
            {
                logBox.Text = $"Erro ao carregar o log: {ex.Message}";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (logWatcher != null)
            {
                logWatcher.EnableRaisingEvents = false;
                logWatcher.Changed -= CarregarLog;
                logWatcher.Created -= CarregarLog;
                logWatcher.Dispose();
                logWatcher = null;
            }
            base.OnFormClosed(e);
        }


    }
}