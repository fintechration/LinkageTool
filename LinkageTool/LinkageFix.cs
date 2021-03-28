using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkageTool
{
    class LinkageFix
    {
        private int lineNumber = 0;
        private int duplicateLines;
        private string pgmName;
        private string _pgmName;
       

        public List<string> StartFix(List<string> lineList)
        {
            List<string> _resultList = new List<string>();
            Console.WriteLine("------Mukerrer Kayıtlar-----");
            //lineList.Clear();
           // lineList = FileToList(filePath);
            for (lineNumber = 0; lineNumber < lineList.Count; lineNumber++)
            {
                pgmName = "";

                if (lineList[lineNumber].Contains("PGMNAME") || lineList[lineNumber].Contains("APPLNAME"))
                {

                    if (lineList[lineNumber].Contains("PGMNAME"))
                    {
                        pgmName = lineList[lineNumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }
                    else if (lineList[lineNumber].Contains("APPLNAME"))
                    {
                        pgmName = lineList[lineNumber].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                    }


                    for (int j = lineNumber + 1; j < lineList.Count; j++) // bir sonraki satirdan itibaren baslayarak tek tek kiyasla
                    {
                        _pgmName = "";

                        if (lineList[j].Contains("PGMNAME") || lineList[lineNumber].Contains("APPLNAME"))
                        {
                            if (lineList[j].Contains("PGMNAME"))
                            {
                                _pgmName = lineList[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            else if (lineList[j].Contains("APPLNAME"))
                            {
                                _pgmName = lineList[j].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                            }

                            if ((_pgmName == pgmName))
                            {
                                Console.WriteLine(lineList[j]);
                                lineList.RemoveAt(j);
                                j--;
                                duplicateLines++;
                                _resultList.Add(_pgmName + " : DUPLICATE");
                            }
                            else if (pgmName.Contains("*") && (pgmName.Split('*')[0].Length <= _pgmName.Length))
                            {
                                if (_pgmName.Substring(0, pgmName.Split('*')[0].Length).Contains(pgmName.Split('*')[0]))
                                {
                                    Console.WriteLine(lineList[j]);
                                    duplicateLines++;
                                    lineList.RemoveAt(j);
                                    _resultList.Add(_pgmName + " : NONESENSE");
                                    j--;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("İlgili kayitlar icin duzeltme tamamlandi");
            Console.WriteLine("Toplam Tekrar Eden Kayıt: " + duplicateLines);
            return _resultList;
        }
    }
}
