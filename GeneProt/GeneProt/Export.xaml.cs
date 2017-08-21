using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneProt
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Page
    {
        private SqlConnection con;


        public Export()
        {
            InitializeComponent();
            con = DBconnection.getConnection();
            con.Open();
        }

        private void Import_Table(object sender, RoutedEventArgs e)
        {
            if (Gene_imp.IsChecked == true)
            {
                getFile("Gene");
                Gene_imp.IsChecked = false;
            }
            else if (Proteina_imp.IsChecked == true)
            {
                getFile("Protein");
                Proteina_imp.IsChecked = false;
            }
            else if (Mutacao_imp.IsChecked == true)
            {
                getFile("Mutation");
                Mutacao_imp.IsChecked = false;
            }
            else if (Metabolitos_imp.IsChecked == true)
            {
                getFile("Metabolites");
                Metabolitos_imp.IsChecked = false;
            }
            else if (Expressao_imp.IsChecked == true)
            {
                getFile("Expression");
                Expressao_imp.IsChecked = false;
            }
            else if (Amostras_imp.IsChecked == true)
            {
                getFile("Sample");
                Amostras_imp.IsChecked = false;
            }
            else
            {
                System.Windows.MessageBox.Show("Tem de selecionar uma entidade para importar!");
                return;
            }
        }

        private void getFile(string table)
        {
            System.Windows.Forms.OpenFileDialog choofdlog = new System.Windows.Forms.OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            string path = "";
            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                con.Close();
                string sFileName = choofdlog.FileName;
                path = sFileName;
                bool header;
                if (toggle_header.IsChecked == true) {
                    header = true;
                    toggle_header.IsChecked = false;
                }
                else
                {
                    header = false;
                }

                char sep;
                if (Separator_tab.IsChecked == true)
                {
                    sep = '\t';
                    Separator_tab.IsChecked = false;
                }
                else if(Separator_space.IsChecked == true)
                {
                    sep = ' ';
                    Separator_space.IsChecked = false;
                }
                else if(Separator_bar.IsChecked == true)
                {
                    sep = '|';
                    Separator_bar.IsChecked = false;
                }
                else if(Separator_pvirg.IsChecked == true)
                {
                    sep = ';';
                    Separator_pvirg.IsChecked = false;
                }
                else if(Separator_virgula.IsChecked == true)
                {
                    sep = ',';
                    Separator_virgula.IsChecked = false;
                }
                else
                {
                    System.Windows.MessageBox.Show("Selecione um separador de texto");
                    return;
                }

                importInfo(path, table, header, sep);
                

            }
  
        }

        private void importInfo(string path, string table, bool header, char sep)
        {
            SqlConnection thisConnection = new SqlConnection("Data Source=TIAGOASUS;Initial Catalog=GeneProt;Integrated Security=true");
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                string Values = "";
                thisConnection.Open();
                if (table == "Metabolites")
                {
                    Values = "(@metabolite_sequential_id, @hmdb_id, @common_name, @avg_molecular_weight, @iupac_name, @kingdom, @class, @status)";
                    nonqueryCommand.Parameters.Add("@metabolite_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@hmdb_id", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@common_name", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@avg_molecular_weight", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@iupac_name", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@kingdom", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@class", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@status", SqlDbType.VarChar);
                }
                else if (table == "Gene")
                {
                    Values = "(@gene_sequential_id, @chromosome_location, @GRCh38_hg38_Assembly, @GRCh37_hg19_Assembly, @symbol, @approved_name)";
                    nonqueryCommand.Parameters.Add("@gene_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@chromosome_location", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@GRCh38_hg38_Assembly", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@GRCh37_hg19_Assembly", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@symbol", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@approved_name", SqlDbType.VarChar);
                }
                else if (table == "Protein")
                {
                    Values = "(@protein_sequential_id, @gene_sequential_id, @protein_name, @uniprot_id, @num_of_residues, @protein_type, @molecular_weight)";
                    nonqueryCommand.Parameters.Add("@protein_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@gene_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@protein_name", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@uniprot_id", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@num_of_residues", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@protein_type", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@molecular_weight", SqlDbType.Decimal);
                }
                else if (table == "Mutation")
                {
                    Values = "(@mutation_sequential_id, @variant_id, @hgvs_ng, @hgvs_c, @hgvs_g, @hgvs_p, @start, @stop, @variant_type,"+
                        "@most_severe_clinical_significance, @minor_allele_1000G, @MAF_1000G, @protein_change, @variant_allele, @molecular_consequences2, @transcript_change, @condition, @gene_sequential_id, @protein_sequential_id)";
                    nonqueryCommand.Parameters.Add("@mutation_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@variant_id", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@hgvs_ng", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@hgvs_c", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@hgvs_g", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@hgvs_p", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@start", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@stop", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@variant_type", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@most_severe_clinical_significance", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@minor_allele_1000G", SqlDbType.Char);
                    nonqueryCommand.Parameters.Add("@MAF_1000G", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@protein_change", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@variant_allele", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@molecular_consequences2", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@transcript_change", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@condition", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@gene_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@protein_sequential_id", SqlDbType.Int);
                }
                else if (table == "Expression")
                {
                    Values = "(@code_exp, @score, @molecular_type, @method, @result_type, @num_samples, @type_exp, @result_exp," +
                        "@gene_sequential_id, @protein_sequential_id, @db_name_exp)";
                    nonqueryCommand.Parameters.Add("@code_exp", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@score", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@molecular_type", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@method", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@result_type", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@num_samples", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@type_exp", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@result_exp", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@gene_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@protein_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@db_name_exp", SqlDbType.Int);
                }
                else if (table == "Sample")
                {
                    Values = "(@sample_id, @study_id, @gene_sequential_id, @condition_sample, @sex, @age, @age_unit, @ethnic_group," +
                        "@develop_stage, @condition, @mutation_sequential_id, @eQTL_pvalue, @eQTL_effect_size, @exp_code_sample)";
                    nonqueryCommand.Parameters.Add("@sample_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@study_id", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@gene_sequential_id", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@condition_sample", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@sex", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@age", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@age_unit", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@ethnic_group", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@develop_stage", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@condition", SqlDbType.VarChar);
                    nonqueryCommand.Parameters.Add("@mutation_sequential_id", SqlDbType.Int);
                    nonqueryCommand.Parameters.Add("@eQTL_pvalue", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@eQTL_effect_size", SqlDbType.Decimal);
                    nonqueryCommand.Parameters.Add("@exp_code_sample", SqlDbType.Int);
                }

                nonqueryCommand.CommandText = "INSERT INTO " + table + " VALUES " + Values;
               

                string[] allLines = File.ReadAllLines(path);
                {
                    int i = 0;
                    if (header == true)
                    {
                        if (table == "Metabolites")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@metabolite_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@hmdb_id"].Value = items[1];
                                nonqueryCommand.Parameters["@common_name"].Value = items[2];
                                nonqueryCommand.Parameters["@avg_molecular_weight"].Value = decimal.Parse(items[3].Replace(".", ","));
                                nonqueryCommand.Parameters["@iupac_name"].Value = items[4];
                                nonqueryCommand.Parameters["@kingdom"].Value = items[5];
                                nonqueryCommand.Parameters["@class"].Value = items[6];
                                nonqueryCommand.Parameters["@status"].Value = items[7];
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Gene")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@chromosome_location"].Value = items[1];
                                nonqueryCommand.Parameters["@GRCh38_hg38_Assembly"].Value = items[2];
                                nonqueryCommand.Parameters["@GRCh37_hg19_Assembly"].Value = items[3];
                                nonqueryCommand.Parameters["@symbol"].Value = items[4];
                                nonqueryCommand.Parameters["@approved_name"].Value = items[5];
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Protein")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@protein_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[1]);
                                nonqueryCommand.Parameters["@protein_name"].Value = items[2];
                                nonqueryCommand.Parameters["@uniprot_id"].Value = items[3];
                                nonqueryCommand.Parameters["@num_of_residues"].Value = Int32.Parse(items[4]);
                                nonqueryCommand.Parameters["@protein_type"].Value = items[5];
                                nonqueryCommand.Parameters["@molecular_weight"].Value = decimal.Parse(items[6].Replace(".", ","));
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Mutation")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@mutation_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@variant_id"].Value = items[1];
                                nonqueryCommand.Parameters["@hgvs_ng"].Value = items[2];
                                nonqueryCommand.Parameters["@hgvs_c"].Value =items[3];
                                nonqueryCommand.Parameters["@hgvs_g"].Value = items[4];
                                nonqueryCommand.Parameters["@hgvs_p"].Value = items[5];
                                nonqueryCommand.Parameters["@start"].Value = decimal.Parse(items[6].Replace(".", ","));
                                nonqueryCommand.Parameters["@stop"].Value = decimal.Parse(items[7].Replace(".", ","));
                                nonqueryCommand.Parameters["@variant_type"].Value = items[8];
                                nonqueryCommand.Parameters["@most_severe_clinical_significance"].Value = items[9];
                                nonqueryCommand.Parameters["@minor_allele_1000G"].Value = items[10];
                                nonqueryCommand.Parameters["@MAF_1000G"].Value = decimal.Parse(items[11].Replace(".", ","));
                                nonqueryCommand.Parameters["@protein_change"].Value = items[12];
                                nonqueryCommand.Parameters["@variant_allele"].Value = items[13];
                                nonqueryCommand.Parameters["@molecular_consequences2"].Value = items[14];
                                nonqueryCommand.Parameters["@transcript_change"].Value = items[15];
                                nonqueryCommand.Parameters["@condition"].Value = items[16];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[18]);
                                nonqueryCommand.Parameters["@protein_sequential_id "].Value = Int32.Parse(items[19]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Expression")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@code_exp"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@score"].Value = decimal.Parse(items[1].Replace(".", ","));
                                nonqueryCommand.Parameters["@molecular_type"].Value = items[2];
                                nonqueryCommand.Parameters["@method"].Value = items[3];
                                nonqueryCommand.Parameters["@result_type"].Value = items[4];
                                nonqueryCommand.Parameters["@num_samples"].Value = Int32.Parse(items[5]);
                                nonqueryCommand.Parameters["@type_exp"].Value = items[6];
                                nonqueryCommand.Parameters["@result_exp"].Value = items[7];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[8]);
                                nonqueryCommand.Parameters["@protein_sequential_id"].Value = Int32.Parse(items[9]);
                                nonqueryCommand.Parameters["@db_name_exp"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Sample")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@sample_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@study_id"].Value = items[1];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = items[2];
                                nonqueryCommand.Parameters["@condition_sample"].Value = items[3];
                                nonqueryCommand.Parameters["@sex"].Value = items[4];
                                nonqueryCommand.Parameters["@age"].Value = items[5];
                                nonqueryCommand.Parameters["@age_unit"].Value = items[6];
                                nonqueryCommand.Parameters["@ethnic_group"].Value = items[7];
                                nonqueryCommand.Parameters["@develop_stage"].Value = items[8];
                                nonqueryCommand.Parameters["@condition"].Value = items[9];
                                nonqueryCommand.Parameters["@mutation_sequential_id"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.Parameters["@eQTL_pvalue"].Value = decimal.Parse(items[8].Replace(".", ","));
                                nonqueryCommand.Parameters["@eQTL_effect_size"].Value = decimal.Parse(items[9].Replace(".", ","));
                                nonqueryCommand.Parameters["@exp_code_sample"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        if (table == "Metabolites")
                        {
                            for (i = 0; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@metabolite_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@hmdb_id"].Value = items[1];
                                nonqueryCommand.Parameters["@common_name"].Value = items[2];
                                nonqueryCommand.Parameters["@avg_molecular_weight"].Value = decimal.Parse(items[3].Replace(".", ","));
                                nonqueryCommand.Parameters["@iupac_name"].Value = items[4];
                                nonqueryCommand.Parameters["@kingdom"].Value = items[5];
                                nonqueryCommand.Parameters["@class"].Value = items[6];
                                nonqueryCommand.Parameters["@status"].Value = items[7];
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Gene")
                        {
                            for (i = 0; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@chromosome_location"].Value = items[1];
                                nonqueryCommand.Parameters["@GRCh38_hg38_Assembly"].Value = items[2];
                                nonqueryCommand.Parameters["@GRCh37_hg19_Assembly"].Value = items[3];
                                nonqueryCommand.Parameters["@symbol"].Value = items[4];
                                nonqueryCommand.Parameters["@approved_name"].Value = items[5];
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Protein")
                        {
                            for (i = 1; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@chromosome_location"].Value = items[1];
                                nonqueryCommand.Parameters["@GRCh38_hg38_Assembly"].Value = items[2];
                                nonqueryCommand.Parameters["@GRCh37_hg19_Assembly"].Value = items[3];
                                nonqueryCommand.Parameters["@symbol"].Value = items[4];
                                nonqueryCommand.Parameters["@approved_name"].Value = items[5];
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Mutation")
                        {
                            for (i = 0; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@mutation_sequential_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@variant_id"].Value = items[1];
                                nonqueryCommand.Parameters["@hgvs_ng"].Value = items[2];
                                nonqueryCommand.Parameters["@hgvs_c"].Value = items[3];
                                nonqueryCommand.Parameters["@hgvs_g"].Value = items[4];
                                nonqueryCommand.Parameters["@hgvs_p"].Value = items[5];
                                nonqueryCommand.Parameters["@start"].Value = decimal.Parse(items[6].Replace(".", ","));
                                nonqueryCommand.Parameters["@stop"].Value = decimal.Parse(items[7].Replace(".", ","));
                                nonqueryCommand.Parameters["@variant_type"].Value = items[8];
                                nonqueryCommand.Parameters["@most_severe_clinical_significance"].Value = items[9];
                                nonqueryCommand.Parameters["@minor_allele_1000G"].Value = items[10];
                                nonqueryCommand.Parameters["@MAF_1000G"].Value = decimal.Parse(items[11].Replace(".", ","));
                                nonqueryCommand.Parameters["@protein_change"].Value = items[12];
                                nonqueryCommand.Parameters["@variant_allele"].Value = items[13];
                                nonqueryCommand.Parameters["@molecular_consequences2"].Value = items[14];
                                nonqueryCommand.Parameters["@transcript_change"].Value = items[15];
                                nonqueryCommand.Parameters["@condition"].Value = items[16];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[18]);
                                nonqueryCommand.Parameters["@protein_sequential_id "].Value = Int32.Parse(items[19]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Expression")
                        {
                            for (i = 0; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@code_exp"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@score"].Value = decimal.Parse(items[1].Replace(".", ","));
                                nonqueryCommand.Parameters["@molecular_type"].Value = items[2];
                                nonqueryCommand.Parameters["@method"].Value = items[3];
                                nonqueryCommand.Parameters["@result_type"].Value = items[4];
                                nonqueryCommand.Parameters["@num_samples"].Value = Int32.Parse(items[5]);
                                nonqueryCommand.Parameters["@type_exp"].Value = items[6];
                                nonqueryCommand.Parameters["@result_exp"].Value = items[7];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = Int32.Parse(items[8]);
                                nonqueryCommand.Parameters["@protein_sequential_id"].Value = Int32.Parse(items[9]);
                                nonqueryCommand.Parameters["@db_name_exp"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                        else if (table == "Sample")
                        {
                            for (i = 0  ; i < allLines.Length; i++)
                            {
                                string[] items = allLines[i].Split(new char[] { sep });
                                nonqueryCommand.Parameters["@sample_id"].Value = Int32.Parse(items[0]);
                                nonqueryCommand.Parameters["@study_id"].Value = items[1];
                                nonqueryCommand.Parameters["@gene_sequential_id"].Value = items[2];
                                nonqueryCommand.Parameters["@condition_sample"].Value = items[3];
                                nonqueryCommand.Parameters["@sex"].Value = items[4];
                                nonqueryCommand.Parameters["@age"].Value = items[5];
                                nonqueryCommand.Parameters["@age_unit"].Value = items[6];
                                nonqueryCommand.Parameters["@ethnic_group"].Value = items[7];
                                nonqueryCommand.Parameters["@develop_stage"].Value = items[8];
                                nonqueryCommand.Parameters["@condition"].Value = items[9];
                                nonqueryCommand.Parameters["@mutation_sequential_id"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.Parameters["@eQTL_pvalue"].Value = decimal.Parse(items[8].Replace(".", ","));
                                nonqueryCommand.Parameters["@eQTL_effect_size"].Value = decimal.Parse(items[9].Replace(".", ","));
                                nonqueryCommand.Parameters["@exp_code_sample"].Value = Int32.Parse(items[10]);
                                nonqueryCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            finally
            {
                System.Windows.MessageBox.Show("Importação bem sucedida");
                thisConnection.Close();
                con.Open();
            }
        }

        private void Export_Table(object sender, RoutedEventArgs e)
        {
        
            if (Gene.IsChecked == true)
            {
                exportInfo("select * from udf_show_Gene()");
            }
            else if(Proteina.IsChecked == true)
            {
                exportInfo("select * from udf_show_prot()");
            }
            else if(Mutacao.IsChecked == true)
            {
                exportInfo("select * from udf_show_mut()");
            }
            else if(Metabolitos.IsChecked == true)
            {
                exportInfo("Select * from udf_show_met()");
            }
            else if(Expressao.IsChecked == true)
            {
                exportInfo("Select * from udf_expr_gene_sample_values()");
            }
            else
            {
                System.Windows.MessageBox.Show("Tem de selecionar uma entidade para exportar!");
                return;
            }
        }

        private void exportInfo(string sql_command)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialogCSV = new Microsoft.Win32.SaveFileDialog();
            saveFileDialogCSV.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialogCSV.FilterIndex = 1;
            saveFileDialogCSV.RestoreDirectory = true;
            saveFileDialogCSV.ShowDialog();
            string path_csv = saveFileDialogCSV.FileName;
            string command = sql_command;
            DumpTableToFile(con, "tbl_trmc", path_csv, command);
            System.Windows.MessageBox.Show("Ficheiro exportado.");
            return;
        }

        public void DumpTableToFile(SqlConnection connection, string tableName, string destinationFile, string sql_command)
        {
            try { 
            using (var command = new SqlCommand(sql_command, connection))
            using (var reader = command.ExecuteReader())
            
                
            using (var outFile = System.IO.File.CreateText(destinationFile))
            {
                string[] columnNames = GetColumnNames(reader).ToArray();
                int numFields = columnNames.Length;
                outFile.WriteLine(string.Join(";", columnNames));
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string[] columnValues =
                            Enumerable.Range(0, numFields)
                                      .Select(i => reader.GetValue(i).ToString())
                                      .Select(field => string.Concat("\"", field.Replace("\"", "\"\""), "\""))
                                      .ToArray();
                        outFile.WriteLine(string.Join(";", columnValues).Replace('"', Char.MinValue));
                    }
                }
            }
            }
            catch
            {
                System.Windows.MessageBox.Show("Ocorreu um erro ao exportar o ficheiro.");
            }
        }

        private IEnumerable<string> GetColumnNames(IDataReader reader)
        {
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                yield return (string)row["ColumnName"];
            }
        }

    }
}