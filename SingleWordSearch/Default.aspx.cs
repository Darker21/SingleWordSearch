using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SingleWordSearch
{
    public partial class _Default : Page
    {
        private Dictionary<int, string> dictPuzzle1 = new Dictionary<int, string>() { { 0, "ESTIO" }, { 1, "MTA" }, { 2, "SOFTWARE" }, { 3, "WORLDWIDEWEB" } };

        protected void Page_PreInit(object sender, EventArgs e)
        {
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WordSearchGenerator gen = new WordSearchGenerator(dictPuzzle1, false);
            gen.GenerateTable(panelWordSearch, PanelWordsToFind);
        }
    }
}