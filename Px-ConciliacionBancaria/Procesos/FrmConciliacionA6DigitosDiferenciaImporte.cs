using Px_ConciliacionBancaria.Utiles.Formas;
using Px_Utiles.Models.Api;
using Px_Utiles.Servicio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Px_ConciliacionBancaria.Utiles.Emun.Enumerados;

using Px_ConciliacionBancaria.Utiles.Generales;
using Px_Controles.Colors;
using Px_Controles.Forms.Msg;
using System.Data.SqlClient;
using Px_Utiles.Models.Sistemas.ConciliacionBancaria.Catalogos;
using Px_Utiles.Models.Sistemas.Contabilidad.Catalogos;
using System.Diagnostics.Eventing.Reader;
using Px_Utiles.Models.Sistemas.CreditosFiscales.Catalogos;
using Px_ConciliacionBancaria.Utiles.Win32;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace PX_ConciliacionBancaria
{
    public partial class FrmConciliacionA6DigitosDiferenciaImporte : FormaGenBar
    {
        private bool ib_complete = true, ib_coment = false;
        private int ii_interval, Ii_Banco, Ii_Cuenta;
        private string Is_Moneda, Is_Coment;
        private DateTime Idt_Fecha;
        private DateTime Id_VigenciaAux, Id_VigenciaBco;
        private decimal Id_SaldoIniAux, Id_SaldoIniBco, Id_SaldoFinAux, Id_SaldoFinBco;
        private decimal Id_SaldoCon, Id_AbonoAux, Id_AbonoBco, Id_CargoAux, Id_CargoBco, Id_Tolerancia, Id_Ajuste;
        private long Il_Inicio, Il_Conciliacion, Il_TotAbonoAux, Il_TotAbonoBco, Il_TotCargoAux, Il_TotCargoBco, Il_TotAjuste, Il_RezagoAux, Il_RezagoBco;

        eRequest oReq = new eRequest();
        DataGridView oGrid = new DataGridView();


        public FrmConciliacionA6DigitosDiferenciaImporte()
        {
            InitializeComponent();
            Start();
        }

      
        private async Task Start()
        {
           _Titulo = "Conciliación Automática Por Similitud de Referencia";

            oReq.Base = Generales._AppState.Base;
            oReq.EndPoint = Generales._AppState.EndPoint;

            await StartForm();
            //await LoadMainGrid();

        }

        private void comboBanco_SelectedValueChanged(object sender, EventArgs e)
        {
            string IdBanco = comboBanco.SelectedValue.ToString();

            var algo = "";
            cargaComboCuenta(IdBanco);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async Task StartForm()
        {

            
            this.Text = string.Empty;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(800, 450);

            this.BackColor = Color.White;

            oReq.Base = Generales._AppState.Base;
            oReq.EndPoint = Generales._AppState.EndPoint;

            if (_Main != null)
                await _Main.Status($"{_Titulo}", (int)MensajeTipo.Info);

            await cargaComboBanco();
            await cargaComboMoneda();
            txtAnio.Text = DateTime.Now.Year.ToString();


        }


        #region "Carga Combo Banco"
        private async Task cargaComboBanco()
        {

            try
            {
                oReq.Tipo = 0;
                oReq.Query = "SELECT BANCO IDBanco, LPAD(BANCO, 2, '0') BANCO, DESCR FROM BANCO ORDER BY BANCO";
                var oRes = await WSServicio.Servicio(oReq);

                if (oRes?.Data?.Tables[0] != null)
                {
                    var dataTable = oRes.Data.Tables[0];

                    // Crear una lista para los items del ComboBox
                    var comboList = new List<ComboBoxItem>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var item = new ComboBoxItem
                        {
                            Value = row["IDBanco"].ToString(),
                            Text = "(" + row["BANCO"].ToString() + ") " + row["DESCR"].ToString()
                        };
                        comboList.Add(item);
                    }
                    // Asignar la lista al ComboBox
                    comboBanco.DataSource = comboList;
                    comboBanco.DisplayMember = "Text";
                    comboBanco.ValueMember = "Value";

                }

            }
            catch (Exception ex)
            { }

        }
        #endregion


        #region "Carga Combo Cuenta"
        private async Task cargaComboCuenta(string IdBanco)
        {

            try
            {
                oReq.Tipo = 0;
                oReq.Query = "SELECT CUENTA IDCUENTA, LPAD(CUENTA, 3, '0') CUENTA, DESCR FROM CUENTA WHERE BANCO = " + IdBanco + " ORDER BY CUENTA asc";
                var oRes = await WSServicio.Servicio(oReq);

                if (oRes?.Data?.Tables[0] != null)
                {
                    var dataTable = oRes.Data.Tables[0];

                    // Crear una lista para los items del ComboBox
                    var comboList = new List<ComboBoxItem>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var item = new ComboBoxItem
                        {
                            Value = row["IDCUENTA"].ToString(),
                            Text = "(" + row["CUENTA"].ToString() + ") " + row["DESCR"].ToString()
                        };
                        comboList.Add(item);
                    }
                    // Asignar la lista al ComboBox
                    comboCuenta.DataSource = comboList;
                    comboCuenta.DisplayMember = "Text";
                    comboCuenta.ValueMember = "Value";
                    comboCuenta.SelectedIndex = -1;

                }

            }
            catch (Exception ex)
            { }

        }
        #endregion


        #region "Carga Combo Moneda"
        private async Task cargaComboMoneda()
        {

            var comboList = new List<ComboBoxItem>();

            var item1 = new ComboBoxItem
            {
                Value = "1",
                Text = "ENERO"
            };
            var item2 = new ComboBoxItem
            {
                Value = "2",
                Text = "FEBRERO"
            };
            var item3 = new ComboBoxItem
            {
                Value = "3",
                Text = "MARZO"
            };
            var item4 = new ComboBoxItem
            {
                Value = "4",
                Text = "ABRIL"
            };
            var item5 = new ComboBoxItem
            {
                Value = "5",
                Text = "MAYO"
            };
            var item6 = new ComboBoxItem
            {
                Value = "6",
                Text = "JUNIO"
            };
            var item7 = new ComboBoxItem
            {
                Value = "7",
                Text = "JULIO"
            };
            var item8 = new ComboBoxItem
            {
                Value = "8",
                Text = "AGOSTO"
            };
            var item9 = new ComboBoxItem
            {
                Value = "9",
                Text = "SEPTIEMBRE"
            };
            var item10 = new ComboBoxItem
            {
                Value = "10",
                Text = "OCTUBRE"
            };
            var item11 = new ComboBoxItem
            {
                Value = "11",
                Text = "NOVIEMBRE"
            };
            var item12 = new ComboBoxItem
            {
                Value = "12",
                Text = "DICIEMBRE"
            };
            comboList.Add(item1);
            comboList.Add(item2);
            comboList.Add(item3);
            comboList.Add(item4);
            comboList.Add(item5);
            comboList.Add(item6);
            comboList.Add(item7);
            comboList.Add(item8);
            comboList.Add(item9);
            comboList.Add(item10);
            comboList.Add(item11);
            comboList.Add(item12);

            // Asignar la lista al ComboBox
            comboMes.DataSource = comboList;
            comboMes.DisplayMember = "Text";
            comboMes.ValueMember = "Value";
            comboMes.SelectedIndex = -1;

        }
        #endregion

        private async void detConciliar(object sender, EventArgs e)
        {
            bool bExito = await detValida();

            if (!bExito)
                return;

            await _Main.Status($"¿Deseas hacer la Conciliación a 6 Dígitos con Diferencia de Importe ?", (int)MensajeTipo.Question);
            if (MessageBoxMX.ShowDialog(null, $"¿Deseas hacer la Conciliación a 6 Dígitos con Diferencia de Importe ?", "Precaución", (int)StatusColorsTypes.Question, true) == System.Windows.Forms.DialogResult.Cancel)
                return;

            string IdBanco = comboBanco.SelectedValue.ToString();
            string IdCuenta = comboCuenta.SelectedValue.ToString();


            var resultado = await ProcesarMovimientosConciliadosAsync(IdBanco, IdCuenta);

            if (resultado == 1)

                resultado = await ProcesarVigenciaAsync();

            if (resultado == 1)
            {
                resultado = await VerificarConciliacionAsync(IdBanco, IdCuenta);

                if (resultado == 1)
                
                MessageBoxMX.ShowDialog(null, "Proceso Autorizado con éxito", "Aviso", (int)StatusColorsTypes.Success, false);

            }

        }



        public async Task<int> ProcesarMovimientosConciliadosAsync(string Ii_Banco,string Ii_Cuenta)
        {
            try
            {
                // Variables locales
                string Ls_user = "UsuarioActual"; // Usuario actual
                DateTime Ld_FechaFin = Idt_Fecha.Date; // Fecha final
                DateTime Ld_FechaIniBco = FProFechaIniMes(Id_VigenciaBco); // Fecha inicial banco
                DateTime Ld_FechaIniAux = FProFechaIniMes(Id_VigenciaAux); // Fecha inicial auxiliar

                // Verificar si el checkbox está seleccionado
                if (checkConciliaSimilitud.Checked)
                {
                    // Configuración de la solicitud
                    var oReq = new eRequest
                    {
                        Tipo = 1, // Llamada al procedimiento almacenado
                        Query = $@"
                        BEGIN
                            DGI.P_CON_DIF_SREF6(
                                {Ii_Banco},
                                {Ii_Cuenta},
                                TO_DATE('{Ld_FechaIniAux:yyyy-MM-dd HH:mm:ss}', 'yyyy-MM-dd HH24:mi:ss'),
                                TO_DATE('{Ld_FechaIniBco:yyyy-MM-dd HH:mm:ss}', 'yyyy-MM-dd HH24:mi:ss'),
                                TO_DATE('{Ld_FechaFin:yyyy-MM-dd HH:mm:ss}', 'yyyy-MM-dd HH24:mi:ss'),
                                '{Ls_user}',
                                {"0"},
                                {Il_Conciliacion}
                            );
                        END;
                    "
                    };

                    // Llamar al servicio
                    var oRes = await WSServicio.Servicio(oReq);

                    // Validar respuesta del servicio
                    if (oRes.Err != 0)
                    {
                        MessageBoxMX.ShowDialog(
                            null,
                            $"ERROR EN P_CON_DIF_SREF6",
                            "Información",
                            (int)StatusColorsTypes.Danger,
                            false
                        );
                        return 0;
                    }
                }

                return 1; // Operación exitosa
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                MessageBoxMX.ShowDialog(
                    null,
                    $"ERROR EN EL PROCESO: {ex.Message}",
                    "Información",
                    (int)StatusColorsTypes.Danger,
                    false
                );
                return 0;
            }
        }


        public async Task<int> VerificarConciliacionAsync(string Ii_Banco, string Ii_Cuenta)
        {
            string Ls_status = string.Empty;

            try
            {
                // Configuración de la consulta
                var oReq = new eRequest
                {
                    Tipo = 0, // Consulta SELECT
                    Query = $@"
                    SELECT 
                        DGI.CONCILIACION.STATUS
                        FROM
                            DGI.CONCILIACION
                        WHERE
                            WHERE BANCO = '{Ii_Banco}' AND CUENTA = '{Ii_Cuenta}' "";
                    "
                };

                // Llamar al servicio
                var oRes = await WSServicio.Servicio(oReq);

                // Validar respuesta del servicio
                if (oRes == null || oRes.Data.Tables.Count < 1 || oRes.Data.Tables[0].Rows.Count < 1)
                {
                    throw new Exception("No se encontró el número de conciliación.");
                }

                // Extraer el estado de la conciliación
                Ls_status = oRes.Data.Tables[0].Rows[0]["STATUS"].ToString();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si ocurre algún problema
                //MessageBoxMX.ShowDialog(
                //    null,
                //    $"Error al buscar el número de conciliación: {ex.Message}",
                //    "Error",
                //    (int)StatusColorsTypes.Danger,
                //    true
                //);
                return 0; // Terminar el proceso
            }

            // Verificar el estado de la conciliación
            if (string.IsNullOrEmpty(Ls_status) || Ls_status != "T")
            {
                MessageBoxMX.ShowDialog(
                    null,
                    "La cuenta no ha sido conciliada a 5 dígitos.",
                    "Aviso",
                    (int)StatusColorsTypes.Warning,
                    true
                );
                return 0; // Terminar el proceso
            }

            return 1; // Conciliación válida
        }


        public async Task<int> ProcesarVigenciaAsync()
        {
            // Variables locales
            DateTime Ld_Fecha = Idt_Fecha.Date;
            DateTime Ldt_FechaAux = DateTime.MinValue;
            DateTime Ldt_FechaBco = DateTime.MinValue;
            decimal Ld_Tolerancia = 0;
            decimal Ld_ToleranciaDlls = 0;
            int Li_Res;

            try
            {
                // Configuración de la consulta

                oReq.Tipo = 0;
                    oReq.Query = $@"
                    SELECT 
                        ADD_MONTHS(TO_DATE('{Idt_Fecha:yyyy-MM-dd}', 'yyyy-MM-dd'), -DGI.PARAMETRO_CB.VIGENCIAAUX),
                            ADD_MONTHS(TO_DATE('{Idt_Fecha:yyyy-MM-dd}', 'yyyy-MM-dd'), -DGI.PARAMETRO_CB.VIGENCIABCO),
                    DGI.PARAMETRO_CB.TOLERANCIA,
                    DGI.PARAMETRO_CB.TOLERANCIADLLS
                        FROM DGI.PARAMETRO_CB";


                // Llamar al servicio
                var oRes = await WSServicio.Servicio(oReq);

                // Validar respuesta del servicio
                if (Object.ReferenceEquals(null, oRes.Data.Tables[0]) || oRes.Data.Tables.Count < 1)
                {
                    throw new Exception("No se pudieron obtener los parámetros de vigencia.");
                }

                // Extraer resultados
                var row = oRes.Data.Tables[0].Rows[0];
                Ldt_FechaAux = Convert.ToDateTime(row["ADD_MONTHS(TO_DATE('{Idt_Fecha:yyyy-MM-dd}', 'yyyy-MM-dd'), -DGI.PARAMETRO_CB.VIGENCIAAUX)"]);
                Ldt_FechaBco = Convert.ToDateTime(row["ADD_MONTHS(TO_DATE('{Idt_Fecha:yyyy-MM-dd}', 'yyyy-MM-dd'), -DGI.PARAMETRO_CB.VIGENCIABCO)"]);
                Ld_Tolerancia = Convert.ToDecimal(row["TOLERANCIA"]);
                Ld_ToleranciaDlls = Convert.ToDecimal(row["TOLERANCIADLLS"]);

                // Asignar tolerancia según la moneda
                decimal Id_tolerancia = Is_Moneda == "P" ? Ld_Tolerancia : Ld_ToleranciaDlls;
                if (Id_tolerancia == null) Id_tolerancia = 0;

                // Validar resultados
                if (Ldt_FechaAux == DateTime.MinValue || Ldt_FechaBco == DateTime.MinValue)
                {

                    if (MessageBoxMX.ShowDialog(null,
                        "Aviso",
                    "No hay registrados parámetros de vigencia. ¿Desea continuar sin considerar la fecha de vigencia y el parámetro de tolerancia?",
                    (int)StatusColorsTypes.Warning, true) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return 0;
                    }

                }
            }
            catch (Exception ex)
            {

                if (MessageBoxMX.ShowDialog(null,
                    $"Error al obtener los parámetros de vigencia. ¿Desea continuar sin considerar la fecha de vigencia y el parámetro de tolerancia?",
                    "Error",
                    (int)StatusColorsTypes.Question, true) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return 0;
                }

            }

            // Asignar vigencias si son nulas
            if (Ldt_FechaAux == DateTime.MinValue) Ldt_FechaAux = FProFechaIniMes(Ld_Fecha);
            if (Ldt_FechaBco == DateTime.MinValue) Ldt_FechaBco = FProFechaIniMes(Ld_Fecha);

            // Calcular y asignar vigencias
            DateTime Id_VigenciaAux = FProFechaIniMes(Ldt_FechaAux);
            DateTime Id_VigenciaBco = FProFechaIniMes(Ldt_FechaBco);

            return 1; // Proceso exitoso
        }

     

        /// <summary>
        /// Calcula el inicio del mes de una fecha dada.
        /// </summary>
        private DateTime FProFechaIniMes(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }


        #region "Validar datos"
        private async Task<bool> detValida()
        {
            bool bExito = true;
            string sErr = string.Empty;


            if (string.IsNullOrWhiteSpace(txtAnio.Text))
                sErr += "Ingrese el mes ";

            if (string.IsNullOrWhiteSpace(comboCuenta.Text))
                sErr += "Seleccione la cuenta. ";

            if (string.IsNullOrWhiteSpace(comboMes.Text))
                sErr += "Seleccione el mes. ";

            if (sErr.Length > 0)
            {
                bExito = false;
                await _Main.Status($"Favor de atender los siguientes mensajes: {sErr}", (int)MensajeTipo.Warning);
            }
            else
            {

                string anio = txtAnio.Text;
                string IdBanco = comboBanco.SelectedValue.ToString();
                string IdCuenta = comboCuenta.SelectedValue.ToString();
                string IdMes = comboMes.SelectedValue.ToString();

            }
            
            return bExito;
        }
        #endregion


        public class ComboBoxItem
        {
            public string Text { get; set; } // Texto que se muestra en el ComboBox
            public object Value { get; set; } // Valor asociado al item

            public override string ToString()
            {
                return Text; // Esto es lo que se muestra en el ComboBox
            }
        }


    }
}
