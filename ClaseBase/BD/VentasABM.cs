﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClaseBase.BD
{
    public class VentasABM
    {

        private DataTable tabla;

        public DataTable Tabla{
            get {
                return tabla;
            }
            set {
                tabla = value;
            }
        }

        private string total_ventas;

        public string Total_ventas{
            get {
                return total_ventas;
            }
            set {
                total_ventas = value;
            }
        }

        private string total_anulada;

        public string Total_anulada {
            get {
                return total_anulada;
            }
            set {
                total_anulada = value;
            }
        }

        private string importe_total_validada;

        public string Importe_total_validada {
            get {
                return importe_total_validada;
            }
            set {
                importe_total_validada = value;
            }
        }

        private string importe_total_anulada;

        public string Importe_total_anulada {
            get {
                return importe_total_anulada;
            }
            set {
                importe_total_anulada = value;
            }
        }

        public void buscarMarcaVenta( string sPattern ) {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);

            //crear el comando
            SqlCommand cmd = new SqlCommand("SPBuscarMarcaVenta", cnn);
            cmd.CommandType = CommandType.StoredProcedure;


            //anadir un parametro de entrada
            SqlParameter param;

            param = new SqlParameter("@mar", SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = sPattern;
            cmd.Parameters.Add(param);
            //anadir un parametro de salida
            param = new SqlParameter("@cuenta_ventas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@cuenta_anuladas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@importe_total", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@imT_anulada", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Tabla = dt;

            //abrir la conexion
            cnn.Open();
            //ejecutar el comando cmd q creamos para llamar al stored procedure
            cmd.ExecuteNonQuery();
            //cerrar la conexion
            cnn.Close();

            //leer el valor del parametro de salida y mostrarlo en el formulario
            Total_ventas = Convert.ToString(cmd.Parameters["@cuenta_ventas"].Value);
            Total_anulada = Convert.ToString(cmd.Parameters["@cuenta_anuladas"].Value);
            Importe_total_validada = Convert.ToString(cmd.Parameters["@importe_total"].Value);
            Importe_total_anulada = Convert.ToString(cmd.Parameters["@imT_anulada"].Value);
        }

        public static void insertVenta(Venta venta)
        {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertVenta";
            cmd.Parameters.Add("@idC", SqlDbType.Int).Value = venta.Cli_Id;
            cmd.Parameters.Add("@idV", SqlDbType.Int).Value = venta.Veh_Id;
            cmd.Parameters.Add("@idU", SqlDbType.Int).Value = venta.Usr_Id;
            cmd.Parameters.Add("@fec", SqlDbType.DateTime).Value = venta.Vta_Fecha;
            cmd.Parameters.Add("@idP", SqlDbType.Int).Value = venta.Pag_Id;
            cmd.Parameters.Add("@vpf", SqlDbType.Decimal).Value = venta.Vta_PrecioFinal;
            cmd.Parameters.Add("@vst", SqlDbType.VarChar).Value = venta.Vta_Estado;

            cmd.Connection = cnn;

            cnn.Open();

            cmd.ExecuteNonQuery();

            cnn.Close();


        }

        public static void set_estado_venta(int id, string estado) {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "anular_venta";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@idV", id);
            cmd.Parameters.AddWithValue("@estado", estado);


            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        public void list_venta()
        {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);

            //crear el comando
            SqlCommand cmd = new SqlCommand("SPListaVenta", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

           
            //anadir un parametro de entrada
            SqlParameter param;

            //anadir un parametro de salida
            param = new SqlParameter("@cuenta_ventas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@cuenta_anuladas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@importe_total", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@imT_anulada", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Tabla = dt;

            //abrir la conexion
            cnn.Open();
            //ejecutar el comando cmd q creamos para llamar al stored procedure
            cmd.ExecuteNonQuery();
            //cerrar la conexion
            cnn.Close();

            //leer el valor del parametro de salida y mostrarlo en el formulario
            Total_ventas = Convert.ToString(cmd.Parameters["@cuenta_ventas"].Value).Trim();
            Total_anulada = Convert.ToString(cmd.Parameters["@cuenta_anuladas"].Value).Trim();
            Importe_total_validada = Convert.ToString(cmd.Parameters["@importe_total"].Value).Trim();
            Importe_total_anulada = Convert.ToString(cmd.Parameters["@imT_anulada"].Value);

        }


        /////////////////////////////////AGREGADO 29/5////////////////////////////////////////


        public void list_Compras(string cli) {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);

            //crear el comando
            SqlCommand cmd = new SqlCommand("venta_cliente", cnn);
            cmd.CommandType = CommandType.StoredProcedure;


            //anadir un parametro de entrada
            SqlParameter param;

            param = new SqlParameter("@cli", SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = cli;
            cmd.Parameters.Add(param);

            //anadir un parametro de salida
            param = new SqlParameter("@cuenta_ventas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@cuenta_anuladas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@importe_total", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@imT_anulada", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Tabla = dt;

            //abrir la conexion
            cnn.Open();
            //ejecutar el comando cmd q creamos para llamar al stored procedure
            cmd.ExecuteNonQuery();
            //cerrar la conexion
            cnn.Close();

            //leer el valor del parametro de salida y mostrarlo en el formulario
            Total_ventas = Convert.ToString(cmd.Parameters["@cuenta_ventas"].Value);
            Total_anulada = Convert.ToString(cmd.Parameters["@cuenta_anuladas"].Value);
            Importe_total_validada = Convert.ToString(cmd.Parameters["@importe_total"].Value);
            Importe_total_anulada = Convert.ToString(cmd.Parameters["@imT_anulada"].Value);
        }

        public void list_Fecha(DateTime inicio, DateTime final) {
            SqlConnection cnn = new SqlConnection(ClaseBase.Properties.Settings.Default.Conexion);

            //crear el comando
            SqlCommand cmd = new SqlCommand("venta_Fechas", cnn);
            cmd.CommandType = CommandType.StoredProcedure;


            //anadir un parametro de entrada
            SqlParameter param;
            param = new SqlParameter("@primeraFecha", SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = inicio;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@segundaFecha", SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = final;
            cmd.Parameters.Add(param);

            //anadir un parametro de salida
            param = new SqlParameter("@cuenta_ventas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@cuenta_anuladas", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@importe_total", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@imT_anulada", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Tabla = dt;

            //abrir la conexion
            cnn.Open();
            //ejecutar el comando cmd q creamos para llamar al stored procedure
            cmd.ExecuteNonQuery();
            //cerrar la conexion
            cnn.Close();

            //leer el valor del parametro de salida y mostrarlo en el formulario
            Total_ventas = Convert.ToString(cmd.Parameters["@cuenta_ventas"].Value);
            Total_anulada = Convert.ToString(cmd.Parameters["@cuenta_anuladas"].Value);
            Importe_total_validada = Convert.ToString(cmd.Parameters["@importe_total"].Value);
            Importe_total_anulada = Convert.ToString(cmd.Parameters["@imT_anulada"].Value);
        }


        ///////////////////////////////////////////////////////////////////////////////////////
    }
}
