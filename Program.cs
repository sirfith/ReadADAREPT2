// See https://aka.ms/new-console-template for more information
{
    DateTime date1 = DateTime.Now;
    string date2 = (date1.ToString("MMddyy"));
    Console.WriteLine("Date1: {0} ", date1);
    Console.Write("Input the Date: ");
    string date3 = Console.ReadLine();
    if (date3 == "")
    { date3 = date2; }
    string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    string source = myDocuments + @"\ADAREPTs\adarepts.d" + date3 + @".txt";
    string target = myDocuments + @"\ADAREPTs\adarepts.d" + date3 + @".Summary.txt";
    // adarepAsync(fileName, target);
    delold(target);
    adarepAsync(source, target);
    static async Task adarepAsync(string fileName, string target)
    {
        string[] lines = System.IO.File.ReadAllLines(fileName);
        //      bool b = false;
        int[] dataext = new int[99];
        int dataextnum = 0;
        int[] assocext = new int[99];
        int assocextnum = 0;
        int dbid = 0;
        string dbids = " ";
        char site = ' ';
        // Display the file contents by using a foreach loop.
        Console.WriteLine("Contents of {0}", fileName);
        foreach (string line in lines)
        {
            //    string line2 = line;
            //     if (!((line.StartsWith("*")) || (line.StartsWith("1")) || line.Contains(@"***") || (line.StartsWith("\r\n"))))
            //    {
            //if ((line.ToUpper().Contains(@"* FILE OPTIONS *")) || (line.ToUpper().Contains(@"CONTENTS OF PPT")))
            //{
            //    b = false;
            //}
            //if (line.ToUpper().Contains(@"DATA BASE NAME          =")) // || (line.ToUpper().Contains(@"DATA BASE NUMBER        =")))
            //{ Console.WriteLine(line); }
            if (line.ToUpper().Contains(@"DATA BASE NUMBER        ="))
            {
                // Console.WriteLine(line);
                dbids = line.Substring(31, 5);
                site = line[0];
                // Console.WriteLine(dbids);
                try
                {
                    dbid = Int32.Parse(dbids);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (line.Contains(@"U N U S E D   S T O R A G E") || (line.ToUpper().Contains(@"CONTENTS OF DATABASE")))
            {
                //b = true;
                if (dataextnum > 0 && assocextnum > 0)
                {
                    int totdata = 0;
                    int totassoc = 0;
                    string data =  site + @" DBID:" + dbids + " D (";
                    string assoc = site + @" DBID:" + dbids + " A (";
                    //     Console.WriteLine("Found DATA: {0} Found ASSOC: {1}", dataextnum, assocextnum);
                    //     Console.Write("Data: ");
                    for (int k = 1; k < dataextnum; k++)
                    {
                        //Console.Write("{0} : {1}  ", k, dataext[k]);
                        //if (k == dataextnum)
                        //{
                        //    data += dataext[k] + @")";
                        //}
                        //  else
                        //{
                        data += dataext[k] + @"+";
                        //}
                        totdata += dataext[k];
                    }
                    //   Console.Write("\r\n");
                    //   Console.Write("Assoc: ");
                    for (int l = 1; l < assocextnum; l++)
                    {
                        //Console.Write("{0} : {1}  ", l, assocext[l]);
                        //if (l == assocextnum)
                        //{
                        //  assoc += assocext [l]+ @")";
                        //}
                        // else
                        //{
                        assoc += assocext[l] + @"+";
                        //}
                        totassoc += assocext[l];
                    }
                    totdata += dataext[dataextnum];
                    data += dataext[dataextnum] + @") Total: " + totdata;
                    totassoc += assocext[assocextnum];
                    assoc += assocext[assocextnum] + @") Total: " + totassoc;
                    //  Console.Write("\r\n");
                    Console.WriteLine(data);
                    Console.WriteLine(assoc);
                    Console.WriteLine("{0} DBID: {1} DATAe: {2} ASSOCe: {3} Total DATA: {4} Total ASSOC: {5} ", site, dbid, dataextnum, assocextnum, totdata, totassoc);
                    using StreamWriter file = new(target, append: true);
                    await file.WriteLineAsync(data);
                    await file.WriteLineAsync(assoc);
                }
                dataext = new int[99];
                dataextnum = 0;
                assocext = new int[99];
                assocextnum = 0;
            }
            //          if (b )
            //             Console.WriteLine(line);
            //                 using StreamWriter file = new(target, append: true);
            //               await file.WriteLineAsync(line);
            if (line.ToUpper().Contains(@"DATAR"))
            {
                dataextnum++;
                string de = line.Substring(19, 6);
                try
                {
                    dataext[dataextnum] = Int32.Parse(de);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (line.ToUpper().Contains(@"ASSOR"))
            {
                assocextnum++;
                string ae = line.Substring(19, 6);
                try
                {
                    assocext[assocextnum] = Int32.Parse(ae);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
    //}
    static void delold(string target)
    {
        // Files to be deleted    
        try
        {
            // Check if file exists with its full path    
            if (File.Exists(target))
            {
                // If file found, delete it    
                File.Delete(target);
                Console.WriteLine("{0} File deleted.", target);
            }
            else Console.WriteLine("{0} File not found", target);
        }
        catch (IOException ioExp)
        {
            Console.WriteLine(ioExp.Message);
        }
    }
}