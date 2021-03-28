using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkageTool
{
    class LinkageDiff
    {
        private int lineNumber = 0;
        private int duplicateLines;
        private string pgmName;
        private string _pgmName;
        private int isExist = 0;
        private int differenceCount = 0;
        private int notExisting = 0;
        List<string> _resultList = new List<string>();

        public List<string> StartCompare(List<string> listA, List<string> listB)
        {


            for (int i = 0; i < listA.Count; i++)
            {
                pgmName = "";

                if (listA[i].Contains("PGMNAME") || listA[i].Contains("APPLNAME"))
                {

                    if (listA[i].Contains("PGMNAME"))
                    {
                        pgmName = listA[i].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }
                    else if (listA[lineNumber].Contains("APPLNAME"))
                    {
                        pgmName = listA[i].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                    }


                    for (int j = 0; j < listB.Count; j++) // bir sonraki satirdan itibaren baslayarak diger satirlari tek tek 
                    {
                        _pgmName = "";

                        if (listB[j].Contains("PGMNAME") || listB[lineNumber].Contains("APPLNAME"))
                        {
                            if (listB[j].Contains("PGMNAME"))
                            {
                                _pgmName = listB[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            else if (listB[j].Contains("APPLNAME"))
                            {
                                _pgmName = listB[j].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                            }



                            if ((_pgmName == pgmName))
                            {
                                isExist = 1; // diger listede bu tanim var
                                if (!listA[i].Equals(listB[j]))// a listesinde yer alan program ile b listesinde yer alan programin conf u ayni degilse
                                {
                                    _resultList.Add(pgmName + ": DIFFERENT_RECORD");
                                    Console.WriteLine(pgmName + " has different linkage in other enviroment");
                                    differenceCount++;
                                }
                                break;
                            }
                            else
                            {
                                isExist = 0;
                            }

                        }
                    }
                    if (isExist == 0)
                    {
                        notExisting++; // A listesindeki pgm b'de yoksa fark var demektir.
                        _resultList.Add(pgmName + ": NO_RECORD");
                        Console.WriteLine(pgmName + " doesnt exist in other enviroment");
                    }
                }

            }

            Console.WriteLine("Total Differences: " + differenceCount);
            Console.WriteLine("Exist only in one enviroment: " + notExisting);
            return _resultList;
        }



    }
}
