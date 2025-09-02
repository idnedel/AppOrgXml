using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace AppOrgXml
{
    public partial class Form1 : Form
    {

        private Timer timer;
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
                ServiceController sc = new ServiceController("OrgXmlService");
                lblStatus.Text = ($"Status do Serviço: {sc.Status}");
            }
            catch (Exception)
            {
                lblStatus.Text = "Serviço não instalado.";
            }

        }


        private void btn_iniciar_click(object sender, EventArgs e)
        {       
            try
                 {
                        ServiceController serviceController = new ServiceController("OrgXmlService");

                            if (serviceController.Status == ServiceControllerStatus.Stopped ||
                                serviceController.Status == ServiceControllerStatus.Paused)
                            {
                                serviceController.Start();
                                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                                MessageBox.Show("Serviço iniciado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("O serviço já está em execução ou não pode ser iniciado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao iniciar o serviço: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
        }

        private void btn_parar_click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("OrgXmlService");

            if (serviceController.Status == ServiceControllerStatus.Running)
            {
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                MessageBox.Show("Serviço parado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("O serviço não está em execução ou não pode ser parado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_reiniciar_click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("OrgXmlService");

            if (serviceController.Status == ServiceControllerStatus.Running)
            {
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                MessageBox.Show("Serviço iniciado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
