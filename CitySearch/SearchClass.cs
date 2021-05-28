using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace CitySearch
{
    public class SearchClass : ICityFinder
    {
        List<string> testCities;

        public SearchClass(List<string> cityList)
        {
            testCities = cityList;
        }

        public ICityResult Search(string SearchString)
        {
            int x = BinarySearch(SearchString, testCities);
            List<string> cities;

            if (x >= 0) // Found a match
            {
                Tuple<int,int> bounds = FindBounds(x, SearchString, testCities);
                cities = testCities.GetRange(bounds.Item1, 1 + bounds.Item2 - bounds.Item1);
            }  
            else // Match not found
            {
                cities = new List<string>();
            }
            
            return new Result(cities, SearchString.Length);
        }

        int BinarySearch(string _searchToken, List<string> _citiesList)
        {
            List<string> allCities = _citiesList;
            int numberOfCities = allCities.Count;
            string searchToken = _searchToken;

            int lowerBound = 0;
            int upperBound = numberOfCities - 1;

            while (true) {
                if (upperBound < lowerBound)
                {
                    // Not found!
                    return -1;
                }

                int midPoint = lowerBound + (upperBound - lowerBound) / 2;

                if (allCities[midPoint].StartsWith(searchToken))
                {
                    // Found
                    return midPoint;
                }
                else
                {
                    // Not Found, see if bigger or lower
                    int x = searchToken.CompareTo(allCities[midPoint]);
                    if(x < 0)
                    {
                        // Pick Lower
                        upperBound = midPoint - 1;
                    }

                    if(x > 0)
                    {
                        // Pick Higher
                        lowerBound = midPoint + 1;
                    }
                }
            }
        }
    
        Tuple<int,int> FindBounds(int matchIndex, string SearchString, List<string> citiesList)
        {
            // Expontential Search in both directions for bounds of cities
            int lowerbound, upperbound;
            lowerbound = upperbound = matchIndex;

            // Find Lower Bound
            int jumpsize = 1;
            while (true)
            {
                
                if ((lowerbound - jumpsize < 0) || (!citiesList[lowerbound - jumpsize].StartsWith(SearchString)))
                {
                    if(jumpsize > 1)
                    {
                        jumpsize = 1;
                        continue; // Need to check against above if statement again
                    } 
                    else
                        break; // Lower bound found
                }

                lowerbound -= jumpsize;
                jumpsize *= 2;

            }

            // Find Upper Bound
            jumpsize = 1;
            while (true)
            {
                if ((upperbound + jumpsize >= citiesList.Count - 1) || (!citiesList[upperbound + jumpsize].StartsWith(SearchString)))
                {
                    if (jumpsize > 1)
                    {
                        jumpsize = 1;
                        continue;
                    }
                    else
                        break; // Upperbound bound found
                }

                upperbound += jumpsize;
                jumpsize *= 2;
            }

            // Upper and Lower Bounds Found!
            return new Tuple<int,int>(lowerbound, upperbound);
        }
    }

    public class Result : ICityResult
    {
        private ICollection<string> nextLetters;
        private ICollection<string> nextCities;

        public ICollection<string> NextLetters { get => nextLetters; set => nextLetters = value; }
        public ICollection<string> NextCities { get => nextCities; set => nextCities = value; }

        // Reference SearchClass which should return list of cities, get next letters and set the corresponding lists above
        public Result(List<string> cities, int searchStringLength)
        {
            // Set up
            nextCities = cities;
            nextLetters = new HashSet<string>();
            foreach (string x in cities)
            {
                if (x.Length > searchStringLength)
                    nextLetters.Add(x.Substring(searchStringLength, 1));
            }
        }
    }

    public class mainClass
    {
        static void Main(string[] args)
        {
            #region Stopwatch Test Code
            //string[] lines = System.IO.File.ReadAllLines(@"F:\Tekgem\CitySearch\CitySearch.UnitTests\WordsList.txt");
            //List<string> cities = new List<string>();
            //foreach (string x in lines)
            //{
            //    cities.Add(x);
            //}

            //SearchClass search = new SearchClass(cities);

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //ICityResult r = search.Search("a");
            //stopwatch.Stop();

            //Console.WriteLine("Elapsed Time: " + stopwatch.ElapsedMilliseconds);


            //Console.WriteLine(r.NextCities.Count);
            //Console.WriteLine(r.NextLetters.Count);
            #endregion
        }
    }
}
