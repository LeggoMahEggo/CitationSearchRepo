using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CitationSearchNew
{
    public partial class Form1 : Form
    {
        //private string _iDirectory = "C:\\Users\\Yehuda\\Desktop\\Citation Project"; // Temp, hardcoded directory path
        private string _masechet = "";
        private List<string> _mishnas = null;
        private List<string> _gemaras = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMasechet();
        }

        // User methods

        // Loads a masechet into memory
        private void LoadMasechet()
        {
            // Open load file dialog
            using (OpenFileDialog loadWindow = new OpenFileDialog())
            {
                // Set initial values
                loadWindow.InitialDirectory = "C:\\"; //_iDirectory;
                loadWindow.DefaultExt = ".txt";
                loadWindow.Title = "Load masechet...";
                loadWindow.Filter = "Text files (*.txt)|*.txt";

                if (loadWindow.ShowDialog() == DialogResult.OK)
                {
                    string filePath = loadWindow.FileName;
                    string[] lines = File.ReadAllLines(filePath); // Read file into memory
                    _masechet = String.Join(" ", lines); // Concatenate lines
                    pnlControls.Visible = true; // Enable panel with controls

                    // Add mishnas to select
                    _mishnas = Methods.ExtractMishnas(_masechet);
                    _gemaras = Methods.ExtractGemaras(_masechet);


                    PopulateMishnaList(); // Fill ListBox with mishnas
                    txtMishna.Text = _mishnas[0]; // Load mishna into a textbox
                    lstMishna.SelectedIndex = 0; // Select first entry in the ListBox
                    ShowQuotes(); // List all quotes from the particular mishna
                    ShowQuoteOccurrences(); // List # of times a particular quote is quoted
                                            //     in the gemara (in order by quote) 
                }
            }
        }

        // Fills a listbox with mishnas ranging from 1 to n, where n
        //     is the total count of mishnas in the masechet
        private void PopulateMishnaList()
        {
            if (lstMishna.Items.Count > 0)
                lstMishna.Items.Clear();

            int mishnaCount = _mishnas.Count;
            for (int i = 0; i < mishnaCount; i++)
                lstMishna.Items.Add(String.Format("Mishna {0}", i + 1));
        }

        // Shows list of quotes used by the gemara from the mishna
        private void ShowQuotes()
        {
            if (lstMishnaQuotes.Items.Count > 0)
                lstMishnaQuotes.Items.Clear();

            // Extract all quotes used by the gemara for that mishna
            List<string> quotes = Methods.ExtractAllQuotes(_mishnas, _gemaras,
                lstMishna.SelectedIndex);

            int quoteCount = quotes.Count;
            for (int i = 0; i < quoteCount; i++)
                lstMishnaQuotes.Items.Add(quotes[i]);
            lstMishnaQuotes.SelectedIndex = 0;
        }

        // Show number of times that each quote is quoted by the gemara for the selected mishna
        private void ShowQuoteOccurrences()
        {
            if (lstQuoteNumberList.Items.Count > 0)
                lstQuoteNumberList.Items.Clear();

            // If there are no quotes for the mishna, do not add # of times quoted
            if (lstMishnaQuotes.Items[0].ToString() == "NONE")
            {
                lstQuoteNumberList.Items.Add("NONE");
                return;
            }

            // List # of times each quote is quoted
            for (int i = 0; i < lstMishnaQuotes.Items.Count; i++)
            {
                int occurences = 0;
                string quote = lstMishnaQuotes.Items[i].ToString();
                //for (int j = 0; j < _gemaras.Count; j++)
                occurences += Regex.Matches(_gemaras[lstMishna.SelectedIndex], quote).Count;
                lstQuoteNumberList.Items.Add(occurences);
            }
        }

        // Saves a list of quotes and the # of times they are consecutively quoted to a CSV file
        private void SaveConsecutiveQuotesToCSV()
        {
            // A simple check to make sure that the # of mishnas and gemaras are equal
            if (_mishnas.Count != _gemaras.Count)
            {
                MessageBox.Show("The number of mishnas are not equal to the number of Gemaras!"
                    + " Exiting attempt!" + "\n\n" 
                    + string.Format("Mishna count: {0}\nGemara count: {1}", _mishnas.Count, 
                    _gemaras.Count));
                return;
            }

            List<string> quotes = Methods.ExtractAllQuotes(_mishnas, _gemaras); // Get all quotes
            Dictionary<string, int> qConOccurrences = new Dictionary<string, int>();

            string headQuote = ""; // The quote we are comparing to consecutive quotes
            for (int i = 0; i < quotes.Count; i++)
            {
                // Setup the initial consecutive quote count
                if (i == 0)
                {
                    headQuote = quotes[i];
                    qConOccurrences.Add(headQuote, 0);
                    continue;
                }

                // If the next quote is the same, similar, or contained in the current one, then 
                //     +1 consecutive quote
                // Note: the maximum distance allowed is 4, to allow for the addition of 3- letter
                //     words that would make the quotes otherwise identical
                if (quotes[i] == headQuote 
                    || Methods.LevenshteinDistance(headQuote, quotes[i]) <= 2
                    || headQuote.Contains(quotes[i])
                    )
                {
                    int num = qConOccurrences[headQuote];
                    qConOccurrences[headQuote] = num + 1; // Update entry
                } else
                {
                    headQuote = quotes[i]; // Otherwise, starting counting consecutive quotes
                                           //     after the current one
                    if (!qConOccurrences.ContainsKey(headQuote))
                        qConOccurrences.Add(headQuote, 0);
                }
            }

            // Open save file dialog and set intial values
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Title = "Saving output of quote occurrences...";
            sDialog.FileName = "output";
            sDialog.Filter = "CSV files (*.csv)|*.csv";
            sDialog.FilterIndex = 1;
            sDialog.RestoreDirectory = true;

            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                // Open stream for writing data
                using (FileStream fStream = new FileStream(sDialog.FileName,
                    FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter sWriter = new StreamWriter(fStream, Encoding.UTF8))
                {
                    List<string> conQuotes = qConOccurrences.Keys.ToList();
                    List<int> conAmount = qConOccurrences.Values.ToList();

                    sWriter.WriteLine("Quote,Consecutive Occurrences"); // Header for the file
                    for (int i = 0; i < conQuotes.Count; i++)
                        sWriter.WriteLine(conQuotes[i] + "," + conAmount[i].ToString());
                }
                MessageBox.Show("File saved!");
            }
            else
                MessageBox.Show("File not saved!");
        }

        // DEPRECATED - saved in case some of the code here is needed
        // Saves a list of quotes and the # of times they are consecutively quoted to a CSV file
        /*private void SaveConsecutiveQuotesToCSV()
        {
            // A simple check to make sure that the # of mishnas and gemaras are equal
            if (_mishnas.Count != _gemaras.Count)
            {
                MessageBox.Show("The number of mishnas are not equal to the number of Gemaras!"
                    + " Exiting attempt!");
                return;
            }

            List<string> quotes = Methods.ExtractAllQuotes(_mishnas, _gemaras); // Get all quotes
            Dictionary<int, string> qByPosition = new Dictionary<int, string>(); // Position a
                    // quote is found, and the particular quote
            List<int> positions = new List<int>(); // A simple list of the positions
            int totalLength = 0; // Length of each gemara added together

            // Add all quotes and their start positions to a dictionary (and the positions to a list)
            for (int i = 0; i < quotes.Count; i++)
            {
                for (int j = 0; j < _gemaras.Count; j++)
                {
                    // Brackets in a quote can mess up regex
                    MatchCollection qColl = Regex.Matches(_gemaras[j],
                        quotes[i].Replace("[", "\\[").Replace("]", "\\]"));
                    for (int a = 0; a < qColl.Count; a++)
                    {
                        // Only add a position with quote if the position isn't already added
                        if (!qByPosition.ContainsKey(qColl[a].Index + totalLength))
                        {
                            positions.Add(qColl[a].Index + totalLength);
                            qByPosition.Add(qColl[a].Index + totalLength, quotes[i]);
                        }
                    }
                    if (j > 0) // Add the current gemara's length to the total length
                        totalLength += _gemaras[j].Length;
                }
                totalLength = 0;
            }

            Dictionary<string, int> qConOccurrences = new Dictionary<string, int>();
            positions.Sort(); // Sort the positions

            // Add entries to a dictionary that keeps track of the # of occurrences for each quote
            for (int i = 0; i < quotes.Count; i++)
                if (!qConOccurrences.ContainsKey(quotes[i]))
                    qConOccurrences.Add(quotes[i], 0);

           for (int i = 0; i < positions.Count; i++)
            {
                if (i + 1 < positions.Count)
                {
                    string cQuote = qByPosition[positions[i]];
                    string nQuote = qByPosition[positions[i + 1]];

                    // If the next quote is the same or similar as the current one, then 
                    //     +1 consecutive quote
                    if (cQuote == nQuote || Methods.LevenshteinDistance(cQuote, nQuote) <= 2)
                    {
                        int num = qConOccurrences[cQuote];
                        qConOccurrences[cQuote] = num + 1; // Update entry
                    }
                }
            }

            // Open save file dialog and set intial values
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Title = "Saving output of quote occurrences...";
            sDialog.FileName = "output";
            sDialog.Filter = "CSV files (*.csv)|*.csv";
            sDialog.FilterIndex = 1;
            sDialog.RestoreDirectory = true;

            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                // Open stream for writing data
                using (FileStream fStream = new FileStream(sDialog.FileName,
                    FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter sWriter = new StreamWriter(fStream, Encoding.UTF8))
                {
                    List<string> conQuotes = qConOccurrences.Keys.ToList();
                    List<int> conAmount = qConOccurrences.Values.ToList();

                    sWriter.WriteLine("Quote,Consecutive Occurrences"); // Header for the file
                    for (int i = 0; i < conQuotes.Count; i++)
                        sWriter.WriteLine(conQuotes[i] + "," + conAmount[i].ToString());
                }
                MessageBox.Show("File saved!");
            }
            else
                MessageBox.Show("File not saved!");
        }*/

        // Update ListBoxes and textbox when a different mishna is selected
        private void lstMishna_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMishna.Text = _mishnas[lstMishna.SelectedIndex];
            ShowQuotes();
            ShowQuoteOccurrences();
        }

        private void btnSaveConsecutiveQuotes_Click(object sender, EventArgs e)
        {
            SaveConsecutiveQuotesToCSV();
        }
    }
}
