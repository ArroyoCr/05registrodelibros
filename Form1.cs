using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05registrolibros
{
    public partial class Form1 : Form
    {
        static int contador;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblNumero.Text = generaNumero();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (valida() == "")
            {
                //capturando los datos el formulario
                double costo = getCosto();
                string categoria = getCategoria();
                double descuento = asignaDescuento(categoria, costo);
                double precioVenta = calculaPrecioVenta(costo, descuento);
                //enviando impresion
                imprimirRegistro(descuento, precioVenta);
                lblNumero.Text = generaNumero();
            }
            else
                MessageBox.Show("el error se encuentra en " + valida());
        }

        private void btnEstadisticas_Click(object sender, EventArgs e)
        {
            double totalDescuentos = calculaTotalDescuentos();
            string libroAlto = libroMasAlto();
            imprimirEstadisticas(totalDescuentos, libroAlto);
        }
        //implementacion de expresiones lambda
        Func<string> generaNumero = () =>
        {
            contador++;
            return contador.ToString("0000");
        };
        //metodo que captura los valores
        int getNumero()
        {
            return int.Parse(lblNumero.Text);
        }
        string getTitulo()
        {
            return txtTitulo.Text;
        }
        double getCosto()
        {
            return double.Parse(txtCosto.Text);
        }
        string getCategoria()
        {
            return cboCategoria.Text;
        }
        Func<string, double, double> asignaDescuento = (categoria, costo) =>
        {
            double descuento = 0;
            switch (categoria)
            {
                case "Gestion": descuento = 10.0 / 100 * costo; break;
                case "Ingenieria": descuento = 12.0 / 100 * costo; break;
                case "Programacion": descuento = 20.0 / 100 * costo; break;
                case "Base de datos": descuento = 15.0 / 100 * costo; break;
            }
            return descuento;
        };
        Func<double, double, double> calculaPrecioVenta = (costo, descuento) => costo - descuento;
        //calculando las estadisticas
        //monto total de descuentos
        double calculaTotalDescuentos()
        {
            double total = 0;
            for (int i = 0; i < lvLibros.Items.Count; i++)
            {
                total += double.Parse(lvLibros.Items[i].SubItems[4].Text);
            }
            return total;
        }
        //libro cn el precio total mas alto
        string libroMasAlto()
        {
            double mayor = double.Parse(lvLibros.Items[0].SubItems[5].Text);
            int posicion = 0;
            for (int i = 0; i < lvLibros.Items.Count; i++)
            {
                if (double.Parse(lvLibros.Items[i].SubItems[5].Text) > mayor)
                {
                    posicion = i;
                }
            }
            return lvLibros.Items[posicion].SubItems[1].Text;
        }
        //imprimir el registro de ventas
        void imprimirRegistro(double descuento, double precioVenta)
        {
            ListViewItem fila = new ListViewItem(getNumero().ToString());
            fila.SubItems.Add(getTitulo());
            fila.SubItems.Add(getCategoria());
            fila.SubItems.Add(getCosto().ToString("0.00"));
            fila.SubItems.Add(descuento.ToString("0.00"));
            fila.SubItems.Add(precioVenta.ToString("0.00"));
            lvLibros.Items.Add(fila);
        }
        //imprimir estadisticas
        void imprimirEstadisticas(double totalDescuentos, string LibroAlto)
        {
            lvEstadisticas.Items.Clear();
            string[] elementosFila = new string[2];
            ListViewItem row;
            elementosFila[0] = "Monto total acumulado de descuentos";
            elementosFila[1] = totalDescuentos.ToString("C");
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "El libro con el precio de ventas mas caro";
            elementosFila[1] = LibroAlto;
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);
        }
        //metodo de validacion de ingreso de datos
        string valida()
        {
            if (txtTitulo.Text.Trim().Length == 0)
            {
                txtTitulo.Focus();
                return "titulo del libro";
            }
            else if (cboCategoria.SelectedIndex == -1)
            {
                cboCategoria.Focus();
                return "categoria del libro";
            }
            else if (txtCosto.Text.Trim().Length == 0)
            {
                txtCosto.Focus();
                return "costo del libro";
            }
            return "";
        }
    }
}
