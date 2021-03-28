using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace LinkageTool
{
    class Program
    {
        static int isFixxerEnabled = 0;
        static int isDifferEnabled = 0;
        static int isChecked = 0;
        static List<string> listA = new List<string>();
        static List<string> listB = new List<string>();
        static List<string> resultList = new List<string>();

        static string envA = "";
        static string envB = "";
        static void Main(string[] args)
        {

            LinkageFix _linkageFix = new LinkageFix();
            LinkageDiff _linkageDiff = new LinkageDiff();
            if (args.Length == 0)
            {
                Console.WriteLine("--------------TUTORIAL----------------");
                Console.WriteLine("To fix the records :");
                Console.WriteLine("-fix X:\\linkagefile");
                Console.WriteLine(" ");
                Console.WriteLine("To compare the records :");
                Console.WriteLine("-d : DEV enviroment ");
                Console.WriteLine("-t : Test enviroment ");
                Console.WriteLine("-pp : Preprod enviroment ");
                Console.WriteLine("-p : Prod enviroment ");
                Console.WriteLine("-diff -p X:\\linkagefilePROD -t X:\\linkagefileTEST");
                Console.WriteLine(" ");
                Console.WriteLine("Fix then compare the records : ");
                Console.WriteLine("-fix - diff -p X:\\linkagefilePROD -t X:\\linkagefileTEST");
                Console.WriteLine(" ");
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-fix")
                {
                    isFixxerEnabled = 1;
                }

                if (args[i] == "-diff")
                {
                    isDifferEnabled = 1;
                }

                if ((args[i]=="-d"|| args[i] == "-t" || args[i] == "-pp" || args[i] == "-p") && isChecked==0)
                {
                    isChecked = 1;
                    envA =GetEnviroment( args[i]); // -diff -p mvs2prd.txt -t mvs2tst.txt
                    envB= GetEnviroment(args[i+2]);
                    
                }

            }


            if ((isDifferEnabled == 1) && (isFixxerEnabled == 1))
            {
                Console.WriteLine("Compare will start after fix operations completed");
                if (args.Length < 6)
                {
                    Console.WriteLine("Error! File paths missing");
                }
                else
                {
                    //-fix -diff -p eglLinkage_PROD.txt -t eglLinkage_TEST.txt
                    resultList.Clear();
                    listA = (FileToList(args[3]));
                    listB = (FileToList(args[5]));
                    resultList = _linkageFix.StartFix(listA);
                    ListToFile(resultList, args[3], "_FixResults");
                    resultList.Clear();
                    resultList = _linkageFix.StartFix(listB);
                    ListToFile(resultList, args[5], "_FixResults");
                    ListToFile(listA, args[3], "_Fixed");
                    ListToFile(listA, args[5], "_Fixed");


                    resultList = _linkageDiff.StartCompare(envA,envB, listA, listB);
                    ListToFile(resultList, args[3], "_" + envA + "_VS_" + envB);
                }
            }
            else if ((isDifferEnabled == 1))
            {
                Console.WriteLine("Compare operations started");
                if (args.Length < 5)
                {
                    Console.WriteLine("Error! Missing parameters");
                }
                else
                {
                    // -diff -t 
                    resultList.Clear();
                    listA = (FileToList(args[2]));
                    listB = (FileToList(args[4]));
                    // _linkageFix.StartFix(listA);
                    // _linkageFix.StartFix(listB);
                    resultList = _linkageDiff.StartCompare(envA,envB,listA, listB);
                    ListToFile(resultList, args[2], "_"+envA+"_VS_"+envB);
                }
            }
            else if (isFixxerEnabled == 1)
            {
                
                if (args.Length < 2)
                {
                    Console.WriteLine("Error ! File path missing");
                }
                else
                {
                    Console.WriteLine("Fix operations started");
                    listA = (FileToList(args[1]));
                    resultList = _linkageFix.StartFix(listA);
                    ListToFile(listA, args[1], "_Fixed");
                    ListToFile(resultList, args[1], "_FixResult");

                }
            }
            if ((isDifferEnabled == 0) && (isFixxerEnabled == 0))
            {
                Console.WriteLine("Error! No operation parameters found ! ");
                
            }

        }


        static List<string> FileToList(string filePath)
        {
            List<string> tmpList = new List<string>();
            string tmpLine;
            try
            {
                StreamReader file = new StreamReader(filePath);
                while ((tmpLine = file.ReadLine()) != null)
                {
                    tmpLine = tmpLine.Trim();
                    tmpLine = tmpLine.ToUpper();
                    tmpLine = tmpLine.Replace('İ', 'I');
                    tmpList.Add(tmpLine);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("EglLinkage dosyasi satirları okunurken exception meydana geldi: " + ex.Message);
                return null;
            }
            return tmpList;
        }
        static void ListToFile(List<string> ls, string filePath, string operation)
        {

            try
            {
                //if (operation == "fix")
                //{
                //    filePath = filePath + "_Fixed";
                //}
                //else if (operation == "diff")
                //{
                //    filePath = filePath + "_Fixed"; // dosya adi + "_fixed". + dosya uzantisi
                //}
                //else if (operation == "fixresult")
                //{
                //    filePath = filePath + "_FixResults"; // dosya adi + "_fixed". + dosya uzantisi
                //}

                filePath = filePath + operation;

                StreamWriter file = new StreamWriter(filePath);
                for (int i = 0; i < ls.Count; i++)
                {
                    file.WriteLine(ls[i]);

                }
                file.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while saving file: " + ex.Message);

            }

        }

        static string GetEnviroment(string envParam)
        {
            switch (envParam)
            {
                case "-d":
                    return "Dev";
                   
                case "-t":
                    return "Test";
                 
                case "-pp":
                    return "Preprod";
                   
                case "-p":
                    return "Prod";
                   
                default:
                    return null;
               
            }
        }
    }
}
