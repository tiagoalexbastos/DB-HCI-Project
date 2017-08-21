using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GeneProt
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
      
        private SqlConnection con;
        public Search()
        {
            InitializeComponent();
            con = DBconnection.getConnection();
            FillGeneTable();
            FillGeneMappingTable();
            FillProteinTable();
            FillMutationTable();
            FillMetaboliteTable();
            FillPPITable();
            FillPCITable();
            FillDbTable();
        }

     
    
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
        *  ##########################----------- GENE -----------##########################
        * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void FillGeneTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_Gene()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("Gene");
            sda.Fill(dt);
            GeneGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        //done before run time. This creates the whole options
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Genes related to proteins");
            data.Add("Genes with mutations");
            data.Add("Gene name and Database id");
            data.Add("Gene expression results");
            data.Add("Genes with Pmids");
            data.Add("Genes with proteins and celular location");
            data.Add("Gene with proteins that developed a mutation");
            data.Add("Gene with mutations that appear in samples");


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Gene_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(id_tb.Text))
            {

                // --> Validations
                int idInt;

                if (!Int32.TryParse(id_tb.Text, out idInt))
                {
                    MessageBox.Show("The ID must be an Integer!");
                    return;
                }
                string CmdString = "SELECT * FROM GetGene_ID(@id)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@id", idInt);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Gene");
                sda.Fill(dt);
                GeneGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(symbol_tb.Text)) {
                // bi, nif and federation id is number
                if (symbol_tb.Text.Length > 25)
                {
                    MessageBox.Show("The symbol must have less than 25 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetGene_Symbol(@symbol)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@symbol", symbol_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Gene");
                sda.Fill(dt);
                GeneGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(app_name_tb.Text))
            {
                // bi, nif and federation id is number
                if (app_name_tb.Text.Length > 50)
                {
                    MessageBox.Show("The symbol must have less than 50 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetGene_Name(@app)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@app", app_name_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Gene");
                sda.Fill(dt);
                GeneGrid.ItemsSource = dt.DefaultView;
            }

            else MessageBox.Show("No field presented.");

        }

        private void Gene_reset(object sender, RoutedEventArgs e)
        {
            FillGeneTable();
        }

        private void Gene_GetRel(object sender, RoutedEventArgs e)
        {
            if (gene_rel.SelectedIndex > -1) //somthing was selected
            {
                if (gene_rel.SelectedItem.Equals("Genes related to proteins"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_prot()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene rel with prot");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                    
                }
                else if (gene_rel.SelectedItem.Equals("Genes with mutations"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_muts()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene with mutations");
                    sda.Fill(dt);
                    GeneGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = Gene_tab;

                }
                else if (gene_rel.SelectedItem.Equals("Gene name and Database id"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_mapping()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene real name and id");
                    sda.Fill(dt);
                    GeneGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = Gene_tab;

                }
                else if (gene_rel.SelectedItem.Equals("Gene expression results"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_ex_results()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene expression results");
                    sda.Fill(dt);
                    GeneGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = Gene_tab;
                }
               else if (gene_rel.SelectedItem.Equals("Genes with Pmids"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_pmids()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene with Pmids");
                    sda.Fill(dt);
                    GeneGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = Gene_tab;
                }
                else if (gene_rel.SelectedItem.Equals("Genes with proteins and celular location"))
                {
                    //comandos que interagem com a BD
                    
                    string CmdString = "SELECT * FROM udf_gene_prot_Celloc()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene with prots and cell loc");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (gene_rel.SelectedItem.Equals("Gene with proteins that developed a mutation"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_prot_with_mut()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene with prots_with_mut");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (gene_rel.SelectedItem.Equals("Gene with mutations that appear in samples"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_mut_with_samples()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene with prots_with_mut");
                    sda.Fill(dt);
                    GeneGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = Gene_tab;
                }

            }

        }
        private void GeneGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)GeneGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            id_tb.Text = row.Row.ItemArray[0].ToString();
            cromo_loc_tb.Text = row.Row.ItemArray[1].ToString();
            ass_38_tb.Text = row.Row.ItemArray[2].ToString();
            ass_19_tb.Text = row.Row.ItemArray[3].ToString();
            symbol_tb.Text = row.Row.ItemArray[4].ToString();
            app_name_tb.Text = row.Row.ItemArray[5].ToString();
        }

        private void Gene_Clear(object sender, RoutedEventArgs e)
        {
            id_tb.Text = "";
            cromo_loc_tb.Text = "";
            ass_38_tb.Text = "";
            ass_19_tb.Text = "";
            symbol_tb.Text = "";
            app_name_tb.Text = "";
        }
        private void Gene_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idInt;

            if (!Int32.TryParse(id_tb.Text, out idInt))
            {
                MessageBox.Show("The ID must be an Integer!");
                return;
            }



            // INSERT GENE

            string CmdString = "sp_createGene";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idInt);
            cmd_gene.Parameters.AddWithValue("@chromosome_location", cromo_loc_tb.Text);
            cmd_gene.Parameters.AddWithValue("@GRCh38_hg38_Assembly", ass_38_tb.Text);
            cmd_gene.Parameters.AddWithValue("@GRCh37_hg19_Assembly", ass_19_tb.Text);
            cmd_gene.Parameters.AddWithValue("@symbol", symbol_tb.Text);
            cmd_gene.Parameters.AddWithValue("@approved_name", app_name_tb.Text);
            
            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Gene_Clear(sender, e);
                FillGeneTable();
                MessageBox.Show("The gene has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Gene_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
           
        }
        private void Gene_Delete(object sender, RoutedEventArgs e)
        {

            String result;
         

            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)GeneGrid.SelectedItem;
                result = (drv["Gene ID"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return; 
            
            }
            
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

                if (!Int32.TryParse(result, out idInt))
                {
                    MessageBox.Show("The ID must be an Integer!");
                    return;
                }

                // DELETE THE GENE

                string CmdString = "sp_deleteGene";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@id", idInt);

                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    Gene_Clear(sender,e);
                    FillGeneTable();
                    FillGeneMappingTable();
                    FillProteinTable();
                    FillMutationTable();
                    MessageBox.Show("The gene has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    Gene_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }
               
            }
        }
        private void Gene_Update(object sender, RoutedEventArgs e) {
            // --> Validations
            int idInt;

            // ID must be an integer
            if (!Int32.TryParse(id_tb.Text, out idInt))
            {
                MessageBox.Show("The ID must be an Integer!");
                return;
            }
            

            // UPDATE GENE
            string CmdString = "sp_modifyGene";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@id", idInt);
            cmd_gene.Parameters.AddWithValue("@chromosome_location", cromo_loc_tb.Text);
            cmd_gene.Parameters.AddWithValue("@GRCh38_hg38_Assembly", ass_38_tb.Text);
            cmd_gene.Parameters.AddWithValue("@GRCh37_hg19_Assembly", ass_19_tb.Text);
            cmd_gene.Parameters.AddWithValue("@symbol", symbol_tb.Text);
            cmd_gene.Parameters.AddWithValue("@approved_name", app_name_tb.Text);

            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Gene_Clear(sender, e);
                FillGeneTable();
                MessageBox.Show("The gene has been updated successfully!");
            }
            catch (Exception exc)
            {
                Gene_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }
            
        }





       /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- GENE  MAPPING-----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        private void FillGeneMappingTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM Gene_mapping";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("Gene_mapping");
            sda.Fill(dt);
            GeneMappGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        //done before run time. This creates the whole options
        private void ComboBoxMapping_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxMapping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void GeneM_reset(object sender, RoutedEventArgs e)
        {
            FillGeneMappingTable();
        }

        private void GeneMapping_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(db_map_tb.Text))
            {

              

                // bi, nif and federation id is number
                if (db_map_tb.Text.Length > 50)
                {
                    MessageBox.Show("The db must have less than 25 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetGeneMapping_DBIDs(@db)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@db", db_map_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("GeneMapping");
                sda.Fill(dt);
                GeneMappGrid.ItemsSource = dt.DefaultView;
            }
           
            else MessageBox.Show("No field presented.");

        }



        private void GeneMappGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)GeneMappGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            id_map_tb.Text = row.Row.ItemArray[0].ToString();
            db_map_tb.Text = row.Row.ItemArray[1].ToString();
            db_id_map_tb.Text = row.Row.ItemArray[2].ToString();
            db_url_map.Text = row.Row.ItemArray[3].ToString();

        }

        private void GeneMapping_Url(object sender, RoutedEventArgs e)
        {
            if(db_url_map.Text == "")
            {
                MessageBox.Show("No URL specified");
                return;
            }
            else
            {
                Process.Start(db_url_map.Text);
            }
        }

        private void GeneM_Clear(object sender, RoutedEventArgs e)
        {
            id_map_tb.Text = "";
            db_map_tb.Text = "";
            db_id_map_tb.Text = "";
            db_url_map.Text = "";
        }

        private void GeneM_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idInt;

           
            if (!Int32.TryParse(id_map_tb.Text, out idInt))
            {
                MessageBox.Show("The ID must be an Integer!");
                return;
            }

            // INSERT GENE

            string CmdString = "sp_createGeneMapping";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idInt);
            cmd_gene.Parameters.AddWithValue("@db", db_map_tb.Text);
            cmd_gene.Parameters.AddWithValue("@db_id", db_id_map_tb.Text);
            cmd_gene.Parameters.AddWithValue("@url", db_url_map.Text);



            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                GeneM_Clear(sender, e);
                FillGeneMappingTable();
                MessageBox.Show("The gene mapping has been inserted successfully!");
            }
            catch (Exception exc)
            {
                GeneM_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }
           

        }
        private void GeneM_Delete(object sender, RoutedEventArgs e)
        {


            String id, db;


            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)GeneMappGrid.SelectedItem;
                id = (drv["Gene Seq ID"]).ToString();
                db = (drv["DB Name"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

                // bi is number
                if (!Int32.TryParse(id, out idInt))
                {
                    MessageBox.Show("The ID must be an Integer!");
                    return;
                }

                // DELETE THE GENE

                string CmdString = "sp_deleteGeneMapping";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@id", idInt);
                cmd_gene.Parameters.AddWithValue("@db", db);

                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    GeneM_Clear(sender, e);
                    FillGeneMappingTable();
                    MessageBox.Show("The gene mapping has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    GeneM_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }
               
            }
        }

        private void GeneM_Update(object sender, RoutedEventArgs e)
        {
           
            MessageBox.Show("Verificar problema da chave dupla!");
           
        }


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
        *  ##########################----------- Proteina-----------##########################
        * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void FillProteinTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_prot()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("Protein");
            sda.Fill(dt);
            ProteinGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        //done before run time. This creates the whole options
        private void ComboBoxProtein_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Protein's Celular Location");
            data.Add("Protein relation with Mutation");
            data.Add("Protein relation with Expression");
            data.Add("Proteins without mutation");
            data.Add("Proteins with mutations that appear on samples");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxProtein_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Prot_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(protein_type_tb.Text))
            {
               
                if (protein_type_tb.Text.Length > 20)
                {
                    MessageBox.Show("The db must have less than 20 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetProt_Type(@prot)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@prot", protein_type_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Protein");
                sda.Fill(dt);
                ProteinGrid.ItemsSource = dt.DefaultView;
            }

            else MessageBox.Show("No fields presented.");

        }

        private void Protein_reset(object sender, RoutedEventArgs e)
        {
            FillProteinTable();
        }


        private void ProteinGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)ProteinGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            protein_sq_id_tb.Text = row.Row.ItemArray[0].ToString();
            gene_sq_id_tb.Text = row.Row.ItemArray[1].ToString();
            uniprot_id_tb.Text = row.Row.ItemArray[2].ToString();
            enzyme_id_tb.Text = row.Row.ItemArray[3].ToString();
            num_of_residues_tb.Text = row.Row.ItemArray[4].ToString();
            protein_type_tb.Text = row.Row.ItemArray[5].ToString();
            molecular_weight_tb.Text = row.Row.ItemArray[6].ToString();

        }


        private void Protein_Clear(object sender, RoutedEventArgs e)
        {
            protein_sq_id_tb.Text = "";
            gene_sq_id_tb.Text = "";
            uniprot_id_tb.Text = "";
            enzyme_id_tb.Text = "";
            num_of_residues_tb.Text = "";
            protein_type_tb.Text = "";
            molecular_weight_tb.Text = "";       
        }

        private void Protein_Rel(object sender, RoutedEventArgs e)
        {
            if (prot_rel.SelectedIndex > -1) //somthing was selected
            {
                if (prot_rel.SelectedItem.Equals("Protein's Celular Location"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_prot_celloc()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Prot1");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (prot_rel.SelectedItem.Equals("Protein relation with Mutation"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_prot_mut()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Prot2");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (prot_rel.SelectedItem.Equals("Protein relation with Expression"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_prot_expr()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Prot3");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (prot_rel.SelectedItem.Equals("Proteins without mutation"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_prot_with_no_mut()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Prot4");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
                else if (prot_rel.SelectedItem.Equals("Proteins with mutations that appear on samples"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_prot_mut_with_samples()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Prot5");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;
                }
            }
        }

        private void Protein_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idProt, idGene, idRes=0;
            Boolean bRes = false;

            
            if (!Int32.TryParse(protein_sq_id_tb.Text, out idProt))
            {
                MessageBox.Show("The protein seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(gene_sq_id_tb.Text, out idGene))
            {
                MessageBox.Show("The gene seq id must be an Integer!");
                return;
            }
            if (num_of_residues_tb.Text.Equals(""))
                bRes = true;
            else
            {
                if (!Int32.TryParse(num_of_residues_tb.Text, out idRes))
                {
                    MessageBox.Show("The number of residues must be an Integer!");
                    return;
                }
            }
            



            // INSERT PROTEINS

            string CmdString = "sp_createProtein";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@protein_sequential_id", idProt);
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idGene);
            cmd_gene.Parameters.AddWithValue("@uniprot_id", uniprot_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@enzyme_id", enzyme_id_tb.Text);
            if(bRes == true)
                cmd_gene.Parameters.AddWithValue("@num_of_residues", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@num_of_residues", idRes);

            cmd_gene.Parameters.AddWithValue("@protein_type", protein_type_tb.Text);
            if (molecular_weight_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@molecular_weight", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@molecular_weight", molecular_weight_tb.Text);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Protein_Clear(sender, e);
                FillProteinTable();
                FillGeneTable();
                MessageBox.Show("The protein has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Protein_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
            
        }

        private void Protein_Delete(object sender, RoutedEventArgs e)
        {


            String id;


            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)ProteinGrid.SelectedItem;
                id = (drv["Prot Id"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

                // bi is number
                if (!Int32.TryParse(id, out idInt))
                {
                    MessageBox.Show("The protein ID must be an Integer!");
                    return;
                }

                // DELETE THE PROTEIN

                string CmdString = "sp_deleteProtein";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@protein_sequential_id", idInt);


                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    Protein_Clear(sender,e);
                    FillProteinTable();
                    FillGeneTable();
                    MessageBox.Show("The protein has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    con.Close();
                    Protein_Clear(sender, e);
                    MessageBox.Show(exc.Message);
                }
               
            }
        }
        private void Protein_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idProt, idGene, idRes=0;
            Boolean bRes = false;
            
            if (!Int32.TryParse(protein_sq_id_tb.Text, out idProt))
            {
                MessageBox.Show("The protein seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(gene_sq_id_tb.Text, out idGene))
            {
                MessageBox.Show("The gene seq id must be an Integer!");
                return;
            }
            if (num_of_residues_tb.Text.Equals(""))
                bRes = true;
            else
            {
                if (!Int32.TryParse(num_of_residues_tb.Text, out idRes))
                {
                    MessageBox.Show("The number of residues must be an Integer!");
                    return;
                }
            }


            // UPDATE PLAYER
            string CmdString = "sp_modifyProtein";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@protein_sequential_id", idProt);
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idGene);
            cmd_gene.Parameters.AddWithValue("@uniprot_id", uniprot_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@enzyme_id", enzyme_id_tb.Text);
            if (bRes == true)
                cmd_gene.Parameters.AddWithValue("@num_of_residues", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@num_of_residues", idRes);

            cmd_gene.Parameters.AddWithValue("@protein_type", protein_type_tb.Text);
            if (molecular_weight_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@molecular_weight", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@molecular_weight", molecular_weight_tb.Text);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Protein_Clear(sender, e);
                FillProteinTable();
                FillGeneTable();
                MessageBox.Show("The protein has been updated successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Protein_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
           
        }




        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- Mutacao-----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        private void FillMutationTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_mut()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("Mutation");
            sda.Fill(dt);
            MutationGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        //done before run time. This creates the whole options
        private void ComboBoxMutation_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Gene's Mutation Molecular Consequences");
            data.Add("Protein's Mutation Molecular Consequences");
            data.Add("Gene's Mutation Publications");
            data.Add("Protein's Mutation Publications");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxMutation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Mutation_Rel(object sender, RoutedEventArgs e)
        {
            if (mutation_rel.SelectedIndex > -1) //somthing was selected
            {
                if (mutation_rel.SelectedItem.Equals("Gene's Mutation Molecular Consequences"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_mut_gene_mol_consequence()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Mut1");
                    sda.Fill(dt);
                    MutationGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_mut;
                }
                else if (mutation_rel.SelectedItem.Equals("Protein's Mutation Molecular Consequences"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_mut_prot_mol_consequence()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Mut2");
                    sda.Fill(dt);
                    MutationGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_mut;
                }
                else if (mutation_rel.SelectedItem.Equals("Gene's Mutation Publications"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_mut_gene_pubs()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Mut3");
                    sda.Fill(dt);
                    MutationGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_mut;
                }
                else if (mutation_rel.SelectedItem.Equals("Protein's Mutation Publications"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_mut_prot_pubs()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Mut4");
                    sda.Fill(dt);
                    MutationGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_mut;
                }
            }
        }



        private void Mut_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(variant_id_tb.Text))
            {
               
                if (variant_id_tb.Text.Length > 20)
                {
                    MessageBox.Show("The variant id must have less than 20 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMut_VariantID(@vari)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@vari", variant_id_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Mutation");
                sda.Fill(dt);
                MutationGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(condition_tb.Text)) {
                // bi, nif and federation id is number
                if (condition_tb.Text.Length > 20)
                {
                    MessageBox.Show("The condition id must have less than 20 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMut_condition(@cond)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@cond", condition_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Mutation");
                sda.Fill(dt);
                MutationGrid.ItemsSource = dt.DefaultView;


            }

            else MessageBox.Show("No fields presented.");

        }


        private void Mutation_reset(object sender, RoutedEventArgs e)
        {
            FillMutationTable();
        }

        private void MutationGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)MutationGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            mutation_sequential_id_tb.Text = row.Row.ItemArray[0].ToString();
            variant_id_tb.Text = row.Row.ItemArray[1].ToString();
            hgvs_ng_tb.Text = row.Row.ItemArray[2].ToString();
            hgvs_c_tb.Text = row.Row.ItemArray[3].ToString();
            hgvs_g_tb.Text = row.Row.ItemArray[4].ToString();
            hgvs_p_tb.Text = row.Row.ItemArray[5].ToString();
            start_tb.Text = row.Row.ItemArray[6].ToString();
            stop_tb.Text = row.Row.ItemArray[7].ToString();
            variant_type_tb.Text = row.Row.ItemArray[8].ToString();
            most_severe_clinical_significance_tb.Text = row.Row.ItemArray[9].ToString();
            minor_allele_1000G_tb.Text = row.Row.ItemArray[10].ToString();
            MAF_1000G_tb.Text = row.Row.ItemArray[11].ToString();
            protein_change_tb.Text = row.Row.ItemArray[12].ToString();
            variant_allele_tb.Text = row.Row.ItemArray[13].ToString();
            molecular_consequences2_tb.Text = row.Row.ItemArray[14].ToString();
            transcript_change_tb.Text = row.Row.ItemArray[15].ToString();
            condition_tb.Text = row.Row.ItemArray[16].ToString();
            gene_sequential_id_tb.Text = row.Row.ItemArray[17].ToString();
            uniprprotein_sequential_idot_mut_tb.Text = row.Row.ItemArray[18].ToString();

        }


        private void Mutation_Clear(object sender, RoutedEventArgs e)
        {
            mutation_sequential_id_tb.Text = "";
            variant_id_tb.Text = "";
            hgvs_ng_tb.Text = "";
            hgvs_c_tb.Text = "";
            hgvs_g_tb.Text = "";
            hgvs_p_tb.Text = "";
            start_tb.Text = "";
            stop_tb.Text = "";
            variant_type_tb.Text = "";
            most_severe_clinical_significance_tb.Text = "";
            minor_allele_1000G_tb.Text = "";
            MAF_1000G_tb.Text = "";
            protein_change_tb.Text = "";
            variant_allele_tb.Text = "";
            molecular_consequences2_tb.Text = "";
            transcript_change_tb.Text = "";
            condition_tb.Text = "";
            gene_sequential_id_tb.Text = "";
            uniprprotein_sequential_idot_mut_tb.Text = "";
        }
        private void Mutation_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idProt, idGene, idMut;

            
            if (!Int32.TryParse(mutation_sequential_id_tb.Text, out idMut))
            {
                MessageBox.Show("The mutation seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(gene_sequential_id_tb.Text, out idGene))
            {
                MessageBox.Show("The gene seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(uniprprotein_sequential_idot_mut_tb.Text, out idProt))
            {
                MessageBox.Show("The prot seq id must be an Integer!");
                return;
            }
            if (minor_allele_1000G_tb.Text.Length > 1)
            {
                MessageBox.Show("The minor allele 1000G must be an only character and not a string!");
                return;

            }


            // INSERT Mutation

            string CmdString = "sp_createMutation";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@mutation_sequential_id", idMut);
            cmd_gene.Parameters.AddWithValue("@variant_id", variant_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_ng", hgvs_ng_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_c", hgvs_c_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_g", hgvs_g_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_p", hgvs_p_tb.Text);

            if(start_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@start", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@start", start_tb.Text);

            if (stop_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@stop", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@stop", stop_tb.Text);

            cmd_gene.Parameters.AddWithValue("@variant_type", variant_type_tb.Text);
            cmd_gene.Parameters.AddWithValue("@most_severe_clinical_significance", most_severe_clinical_significance_tb.Text);
            cmd_gene.Parameters.AddWithValue("@minor_allele_1000G", minor_allele_1000G_tb.Text);

            if (MAF_1000G_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@MAF_1000G", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@MAF_1000G", MAF_1000G_tb.Text);

            cmd_gene.Parameters.AddWithValue("@protein_change", protein_change_tb.Text);
            cmd_gene.Parameters.AddWithValue("@variant_allele", variant_allele_tb.Text);
            cmd_gene.Parameters.AddWithValue("@molecular_consequences2", molecular_consequences2_tb.Text);
            cmd_gene.Parameters.AddWithValue("@transcript_change", transcript_change_tb.Text);
            cmd_gene.Parameters.AddWithValue("@condition", condition_tb.Text);
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idGene);
            cmd_gene.Parameters.AddWithValue("@uniprprotein_sequential_idot_mut", idProt);

            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Mutation_Clear(sender, e);
                FillMutationTable();
                FillGeneTable();
                MessageBox.Show("The mutation has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Mutation_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
            
        }

        private void Mutation_Delete(object sender, RoutedEventArgs e)
        {
            String id;
            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)MutationGrid.SelectedItem;
                id = (drv["Mut Id"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

               
                if (!Int32.TryParse(id, out idInt))
                {
                    MessageBox.Show("The mutation ID must be an Integer!");
                    return;
                }

                // DELETE THE PROTEIN

                string CmdString = "sp_deleteMutation";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@mutation_sequential_id", idInt);


                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    Mutation_Clear(sender, e);
                    FillMutationTable();
                    FillGeneTable();
                    MessageBox.Show("The mutation has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    Mutation_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }
                
            }
        }
        private void Mutation_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idProt, idGene, idMut;

           
            if (!Int32.TryParse(uniprprotein_sequential_idot_mut_tb.Text, out idProt))
            {
                MessageBox.Show("The protein seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(gene_sequential_id_tb.Text, out idGene))
            {
                MessageBox.Show("The gene seq id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(mutation_sequential_id_tb.Text, out idMut))
            {
                MessageBox.Show("The mutation id must be an Integer!");
                return;
            }
            if (minor_allele_1000G_tb.Text.Length > 1)
            {
                MessageBox.Show("The minor allele 1000G must be an only character and not a string!");
                return;

            }


            // UPDATE PLAYER
            string CmdString = "sp_modifyMutation";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@mutation_sequential_id", idMut);
            cmd_gene.Parameters.AddWithValue("@variant_id", variant_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_ng", hgvs_ng_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_c", hgvs_c_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_g", hgvs_g_tb.Text);
            cmd_gene.Parameters.AddWithValue("@hgvs_p", hgvs_p_tb.Text);

            if (start_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@start", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@start", start_tb.Text);

            if (stop_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@stop", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@stop", stop_tb.Text);

            cmd_gene.Parameters.AddWithValue("@variant_type", variant_type_tb.Text);
            cmd_gene.Parameters.AddWithValue("@most_severe_clinical_significance", most_severe_clinical_significance_tb.Text);
            cmd_gene.Parameters.AddWithValue("@minor_allele_1000G", minor_allele_1000G_tb.Text);

            if (MAF_1000G_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@MAF_1000G", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@MAF_1000G", MAF_1000G_tb.Text);

            cmd_gene.Parameters.AddWithValue("@protein_change", protein_change_tb.Text);
            cmd_gene.Parameters.AddWithValue("@variant_allele", variant_allele_tb.Text);
            cmd_gene.Parameters.AddWithValue("@molecular_consequences2", molecular_consequences2_tb.Text);
            cmd_gene.Parameters.AddWithValue("@transcript_change", transcript_change_tb.Text);
            cmd_gene.Parameters.AddWithValue("@condition", condition_tb.Text);
            cmd_gene.Parameters.AddWithValue("@gene_sequential_id", idGene);
            cmd_gene.Parameters.AddWithValue("@uniprprotein_sequential_idot_mut", idProt);

            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Mutation_Clear(sender, e);
                FillMutationTable();
                FillGeneTable();
                MessageBox.Show("The mutation has been updated successfully!");
            }
            catch (Exception exc)
            {
                Mutation_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }
            
        }




        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- Metabolitos-----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void Metabolites_Clear(object sender, RoutedEventArgs e)
        {
            metabolite_sequential_id_tb.Text = "";
            hmdb_id_tb.Text = "";
            common_name_tb.Text = "";
            avg_molecular_weight_tb.Text = "";
            iupac_name_tb.Text = "";
            kingdom_tb.Text = "";
            class_tb.Text = "";
            status_tb.Text = "";
        }

        //done before run time. This creates the whole options
        private void ComboBoxMetabolite_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Metab's concentration values");
            data.Add("Metab's concentration samples");
            data.Add("Metab which concentration is a biofluid");
            data.Add("Metab's cell locations");
            data.Add("Metab's tissue location");
            data.Add("Metab's origins");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxMetabolite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Metabolite_reset(object sender, RoutedEventArgs e)
        {
            FillMutationTable();
        }

        private void Met_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(metabolite_sequential_id_tb.Text))
            {

                // --> Validations
                int idInt;

                
                if (!Int32.TryParse(metabolite_sequential_id_tb.Text, out idInt))
                {
                    MessageBox.Show("The Met ID must be an Integer!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMetab_ID(@id)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@id", idInt);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Metabolite");
                sda.Fill(dt);
                MetabolitesGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(hmdb_id_tb.Text))
            {
                if (hmdb_id_tb.Text.Length > 15)
                {
                    MessageBox.Show("The condition id must have less than 15 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMetab_HMDBID(@hmdb)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@hmdb", hmdb_id_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Metabolite");
                sda.Fill(dt);
                MetabolitesGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(common_name_tb.Text))
            {
                if (common_name_tb.Text.Length > 50)
                {
                    MessageBox.Show("The common name must have less than 50 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMetab_Name(@common)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@common", common_name_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Metabolite");
                sda.Fill(dt);
                MetabolitesGrid.ItemsSource = dt.DefaultView;
            }
            else if (!string.IsNullOrWhiteSpace(class_tb.Text))
            {
                if (class_tb.Text.Length > 50)
                {
                    MessageBox.Show("The common name must have less than 50 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetMetab_Class(@class)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@class", class_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Metabolite");
                sda.Fill(dt);
                MetabolitesGrid.ItemsSource = dt.DefaultView;
            }

            else MessageBox.Show("No fields presented.");

        }

        private void Met_GetRel(object sender, RoutedEventArgs e)
        {
            if (metabolites_rel.SelectedIndex > -1) //somthing was selected
            {
                if (metabolites_rel.SelectedItem.Equals("Metab's concentration values"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_with_concentration_Values()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met1");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
                else if (metabolites_rel.SelectedItem.Equals("Metab's concentration samples"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_concentration_samples()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met2");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
                else if (metabolites_rel.SelectedItem.Equals("Metab which concentration is a biofluid"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_concentration_is_biofluid()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met3");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
                else if (metabolites_rel.SelectedItem.Equals("Metab's cell locations"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_cel_loc()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met4");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
                else if (metabolites_rel.SelectedItem.Equals("Metab's tissue location"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_tissue_loc()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met5");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
                else if (metabolites_rel.SelectedItem.Equals("Metab's origins"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_metab_origins()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Met6");
                    sda.Fill(dt);
                    MetabolitesGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_metab;
                }
            }
        }



        private void MetabolitesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)MetabolitesGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            metabolite_sequential_id_tb.Text = row.Row.ItemArray[0].ToString();
            hmdb_id_tb.Text = row.Row.ItemArray[1].ToString();
            common_name_tb.Text = row.Row.ItemArray[2].ToString();
            avg_molecular_weight_tb.Text = row.Row.ItemArray[3].ToString();
            iupac_name_tb.Text = row.Row.ItemArray[4].ToString();
            kingdom_tb.Text = row.Row.ItemArray[5].ToString();
            class_tb.Text = row.Row.ItemArray[6].ToString();
            status_tb.Text = row.Row.ItemArray[7].ToString();

        }

        private void FillMetaboliteTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_met()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("Metabolite");
            sda.Fill(dt);
            MetabolitesGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        private void Metabolite_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idMet;

            if (!Int32.TryParse(metabolite_sequential_id_tb.Text, out idMet))
            {
                MessageBox.Show("The mutation seq id must be an Integer!");
                return;
            }
            

            // INSERT GENE

            string CmdString = "sp_createMetabolites";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@metabolite_sequential_id", idMet);
            cmd_gene.Parameters.AddWithValue("@hmdb_id", hmdb_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@common_name", common_name_tb.Text);
            if (avg_molecular_weight_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@avg_molecular_weight", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@avg_molecular_weight", avg_molecular_weight_tb.Text);

            cmd_gene.Parameters.AddWithValue("@iupac_name", iupac_name_tb.Text);
            cmd_gene.Parameters.AddWithValue("@kingdom", kingdom_tb.Text);
            cmd_gene.Parameters.AddWithValue("@class", class_tb.Text);
            cmd_gene.Parameters.AddWithValue("@status", status_tb.Text);
         
            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Metabolites_Clear(sender, e);
                FillMetaboliteTable();
                MessageBox.Show("The metabolite has been inserted successfully!");
            }
            catch (Exception exc)
            {
                Metabolites_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }
           
        }
        private void Metabolite_Delete(object sender, RoutedEventArgs e)
        {
            String id;
            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)MetabolitesGrid.SelectedItem;
                id = (drv["Met ID"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

                // bi is number
                if (!Int32.TryParse(id, out idInt))
                {
                    MessageBox.Show("The metabolite ID must be an Integer!");
                    return;
                }

                // DELETE THE METABOLITE

                string CmdString = "sp_deleteMetabolites";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@metabolite_sequential_id", idInt);


                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    Metabolites_Clear(sender, e);
                    FillMetaboliteTable();
                    MessageBox.Show("The metabolite has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    con.Close();
                    Metabolites_Clear(sender, e);
                    MessageBox.Show(exc.Message);
                }
               
            }
        }
        private void Metabolite_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idMet;

            if (!Int32.TryParse(metabolite_sequential_id_tb.Text, out idMet))
            {
                MessageBox.Show("The metabolite seq id must be an Integer!");
                return;
            }


            // INSERT GENE

            string CmdString = "sp_modifyMetabolites";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@metabolite_sequential_id", idMet);
            cmd_gene.Parameters.AddWithValue("@hmdb_id", hmdb_id_tb.Text);
            cmd_gene.Parameters.AddWithValue("@common_name", common_name_tb.Text);

            if (avg_molecular_weight_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@avg_molecular_weight", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@avg_molecular_weight", avg_molecular_weight_tb.Text);

            cmd_gene.Parameters.AddWithValue("@iupac_name", iupac_name_tb.Text);
            cmd_gene.Parameters.AddWithValue("@kingdom", kingdom_tb.Text);
            cmd_gene.Parameters.AddWithValue("@class", class_tb.Text);
            cmd_gene.Parameters.AddWithValue("@status", status_tb.Text);

            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Metabolites_Clear(sender, e);
                FillMetaboliteTable();
                MessageBox.Show("The metabolite has been updated successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Metabolites_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
           
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- PPI -----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void FillPPITable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_ppi()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("PPI");
            sda.Fill(dt);
            PPIGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        //done before run time. This creates the whole options
        private void ComboBoxPPI_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Rel 1");
            data.Add("Rel 2");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxPPI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void PPI_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(db_ppi_tb.Text))
            {

                // --> Validations
                int idInt;

                if (!Int32.TryParse(db_ppi_tb.Text, out idInt))
                {
                    MessageBox.Show("The DB code must be an Integer!");
                    return;
                }
                string CmdString = "SELECT * FROM GetPPI_dbcode(@id)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@id", idInt);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PPI");
                sda.Fill(dt);
                PPIGrid.ItemsSource = dt.DefaultView;
            }
            

            else MessageBox.Show("No fields presented.");
        }

        private void PPI_reset(object sender, RoutedEventArgs e)
        {
            FillPPITable();
        }

        private void PPI_GetRel(object sender, RoutedEventArgs e)
        {
            if (gene_rel.SelectedIndex > -1) //somthing was selected
            {
                if (gene_rel.SelectedItem.Equals("Genes related to proteins"))
                {
                    //comandos que interagem com a BD
                    string CmdString = "SELECT * FROM udf_gene_prot()";
                    //novo comando sql com o que definimos na string
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    con.Open();

                    DataTable dt = new DataTable("Gene rel with prot");
                    sda.Fill(dt);
                    ProteinGrid.ItemsSource = dt.DefaultView;
                    con.Close();
                    tab_control.SelectedItem = tab_prot;

                }
                else if (gene_rel.SelectedItem.Equals("Genes with mutations"))
                {
                    MessageBox.Show("mutation!");

                }
            }

        }
        private void PPI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)PPIGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            db_ppi_tb.Text = row.Row.ItemArray[0].ToString();
            ppi_method_tb.Text = row.Row.ItemArray[1].ToString();
            ppi_throughput_tb.Text = row.Row.ItemArray[2].ToString();
            ppi_combined_tb.Text = row.Row.ItemArray[3].ToString();
            ppi_text_mining_tb.Text = row.Row.ItemArray[4].ToString();
            ppi_coexpr_tb.Text = row.Row.ItemArray[5].ToString();
            ppi_experimental_tb.Text = row.Row.ItemArray[6].ToString();
            ppi_knowledge_tb.Text = row.Row.ItemArray[7].ToString();
            ppi_prot_id1_tb.Text = row.Row.ItemArray[8].ToString();
            ppi_prot_id2_tb.Text = row.Row.ItemArray[9].ToString();
        }

        private void PPI_Clear(object sender, RoutedEventArgs e)
        {
            db_ppi_tb.Text = "";
            ppi_method_tb.Text = "";
            ppi_throughput_tb.Text = "";
            ppi_combined_tb.Text = "";
            ppi_text_mining_tb.Text = "";
            ppi_coexpr_tb.Text = "";
            ppi_experimental_tb.Text = "";
            ppi_knowledge_tb.Text = "";
            ppi_prot_id1_tb.Text = "";
            ppi_prot_id2_tb.Text = "";
        }
        private void PPI_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int db_code, ppi_m, ppi1, ppi2;

          
            if (!Int32.TryParse(db_ppi_tb.Text, out db_code))
            {
                MessageBox.Show("The db code must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_method_tb.Text, out ppi_m))
            {
                MessageBox.Show("The PPI method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_prot_id1_tb.Text, out ppi1))
            {
                MessageBox.Show("The PPI id1 method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_prot_id2_tb.Text, out ppi2))
            {
                MessageBox.Show("The PPI id2 method must be an Integer!");
                return;
            }

            // INSERT PPI

            string CmdString = "sp_createPPI";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_code_ppi", db_code);
            cmd_gene.Parameters.AddWithValue("@ppi_method", ppi_m);
            cmd_gene.Parameters.AddWithValue("@ppi_throughput", ppi_throughput_tb.Text);

            if (ppi_combined_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_combined", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_combined", ppi_combined_tb.Text);

            if (ppi_text_mining_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_text_mining", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_text_mining", ppi_text_mining_tb.Text);

            if (ppi_coexpr_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_coexpr", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_coexpr", ppi_coexpr_tb.Text);

            if (ppi_experimental_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_experimental", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_experimental", ppi_experimental_tb.Text);

            if (ppi_knowledge_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_knowledge", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_knowledge", ppi_knowledge_tb.Text);

            cmd_gene.Parameters.AddWithValue("@ppi_prot_id1", ppi1);
            cmd_gene.Parameters.AddWithValue("@ppi_prot_id2", ppi2);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                PPI_Clear(sender, e);
                FillPPITable();
                MessageBox.Show("The PPI has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                PPI_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
            


        }
        private void PPI_Delete(object sender, RoutedEventArgs e)
        {
            String result1, result2;


            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)PPIGrid.SelectedItem;
                result1 = (drv["Prot 1"]).ToString();

                result2 = (drv["Prot 2"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt, idInt2;

                if (!Int32.TryParse(result1, out idInt))
                {
                    MessageBox.Show("The ppi1 must be an Integer!");
                    return;
                }

                if (!Int32.TryParse(result2, out idInt2))
                {
                    MessageBox.Show("The ppi2 must be an Integer!");
                    return;
                }

                // DELETE THE PPI

                string CmdString = "sp_deletePPI";
                SqlCommand cmd_player = new SqlCommand(CmdString, con);
                cmd_player.CommandType = CommandType.StoredProcedure;
                cmd_player.Parameters.AddWithValue("@ppi_prot_id1", idInt);
                cmd_player.Parameters.AddWithValue("@ppi_prot_id2", idInt2);

                try
                {
                    con.Open();
                    cmd_player.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    PPI_Clear(sender, e);
                    FillPPITable();
                    MessageBox.Show("The PPI has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    PPI_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }
                
            }

        }
        private void PPI_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int db_code, ppi_m, ppi1, ppi2;


            if (!Int32.TryParse(db_ppi_tb.Text, out db_code))
            {
                MessageBox.Show("The db code must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_method_tb.Text, out ppi_m))
            {
                MessageBox.Show("The PPI method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_prot_id1_tb.Text, out ppi1))
            {
                MessageBox.Show("The PPI id1 method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(ppi_prot_id2_tb.Text, out ppi2))
            {
                MessageBox.Show("The PPI id2 method must be an Integer!");
                return;
            }

            // INSERT PPI

            string CmdString = "sp_modifyPPI";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_code_ppi", db_code);
            cmd_gene.Parameters.AddWithValue("@ppi_method", ppi_m);
            cmd_gene.Parameters.AddWithValue("@ppi_throughput", ppi_throughput_tb.Text);

            if (ppi_combined_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_combined", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_combined", ppi_combined_tb.Text);

            if (ppi_text_mining_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_text_mining", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_text_mining", ppi_text_mining_tb.Text);

            if (ppi_coexpr_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_coexpr", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_coexpr", ppi_coexpr_tb.Text);

            if (ppi_experimental_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_experimental", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_experimental", ppi_experimental_tb.Text);

            if (ppi_knowledge_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@ppi_knowledge", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@ppi_knowledge", ppi_knowledge_tb.Text);

            cmd_gene.Parameters.AddWithValue("@ppi_prot_id1", ppi1);
            cmd_gene.Parameters.AddWithValue("@ppi_prot_id2", ppi2);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                PPI_Clear(sender, e);
                FillPPITable();
                MessageBox.Show("The PPI has been updated successfully!");
            }
            catch (Exception exc)
            {
                PPI_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }
            

        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- PCI -----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void FillPCITable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_pci()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("PCI");
            sda.Fill(dt);
            PCIGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        //done before run time. This creates the whole options
        private void ComboBoxPCI_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Rel 1");
            data.Add("Rel 2");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxPCI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void PCI_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(db_code_pci_tb.Text))
            {

                // --> Validations
                int idInt;

                if (!Int32.TryParse(db_code_pci_tb.Text, out idInt))
                {
                    MessageBox.Show("The DB code must be an Integer!");
                    return;
                }
                string CmdString = "SELECT * FROM GetPCI_dbcode(@id)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@id", idInt);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PCI");
                sda.Fill(dt);
                PCIGrid.ItemsSource = dt.DefaultView;
            }


            else MessageBox.Show("No fields presented.");

        }

        private void PCI_reset(object sender, RoutedEventArgs e)
        {
            FillPCITable();
        }

        private void PCI_GetRel(object sender, RoutedEventArgs e)
        {
           

        }
        private void PCI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)PPIGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            db_code_pci_tb.Text = row.Row.ItemArray[0].ToString();
            pci_method_tb.Text = row.Row.ItemArray[1].ToString();
            pci_throughput_tb.Text = row.Row.ItemArray[2].ToString();
            pci_combined_tb.Text = row.Row.ItemArray[3].ToString();
            pci_text_mining_tb.Text = row.Row.ItemArray[4].ToString();
            pci_coexpr_tb.Text = row.Row.ItemArray[5].ToString();
            pci_experimental_tb.Text = row.Row.ItemArray[6].ToString();
            pci_knowledge_tb.Text = row.Row.ItemArray[7].ToString();
            pci_prot_id_tb.Text = row.Row.ItemArray[8].ToString();
            pci_metab_id_tb.Text = row.Row.ItemArray[8].ToString();
        }

        private void PCI_Clear(object sender, RoutedEventArgs e)
        {
            db_code_pci_tb.Text = "";
            pci_method_tb.Text = "";
            pci_throughput_tb.Text = "";
            pci_combined_tb.Text = "";
            pci_text_mining_tb.Text = "";
            pci_coexpr_tb.Text = "";
            pci_experimental_tb.Text = "";
            pci_knowledge_tb.Text = "";
            pci_prot_id_tb.Text = "";
            pci_metab_id_tb.Text = "";
        }
        private void PCI_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int db_code, pci_m, pid, mid;


            if (!Int32.TryParse(db_code_pci_tb.Text, out db_code))
            {
                MessageBox.Show("The db code must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_method_tb.Text, out pci_m))
            {
                MessageBox.Show("The PCI method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_prot_id_tb.Text, out pid))
            {
                MessageBox.Show("The PCI prot id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_metab_id_tb.Text, out mid))
            {
                MessageBox.Show("The PCI metab id must be an Integer!");
                return;
            }

            // INSERT PPI

            string CmdString = "sp_createPCI";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_code_pci", db_code);
            cmd_gene.Parameters.AddWithValue("@pci_method", pci_m);
            cmd_gene.Parameters.AddWithValue("@pci_throughput", pci_throughput_tb.Text);

            if (pci_combined_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_combined", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_combined", pci_combined_tb.Text);

            if (pci_text_mining_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_text_mining", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_text_mining", pci_text_mining_tb.Text);

            if (pci_coexpr_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_coexpr", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_coexpr", pci_coexpr_tb.Text);

            if (pci_experimental_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_experimental", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_experimental", pci_experimental_tb.Text);

            if (pci_knowledge_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_knowledge", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_knowledge", pci_knowledge_tb.Text);

            cmd_gene.Parameters.AddWithValue("@pci_prot_id", pid);
            cmd_gene.Parameters.AddWithValue("@pci_metab_id", mid);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                PCI_Clear(sender, e);
                FillPCITable();
                MessageBox.Show("The PCI has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                PCI_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
        }
        private void PCI_Delete(object sender, RoutedEventArgs e)
        {
            String result1, result2;


            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)PPIGrid.SelectedItem;
                result1 = (drv["Protein"]).ToString();

                result2 = (drv["Metab"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt, idInt2;

                if (!Int32.TryParse(result1, out idInt))
                {
                    MessageBox.Show("The pci prot id must be an Integer!");
                    return;
                }

                if (!Int32.TryParse(result2, out idInt2))
                {
                    MessageBox.Show("The pci met id must be an Integer!");
                    return;
                }

                // DELETE THE PCI

                string CmdString = "sp_deletePCI";
                SqlCommand cmd_player = new SqlCommand(CmdString, con);
                cmd_player.CommandType = CommandType.StoredProcedure;
                cmd_player.Parameters.AddWithValue("@pci_prot_id", idInt);
                cmd_player.Parameters.AddWithValue("@pci_metab_id", idInt2);

                try
                {
                    con.Open();
                    cmd_player.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    PCI_Clear(sender, e);
                    FillPCITable();
                    MessageBox.Show("The PCI has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    PCI_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }

            }

        }
        private void PCI_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int db_code, pci_m, pid, mid;


            if (!Int32.TryParse(db_code_pci_tb.Text, out db_code))
            {
                MessageBox.Show("The db code must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_method_tb.Text, out pci_m))
            {
                MessageBox.Show("The PCI method must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_prot_id_tb.Text, out pid))
            {
                MessageBox.Show("The PCI prot id must be an Integer!");
                return;
            }
            if (!Int32.TryParse(pci_metab_id_tb.Text, out mid))
            {
                MessageBox.Show("The PCI metab id must be an Integer!");
                return;
            }

            // INSERT PCI

            string CmdString = "sp_modifyPCI";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_code_pci", db_code);
            cmd_gene.Parameters.AddWithValue("@pci_method", pci_m);
            cmd_gene.Parameters.AddWithValue("@pci_throughput", pci_throughput_tb.Text);

            if (pci_combined_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_combined", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_combined", pci_combined_tb.Text);

            if (pci_text_mining_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_text_mining", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_text_mining", pci_text_mining_tb.Text);

            if (pci_coexpr_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_coexpr", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_coexpr", pci_coexpr_tb.Text);

            if (pci_experimental_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_experimental", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_experimental", pci_experimental_tb.Text);

            if (pci_knowledge_tb.Text.Equals(""))
                cmd_gene.Parameters.AddWithValue("@pci_knowledge", DBNull.Value);
            else
                cmd_gene.Parameters.AddWithValue("@pci_knowledge", pci_knowledge_tb.Text);

            cmd_gene.Parameters.AddWithValue("@ppi_prot_id1", pid);
            cmd_gene.Parameters.AddWithValue("@ppi_prot_id2", mid);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                PCI_Clear(sender, e);
                FillPCITable();
                MessageBox.Show("The PCI has been updated successfully!");
            }
            catch (Exception exc)
            {
                PCI_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }


        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
       *  ##########################----------- DB origin -----------##########################
       * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private void FillDbTable()
        {
            //comandos que interagem com a BD
            string CmdString = "SELECT * FROM udf_show_db()";
            //novo comando sql com o que definimos na string
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Open();

            DataTable dt = new DataTable("DBo");
            sda.Fill(dt);
            DBoGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        //done before run time. This creates the whole options
        private void ComboBoxDBO_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Rel 1");
            data.Add("Rel 2");
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

        }
        //done in real time in order to change the displayed item first when it is selected
        private void ComboBoxDBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Dbo_Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(url_proj_tb.Text))
            {
                if (url_proj_tb.Text.Length > 100)
                {
                    MessageBox.Show("The symbol must have less than 100 characters!");
                    return;
                }
                string CmdString = "SELECT * FROM GetDB_url(@url)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@url", url_proj_tb.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("DBorigin");
                sda.Fill(dt);
                DBoGrid.ItemsSource = dt.DefaultView;
            }


            else MessageBox.Show("No fields presented.");



        }

        private void DBo_reset(object sender, RoutedEventArgs e)
        {
            FillDbTable();
        }

        private void DBo_GetRel(object sender, RoutedEventArgs e)
        {


        }
        private void Dbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)PPIGrid.SelectedItem;
            string search_id;
            try
            {
                // quando autalizamos a DataGrid numa segunda vez e havia algo selecionado antes...
                search_id = row.Row.ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return;
            }


            db_sequential_id_tb.Text = row.Row.ItemArray[0].ToString();
            name_db_origin_tb.Text = row.Row.ItemArray[1].ToString();
            url_proj_tb.Text = row.Row.ItemArray[2].ToString();
            info_type_tb.Text = row.Row.ItemArray[3].ToString();
           
        }

        private void Dbo_Clear(object sender, RoutedEventArgs e)
        {
            db_sequential_id_tb.Text = "";
            name_db_origin_tb.Text = "";
            url_proj_tb.Text = "";
            info_type_tb.Text = "";
        }
        private void Dbo_New(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int db_code;


            if (!Int32.TryParse(db_sequential_id_tb.Text, out db_code))
            {
                MessageBox.Show("The db seq id must be an Integer!");
                return;
            }

            // INSERT DBO

            string CmdString = "sp_createDBorigin";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_sequential_id", db_code);
            cmd_gene.Parameters.AddWithValue("@name_db_origin", name_db_origin_tb.Text);
            cmd_gene.Parameters.AddWithValue("@url_proj", url_proj_tb.Text);
            cmd_gene.Parameters.AddWithValue("@info_type", info_type_tb.Text);


            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Dbo_Clear(sender, e);
                FillDbTable();
                MessageBox.Show("The DB origin has been inserted successfully!");
            }
            catch (Exception exc)
            {
                con.Close();
                Dbo_Clear(sender, e);
                MessageBox.Show(exc.Message);
            }
        }
        private void Dbo_Delete(object sender, RoutedEventArgs e)
        {
            String result1;


            //verificar se linha esta selecionada
            try
            {
                DataRowView drv = (DataRowView)DBoGrid.SelectedItem;
                result1 = (drv["DB ID"]).ToString();
            }
            catch
            {
                MessageBox.Show("You must select a row from the table!");
                return;

            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // --> Validations
                int idInt;

                if (!Int32.TryParse(result1, out idInt))
                {
                    MessageBox.Show("The db seq id prot id must be an Integer!");
                    return;
                }

                // DELETE THE PLAYER


                string CmdString = "sp_deleteDBorigin";
                SqlCommand cmd_gene = new SqlCommand(CmdString, con);
                cmd_gene.CommandType = CommandType.StoredProcedure;
                cmd_gene.Parameters.AddWithValue("@db_sequential_id", idInt);


                try
                {
                    con.Open();
                    cmd_gene.ExecuteNonQuery();
                    con.Close();

                    // limpar as text boxs
                    Dbo_Clear(sender, e);
                    FillDbTable();
                    MessageBox.Show("The DBorigin has been deleted successfully!");
                }
                catch (Exception exc)
                {
                    Dbo_Clear(sender, e);
                    con.Close();
                    MessageBox.Show(exc.Message);
                }

            }
        }
        private void Dbo_Update(object sender, RoutedEventArgs e)
        {
            // --> Validations
            int idInt;

            if (!Int32.TryParse(db_sequential_id_tb.Text, out idInt))
            {
                MessageBox.Show("The db seq id id must be an Integer!");
                return;
            }


            // INSERT PCI

            string CmdString = "sp_modifyDBorigin";
            SqlCommand cmd_gene = new SqlCommand(CmdString, con);
            cmd_gene.CommandType = CommandType.StoredProcedure;
            cmd_gene.Parameters.AddWithValue("@db_sequential_id", idInt);
            cmd_gene.Parameters.AddWithValue("@name_db_origin", name_db_origin_tb.Text);
            cmd_gene.Parameters.AddWithValue("@url_proj", url_proj_tb.Text);
            cmd_gene.Parameters.AddWithValue("@info_type_tb", info_type_tb.Text);

         

            try
            {
                con.Open();
                cmd_gene.ExecuteNonQuery();
                con.Close();
                Dbo_Clear(sender, e);
                FillDbTable();
                MessageBox.Show("The DBorigin has been deleted successfully!");
            }
            catch (Exception exc)
            {
                Dbo_Clear(sender, e);
                con.Close();
                MessageBox.Show(exc.Message);
            }


        }






    }
}
