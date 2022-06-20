using APIModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace PreFinal
{
    class WordHelper
    {
        public WordHelper(string fileName)
        {
            _fileInfo = new FileInfo(fileName);
        }

        private FileInfo _fileInfo;

        public void AddToTable(Word.Application app, Object missing, List<Inventorys> DisInv)
        {

            //Указываем таблицу в которую будем помещать данные (таблица должна существовать в шаблоне документа!)
            Microsoft.Office.Interop.Word.Table tbl = app.ActiveDocument.Tables[1];
            int i = 1;
            foreach (var item in DisInv)
            {
                //Добавляем в таблицу строку.
                tbl.Rows.Add(ref missing);
                tbl.Rows.Last.Cells[1].Range.Text = i++.ToString(); //№ п/п
                tbl.Rows.Last.Cells[2].Range.Text = item.Name; //наименование предмета
                tbl.Rows.Last.Cells[3].Range.Text = item.Amount.ToString(); //количество
                tbl.Rows.Last.Cells[4].Range.Text = item.Price.ToString(); //сумма
            }
        }

        internal bool Process(Dictionary<string, string> items, List<Inventorys> DisInv)
        {
            try
            {
                var app = new Word.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                app.Documents.Open(file);
                AddToTable(app, missing, DisInv);
                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;

                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;
                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing, Replace: replace);
                }
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Акты списания");
                var dateCreate = DateTime.Now;
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                string path1 = System.IO.Path.Combine(dirInfo.FullName, $"{dateCreate.ToString("d")}");
                DirectoryInfo dateWriteOff = new DirectoryInfo(path1);
                if (!dateWriteOff.Exists)
                {
                    dateWriteOff.Create();
                }
                string fileName = System.IO.Path.Combine(dateWriteOff.FullName, "Списание от"
                    + dateCreate.ToString("HH-mm") + ".docx");
                app.ActiveDocument.SaveAs(fileName);
                app.ActiveDocument.Close();
                app.Quit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
