using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Model.DataBaseModel;
using Model.XmlModel;
using Attribute = Model.XmlModel.Attribute;

namespace Client
{
    public partial class MainForm : Form
    {
        private TestClient testClient;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(TestClient testClient) : this()
        {
            this.testClient = testClient;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var request = new Request {Function = Functions.GetGoodsList};

            var result = await testClient.SendRequest(request);
            var goods = result.Data.Nested.Data.Select(x => new Goods()
            {
                Id = Convert.ToInt32(x.Input.Single(o => o.Key == nameof(Goods.Id)).Value),
                Name = x.Input.Single(o => o.Key == nameof(Goods.Name)).Value,
                Description = x.Input.Single(o => o.Key == nameof(Goods.Description)).Value,
                Price = Convert.ToDecimal(x.Input.Single(o => o.Key == nameof(Goods.Price)).Value, CultureInfo.InvariantCulture)
            }).ToList();
            dataGridView1.DataSource = goods;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedRows = dataGridView1.SelectedRows;
        }

        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            var request = new Request
            {
                Function = Functions.GetOrderList,
                Attribute = new List<Attribute>()
                {
                    new Attribute()
                    {
                        Value = "clientId"
                    }
                }
            };

            var result = await testClient.SendRequest(request);
            var orders = result.Data.Nested.Data.GroupBy(x => new Order
            {
                OrderNumber = Convert.ToInt32(x.Input.Single(o => o.Key == nameof(Order.OrderNumber)).Value),
                OrderDate = Convert.ToDateTime(x.Input.Single(o => o.Key == nameof(Order.OrderDate)).Value),
                State = Convert.ToInt32(x.Input.Single(o => o.Key == nameof(Order.State)).Value),
            }).ToList();
            dataGridView2.DataSource = orders;
        }
    }

    internal class OrderItem
    {
    }
}