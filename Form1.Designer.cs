using System;
using System.ServiceProcess;

namespace AppOrgXml
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
#region Código gerado pelo Windows Form Designer

/// <summary>
/// Método necessário para suporte ao Designer - não modifique 
/// o conteúdo deste método com o editor de código.
/// </summary>
private void InitializeComponent()
        {
            this.btn_iniciar = new System.Windows.Forms.Button();
            this.btn_parar = new System.Windows.Forms.Button();
            this.btn_reiniciar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_iniciar
            // 
            this.btn_iniciar.Location = new System.Drawing.Point(12, 31);
            this.btn_iniciar.Name = "btn_iniciar";
            this.btn_iniciar.Size = new System.Drawing.Size(126, 25);
            this.btn_iniciar.TabIndex = 0;
            this.btn_iniciar.Text = "Iniciar";
            this.btn_iniciar.UseVisualStyleBackColor = true;
            this.btn_iniciar.Click += new System.EventHandler(this.btn_iniciar_click);
            // 
            // btn_parar
            // 
            this.btn_parar.Location = new System.Drawing.Point(12, 62);
            this.btn_parar.Name = "btn_parar";
            this.btn_parar.Size = new System.Drawing.Size(126, 25);
            this.btn_parar.TabIndex = 1;
            this.btn_parar.Text = "Parar";
            this.btn_parar.UseVisualStyleBackColor = true;
            this.btn_parar.Click += new System.EventHandler(this.btn_parar_click);
            // 
            // btn_reiniciar
            // 
            this.btn_reiniciar.Location = new System.Drawing.Point(12, 93);
            this.btn_reiniciar.Name = "btn_reiniciar";
            this.btn_reiniciar.Size = new System.Drawing.Size(126, 25);
            this.btn_reiniciar.TabIndex = 2;
            this.btn_reiniciar.Text = "Reiniciar";
            this.btn_reiniciar.UseVisualStyleBackColor = true;
            this.btn_reiniciar.Click += new System.EventHandler(this.btn_reiniciar_click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ações do serviço OrgXml";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 131);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status Atual";
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.LightGray;
            this.logBox.Location = new System.Drawing.Point(152, 9);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(507, 408);
            this.logBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(671, 429);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_reiniciar);
            this.Controls.Add(this.btn_parar);
            this.Controls.Add(this.btn_iniciar);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Name = "Form1";
            this.Text = "logBox";
            this.Load += new System.EventHandler(this.logBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_iniciar;
        private System.Windows.Forms.Button btn_parar;
        private System.Windows.Forms.Button btn_reiniciar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox logBox;
    }

    

}

    

