using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
//using WatTmdb.V3;
using System.Data;
using System.IO;

namespace IMDBforEduConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            TMDbClient client = new TMDbClient("f97bb3532c5d9095d9413d3f14ac981a");
            Console.WriteLine("enter initial id");        
            int initialID =  int.Parse(Console.ReadLine());
            Console.WriteLine("enter last id");
            int endID = int.Parse(Console.ReadLine());
            Console.WriteLine("create a file(with extension .csv) on Desktop and enter its exact name");
            string fileName = Console.ReadLine();
            Console.WriteLine("Open file explorer and go to users and go to the current user and copy and paste its name, eg: ramit-pc, malvi_000");
            string systemName = Console.ReadLine();
            Console.WriteLine("Want null values? Y/N");
            string readNulls = Console.ReadLine();
            DataTable dataTable = getDataTableInstance();
            for (int i = initialID; i < endID; i++)
            {
                string stringID = getID(i);
                MyMovie myMovie = GetMovieDetails(stringID, client, readNulls);
                if (myMovie != null)
                {
                    DataRow row = dataTable.NewRow();
                    row["IMDB_ID"] = myMovie.imdbID;
                    row["Title"] = myMovie.title;
                    row["Budget"] = myMovie.budget;
                    row["Genre1"] = myMovie.genre1;
                    row["Genre2"] = myMovie.genre2;
                    row["Genre3"] = myMovie.genre3;
                    row["Language"] = myMovie.language;
                    row["Production_Companies"] = myMovie.companies;
                    row["Year"] = myMovie.year;
                    row["Runtime"] = myMovie.runtime;
                    row["Vote_Count"] = myMovie.voteCount;
                    row["Vote_Avg"] = myMovie.voteAvg;
                    row["Countries"] = myMovie.countries;
                    row["Cast1"] = myMovie.cast1;
                    row["Cast2"] = myMovie.cast2;
                    row["Cast3"] = myMovie.cast3;
                    row["Cast4"] = myMovie.cast4;
                    row["Cast5"] = myMovie.cast5;
                    row["Writer"] = myMovie.writer;
                    row["Director"] = myMovie.director;
                    row["Producer"] = myMovie.producer;
                    row["Composer"] = myMovie.composer;
                    row["Editor"] = myMovie.editor;


                    dataTable.Rows.Add(row);
                    Console.WriteLine(myMovie.title);
                }

                if (dataTable.Rows.Count % 100 == 0 && dataTable.Rows.Count != 0)
                {

                    writeToCSV(dataTable, fileName, systemName);
                    dataTable = getDataTableInstance();
                }
            }
            //GetMovieDetails("0000001", client);
        }

        private static DataTable getDataTableInstance()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("IMDB_ID", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Budget", typeof(string));
            dataTable.Columns.Add("Genre1", typeof(string));
            dataTable.Columns.Add("Genre2", typeof(string));
            dataTable.Columns.Add("Genre3", typeof(string));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("Production_Companies", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));
            dataTable.Columns.Add("Runtime", typeof(string));
            dataTable.Columns.Add("Vote_Count", typeof(string));
            dataTable.Columns.Add("Vote_Avg", typeof(string));
            dataTable.Columns.Add("Countries", typeof(string));
            dataTable.Columns.Add("Cast1", typeof(string));
            dataTable.Columns.Add("Cast2", typeof(string));
            dataTable.Columns.Add("Cast3", typeof(string));
            dataTable.Columns.Add("Cast4", typeof(string));
            dataTable.Columns.Add("Cast5", typeof(string));
            dataTable.Columns.Add("Writer", typeof(string));
            dataTable.Columns.Add("Director", typeof(string));
            dataTable.Columns.Add("Producer", typeof(string));
            dataTable.Columns.Add("Composer", typeof(string));
            dataTable.Columns.Add("Editor", typeof(string));

            return dataTable;
        }

        private static void writeToCSV(DataTable dt, string fileName, string systemName)
        {
            string file = "C:\\Users\\"+ systemName + "\\Desktop\\" + fileName;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            File.AppendAllText(file, sb.ToString());
        }

        public static MyMovie GetMovieDetails(string movieID, TMDbClient client, string readNulls)
        {
            Movie movie = client.GetMovie(movieID);
            List<TMDbLib.Objects.General.Genre> genres = movie.Genres;
            Credits credits1 = client.GetMovieCredits(movie.Id);
            readNulls = readNulls.ToUpper();
            MyMovie myMovie = new MyMovie();

            myMovie.budget = movie.Budget.ToString();

            myMovie.imdbID = movieID;
            if (movie.Title != null)
                myMovie.title = movie.Title;
            else
                    return null;
            
                
            if (genres.ToList().Count > 1)
                myMovie.genre1 = genres.ToList()[genres.Count - 1].Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (genres.ToList().Count > 2)
                myMovie.genre2 = genres.ToList()[genres.Count - 2].Name;            

            if (genres.ToList().Count > 3)
                myMovie.genre3 = genres.ToList()[genres.Count - 3].Name;
            

            if (movie.OriginalLanguage != null)
                myMovie.language = movie.OriginalLanguage;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (movie.ProductionCompanies.Count != 0)
                myMovie.companies = movie.ProductionCompanies.ToList()[0].Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (movie.ReleaseDate != null)
                myMovie.year = movie.ReleaseDate.Value.Year.ToString();
            else
            {
                if (readNulls == "N")
                    return null;
            }


            if (movie.Runtime != 0)
                myMovie.runtime = movie.Runtime.ToString();
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (movie.VoteCount != 0)
                myMovie.voteCount = movie.VoteCount.ToString();
            else
            {
                if (readNulls == "N")
                    return null;
            }

            myMovie.voteAvg = movie.VoteAverage.ToString();

            if (movie.ProductionCountries.ToList().Count > 0)
                myMovie.countries = movie.ProductionCountries.ToList()[0].Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (credits1.Cast.ToList().Count > 0)
                myMovie.cast1 = credits1.Cast.ToList()[0].Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (credits1.Cast.ToList().Count > 1)
                myMovie.cast2 = credits1.Cast.ToList()[1].Name;
            if (credits1.Cast.ToList().Count > 2)
                myMovie.cast3 = credits1.Cast.ToList()[2].Name;
            if (credits1.Cast.ToList().Count > 3)
                myMovie.cast4 = credits1.Cast.ToList()[3].Name;
            if (credits1.Cast.ToList().Count > 4)
                myMovie.cast5 = credits1.Cast.ToList()[4].Name;

            if (credits1.Crew.ToList().Where(c => c.Job.Contains("Author")).ToList().Count() > 0)
                myMovie.writer = credits1.Crew.ToList().FirstOrDefault(c => c.Job.Contains("Author")).Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (credits1.Crew.ToList().Where(c => c.Job.Contains("Director")).Count() > 0)
                myMovie.director = credits1.Crew.ToList().FirstOrDefault(c => c.Job.Contains("Director")).Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }


            if (credits1.Crew.ToList().Where(c => c.Job.Contains("Producer")).Count() > 0)
                myMovie.producer = credits1.Crew.ToList().FirstOrDefault(c => c.Job.Contains("Producer")).Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (credits1.Crew.ToList().Where(c => c.Job.Contains("Composer")).Count() > 0)
                myMovie.composer = credits1.Crew.ToList().FirstOrDefault(c => c.Job.Contains("Composer")).Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            if (credits1.Crew.ToList().Where(c => c.Job.Contains("Director")).Count() > 0)
                myMovie.editor = credits1.Crew.ToList().FirstOrDefault(c => c.Job.Contains("Director")).Name;
            else
            {
                if (readNulls == "N")
                    return null;
            }

            return myMovie;
        }

        public static string getID(int intID)
        {
            string stringID = intID.ToString();
            if (stringID.Length == 1)
            {
                stringID = "000000" + stringID;
            }
            else if (stringID.Length == 2)
            {
                stringID = "00000" + stringID;
            }
            else if (stringID.Length == 3)
            {
                stringID = "0000" + stringID;
            }
            else if (stringID.Length == 4)
            {
                stringID = "000" + stringID;
            }
            else if (stringID.Length == 5)
            {
                stringID = "00" + stringID;
            }
            else if (stringID.Length == 6)
            {
                stringID = "0" + stringID;
            }
            return stringID;
        }

    }
}
