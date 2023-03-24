using EjemploDivisa2.Model.Api;
using System;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;


namespace EjemploDivisa2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RespuestaMoneda respuestaMoneda;
        private RespuestaTasas respuestaTasas;
        private Dictionary<string, string> monedas;
        private Dictionary<string, double> tasas;
        public MainWindow()
        {
            InitializeComponent();
            GetRespuestaMoneda();
            GetRespuestaTasasConversion();
        }

        public async void GetRespuestaMoneda() 
        {
            respuestaMoneda = await DivisaServices.getMoneda();
            ConfiguracionMonedas();
        }
        public async void GetRespuestaTasasConversion() 
        { 
            respuestaTasas = await DivisaServices.getTasasConversion();
            ConfiguracionTasas();
        }

        private void ConfiguracionMonedas()
        {
            if (!respuestaMoneda.Error)
            {
                if (respuestaMoneda.Monedas != null)
                {
                    monedas = respuestaMoneda.Monedas;
                    this.cmb_destinno.ItemsSource = monedas;
                    this.cmb_origen.ItemsSource = monedas;
                }
                else
                {
                    MessageBox.Show(respuestaMoneda.Mensaje, "ADVERTENCIA respuestaMoneda.Monedas", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(respuestaMoneda.Mensaje, "ADVERTENCIA respuestaMoneda", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ConfiguracionTasas()
        {
            if (!respuestaTasas.Error)
            {
                if (respuestaTasas.Tasas != null && respuestaTasas.Tasas.Rates != null)
                {
                    tasas = respuestaTasas.Tasas.Rates;
                }
                else
                {
                    MessageBox.Show(respuestaTasas.Mensaje, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(respuestaTasas.Mensaje, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void NumeroValidacionTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void bt_Convertir2(object sender, RoutedEventArgs e)
        {
            Double importe;
            if (!string.IsNullOrEmpty(this.txt_importe2.Text))
            {
                importe = Double.Parse(this.txt_importe2.Text);
                RespuestaMoneda res = respuestaMoneda;
                if (!res.Error)
                {
                    string valorOrigen = ((KeyValuePair<string, string>)cmb_origen.SelectedValue).Key.ToString();
                    string valorDestino = ((KeyValuePair<string, string>)cmb_destinno.SelectedValue).Key.ToString();
                    if (tasas.TryGetValue(valorOrigen, out double origenMoneda) &&
                        tasas.TryGetValue(valorDestino, out double destinoMoneda))
                    {
                        double conversionResultado = origenMoneda / destinoMoneda;
                        lbl_tasa2.Content = string.Format("{0:#,##0.0000}", conversionResultado);
                        conversionResultado = importe / conversionResultado;
                        lbl_resultado2.Content = string.Format("{0:#,##0.00}", conversionResultado);
                        //-----------FECHA ACTUALIZACION-------------//
                        long tiempoactualizacion = respuestaTasas.Tasas.Timestamp;
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                        this.lbl_fecha2.Content = new DateTime(1970, 1, 1, 0, 0, 0,
                        DateTimeKind.Utc)
                        .AddSeconds(tiempoactualizacion).ToString("dd/MM/yyyy HH:mm:ss");
                    }else
                    {
                        MessageBox.Show(respuestaMoneda.Mensaje, "Error res.Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(respuestaMoneda.Mensaje, "Error IsNullOrEmpty", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Introduce un importe válido para hacer la conversión",
                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void bt_Convertir1(object sender, RoutedEventArgs e)
        {
            Double tasamxn = 0.0;
            Double importe;
            if (!string.IsNullOrEmpty(this.txt_importe.Text))
            {
                importe = Double.Parse(this.txt_importe.Text);
                RespuestaTasas res = respuestaTasas;
                if (!res.Error)
                {
                    if (res.Tasas != null && res.Tasas.Rates != null &&
                    res.Tasas.Rates.TryGetValue("MXN", out tasamxn))
                    {
                        //-----------RESULTADO CONVERSION------------//
                        if (rdb_mxn_usd.IsChecked == true)
                        {
                            //-----------MXN A USD-------------------//
                            this.lbl_resultado.Content = String.Format("{0:#,##0.00}",
                            importe / tasamxn);
                        }
                        else
                        {
                            //-----------USD A MXN-------------------//
                            this.lbl_resultado.Content = String.Format("{0:#,##0.00}",
                            importe * tasamxn);
                        }
                        //-----------FECHA ACTUALIZACION-------------//
                        long tiempoactualizacion = res.Tasas.Timestamp;
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                        this.lbl_fecha.Content = new DateTime(1970, 1, 1, 0, 0, 0,
                        DateTimeKind.Utc)
                        .AddSeconds(tiempoactualizacion).ToString("dd/MM/yyyy HH:mm:ss");
                        //-------------TASA CONVERSION---------------//
                        this.lbl_tasa.Content = String.Format("{0:#,##0.0000}", tasamxn);
                    }
                    else
                    {
                        MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Introduce un importe válido para hacer la conversión",
                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txt_importe_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

