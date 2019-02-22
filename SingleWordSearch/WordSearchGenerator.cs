using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SingleWordSearch
{
    public class WordSearchGenerator
    {
        private bool bShowClues { get; set; }
        protected List<string> listWords { get; set; }
        protected List<string> listClues { get; set; }
        protected Dictionary<int, string> dictIdClues { get; set; }
        protected int gridLength { get; set; }
        protected Table tableCreating { get; set; }
        protected Random rand { get; set; }
        protected List<string> listUsersFoundWords { get; set; }

        public WordSearchGenerator(Dictionary<int, string> dictWords, bool bClues)
        {
            dictIdClues = new Dictionary<int, string>();
            listWords = new List<string>();
            listClues = new List<string>();

            foreach (KeyValuePair<int, string> wrd in dictWords)
            {
                listWords.Add(wrd.Value);
                listClues.Add(wrd.Value);
                dictIdClues.Add(wrd.Key, wrd.Value + ";" + wrd.Value);
            }

            bShowClues = bClues;

            gridLength = GetSize(listWords) + 5;
            tableCreating = new Table();

            if (rand == null)
            {
                rand = new Random();
            }
        }

        public void GenerateTable(WebControl wordSearchPlaceholder, Panel cluesPlaceHolder)
        {
            bool bSuccessful = false;
            while (!bSuccessful)
            {
                TryGenerateTable(wordSearchPlaceholder, cluesPlaceHolder, out bSuccessful);
            }
        }

        private void TryGenerateTable(WebControl wordSearchPlaceholder, Panel cluesPlaceHolder, out bool bSuccess)
        {
            wordSearchPlaceholder.Controls.Clear();
            cluesPlaceHolder.Controls.Clear();
            tableCreating = new Table();
            List<CheckBox> listCluesCheck = new List<CheckBox>();

            tableCreating.Style.Add("width", "auto");
            tableCreating.Style.Add("margin-right", "auto");
            tableCreating.Style.Add("margin-left", "auto");
            tableCreating.ID = "tableWordSearch";

            for (int i = 0; i < gridLength; i++)
            {
                int y = i;
                TableRow tr = new TableRow();
                tr.ID = "row_" + y;
                for (int j = 0; j < gridLength; j++)
                {
                    int x = j;
                    TableCell tCell = GenerateCell(x, y);
                    tr.Controls.Add(tCell);
                }
                tableCreating.Controls.Add(tr);
            }

            try
            {
                PopulateTable();
                listCluesCheck = GenerateClues();
            }
            catch (Exception ex)
            {
                bSuccess = false;
                return;
            }

            int iCells = new int();

            foreach (string s in listWords)
            {
                iCells += s.Length;
            }

            List<TableCell> listCellsExpected = new List<TableCell>();
            foreach (TableRow row in tableCreating.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Attributes["data-filled"] == "true")
                    {
                        listCellsExpected.Add(cell);
                    }
                }
            }

            if (listCellsExpected.Count < iCells)
            {
                bSuccess = false;
                return;
            }

            wordSearchPlaceholder.Controls.Add(tableCreating);


            if (listCluesCheck.Count > listClues.Count)
            {
                listCluesCheck.Clear();
                listCluesCheck = GenerateClues();
            }

            foreach (CheckBox check in listCluesCheck)
            {
                cluesPlaceHolder.Controls.Add(check);
            }

            if (listCluesCheck.Count > 0)
            {
                while (listCluesCheck.Count < cluesPlaceHolder.Controls.Count)
                {
                    cluesPlaceHolder.Controls.RemoveAt(cluesPlaceHolder.Controls.Count - 1);
                }
            }

            bSuccess = true;

        }

        private void PopulateTable()
        {
            foreach (string s in listWords)
            {

                char[] acLetters = s.ToCharArray();
                List<TableCell> cells = new List<TableCell>();
                while (cells.Count <= 0)
                {
                    cells = GetCells(s);
                }
                int i = new int();
                foreach (TableCell c in cells)
                {
                    c.CssClass = "WordSearchTableCell";

                    int x = int.Parse(c.Attributes["data-x"]);
                    int y = int.Parse(c.Attributes["data-y"]);

                    c.Text = acLetters[i].ToString();
                    c.Attributes["data-filled"] = "true";

                    try
                    {
                        c.ID = acLetters[i].ToString();
                    }
                    catch
                    {
                        c.ID = acLetters[i].ToString() + "_" + rand.Next().ToString();
                    }

                    i++;
                }
            }
        }

        private List<TableCell> GetCells(string word)
        {
            List<TableCell> cells = new List<TableCell>();

            char[] acLetters = new char[word.Length];
            acLetters = word.ToCharArray();
            string sDirection = GetDirection();

            int iStartX = rand.Next(gridLength + 1);
            int iStartY = rand.Next(gridLength + 1);

            if (sDirection.Contains("RIGHT"))
            {
                if ((iStartX + acLetters.Length) > gridLength)
                {
                    iStartX = gridLength - acLetters.Length;
                }
            }

            if (sDirection.Contains("LEFT"))
            {
                if ((iStartX - acLetters.Length) < 0)
                {
                    iStartX = 0 + acLetters.Length;
                }
            }

            if (sDirection.Contains("DOWN"))
            {
                if ((iStartY + acLetters.Length) > gridLength)
                {
                    iStartY = gridLength - iStartY;
                }
            }

            if (sDirection.Contains("UP"))
            {
                if ((iStartY - acLetters.Length) < 0)
                {
                    iStartY = 0 + iStartY;
                }
            }

            int iEndX = new int();
            if (sDirection.Contains("RIGHT")) iEndX = iStartX + acLetters.Length;
            else if (sDirection == "UP" || sDirection == "DOWN") iEndX = iStartX;
            else if (sDirection.Contains("LEFT")) iEndX = iStartX - acLetters.Length;

            int iEndY = new int();
            if (sDirection.Contains("DOWN")) iEndY = iStartY + acLetters.Length;
            else if (sDirection == "RIGHT" || sDirection == "LEFT") iEndY = iStartY;
            else if (sDirection.Contains("UP")) iEndY = iStartY - acLetters.Length;

            if (iEndY > gridLength)
            {
                int iDiff = iEndY - gridLength;
                iStartY = iStartY - iDiff;
                iEndY = iEndY - iDiff;
            }
            else if (iStartY < 0)
            {
                int iDiff = iStartY * -1;
                iStartY += iDiff;
                iEndY += iDiff;
            }

            if (iEndX > gridLength)
            {
                int iDiff = iEndX - gridLength;
                iStartX = iStartX - iDiff;
                iEndX = iEndX - iDiff;
            }
            else if (iStartX < 0)
            {
                int iDiff = iStartX * -1;
                iStartX += iDiff;
                iEndX += iDiff;
            }


            int y = new int();
            int x = new int();

            if (sDirection == "UP")
            {
                x = iStartX;
                for (y = iEndY; y < iStartY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                }
            }
            else if (sDirection == "DOWN")
            {
                x = iStartX;
                for (y = iStartY; y < iEndY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                }
            }
            else if (sDirection == "RIGHT")
            {
                y = iStartY;
                TableRow tr = tableCreating.Rows[y];
                for (x = iStartX; x < iEndX; x++)
                {
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                }
            }
            else if (sDirection == "LEFT")
            {
                y = iStartY;
                TableRow tr = tableCreating.Rows[y];
                for (x = iEndX; x < iStartX; x++)
                {
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                }
            }
            else if (sDirection == "UP-RIGHT")
            {
                x = iStartX;
                for (y = iEndY; y < iStartY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                    x++;
                }
            }
            else if (sDirection == "UP-LEFT")
            {
                x = iStartX;
                for (y = iEndY; y < iStartY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                    x--;
                }
            }
            else if (sDirection == "DOWN-LEFT")
            {
                x = iStartX;

                for (y = iStartY; y < iEndY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                    x--;
                }
            }
            else if (sDirection == "DOWN-RIGHT")
            {
                x = iStartX;
                for (y = iStartY; y < iEndY; y++)
                {
                    if (y < 0)
                    {
                        iEndY += (y * -1);
                        y = 0;

                    }
                    TableRow tr = tableCreating.Rows[y];
                    TableCell tcell = tr.Cells[x];
                    if (tcell.Attributes["data-filled"] == "true")
                    {
                        cells = new List<TableCell>();
                        return cells;
                    }
                    cells.Add(tr.Cells[x]);
                    x++;
                }
            }

            if (sDirection.Contains("LEFT") || sDirection.Contains("UP"))
            {
                TableCell[] a = cells.ToArray();
                Array.Reverse(a);
                cells = a.ToList();
            }

            return cells;
        }

        private TableCell GenerateCell(int x, int y)
        {
            TableCell cell = new TableCell();
            cell.CssClass = "WordSearchTableCell";
            cell.Attributes.Add("data-x", x.ToString());
            cell.Attributes.Add("data-y", y.ToString());
            cell.Attributes.Add("data-filled", "false");
            cell.Attributes.Add("data-found", "false");
            cell.Text = GetRandomLetter();
            return cell;
        }

        private string GetRandomLetter()
        {
            char letter;
            int iRandInt = rand.Next(26);
            letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[iRandInt];
            if (letter == '\0')
            {
                letter = '5';
            }

            return letter.ToString();
        }

        private string GetDirection()
        {
            string direction = "";
            int iRandNumber = rand.Next(0, 8);
            switch (iRandNumber)
            {
                case 0:
                    direction = "UP";
                    break;
                case 1:
                    direction = "UP-RIGHT";
                    break;
                case 2:
                    direction = "LEFT";
                    break;
                case 3:
                    direction = "DOWN-RIGHT";
                    break;
                case 4:
                    direction = "DOWN";
                    break;
                case 5:
                    direction = "DOWN-LEFT";
                    break;
                case 6:
                    direction = "RIGHT";
                    break;
                case 7:
                    direction = "UP-LEFT";
                    break;
                default:
                    direction = "LEFT";
                    break;
            }
            return direction;
        }

        public int GetSize(List<string> words)
        {
            int imaxSize = new int();
            imaxSize = words[0].Length;

            foreach (string s in words)
            {
                if (s.Length > imaxSize) imaxSize = s.Length;
                else continue;
            }

            return imaxSize;
        }

        public List<CheckBox> GenerateClues()
        {
            List<CheckBox> listCheckboxes = new List<CheckBox>();

            for (int i = 0; i < dictIdClues.Count; i++)
            {
                int iId = dictIdClues.Keys.ToList()[i];
                string sText = dictIdClues[iId].Split(';')[0];

                if (bShowClues)
                {
                    sText = dictIdClues[iId].Split(';')[1];
                }

                CheckBox checkClue = new CheckBox();
                checkClue.Text = sText;
                checkClue.AutoPostBack = false;
                checkClue.ToolTip = sText;
                checkClue.Enabled = false;
                checkClue.ID = "clue_" + listWords[i];
                checkClue.Width = Unit.Percentage(50);
                checkClue.Attributes.Add("data-Id", iId.ToString());

                checkClue.Checked = false;
                listCheckboxes.Add(checkClue);
            }

            return listCheckboxes;
        }

    }
}
