using System;
using System.ServiceProcess;
using System.Windows.Forms;
using System.IO;

namespace AppOrgXml
{
    public partial class Form1 : Form
    {
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            
            //timer para status do serviço
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
    }
}