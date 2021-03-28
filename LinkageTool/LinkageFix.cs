using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkageTool
{
    class LinkageFix
    {
        private int lineANumber = 0;
        private int lineBNumber = 0;
        private int duplicateCnt=0;
        private int noneffectiveCnt=0;
        private string pgmName;
        private string _pgmName;


        public List<string> StartFix(List<string> lineList)
        {
            List<string> _resultList = new List<string>();
            // Console.WriteLine("------Mukerrer Kayıtlar-----");
            //lineList.Clear();
            // lineList = FileToList(filePath);
            for (lineANumber = 0; lineANumber < lineList.Count; lineANumber++)
            {


                while (lineList[lineANumber] != "</LINKAGEOPTIONS>" && lineANumber < lineList.Count)  // bu satir gelene kadar satirlari incelemeye devam et ki her linkageoption block'unu kendi icinde incelemis olalim
                {

                    pgmName = "";
                    if (lineList[lineANumber].Contains("PGMNAME") || lineList[lineANumber].Contains("APPLNAME"))
                    {

                        if (lineList[lineANumber].Contains("PGMNAME"))
                        {
                            pgmName = lineList[lineANumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                        }
                        else if (lineList[lineANumber].Contains("APPLNAME"))
                        {
                            pgmName = lineList[lineANumber].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                        }

                        lineBNumber = lineANumber + 1;// a satirindan sonraki satira b satiri dedik ve numarasi ise a+1
                        if (lineBNumber >= lineList.Count)
                            break;

                        while ((lineList[lineBNumber] != "</LINKAGEOPTIONS>") && (lineBNumber < lineList.Count))  // bu satir gelene kadar satirlari incelemeye devam et ki her linkageoptionlari kendi icinde incelemis olalim
                        {
                            _pgmName = "";

                            if (lineList[lineBNumber].Contains("PGMNAME") || lineList[lineBNumber].Contains("APPLNAME"))
                            {
                                if (lineList[lineBNumber].Contains("PGMNAME"))
                                {
                                    _pgmName = lineList[lineBNumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                                }
                                else if (lineList[lineBNumber].Contains("APPLNAME"))
                                {
                                    _pgmName = lineList[lineBNumber].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                                }

                                if ((_pgmName == pgmName))
                                {
                                    Console.WriteLine(lineList[lineBNumber]);
                                    _resultList.Add("DUPLICATE: " + _pgmName + " | " + lineList[lineBNumber]);
                                    lineList.RemoveAt(lineBNumber);
                                    
                                    lineBNumber--;
                                    duplicateCnt++;
                                }
                                else if (pgmName.Contains("*") && (pgmName.Split('*')[0].Length <= _pgmName.Length))
                                {
                                    if (_pgmName.Substring(0, pgmName.Split('*')[0].Length).Contains(pgmName.Split('*')[0]))
                                    {
                                        Console.WriteLine(lineList[lineBNumber]);
                                        _resultList.Add("NOEFFECT: " + _pgmName + "("+ pgmName+ " is already defined) | " + lineList[lineBNumber]);
                                        lineList.RemoveAt(lineBNumber);

                                        noneffectiveCnt++;
                                        lineBNumber--;
                                    }
                                }
                            }
                            lineBNumber++;
                            if (lineBNumber >= lineList.Count)
                                break;
                        }

                    }
                    lineANumber++;
                    if (lineANumber >= lineList.Count)
                        break;
                }

            }
            _resultList.Add("\n\n\nNONEFFECTIVES: " + noneffectiveCnt);
            _resultList.Add("DUPLICATES: " + duplicateCnt);
            _resultList.Add("TOTAL: " + (duplicateCnt+noneffectiveCnt));

            return _resultList;
        }
    }
}
