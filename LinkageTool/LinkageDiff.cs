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

        public List<string> StartCompare(string envA, string envB, List<string> listA, List<string> listB)
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
                    else if (listA[i].Contains("APPLNAME"))
                    {
                        pgmName = listA[i].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                    }


                    for (int j = 0; j < listB.Count; j++) // bir sonraki satirdan itibaren baslayarak diger satirlari tek tek 
                    {
                        _pgmName = "";

                        if (listB[j].Contains("PGMNAME") || listB[j].Contains("APPLNAME"))
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
                                    _resultList.Add("DIFFERENT_RECORD (" + envB + "): " + pgmName + " | " + listB[j]);
                                    Console.WriteLine("DIFFERENT_RECORD (" + envB + "): " + pgmName + " | " + listB[j]);
                                    differenceCount++;
                                }
                                break;
                            }
                            else
                            {
                                isExist = 0;
                            }
                           
                        }
                        lineNumber = j;
                    }
                    if (isExist == 0)
                    {
                        notExisting++; // A listesindeki pgm b'de yoksa fark var demektir.
                        _resultList.Add("EXIST_ONLY (" + envA + "): "+ pgmName + " | " + listA[i]);
                        Console.WriteLine("EXIST_ONLY (" + envA + "): " + pgmName+ " | " + listA[i]);
                    }
                }

            }
            _resultList.Add(" ");
            _resultList.Add("DIFFERENCES: " + differenceCount);
            _resultList.Add("EXIST_ONLY " + "(" + envA + "): " + notExisting);
            Console.WriteLine("DIFFERENCES: " + differenceCount);
            Console.WriteLine("EXIST_ONLY "+"("+envA+"): " + notExisting);
           
            return _resultList;
        }



    }
}
