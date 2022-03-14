using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AlterandoTabelas
{
    public class Campos
    {
        public int id;
        public string nome;
        public string ender;
        public decimal sal;
    }
    public class ClassDao
    {
        public ClassDao()
        {

        }

        public Campos campos = new Campos();

        public MySqlConnection minhaConexao;

        public string usuarioBD = "root";
        public string senhaBD = "coloqueiumasenha2020";
        public string servidor = "localhost";
        string bancoDados;
        string tabela;

        public void Conecte(string BancoDados, string Tabela)
        {
            bancoDados = BancoDados;
            tabela = Tabela;
            minhaConexao = new MySqlConnection("server=" + servidor + "; database=" + bancoDados + 
                                               "; uid=" + usuarioBD + "; password=" + senhaBD);
        }

        void Abrir()
        {
            minhaConexao.Open();
        }

        void Fechar()
        {
            minhaConexao.Close();
        }

        public void PreencheTabela(System.Windows.Forms.DataGridView dataGridView)
        {
            Abrir();

            MySqlDataAdapter meuAdapter = new MySqlDataAdapter("Select * from " + tabela, minhaConexao);
            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.Clear();
            meuAdapter.Fill(dataSet, tabela);
            dataGridView.DataSource = dataSet;
            dataGridView.DataMember = tabela;

            Fechar();
        }

        public void Insere(string campoNome, string campoEnder, decimal campoSalario)
        {
            Abrir();

            // insert into tabelaTeste (nome, endereco, salario) values ("Juca", "rua tal", 1230.70)
            MySqlCommand comando = new MySqlCommand("insert into " + tabela + 
                                                    "(nome, endereco, salario) values(@nome, @endereco, @salario)", minhaConexao);
            comando.Parameters.AddWithValue("@nome", campoNome);
            comando.Parameters.AddWithValue("@endereco", campoEnder);
            comando.Parameters.AddWithValue("@salario", campoSalario);
            comando.ExecuteNonQuery();

            Fechar();
        }

        public void Atualiza(string campoNome, string campoEnder, decimal campoSalario, int id)
        {
            Abrir();
            MySqlCommand comando = new MySqlCommand("update " + tabela
                                                   + " set nome=@nome, endereco=@endereco , "
                                                   + "salario=@salario where numCli=@id",
                                                   minhaConexao);

            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@nome", campoNome);
            comando.Parameters.AddWithValue("@endereco", campoEnder);
            comando.Parameters.AddWithValue("@salario", campoSalario);
            comando.ExecuteNonQuery();
            Fechar();
        }

        public void Consulta(string campoNome)
        {
            // consulta por nome
            Abrir();

            MySqlCommand comando = new MySqlCommand("select * from " + tabela
                                                    + " where nome = '" + campoNome + "'", minhaConexao);
            MySqlDataReader dtReader = comando.ExecuteReader();
            if (dtReader.Read())
            {
                campos.id = int.Parse(dtReader["numCli"].ToString());
                campos.nome = dtReader["nome"].ToString();
                campos.ender = dtReader["endereco"].ToString();
                campos.sal = decimal.Parse(dtReader["salario"].ToString());
            }

            Fechar();
        }

        public void Consulta(int id)

        {
            // sobrecarga do método de Consulta para permitir consulta por id também
            Abrir();
            MySqlCommand comando = new MySqlCommand("select * from " + tabela
                                                   + " where numCli = '" + id + "'", minhaConexao);
            MySqlDataReader dtReader = comando.ExecuteReader();
            if (dtReader.Read())
            {
                campos.id = int.Parse(dtReader["numCli"].ToString());
                campos.nome = dtReader["nome"].ToString();
                campos.ender = dtReader["endereco"].ToString();
                campos.sal = decimal.Parse(dtReader["salario"].ToString());
            }

            Fechar();
        }

        public void Deleta(int id)
        {
            Abrir();

            MySqlCommand comando = new MySqlCommand("delete from "
                                                   + tabela + " where numCli = @id", minhaConexao);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();

            Fechar();
        }

        public int NumRegistro()
        {
            Abrir();
            // MAX retorna o número do último valor de numCli
            MySqlCommand comando = new MySqlCommand("SELECT MAX(numCli) FROM " + tabela, minhaConexao); // MAX retorna o número do último valor de numCli
            string n = comando.ExecuteScalar().ToString(); // ExecuteScalar retorna um dado do tipo object. É preciso converter para string.
            int num = int.Parse(n) + 1; // Agora convertemos o dado para int e somamos um para obter o número do próximo registro

            Fechar();

            return num; // retorna o número do próximo registro do autoincrement de numCli
        }
    }
}
