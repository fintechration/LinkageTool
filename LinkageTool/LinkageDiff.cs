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
        private string linkageOptionsName = " ";
        private string _linkageOptionsName = " ";
        private int isExist = 0;
        private int differenceCount = 0;
        private int notExistingA = 0;
        private int notExistingB = 0;
        List<string> _resultList = new List<string>();

        public List<string> StartCompare(string envA, string envB, List<string> listA, List<string> listB)
        {


            for (int i = 0; i < listA.Count; i++)
            {
                pgmName = "";

             
                if (listA[i].Contains("<LINKAGEOPTIONS NAME=\""))
                {
                    linkageOptionsName = listA[i].Split(new string[] { "<LINKAGEOPTIONS NAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                }

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

                    int j = 0;
                    for (j=0;j < listB.Count;j++)  // bu satir gelene kadar satirlari incelemeye devam et ki her linkageoptionlari kendi icinde incelemis olalim
                    {
                        if (listB[j].Contains("<LINKAGEOPTIONS NAME=\""))
                        {
                            _linkageOptionsName = listB[j].Split(new string[] { "<LINKAGEOPTIONS NAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                        }
                        _pgmName = "";

                        if ((listB[j].Contains("PGMNAME") || listB[j].Contains("APPLNAME")) && _linkageOptionsName==linkageOptionsName)
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
                        notExistingB++; // A listesindeki pgm b'de yoksa fark var demektir.
                        _resultList.Add("EXIST_ONLY (" + envA + "): " + pgmName + " | " + listA[i]);
                        Console.WriteLine("EXIST_ONLY (" + envA + "): " + pgmName + " | " + listA[i]);
                    }
                }
               
             

            }

            for (int i = 0; i < listB.Count; i++)
            {
                pgmName = "";


                if (listB[i].Contains("<LINKAGEOPTIONS NAME=\""))
                {
                    linkageOptionsName = listB[i].Split(new string[] { "<LINKAGEOPTIONS NAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                }

                if (listB[i].Contains("PGMNAME") || listB[i].Contains("APPLNAME"))
                {

                    if (listB[i].Contains("PGMNAME"))
                    {
                        pgmName = listB[i].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }
                    else if (listA[i].Contains("APPLNAME"))
                    {
                        pgmName = listB[i].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                    }

                    int j = 0;
                    for (j = 0; j < listA.Count; j++)  // bu satir gelene kadar satirlari incelemeye devam et ki her linkageoptionlari kendi icinde incelemis olalim
                    {
                        if (listA[j].Contains("<LINKAGEOPTIONS NAME=\""))
                        {
                            _linkageOptionsName = listA[j].Split(new string[] { "<LINKAGEOPTIONS NAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                        }
                        _pgmName = "";

                        if ((listA[j].Contains("PGMNAME") || listA[j].Contains("APPLNAME")) && _linkageOptionsName == linkageOptionsName)
                        {
                            if (listA[j].Contains("PGMNAME"))
                            {
                                _pgmName = listA[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            else if (listA[j].Contains("APPLNAME"))
                            {
                                _pgmName = listA[j].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                            }

                            if ((_pgmName == pgmName))
                            {
                                isExist = 1;
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
                        notExistingA++; // A listesindeki pgm b'de yoksa fark var demektir.
                        _resultList.Add("EXIST_ONLY (" + envB + "): " + pgmName + " | " + listB[i]);
                        Console.WriteLine("EXIST_ONLY (" + envB + "): " + pgmName + " | " + listB[i]);
                    }
                }



            }


            _resultList.Add(" ");
            _resultList.Add("DIFFERENCES: " + differenceCount);
            _resultList.Add("EXIST_ONLY " + "(" + envA + "): " + notExistingA);
            _resultList.Add("EXIST_ONLY " + "(" + envB + "): " + notExistingB);
            _resultList.Add("TOTAL: " + (differenceCount + notExistingA+ notExistingB));
            Console.WriteLine(" ");
            Console.WriteLine("DIFFERENCES: " + differenceCount);
            Console.WriteLine("EXIST_ONLY " + "(" + envA + "): " + notExistingB);
            Console.WriteLine("EXIST_ONLY " + "(" + envB + "): " + notExistingB);
            Console.WriteLine("TOTAL: " + (differenceCount + notExistingA + notExistingB));
            //-diff -t eglLinkage_Fixed -p eglLinkage_Fixed_1
            return _resultList;
        }



    }
}
