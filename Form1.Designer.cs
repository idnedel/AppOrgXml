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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_iniciar = new System.Windows.Forms.Button();
            this.btn_parar = new System.Windows.Forms.Button();
            this.btn_reiniciar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_iniciar
            // 
            this.btn_iniciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_iniciar.Location = new System.Drawing.Point(12, 41);
            this.btn_iniciar.Name = "btn_iniciar";
            this.btn_iniciar.Size = new System.Drawing.Size(291, 25);
            this.btn_iniciar.TabIndex = 0;
            this.btn_iniciar.Text = "Iniciar";
            this.btn_iniciar.UseVisualStyleBackColor = true;
            this.btn_iniciar.Click += new System.EventHandler(this.btn_iniciar_click);
            // 
            // btn_parar
            // 
            this.btn_parar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_parar.Location = new System.Drawing.Point(12, 72);
            this.btn_parar.Name = "btn_parar";
            this.btn_parar.Size = new System.Drawing.Size(291, 25);
            this.btn_parar.TabIndex = 1;
            this.btn_parar.Text = "Parar";
            this.btn_parar.UseVisualStyleBackColor = true;
            this.btn_parar.Click += new System.EventHandler(this.btn_parar_click);
            // 
            // btn_reiniciar
            // 
            this.btn_reiniciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_reiniciar.Location = new System.Drawing.Point(12, 103);
            this.btn_reiniciar.Name = "btn_reiniciar";
            this.btn_reiniciar.Size = new System.Drawing.Size(291, 25);
            this.btn_reiniciar.TabIndex = 2;
            this.btn_reiniciar.Text = "Reiniciar";
            this.btn_reiniciar.UseVisualStyleBackColor = true;
            this.btn_reiniciar.Click += new System.EventHandler(this.btn_reiniciar_click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ações do serviço OrgXml";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus.Location = new System.Drawing.Point(15, 143);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(66, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status Atual:";
            // 
            // Form1
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(315, 177);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_reiniciar);
            this.Controls.Add(this.btn_parar);
            this.Controls.Add(this.btn_iniciar);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(331, 216);
            this.MinimumSize = new System.Drawing.Size(331, 216);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Center - Service OrgXml";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_iniciar;
        private System.Windows.Forms.Button btn_parar;
        private System.Windows.Forms.Button btn_reiniciar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
    }

    

}

    

