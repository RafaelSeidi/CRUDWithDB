using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AlterandoTabelas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ClassDao dao = new ClassDao();

        void LimparCampos()
        {
            txtNome.Clear();
            txtEndereco.Clear();
            txtSalario.Clear();
            lblNumCli.Text = dao.NumRegistro().ToString(); // insere no label o número do próximo registro a cadastrar
        }

        void ExibirDados()
        {
            lblNumCli.Text = dao.campos.id.ToString();
            txtNome.Text = dao.campos.nome;
            txtEndereco.Text = dao.campos.ender;
            txtSalario.Text = dao.campos.sal.ToString();
            btnAlterar.Enabled = true;
            btnDeletar.Enabled = true;
            btnSalvar.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // estabelecer conexão com o BD
            dao.Conecte("Teste", "tabelateste");
            dao.PreencheTabela(dataGridView1);
            LimparCampos();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            //inserir
            if (txtNome.Text=="" || txtEndereco.Text=="" || txtSalario.Text == "")
            {
                MessageBox.Show("Campos em branco", "AVISO");
            }
            else
            {
                dao.Insere(txtNome.Text, txtEndereco.Text, decimal.Parse(txtSalario.Text));
                MessageBox.Show("Registro gravado com sucesso", "Informação do Sistema");
                LimparCampos();
                dao.PreencheTabela(dataGridView1);
                btnSalvar.Enabled = false;
            }
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            btnSalvar.Enabled = true;
            btnAlterar.Enabled = false;
            btnDeletar.Enabled = false;
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            //consultar por nome
            if (txtNome.Text != "")
            {
                dao.Consulta(txtNome.Text);
                ExibirDados();
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int numLinha = e.RowIndex; // retorna o número da linha selecionada

            if (numLinha >= 0)
            {
                int idCliente = int.Parse(dataGridView1.Rows[numLinha].Cells[0].Value.ToString());
                dao.Consulta(idCliente);
                ExibirDados();
            }
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            // alterar
            dao.Atualiza(txtNome.Text, txtEndereco.Text, decimal.Parse(txtSalario.Text),
                int.Parse(lblNumCli.Text));

            dao.PreencheTabela(dataGridView1);
            MessageBox.Show("Registro alterado com sucesso", "AVISO");
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            // deletar
            if (MessageBox.Show("Deseja mesmo excluir esse registro?", "AVISO DE EXCLUSÃO!!!",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dao.Deleta(int.Parse(lblNumCli.Text));
                MessageBox.Show("Registro excluído com sucesso");
                dao.PreencheTabela(dataGridView1);
                LimparCampos();
                btnAlterar.Enabled = false;
                btnDeletar.Enabled = false;
            }
            else
            {
                MessageBox.Show("Registro mantido");
            }
        }

        private void BtnConsultar_Click_1(object sender, EventArgs e)
        {

        }
    }
}
