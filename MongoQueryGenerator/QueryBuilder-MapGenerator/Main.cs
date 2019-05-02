using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace QueryBuilder_MapGenerator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Button1_Click( object sender, EventArgs e )
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JSON Files|*.json";
            openFile.Title = "Load JSON data";

            if ( openFile.ShowDialog() == DialogResult.OK )
            {
                JObject data = JObject.Parse( File.ReadAllText( openFile.FileName ) );
                MessageBox.Show( data.GetValue( "name" ).ToString() );
            }
        }
    }
}
