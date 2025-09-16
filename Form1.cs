using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace AppOrgXml
{
    public partial class Form1 : Form
    {
        private Timer timer;

        // Pipe
        private PipeLogClient _pipeLogClient;

        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (var serviceController = new ServiceController("OrgXmlService"))
                {
                    serviceController.Refresh();
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        timer.Interval = 500;
                    }
                }

                InicializarPipeLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarPipeLogs()
        {
            _pipeLogClient = new PipeLogClient();
            _pipeLogClient.LogReceived += PipeLogClient_LogReceived;
            _pipeLogClient.Start();
            AppendPipeLine($"[PIPE] Cliente de pipe inicializado. Aguardando logs...{Environment.NewLine}");
        }

        private void PipeLogClient_LogReceived(PipeLogEntry entry)
        {
            if (!IsHandleCreated) return;

            BeginInvoke((Action)(() =>
            {
                string line;
                if (entry.IsRaw && !string.IsNullOrEmpty(entry.ParseError))
                {
                    line = $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff} [PIPE/{entry.Level}] (raw, erro parse: {entry.ParseError}) {entry.Message}";
                }
                else
                {
                    line = $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff} [PIPE/{entry.Level}] {entry.Message}";
                }
                AppendPipeLine(line + Environment.NewLine);
            }));
        }

        private void AppendPipeLine(string text)
        {
            try
            {
                logBox.AppendText(text);
                logBox.SelectionStart = logBox.TextLength;
                logBox.ScrollToCaret();
            }
            catch { }
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_pipeLogClient != null)
            {
                _pipeLogClient.LogReceived -= PipeLogClient_LogReceived;
                _pipeLogClient.Stop();
                _pipeLogClient.Dispose();
                _pipeLogClient = null;
            }

            base.OnFormClosed(e);
        }
    }
}