using System.Diagnostics;
namespace Banorte.VerificarFacturas
{
    partial class Servicio
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    components = new System.ComponentModel.Container();
        //    this.ServiceName = "BanorteVerificarFacturasSAT";
        //}


        /////// <summary> 
        /////// Required method for Designer support - do not modify 
        /////// the contents of this method with the code editor.
        /////// </summary>
        private void InitializeComponent()
        {
            this.eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            // 
            // eventLog
            // 
            this.eventLog.Log = "BanorteVerificarFacturaSATLog";
            this.eventLog.Source = "BanorteVerificarFacturaSAT";
            this.eventLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog_EntryWritten);
            // 
            // Servicio
            // 
            this.ServiceName = "BanorteVerificarFacturasSAT";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

        }




        #endregion
    }
}
